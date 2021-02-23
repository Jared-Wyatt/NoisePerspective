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
        public class Terrace : GraphNode
        {
            public SharpNoise.Modules.Terrace outputModule = new SharpNoise.Modules.Terrace();

            public Terrace(Scene scene, Vector2 position) : base(scene, position)
            {

            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public bool IsInverted
            {
                get { return outputModule.InvertTerraces; }
                set
                {
                    if (value != outputModule.InvertTerraces)
                    {
                        outputModule.InvertTerraces = value;
                        Refresh();
                    }
                }
            }

            public override void RefreshName()
            {
                name = "Terrace - " + id;
            }

            public override void Refresh()
            {
                outputModule.InvertTerraces = IsInverted;

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
                        fadeGroupValue = 0.00001f; //0 Causes controls to still be visible without background
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

                    //Options
                    using (var horizontalScope = new EditorGUILayout.HorizontalScope("fieldGroup"))
                    {
                        using (var optionsContainer = new EditorGUILayout.HorizontalScope("box"))
                        {
                            IsInverted = GUILayout.Toggle(IsInverted, "Invert");
                        }
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

                                    ControlPoints[selectedPoint].Coordinate = new Vector2(valueX, valueX);

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

            protected override void DrawGraphLines()
            {
                Vector2 start; //line start
                Vector2 end; //line end

                //Used to draw line in exact center of point
                Vector2 pointOffset = new Vector2(1, 2);

                //End points
                Vector2 graphStart = new Vector2(0, overlayWindowRect.height);
                Vector2 graphEnd = new Vector2(overlayWindowRect.width, 0);

                //Draw from graph start    
                start = graphStart;
                end = ControlPoints[0].Rect.center + pointOffset;        
                DrawBezier(start, end);

                //Draw Lines
                for (int i = 0; i < ControlPoints.Count - 1; i++)
                {
                    start = ControlPoints[i].Rect.center + pointOffset;
                    end = ControlPoints[i + 1].Rect.center + pointOffset;
                    DrawBezier(start, end);
                }

                //Draw to graph end
                start = ControlPoints[ControlPoints.Count - 1].Rect.center;
                end = graphEnd;
                DrawBezier(start, end);
            }

            void DrawBezier(Vector2 start, Vector2 end)
            {
                Vector2 startTangent = start + (Vector2.right * 30);
                Vector2 endTangent = end - (Vector2.right * 30);

                Handles.DrawBezier(start, end, startTangent, endTangent, Color.white, null, 2);
            }
        }
    }
}

