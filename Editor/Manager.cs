using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    public sealed class Manager : IDisposable
    {
        public NoiseAsset asset;

        public MainWindow mainWindow;
        public Scene scene;
        public Light sceneLight;

        private Manager()
        {

        }

        private static Manager instance = null;
        public static Manager Instance
        {
            get
            {
                if (instance == null)
                    instance = new Manager();

                return instance;
            }
        }

        //Called when MainWindow is opened
        public void CreateScene()
        {
            //Scene
            scene = EditorSceneManager.NewPreviewScene();
            scene.name = "Noise Perspective";

            //Culling mask
            ulong cullingMask = EditorSceneManager.CalculateAvailableSceneCullingMask();
            EditorSceneManager.SetSceneCullingMask(scene, cullingMask); //Prevents running out of culling masks if editor is reopened several times

            //Light
            GameObject lightGo = new GameObject();
            sceneLight = lightGo.AddComponent<Light>();
            sceneLight.type = LightType.Directional;
            EditorSceneManager.MoveGameObjectToScene(lightGo, scene);
            lightGo.transform.position = lightGo.transform.up * 10;
            lightGo.transform.rotation = Quaternion.Euler(60, 0, 0);
            sceneLight.shadows = LightShadows.None;
        }

        public void Save()
        {
            string path;

            if (asset == null)
            {
                asset = (NoiseAsset)ScriptableObject.CreateInstance(typeof(NoiseAsset));
                path = EditorUtility.SaveFilePanelInProject("Save Asset", "New NoisePerspective Asset", "asset", "Select Location");

                try
                {
                    AssetDatabase.CreateAsset(asset, path);
                }
                catch
                {
                    Debug.LogWarning("Asset Not Saved");
                    return;
                }
            }

            if (NodeManager.Instance.nodes != null && NodeManager.Instance.nodes.Count > 0)
            {
                Node[] nodeArray = NodeManager.Instance.nodes.ToArray();
                Connection[] connectionArray;

                if (ConnectionManager.Instance.connections != null)
                    connectionArray = ConnectionManager.Instance.connections.ToArray();
                else
                    connectionArray = new Connection[0];

                asset.saveData = Data.DataProcessor.WriteToString(nodeArray, connectionArray);

                if (ConnectionManager.Instance.connections != null)
                    asset.connections = Data.DataProcessor.WriteConnections(ConnectionManager.Instance.connections.ToArray());
            }

            asset.useBezier = ConnectionManager.Instance.useBezierConnections;
            asset.globalHeightStrength = NodeManager.Instance.globalHeightStrength;

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            EditorUtility.ClearDirty(asset);
        }

        public void Load(NoiseAsset openedAsset)
        {
            //Allows loading while window already open
            if (NodeManager.Instance.nodes != null)
                Save();
            NodeManager.Instance.Dispose();
            ConnectionManager.Instance.Dispose();

            //Load asset
            asset = openedAsset;
            CreateScene();

            Data.DataProcessor.Read(asset.saveData); //Reads save data and loads data into proper manager lists
            ConnectionManager.Instance.useBezierConnections = asset.useBezier;
            NodeManager.Instance.globalHeightStrength = asset.globalHeightStrength;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            
            mainWindow = null;
            sceneLight = null;

            ConnectionManager.Instance.Dispose();
            NodeManager.Instance.Dispose();
            ContextMenuManager.Instance.Dispose();

            instance = null;
        }
    }
}
