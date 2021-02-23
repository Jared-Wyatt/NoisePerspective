using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Scale : Operator
        {
            public SharpNoise.Modules.ScalePoint outputModule = new SharpNoise.Modules.ScalePoint();
                
            public Scale(Scene scene, Vector2 position) : base(scene, position)
            {
                
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float X
            {
                get { return (float)outputModule.XScale; }
                set
                {
                    if (value != outputModule.XScale)
                    {
                        outputModule.XScale = value;
                        Refresh();
                    }
                }
            }

            public float Y
            {
                get { return (float)outputModule.YScale; }
                set
                {
                    if (value != outputModule.YScale)
                    {
                        outputModule.YScale = value;
                        Refresh();
                    }
                }
            }

            public float Z
            {
                get { return (float)outputModule.ZScale; }
                set
                {
                    if (value != outputModule.ZScale)
                    {
                        outputModule.ZScale = value;
                        Refresh();
                    }
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                X = data.x;
                Y = data.y;
                Z = data.z;
            }

            public override void RefreshName()
            {
                name = "Scale - " + id;
            }

            public override void OnInputConnected(Node connectedNode, ConnectionPoint inputPoint)
            {
                base.OnInputConnected(connectedNode, inputPoint);

                for (int i = 0; i < inputConnectionPoints.Count; i++)
                {
                    if (inputPoint == inputConnectionPoints[i])
                        inputNodes[i] = connectedNode;
                }

                Refresh();
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.SetScale(X, Y, Z);

                base.Refresh();
            }

            public override void DrawWindow()
            {
                base.DrawWindow();

                //X
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("X", "fieldHeader");
                    X = EditorGUILayout.FloatField(" ", X, "floatField");
                }

                GUILayout.Space(5);

                //Y
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Y", "fieldHeader");
                    Y = EditorGUILayout.FloatField(" ", Y, "floatField");
                }

                GUILayout.Space(5);

                //Z
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Z", "fieldHeader");
                    Z = EditorGUILayout.FloatField(" ", Z, "floatField");
                }

                GUILayout.Space(5);
            }
        }
    }
}

