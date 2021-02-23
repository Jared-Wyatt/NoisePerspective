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
        public class ScaleBias : Operator
        {
            public SharpNoise.Modules.ScaleBias outputModule = new SharpNoise.Modules.ScaleBias();

            public ScaleBias(Scene scene, Vector2 position) : base(scene, position)
            {

            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float Scale
            {
                get { return (float)outputModule.Scale; }
                set 
                {
                    if (value != outputModule.Scale)
                    {
                        outputModule.Scale = value;
                        Refresh();
                    } 
                }
            }

            public float Bias
            {
                get { return (float)outputModule.Bias; }
                set 
                {
                    if (value != outputModule.Bias)
                    {
                        outputModule.Bias = value;
                        Refresh();
                    }
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                Scale = data.scaleValue;
                Bias = data.bias;
            }

            public override void RefreshName()
            {
                name = "Scale Bias - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.Scale = Scale;
                outputModule.Bias = Bias;

                base.Refresh();
            }

            public override void DrawWindow()
            {
                base.DrawWindow();

                //Scale
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Scale", "fieldHeader");
                    Scale = EditorGUILayout.FloatField(" ", Scale, "floatField");
                }

                GUILayout.Space(5);

                //Bias
                using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Bias", "fieldHeader");
                    Bias = EditorGUILayout.FloatField(" ", Bias, "floatField");
                }

                GUILayout.Space(5);

            }
        }
    }
}

