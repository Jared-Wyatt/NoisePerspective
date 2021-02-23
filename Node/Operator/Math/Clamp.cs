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
        public class Clamp : Operator
        {
            public SharpNoise.Modules.Clamp outputModule = new SharpNoise.Modules.Clamp();

            public Clamp(Scene scene, Vector2 position) : base(scene, position)
            {

            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float Minimum
            {
                get { return (float)outputModule.LowerBound; }
                set
                {
                    if (value != outputModule.LowerBound)
                    {
                        outputModule.LowerBound = value;
                        Refresh();
                    }
                }
            }

            public float Maximum
            {
                get { return (float)outputModule.UpperBound; }
                set
                {
                    if (value != outputModule.UpperBound)
                    {
                        outputModule.UpperBound = value;
                        Refresh();
                    }
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                Maximum = data.maximum;
                Minimum = data.minimum;
            }

            public override void RefreshName()
            {
                name = "Clamp - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.UpperBound = Maximum;
                outputModule.LowerBound = Minimum;

                base.Refresh();
            }

            public override void DrawWindow()
            {
                base.DrawWindow();

                //Minimum
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Minimum", "fieldHeader");
                    Minimum = EditorGUILayout.FloatField(" ", Minimum, "floatField");
                }

                GUILayout.Space(5);

                //Maximum
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Maximum", "fieldHeader");
                    Maximum = EditorGUILayout.FloatField(" ", Maximum, "floatField");
                }

                GUILayout.Space(5);
            }
        }
    }
}

