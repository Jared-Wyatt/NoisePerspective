using NoisePerspective.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public class Checker : Noise
        {
            public SharpNoise.Modules.Checkerboard outputModule = new SharpNoise.Modules.Checkerboard();

            public Checker(Scene scene, Vector2 position) : base(scene, position)
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

            public override void RefreshName()
            {
                name = "Checker - " + id;
            }

            public override void SetData(NodeData data)
            {
                base.SetData(data);

                Amplitude = data.amplitude;
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

                    //Amplitude
                    using (var qualityBG = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        using (var qualityScope = new EditorGUILayout.VerticalScope("box"))
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

