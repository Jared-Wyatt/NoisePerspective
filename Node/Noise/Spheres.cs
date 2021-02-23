using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public class Spheres : Noise
        {
            public SharpNoise.Modules.Spheres outputModule = new SharpNoise.Modules.Spheres();

            public Spheres(Scene scene, Vector2 position) : base(scene, position)
            {
                Refresh();
                SetHeights();
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

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
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
                name = "Spheres - " + id;
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
                    using (var verticalScope = new EditorGUILayout.VerticalScope())
                    {
                        //Frequency
                        using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Frequency", "fieldHeader");
                            Frequency = EditorGUILayout.FloatField(" ", Frequency, "floatField");
                        }

                        GUILayout.Space(5);

                        //Amplitude
                        using (var amplitudeScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                        {
                            using (var veritcalScope = new EditorGUILayout.VerticalScope("box"))
                            {
                                GUILayout.Label("Amplitude", "fieldHeader");
                                Amplitude = GUILayout.HorizontalSlider(Amplitude, 0, 1, GUI.skin.GetStyle("slider"), GUI.skin.GetStyle("sliderThumb"));
                            }
                        }

                        GUILayout.Space(5);
                    }
                }
            }
        }
    }
}

