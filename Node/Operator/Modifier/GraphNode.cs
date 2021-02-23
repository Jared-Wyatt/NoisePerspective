using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public abstract class GraphNode : Operator
        {
            protected int selectedPoint = -1;
            protected Rect selectionRect = new Rect();

            public GraphNode(Scene scene, Vector2 position) : base(scene, position)
            {
                overlayWindowRect = new Rect(0, 0, 124, 124);

                ControlPoints = new List<GraphPoint>()
                {
                    new GraphPoint(new Vector2(-0.5f, -0.5f), overlayWindowRect),
                    new GraphPoint(new Vector2(0.5f, 0.5f), overlayWindowRect),
                };

                //Set initial points on graph
                foreach (GraphPoint point in ControlPoints)
                    point.CalculateGraphPoint();

                Refresh();
            }

            protected List<GraphPoint> controlPoints;
            public List<GraphPoint> ControlPoints
            {
                get
                {
                    return controlPoints;
                }

                set
                {
                    controlPoints = value.OrderBy(o => o.Coordinate.x).ToList();
                }
            }

            //Used for setting data from saved file
            public override void SetData(Data.NodeData data)
            {
                base.SetData(data);

                ControlPoints = GraphPoint.ToGraphPointList(data.controlPoints, overlayWindowRect);
                
                //Get position on graph from coordinate
                foreach (GraphPoint controlPoint in ControlPoints)
                    controlPoint.CalculateGraphPoint();
            }
           
            //Draw graph
            public override void DrawOverlayWindow()
            {
                if (showValues)
                {
                    GUI.BringWindowToFront(-id - 1);
                    Rect backgroundRect = new Rect(0, 0, overlayWindowRect.width, overlayWindowRect.height);
                    backgroundRect.x += 2;
                    backgroundRect.y += 2;
                    backgroundRect.width -= 4;
                    backgroundRect.height -= 4;

                    //GUI.Box(backgroundBorderRect, "", "propertyField");
                    GUI.Box(backgroundRect, "", "box");

                    //Graph
                    DrawGraphLines();
                    ControlPoints = CustomLayout.CurveGraph(ControlPoints, "Graph View");

                    ContextMenu();
                    DeletePoint();

                    //Highlight selected point
                    if (selectedPoint != -1)
                        GUI.Box(selectionRect, "", "graphPointSelection");
                }
            }

            protected virtual void DrawGraphLines()
            {
                //Used to draw line in exact center of point
                Vector2 pointOffset = new Vector2(1, 2);

                //End points
                Vector2 graphStart = new Vector2(0, ControlPoints[0].Rect.center.y) + pointOffset;
                Vector2 graphEnd = new Vector2(overlayWindowRect.width, ControlPoints[ControlPoints.Count - 1].Rect.center.y) + pointOffset;

                //Draw from graph start
                Handles.DrawLine(graphStart, ControlPoints[0].Rect.center + pointOffset);

                //Draw Lines
                for (int i = 0; i < ControlPoints.Count - 1; i++)
                {
                    Handles.DrawLine(ControlPoints[i].Rect.center + pointOffset, ControlPoints[i + 1].Rect.center + pointOffset);
                }

                //Draw to graph end
                Handles.DrawLine(ControlPoints[ControlPoints.Count - 1].Rect.center + pointOffset, graphEnd);
            }

            protected void GetSelectedPoint()
            {
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    if (ControlPoints[i].drag)
                        selectedPoint = i;
                }
            }

            protected void ContextMenu()
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    if (Event.current.button == 1)
                    {
                        GenericMenu curveMenu = new GenericMenu();
                        curveMenu.AddItem(new GUIContent("Add Control Point"), false, AddControlPoint, Event.current.mousePosition);
                        curveMenu.ShowAsContext();
                        Event.current.Use();
                    }
                }
            }

            protected void AddControlPoint(object mousePosition)
            {
                GraphPoint point = new GraphPoint(Vector2.zero, overlayWindowRect);
                Rect pointRect = point.Rect;
                pointRect.position = (Vector2)mousePosition;
                point.Rect = pointRect;

                ControlPoints.Add(point);
                controlPoints = controlPoints.OrderBy(o => o.Coordinate.x).ToList(); //sort to get proper index
                selectedPoint = ControlPoints.IndexOf(point); //Selected created point

                Refresh();
            }

            protected void DeletePoint()
            {
                //Make sure something is selected
                if (selectedPoint != -1)
                {
                    //Require at least 2 points
                    if (controlPoints.Count > 2)
                    {
                        if (Event.current.type == EventType.KeyDown)
                        {
                            if (Event.current.keyCode == KeyCode.Delete)
                            {
                                ControlPoints.Remove(ControlPoints[selectedPoint]);
                                selectedPoint = -1;
                            }
                        }
                    }
                }
            }
        }
    }
}
