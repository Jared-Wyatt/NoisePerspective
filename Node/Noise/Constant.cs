using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public class Constant : Noise
        {
            public SharpNoise.Modules.Constant outputModule = new SharpNoise.Modules.Constant();

            public Constant(Scene scene, Vector2 position) : base(scene, position)
            {
                Refresh();
                SetHeights();
            }
            
            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float Value
            {
                get { return (float)outputModule.ConstantValue; }
                set
                {
                    if (value != outputModule.ConstantValue)
                    {
                        outputModule.ConstantValue = value;
                        Refresh();
                    }
                }
            }

            public override void RefreshName()
            {
                name = "Constant - " + id;
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                Value = data.value;
            }

            public override void DrawWindow()
            {
                base.DrawWindow();

                float fadeGroupValue = 1;

                //Draw toggle above when values hiden
                if (showValues == false)
                {
                    if (showValues = GUILayout.Toggle(showValues, "", "fadeToggle2"))
                        fadeGroupValue = 1;
                    else
                        fadeGroupValue = 0.00001f;
                }

                using (var fadeScope = new EditorGUILayout.FadeGroupScope(fadeGroupValue))
                {
                    //Value
                    using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                    {
                        GUILayout.Label("Value", "fieldHeader");
                        Value = EditorGUILayout.FloatField(" ", Value, "floatField");
                    }

                    GUILayout.Space(5);
                }

                //Draw toggle on bottom when values visible
                if (showValues)
                {
                    if (showValues = GUILayout.Toggle(showValues, "", "fadeToggle"))
                        fadeGroupValue = 1;
                    else
                        fadeGroupValue = 0.00001f;
                }
            }
        }
    }
}

