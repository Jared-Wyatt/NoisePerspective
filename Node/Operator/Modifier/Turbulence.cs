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
        public class Turbulence : Operator
        {
            public SharpNoise.Modules.Turbulence outputModule = new SharpNoise.Modules.Turbulence();  ///=======================================CAN ACCEPT PERLIN.X, PERLIN.Y, AND PERLIN.Z

            public Turbulence(Scene scene, Vector2 position) : base(scene, position)
            {
                Seed = UnityEngine.Random.Range(0, int.MaxValue);
                Roughness = outputModule.Roughness;
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
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

            public float Power
            {
                get { return (float)outputModule.Power; }
                set
                {
                    if (value != outputModule.Power)
                    {
                        outputModule.Power = value;
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

            public int Roughness
            {
                get { return outputModule.Roughness; }
                set
                {
                    if (value != outputModule.Roughness)
                    {
                        outputModule.Roughness = value;
                        Refresh();
                    }
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                Frequency = data.frequency;
                Power = data.power;
                Roughness = data.roughness;
            }

            public override void RefreshName()
            {
                name = "Turbulence - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.Seed = Seed;
                outputModule.Frequency = Frequency;
                outputModule.Power = Power;
                outputModule.Roughness = Roughness;

                base.Refresh();
            }

            public override void DrawWindow()
            {
                base.DrawWindow();

                using (var verticalScope = new EditorGUILayout.VerticalScope())
                {
                    //Seed
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope("box"))
                    {
                        GUILayout.Label("Seed");
                        Seed = EditorGUILayout.IntField(Seed);
                    }

                    GUILayout.Space(5);

                    //Frequency
                    using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                    {
                        GUILayout.Label("Frequency", "fieldHeader");
                        Frequency = EditorGUILayout.FloatField(" ", Frequency, "floatField");
                    }

                    GUILayout.Space(5);

                    //Power
                    using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                    {
                        GUILayout.Label("Power", "fieldHeader");
                        Power = EditorGUILayout.FloatField(" ", Power, "floatField");
                    }

                    GUILayout.Space(5);

                    //Roughness
                    using (var displacementContainer = new EditorGUILayout.VerticalScope("box"))
                    {
                        GUILayout.Label("Roughness", "fieldHeader");
                        Roughness = EditorGUILayout.IntField(" ", Roughness, "floatField");
                    }

                    GUILayout.Space(5);
                }
            }
        }
    }
}

