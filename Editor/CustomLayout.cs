using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoisePerspective
{
    public sealed class CustomLayout
    {
        public CustomLayout()
        {

        }

        public static List<GraphPoint> CurveGraph(List<GraphPoint> graphPoints, string label)
        {
            List<GraphPoint> points = graphPoints;

            //Control Rect
            Rect labelRect = new Rect(4, 0, 50, 16);
            GUI.Label(labelRect, label);

            //Draw points and force to be sequential
            for (int i = 0; i < points.Count; i++)
            {
                points[i].Draw("graphPoint");
            }

            return points;
        }
    }
}

