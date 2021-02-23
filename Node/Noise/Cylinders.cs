using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public class Cylinders : Noise
        {
            public SharpNoise.Modules.Cylinders outputModule = new SharpNoise.Modules.Cylinders();

            public Cylinders(Scene scene, Vector2 position) : base(scene, position)
            {
                Refresh();
                SetHeights();
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public float Amplitude
            {
                get { return (float)outputModule.Amplitude; }
                set
                {
                    if (value != outputModule.Amplitude)
                    {
                        outputModule.Amplitude = value;
                        Refresh();
                    }
                }
            }

            public float Frequency
            {
                get { return (float)outputModule.Frequency; }
                set
                {
                    if (value != outputModule.Frequency)
                    {
                        outputModule.Frequency = value;
                        Refresh();
                    }
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                Frequency = data.frequency;
                Amplitude = data.amplitude;
            }

            public override void RefreshName()
            {
                name = "Cylinders - " + id;
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
                    //Frequency
                    using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                    {
                        GUILayout.Label("Frequency", "fieldHeader");
                        Frequency = EditorGUILayout.FloatField(" ", Frequency, "floatField");
                    }

                    GUILayout.Space(5);

                    //Amplitude
                    using (var qualityBG = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        using (var qualityScope = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Amplitude", "fieldHeader");
                            Amplitude = GUILayout.HorizontalSlider(Amplitude, 0, 1, GUI.skin.GetStyle("slider"), GUI.skin.GetStyle("sliderThumb"));
                        }
                    }
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

