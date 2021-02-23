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
        public class Exponent : Operator
        {
            public SharpNoise.Modules.Exponent outputModule = new SharpNoise.Modules.Exponent();

            public Exponent(Scene scene, Vector2 position) : base(scene, position)
            {

            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float Value
            {
                get { return (float)outputModule.Exp; }
                set
                {
                    if (value != outputModule.Exp)
                    {
                        outputModule.Exp = value;
                        Refresh();
                    }
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                Value = data.value;
            }

            public override void RefreshName()
            {
                name = "Exponent - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.Exp = Value;

                base.Refresh();
            }

            public override void DrawWindow()
            {
                base.DrawWindow();

                //Value
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Value", "fieldHeader");
                    Value = EditorGUILayout.FloatField(" ", Value, "floatField");
                }

                GUILayout.Space(5);
            }
        }
    }
}

