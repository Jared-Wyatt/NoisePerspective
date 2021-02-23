using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{

    public abstract class Node
    {
        public string name;
        public int id;
        public Scene Scene { get; set; }
        public RenderTexture renderTexture;

        public List<ConnectionPoint> outputConnectionPoints = new List<ConnectionPoint>(1);
        public List<ConnectionPoint> inputConnectionPoints = new List<ConnectionPoint>(0);
        public Node[] inputNodes = new Node[0];

        public Rect windowRect = new Rect(100, 100, 120, 120);
        public Rect overlayWindowRect;
        Rect terrainRect;
        public Camera camera;
        public GameObject terrain;

        //Terrain Settings
        readonly int terrainSegments = 2;
        readonly float segmentSize = 500;
        readonly float terrainHeight = 600;
        readonly int heightResolution = 33;
        readonly int detailResolution = 32;
        public float heightStrength = 1;
        TerrainData[,] terrainDatas;

        public virtual SharpNoise.Modules.Module OutputModule { get; }

        //Used to reset camera
        Vector3 camStartPosition;
        Vector3 camParentStartPosition;
        Quaternion camStartRotation;
        Quaternion camParentStartRotation;

        public bool isPerspective;
        public bool showValues = true;
        public bool needsRender; //Only renders the terrain on the frame that it was updated on

        public Node(Scene scene, Vector2 position)
        {
            id = NodeManager.Instance.nextID++;
            RefreshName();
            Scene = scene;

            renderTexture = new RenderTexture((int)windowRect.width, (int)windowRect.height, 24, RenderTextureFormat.ARGB32);
            renderTexture.Create();

            CreateTerrain(scene);
            CreateCamera(scene);

            windowRect.position = position;
        }

        public virtual void RefreshName()
        {

        }

        public virtual void SetData(Data.NodeData data)
        {
            name = data.name;
            id = data.id;
            windowRect.position = data.windowPosition;
            showValues = data.showValues;

            //Set view
            if (data.isPerspective)
                TogglePerspective();
        }

        public void BuildConnectionPoints()
        {
            //Input Points
            for (int i = 0; i < inputConnectionPoints.Capacity; i++)
                inputConnectionPoints.Add(new ConnectionPoint(this, inputConnectionPoints.Count, ConnectionPointType.Input, ConnectionManager.Instance.OnClickInputPoint));

            //Output Points
            for (int i = 0; i < outputConnectionPoints.Capacity; i++)
                outputConnectionPoints.Add(new ConnectionPoint(this, outputConnectionPoints.Count, ConnectionPointType.Output, ConnectionManager.Instance.OnClickOutputPoint));
        }

        //Terrain
        public virtual void CreateTerrain(Scene scene)
        {
            terrain = new GameObject(name + ": Terrains");
            EditorSceneManager.MoveGameObjectToScene(terrain, scene);

            terrainDatas = new TerrainData[terrainSegments, terrainSegments];

            //Create Terrain Segments
            for (int x = 0; x < terrainSegments; x++)
            {
                for (int y = 0; y < terrainSegments; y++)
                {
                    TerrainData terrainData = new TerrainData();

                    terrainData.SetDetailResolution(detailResolution, 32);
                    terrainData.heightmapResolution = heightResolution;
                    terrainData.size = new Vector3(segmentSize, terrainHeight, segmentSize);
                    terrainData.name = "TerrainData-" + x.ToString("D3") + "-" + y.ToString("D3");

                    GameObject terrainSegment = Terrain.CreateTerrainGameObject(terrainData);
                    EditorSceneManager.MoveGameObjectToScene(terrainSegment, scene);
                    terrainSegment.name = "Terrain-" + x.ToString("D3") + "-" + y.ToString("D3");
                    terrainSegment.transform.parent = terrain.transform;
                    terrainSegment.transform.localPosition = new Vector3(x * segmentSize, 0, y * segmentSize);
                    terrainDatas[x, y] = terrainData;

                    //Destroy collider as not needed and can improve performance
                    UnityEngine.Object.DestroyImmediate(terrainSegment.GetComponent<TerrainCollider>());
                }
            }

            Terrain.SetConnectivityDirty();
            terrain.transform.position = new Vector3((-segmentSize * terrainSegments) / 2, 0, (-segmentSize * terrainSegments) / 2); //Center Terrain

            //Set active during render and disabled again so each camera only renders its target terrain
            terrain.SetActive(false);
        }

        //Camera
        public virtual void CreateCamera(Scene scene)
        {
            //Camera Parent
            GameObject camParent = new GameObject();
            EditorSceneManager.MoveGameObjectToScene(camParent, scene);

            camParent.hideFlags = HideFlags.HideAndDontSave;

            camParent.transform.position = Vector3.zero;
            camParent.transform.position += Vector3.up * terrainSegments * segmentSize * 0.9f;
            camParent.transform.rotation = Quaternion.Euler(90, 0, 0);
            camParent.transform.RotateAround(Vector3.zero, camParent.transform.right, -90);

            GameObject camGO = new GameObject();
            camera = camGO.AddComponent<Camera>();

            EditorSceneManager.MoveGameObjectToScene(camGO, scene);
            camGO.transform.parent = camParent.transform;
            camera.scene = scene;
            camera.clearFlags = CameraClearFlags.Color;
            camera.backgroundColor = Color.black;
            camGO.hideFlags = HideFlags.HideAndDontSave;

            //Move and rotate Camera
            camera.transform.localPosition = Vector3.zero;
            camera.transform.rotation = camParent.transform.rotation;
            camera.transform.RotateAround(Vector3.zero, camParent.transform.right, 90);

            camera.targetTexture = renderTexture;
            camera.farClipPlane = 10000;
            camera.orthographicSize = terrainSegments * segmentSize / 1.9f;

            //Set starting transforms
            camParentStartPosition = camParent.transform.position;
            camParentStartRotation = camParent.transform.rotation;
            camStartPosition = camera.transform.position;
            camStartRotation = camera.transform.rotation;

            camera.orthographic = true; //starting 2d top down view
        }

        public virtual void Resize(Vector2 size)
        {
            //windowRect.size = size;
            //camera.targetTexture = new RenderTexture((int)size.x, (int)size.y, 24, RenderTextureFormat.ARGB32);
        }

        public virtual void SetHeights()
        {
            float[,][,] combinedHeights = new float[terrainSegments, terrainSegments][,];

            //Initialize matrix of arrays for heightmaps
            for (int segmentX = 0; segmentX < combinedHeights.GetLength(0); segmentX++)
            {
                for (int segmentY = 0; segmentY < combinedHeights.GetLength(1); segmentY++)
                {
                    float[,] heights = new float[heightResolution, heightResolution];

                    //Fill heightmap for segment
                    for (int x = 0; x < heightResolution; x++)
                    {
                        for (int y = 0; y < heightResolution; y++)
                        {
                            //Get height positions and subtract segment iteration to align seams
                            float heightX = x + (segmentX * heightResolution) - segmentX;
                            float heightY = y + (segmentY * heightResolution) - segmentY;


                            //Normalize to height resolution (0, 1)
                            heightX /= heightResolution;
                            heightY /= heightResolution;
                            //heightY = (heightY - -1) / (1 - -1); //Normalize to (-1, 1)
                            //heightY = (heightY - -1) / (1 - -1); //Normalize to (-1, 1)

                            //Read in heights from heightmap
                            heights[y, x] = (float)OutputModule.GetValue(heightX, heightY, 0) * heightStrength; //heights x and y value need to be swapped for terrain to line up (loop order issue?)
                        }
                    }

                    combinedHeights[segmentX, segmentY] = heights; //Assign heights to segment
                }
            }

            Terrain.SetConnectivityDirty();

            //Set Heights
            for (int x = 0; x < terrainSegments; x++)
            {
                for (int y = 0; y < terrainSegments; y++)
                {
                    terrainDatas[x, y].SetHeights(0, 0, combinedHeights[x, y]);
                }
            }
        }

        public virtual void TogglePerspective()
        {
            if (isPerspective)
            {
                //Reset transforms
                camera.transform.parent.position = camParentStartPosition;
                camera.transform.parent.rotation = camParentStartRotation;
                camera.transform.position = camStartPosition;
                camera.transform.rotation = camStartRotation;
                isPerspective = false;
                camera.orthographic = true;
            }
            else
            {
                camera.orthographic = false;
                camera.transform.RotateAround(Vector3.zero, camera.transform.right, -50); //Angle camera down
                camera.transform.position += camera.transform.forward * -90; //zoom out
                isPerspective = true;
            }

            needsRender = true;
        }

        public virtual void OnInputConnected(Node connectedNode, ConnectionPoint inputPoint)
        {

        }

        public virtual List<Connection> GetConnections()
        {
            List<Connection> connections = new List<Connection>();

            //Output
            foreach (ConnectionPoint connectionPoint in outputConnectionPoints)
            {
                if (connectionPoint.connection != null)
                    connections.Add(connectionPoint.connection);
            }

            //Input
            foreach (ConnectionPoint connectionPoint in inputConnectionPoints)
            {
                if (connectionPoint.connection != null)
                    connections.Add(connectionPoint.connection);
            }

            return connections;
        }

        public virtual void Refresh()
        {
            SetHeights();

            //Update nodes receiving output from this node
            foreach (ConnectionPoint outputConnection in outputConnectionPoints)
            {
                if (outputConnection.connection != null)
                {
                    Node connectedNode = outputConnection.connection.inPoint.node;

                    if (connectedNode != null)
                        connectedNode.Refresh();
                }
            }

            needsRender = true;
        }

        public virtual void CleanUp()
        {
            if (camera != null)
            {
                UnityEngine.Object.DestroyImmediate(camera);
            }
        }

        public virtual void DrawWindow()
        {
            Rect dragArea;

            using (var verticalScope = new EditorGUILayout.VerticalScope())
            {
                //Title Bar
                using (var horizontalScope = new EditorGUILayout.HorizontalScope("titleBar"))
                {
                    //Draggable Area
                    dragArea = verticalScope.rect;
                    dragArea.height = 20;

                    //Title
                    GUILayout.Label(name, "titleBarLabel");

                    //Close Button
                    if (GUILayout.Button("X", "closeButton")) { NodeManager.Instance.nodesToRemove.Add(this); }
                }

                //Temp Buttons
                using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    
                    if (GUILayout.Button("3d", "toolButton")) { TogglePerspective(); }
                    
                    GUILayout.FlexibleSpace();
                    
                    GUILayout.Button("1", "toolButton");
                    GUILayout.Button("2", "toolButton");
                    GUILayout.Button("3", "toolButton");
                    GUILayout.Button("4", "toolButton");
                   
                    GUILayout.FlexibleSpace();
                }

                //Render Image
                using (var renderImageGroup = new EditorGUILayout.VerticalScope("fieldGroup"))
                {
                    GUILayout.Box(renderTexture);
                    terrainRect = GUILayoutUtility.GetLastRect();
                }

                if (isPerspective)
                    ControlCamera();
            }

            GUI.DragWindow(dragArea);
        }

        public virtual void DrawOutputPoints()
        {
            GUILayout.Space(25);

            //Output ConnectionPoints
            for (int i = 0; i < outputConnectionPoints.Count; i++)
            {
                outputConnectionPoints[i].Draw();
            }
        }

        public virtual void DrawInputPoints()
        {
            GUILayout.Space(25);

            //Input ConnectionPoints
            for (int i = 0; i < inputConnectionPoints.Count; i++)
            {
                inputConnectionPoints[i].Draw();
            }
        }

        public void ControlCamera()
        {
            if (terrainRect.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.MouseDrag)
                {
                    //Rotate
                    if (Event.current.button == 0)
                    {
                        camera.transform.RotateAround(Vector3.zero, camera.transform.right, Event.current.delta.y);
                        camera.transform.parent.RotateAround(Vector3.zero, camera.transform.up, Event.current.delta.x);
                        needsRender = true;
                    }

                    //Pan
                    if (Event.current.button == 2)
                    {
                        camera.transform.parent.position += camera.transform.right * -Event.current.delta.x * 5;
                        camera.transform.parent.position += camera.transform.up * Event.current.delta.y * 5;
                        needsRender = true;
                    }

                    Event.current.Use();
                }
            }

            //Zoom
            if (Event.current.type == EventType.ScrollWheel)
            {
                camera.transform.position += camera.transform.forward * -Event.current.delta.y * 5;
                needsRender = true;
                Event.current.Use();
            }
        }


        public virtual void DrawOverlayWindow()
        {
            //Used for nodes that needs to draw GUI items with GUILayout items such as graph nodes
        }
    }
}
