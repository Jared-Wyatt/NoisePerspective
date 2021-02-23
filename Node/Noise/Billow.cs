using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public class Billow : Noise
        {
            public SharpNoise.Modules.Billow outputModule = new SharpNoise.Modules.Billow();

            public Billow(Scene scene, Vector2 position) : base(scene, position)
            {
                Seed = Random.Range(0, int.MaxValue);
                Quality = SharpNoise.NoiseQuality.Standard;
                SetHeights();           
            }

            public int Seed
            {
                get { return outputModule.Seed; }
                set
                {
                    if (value != outputModule.Seed)
                    {
                        outputModule.Seed = value;
                        Refresh();
                    }
                }
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

            public float Lacunarity
            {
                get { return (float)outputModule.Lacunarity; }
                set
                {
                    if (value != outputModule.Lacunarity)
                    {
                        outputModule.Lacunarity = value;
                        Refresh();
                    }
                }
            }

            public int OctaveCount
            {
                get { return outputModule.OctaveCount; }
                set
                {
                    if (value != outputModule.OctaveCount)
                    {
                        outputModule.OctaveCount = value;
                        Refresh();
                    }
                }
            }

            public float Persistence
            {
                get { return (float)outputModule.Persistence; }
                set
                {
                    if (value != outputModule.Persistence)
                    {
                        outputModule.Persistence = value;
                        Refresh();
                    }
                }
            }

            public SharpNoise.NoiseQuality Quality
            {
                get { return outputModule.Quality; }
                set
                {
                    if (value != outputModule.Quality)
                    {
                        outputModule.Quality = value;
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
                
                Seed = data.seed;
                Frequency = data.frequency;
                Lacunarity = data.lacunarity;
                OctaveCount = data.octaveCount;
                Persistence = data.persistence;
                Quality = data.quality;
                Amplitude = data.amplitude;
            }

            public override void RefreshName()
            {
                name = "Billow - " + id;
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
                    //Seed
                    using (var seedBG = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        using (var seedScope = new EditorGUILayout.HorizontalScope("box"))
                        {
                            GUILayout.Label("Seed", GUILayout.Width(windowRect.width * 0.16f));
                            Seed = EditorGUILayout.IntField(Seed);
                        }
                    }

                    using (var horizontalScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        //Frequency
                        using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Frequency", "fieldHeader");
                            Frequency = EditorGUILayout.FloatField(" ", Frequency, "floatField");
                        }

                        //Persistence
                        using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Persistence", "fieldHeader");
                            Persistence = EditorGUILayout.FloatField(" ", Persistence, "floatField");
                        }
                    }

                    using (var horizontalScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        //Lacunarity
                        using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Lacunarity", "fieldHeader");
                            Lacunarity = EditorGUILayout.FloatField(" ", Lacunarity, "floatField");
                        }

                        //Octave
                        using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Octave", "fieldHeader");
                            OctaveCount = EditorGUILayout.IntField(" ", OctaveCount, "floatField");
                        }
                    }

                    //Amplitude
                    using (var amplitudeScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        using (var veritcalScope = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Amplitude", "fieldHeader");
                            Amplitude = GUILayout.HorizontalSlider(Amplitude, 0, 1, GUI.skin.GetStyle("slider"), GUI.skin.GetStyle("sliderThumb"));
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
}

