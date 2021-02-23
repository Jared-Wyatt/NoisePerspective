using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace NoisePerspective
{
    public sealed class NodeManager : IDisposable
    {
        public MainWindow mainWindow;

        public int nextID = 0;

        public List<Node> nodes;
        public List<Node> nodesToRemove = new List<Node>();

        public float globalHeightStrength = 1;

        private NodeManager()
        {

        }

        private static NodeManager instance = null;
        public static NodeManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new NodeManager();

                return instance;
            }
        }

        public void DrawNodeWindows()
        {
            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    node.windowRect = GUILayout.Window(node.id, node.windowRect, NodeWindow, "", GUIStyle.none);

                    if (node.overlayWindowRect != null)
                        GUI.Window(-node.id - 1, node.overlayWindowRect, OverlayWindow, "", GUIStyle.none); //Invert node and ensure not 0 idea to keep track of node id but not conflict with main window
                }
            }
        }

        void NodeWindow(int windowID)
        {
            using (var horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                //Input Points
                using (var verticalScope = new EditorGUILayout.VerticalScope())
                {
                    nodes[windowID].DrawInputPoints();
                }

                //Window
                using (var verticalScope = new EditorGUILayout.VerticalScope("window"))
                {
                    nodes[windowID].DrawWindow();
                }

                //Output Points
                using (var verticalScope = new EditorGUILayout.VerticalScope())
                {
                    nodes[windowID].DrawOutputPoints();
                }
            }
        }

        //Used to create controls using GUI over node window preventing unclickable from conflict with GUILayout
        void OverlayWindow(int windowID)
        {
            NodeManager.Instance.nodes[-windowID - 1].DrawOverlayWindow(); //overlay ids are negative and must be set positive
        }

        //Create Nodes From Right Click Menu
        public void InstantiateCreateGenericNode(object type)
        {
            if (nodes == null)
                nodes = new List<Node>();

            //Create instance of selected node type
            Node node = (Node)Activator.CreateInstance((Type)type, Manager.Instance.scene, EventManager.Instance.mousePosition);
            node.BuildConnectionPoints();

            nodes.Add(node);
        }

        //Used when loading nodes from saved data
        public void InstantiateGenericNode(Data.NodeData nodeData)
        {
            if (nodes == null)
                nodes = new List<Node>();

            //Create instance of selected node type
            Node node = (Node)Activator.CreateInstance(Type.GetType(nodeData.type), Manager.Instance.scene, EventManager.Instance.mousePosition);
            node.SetData(nodeData);
            node.BuildConnectionPoints();

            nodes.Add(node);
        }

        public SharpNoise.Modules.Module InstantiateGenericModule(Data.NodeData nodeData)
        {
            //Create instance of selected node type
            SharpNoise.Modules.Module module = (SharpNoise.Modules.Module)Activator.CreateInstance(Type.GetType(nodeData.moduleType));
            module.SetData(nodeData);

            return module;
        }

        public void RenderCameras()
        {
            //Render Cameras
            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    if (node.camera != null)
                    {
                        if (node.needsRender)
                        {
                            //Set active during render and disable again so each camera only renders its target terrain
                            node.camera.clearFlags = CameraClearFlags.Color;
                            node.terrain.SetActive(true);
                            node.camera.Render();
                            node.terrain.SetActive(false);
                            node.needsRender = false;
                            node.camera.clearFlags = CameraClearFlags.Nothing;
                        }
                    } 
                }
            }
        }

        public void DrawControls()
        {

            using (var verticalScope = new EditorGUILayout.VerticalScope())
            {
                GUILayout.Label("Global Height Strength", "fieldHeader", GUILayout.Width(100));
                globalHeightStrength = GUILayout.HorizontalSlider(globalHeightStrength, 0, 1, GUI.skin.GetStyle("slider"), GUI.skin.GetStyle("sliderThumb"), GUILayout.Width(100));
            }
            
            if (nodes != null)
            {
                foreach (Node node in nodes)
                {
                    if (node.heightStrength != globalHeightStrength)
                    {
                        node.heightStrength = globalHeightStrength;
                        node.Refresh();
                    }
                }
            }
        }

        //Called at end of OnGUI on MainWindow
        public void CleanUp()
        {
            //Remove Nodes that were marked for deletion
            if (nodesToRemove != null)
            {
                foreach (Node removedNode in nodesToRemove)
                {
                    foreach (Node node in nodes)
                    {
                        if (node.id > removedNode.id)
                        {
                            //Realign Ids
                            node.id -= 1;

                            node.RefreshName();
                        }
                    }

                    //Remove Connection
                    if (removedNode.GetConnections() != null)
                    {
                        foreach (Connection connection in removedNode.GetConnections())
                        {
                            if (ConnectionManager.Instance.connections.Contains(connection))
                                ConnectionManager.Instance.RemoveConnection(connection);
                        }
                    }

                    //make next align with node ids
                    nextID -= 1;

                    nodes.Remove(removedNode);
                    removedNode.name = null;
                }

                nodesToRemove.Clear();
            }
        }

        public void Dispose()
        {
            nodes = null;
            nodesToRemove = null;
            instance = null;

            GC.SuppressFinalize(this);
        }
    }
}
