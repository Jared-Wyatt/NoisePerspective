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
        public class Rotate : Operator
        {
            public SharpNoise.Modules.RotatePoint outputModule = new SharpNoise.Modules.RotatePoint();

            public Rotate(Scene scene, Vector2 position) : base(scene, position)
            {

            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float X
            {
                get { return (float)outputModule.XAngle; }
                set
                {
                    if (value != outputModule.XAngle)
                    {
                        outputModule.XAngle = value;
                        Refresh();
                    }
                }
            }

            public float Y
            {
                get { return (float)outputModule.YAngle; }
                set
                {
                    if (value != outputModule.YAngle)
                    {
                        outputModule.YAngle = value;
                        Refresh();
                    }
                }
            }

            public float Z
            {
                get { return (float)outputModule.ZAngle; }
                set
                {
                    if (value != outputModule.ZAngle)
                    {
                        outputModule.ZAngle = value;
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
                name = "Rotate - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.SetAngles(X, Y, Z);

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

