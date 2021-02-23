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
        public class Select : Operator
        {
            public SharpNoise.Modules.Select outputModule = new SharpNoise.Modules.Select();         

            public Select(Scene scene, Vector2 position) : base(scene, position)
            {
                //Can Have Controller
                inputConnectionPoints = new List<ConnectionPoint>(3);
                inputNodes = new Node[3];
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

            public float FallOff
            {
                get { return (float)outputModule.EdgeFalloff; }
                set
                {
                    if (value != outputModule.EdgeFalloff)
                    {
                        outputModule.EdgeFalloff = value;
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

                Minimum = data.minimum;
                Maximum = data.maximum;
                FallOff = data.fallOff;
            }

            public override void RefreshName()
            {
                name = "Select - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.Source1 = inputModules[1];
                outputModule.Control = inputModules[2];
                outputModule.LowerBound = Minimum;
                outputModule.UpperBound = Maximum;

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

                //FallOff
                using (var frequencyContainer = new EditorGUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("FallOff", "fieldHeader");
                    FallOff = EditorGUILayout.FloatField(" ", FallOff, "floatField");
                }

                GUILayout.Space(5);
            }
        }
    }
}

