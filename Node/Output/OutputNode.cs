using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
	namespace Output
	{
		public abstract class OutputNode : Node
		{
            public List<Node> outputChain;

            public OutputNode(Scene scene, Vector2 position) : base(scene, position)
			{
				inputConnectionPoints = new List<ConnectionPoint>(1);
				inputNodes = new Node[1];

				outputConnectionPoints = new List<ConnectionPoint>(0); //no Outputpoints
			}

			public override SharpNoise.Modules.Module OutputModule
            {
				get { return inputNodes[0].OutputModule; }
			}

			public override void RefreshName()
			{
				name = "Output - " + id;
			}

            public override void DrawWindow()
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
                }

                GUI.DragWindow(dragArea);

                //Options
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Add Options Here", "fieldHeader");
                    EditorGUILayout.TextField(" ", "test", "floatField"); //Placeholder field (REMOVE)
                }
            }

            //Get all nodes connected to output node
            protected List<Node> GetOutputChain()
            {
                outputChain = new List<Node>();

                if (inputNodes[0] != null)
                {
                    outputChain.Add(inputNodes[0]);
                    bool inputNodesLeft = true;

                    //Loop through source modules till no new ones are found
                    while (inputNodesLeft)
                    {
                        inputNodesLeft = false;

                        for (int i = 0; i < outputChain.Count; i++)
                        {
                            if (outputChain[i] != null)
                            {
                                foreach (Node inputNode in outputChain[i].inputNodes)
                                {
                                    if (outputChain.Contains(inputNode) == false)
                                    {
                                        outputChain.Add(inputNode);
                                        inputNodesLeft = true;
                                    }
                                }
                            }
                        }
                    }
                }
            
                return outputChain;
            }

            public override void OnInputConnected(Node connectedNode, ConnectionPoint inputPoint)
            {
                inputNodes[0] = connectedNode;
                Manager.Instance.asset.terrainOutput = Data.DataProcessor.WriteNodes(GetOutputChain().ToArray());
                Refresh();
            }

            public override void CreateTerrain(Scene scene)
            {
				//Does Not Need Terrain
            }

			public override void CreateCamera(Scene scene)
            {
				//Does not need camera
            }

            public override void SetHeights()
            {
                //Does not need
            }

            public override void DrawOutputPoints()
            {
                //Does not need
            }

            public override void CleanUp()
            {
                //Does not need
            }

            public override void TogglePerspective()
            {
                //Does not need
            }

            public override void Resize(Vector2 size)
            {
                //Does not need
            }

            public override void Refresh()
			{
                //Does Not need
			}
		}
	}
}
