using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Curve : GraphNode
        {
            public SharpNoise.Modules.Curve outputModule = new SharpNoise.Modules.Curve();

            public Curve(Scene scene, Vector2 position) : base(scene, position)
            {
                
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public override void RefreshName()
            {
                name = "Curve - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();
                outputModule.Source0 = inputModules[0];

                outputModule.ControlPoints = GraphPoint.ToVector2List(controlPoints);

                base.Refresh();
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
                    //Place holder for overlayWindow (graph) so GUI buttons work in GUILayout
                    GUILayout.Box("", "fieldGroup", GUILayout.Width(overlayWindowRect.width), GUILayout.Height(overlayWindowRect.height));

                    if (Event.current.type == EventType.Repaint)
                    {
                        overlayWindowRect = GUILayoutUtility.GetLastRect();
                        overlayWindowRect.position += windowRect.position;
                    }

                    using (var horizontalScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        using (var controlPointContainer = new EditorGUILayout.VerticalScope("box"))
                        {
                            GUILayout.Label("ControlPoint", "fieldHeader");

                            using (var pointScope = new EditorGUILayout.HorizontalScope())
                            {
                                GetSelectedPoint();

                                if (selectedPoint != -1)
                                {
                                    //Highlight Selected point
                                    selectionRect = ControlPoints[selectedPoint].Rect;

                                    //Coordinate values
                                    float valueX = ControlPoints[selectedPoint].Coordinate.x;
                                    valueX = EditorGUILayout.FloatField(" ", valueX, "floatField");

                                    float valueY = ControlPoints[selectedPoint].Coordinate.y;
                                    valueY = EditorGUILayout.FloatField(" ", valueY, "floatField");

                                    ControlPoints[selectedPoint].Coordinate = new Vector2(valueX, valueY);

                                    //Refresh on changed value
                                    if (ControlPoints[selectedPoint].Coordinate != outputModule.ControlPoints[selectedPoint])
                                        Refresh();
                                }
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
}

