using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public class Cell : Noise
        {
            public SharpNoise.Modules.Cell outputModule = new SharpNoise.Modules.Cell();

            //Options for dropdown
            readonly string[] cellTypeOptions = System.Enum.GetNames(typeof(SharpNoise.Modules.Cell.CellType));
            int selectedType = 0;

            public Cell(Scene scene, Vector2 position) : base(scene, position)
            {
                Seed = Random.Range(0, int.MaxValue);

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

            public bool UseDistance
            {
                get { return outputModule.EnableDistance; }
                set 
                {
                    if (value != outputModule.EnableDistance)
                    {
                        outputModule.EnableDistance = value;
                        Refresh();
                    }
                }
            }

            public int Coefficient
            {
                get { return outputModule.MinkowskyCoefficient; }
                set
                {
                    if (value != outputModule.MinkowskyCoefficient)
                    {
                        outputModule.MinkowskyCoefficient = value;
                        Refresh();
                    }
                }
            }

            public SharpNoise.Modules.Cell.CellType CellType
            {
                get { return outputModule.Type; }
                set
                {
                    if (value != outputModule.Type)
                    {
                        outputModule.Type = value;
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

            public float Displacement
            {
                get { return (float)outputModule.Displacement; }
                set
                {
                    if (value != outputModule.Displacement)
                    {
                        outputModule.Displacement = value;
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
                Displacement = data.displacement;
                UseDistance = data.useDistance;
                CellType = data.cellType;
                Coefficient = data.Coefficient;
                Amplitude = data.amplitude;
            }

            public override void RefreshName()
            {
                name = "Cells - " + id;
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
                        //UseDistance
                        using (var UseDistanceContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Distance", "fieldHeader", GUILayout.Width(40));
                            UseDistance = EditorGUILayout.Toggle(" ", UseDistance, GUILayout.Width(40));
                        }

                        //CellType
                        using (var CellTypeContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Cell Type", "fieldHeader");

                            selectedType = EditorGUILayout.Popup((int)CellType, cellTypeOptions);
                            CellType = (SharpNoise.Modules.Cell.CellType)selectedType;
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

                        //Displacement
                        using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("Displacement", "fieldHeader");
                            Displacement = EditorGUILayout.FloatField(" ", Displacement, "floatField");
                        }
                    }

                    //Minkowsky Coefficient
                    if (CellType == SharpNoise.Modules.Cell.CellType.Minkowsky)
                    {
                        using (var horizontalScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                        {
                            using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                            {
                                GUILayout.Label("Coefficient", "fieldHeader");
                                Coefficient = EditorGUILayout.IntField(" ", Coefficient, "floatField");
                            }
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

