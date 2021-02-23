using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEditor.Callbacks;

namespace NoisePerspective
{
    public class MainWindow : EditorWindow
    {
        Texture2D background;
        public GUISkin guiSkin;

        readonly Color gridOuterColor = Color.grey;
        readonly float gridOuterSpacing = 135;
        readonly float gridOuterOpacity = 0.2f;
        
        readonly Color gridInnerColor = Color.grey;
        readonly float gridInnerSpacing = 27;
        readonly float gridInnerOpacity = 0.1f;

        //Open from asset
        [OnOpenAssetAttribute(1)]
        public static bool Step1(int instanceID, int _line)
        {
            Type type = EditorUtility.InstanceIDToObject(instanceID).GetType();

            if (type == typeof(NoiseAsset))
            {
                Manager.Instance.Load((NoiseAsset)EditorUtility.InstanceIDToObject(instanceID));
                Manager.Instance.mainWindow = (MainWindow)GetWindow(typeof(MainWindow)); //open window
                Manager.Instance.mainWindow.titleContent = new GUIContent("Noise Perspective");

                string assetPath = AssetDatabase.GetAssetPath(Manager.Instance.asset);
                EditorPrefs.SetString("LastNoiseAsset", assetPath);

                return true;
            }
            else
            {
                return false;
            }
        }

        //Open from Window
        [MenuItem("Window/Noise Perspective")]
        public static void ShowWindow()
        {
            Manager.Instance.mainWindow = (MainWindow)GetWindow(typeof(MainWindow)); //open window
            Manager.Instance.mainWindow.titleContent = new GUIContent("Noise Perspective");
        }
        
        public void OnEnable()
        {
            NodeManager.Instance.nodesToRemove = NodeManager.Instance.nodes;

            if (Manager.Instance.asset != null)
            {
                Manager.Instance.Load(Manager.Instance.asset);
            }
            else
            {
                string lastAssetPath = EditorPrefs.GetString("LastNoiseAsset");
                NoiseAsset lastAsset = (NoiseAsset)AssetDatabase.LoadAssetAtPath(lastAssetPath, typeof(NoiseAsset));
                if (lastAsset != null)
                    Manager.Instance.Load(lastAsset);
                else
                Manager.Instance.Save();
            }

            Manager.Instance.CreateScene();
            ConnectionManager.Instance.Initialize();
        }

        private void Update()
        {
            //Repaint(); //Probably remove this later          
        }

        void OnGUI()
        {
            guiSkin = GUI.skin = Resources.Load("Styles/NoisePerspectiveGUISkin") as GUISkin;
            background = guiSkin.FindStyle("mainBackground").normal.background;
            EditorGUIUtility.labelWidth = 1; //Hide builtin field labels to remove extra padding, but still allows float field edge sliding 

            //Set Background for main Window
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), background, ScaleMode.StretchToFill);

            //Draw Grid Overlay
            DrawGrid(gridInnerSpacing, gridInnerOpacity, gridInnerColor);
            DrawGrid(gridOuterSpacing, gridOuterOpacity, gridOuterColor);

            BeginWindows();
            NodeManager.Instance.DrawNodeWindows();
            EndWindows();

            ConnectionManager.Instance.DrawConnections();
            ConnectionManager.Instance.DrawConnectionLine(Event.current);
            EventManager.Instance.ProcessEvents();
            NodeManager.Instance.RenderCameras();

            //Temp buttons
            using (var verticalScope = new EditorGUILayout.VerticalScope("fieldGroup"))
            {
                using (var tempControls = new EditorGUILayout.HorizontalScope("box"))
                {
                    ConnectionManager.Instance.useBezierConnections = GUILayout.Toggle(ConnectionManager.Instance.useBezierConnections, "Bezier Lines", GUILayout.Width(100));
                    if (GUILayout.Button("Save", GUILayout.Width(100))) { Manager.Instance.Save(); }
                    NodeManager.Instance.DrawControls();
                }
            }

            if (GUI.changed)
                Repaint();

            NodeManager.Instance.CleanUp();
        }

        //Draw Grid
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            Manager.Instance.asset.gridOffset += EventManager.Instance.mouseDragDelta * 0.5f;
            Vector3 newOffset = new Vector3(Manager.Instance.asset.gridOffset.x % gridSpacing, Manager.Instance.asset.gridOffset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        public void OnDisable()
        {
            string assetPath = AssetDatabase.GetAssetPath(Manager.Instance.asset);
            EditorPrefs.SetString("LastNoiseAsset", assetPath);          

            //if (NodeManager.Instance.nodes != null && NodeManager.Instance.nodes.Count > 0) //Commented out to make developing faster
            //    Manager.Instance.Save();

            if (NodeManager.Instance.nodes != null)
            {
                foreach (Node node in NodeManager.Instance.nodes)
                {
                    node.CleanUp();
                    EditorSceneManager.ClosePreviewScene(Manager.Instance.scene);
                }
            }

            Manager.Instance.Dispose();
        }
    }
}

