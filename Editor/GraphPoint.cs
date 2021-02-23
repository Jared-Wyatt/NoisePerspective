using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoisePerspective
{
    public class GraphPoint
    {
        Rect rect = new Rect(0, 0, 7, 10);
        Vector2 coordinate;
        Rect graphRect;
        public bool drag = false;
        readonly float margin = 5;

        public GraphPoint(Vector2 coordinate, Rect graph)
        {
            this.coordinate = coordinate;
            graphRect = graph;
            
            //Prevent points from going off graph
            graphRect.size = new Vector2(graphRect.width - (margin * 2), graphRect.height - (margin * 2));
            graphRect.position += Vector2.one * (margin);
        }

        public Vector2 Coordinate
        {
            get
            {
                return coordinate;
            }

            set
            {
                coordinate = value;
                
                //Clamp X
                if (coordinate.x < -1)
                    coordinate = new Vector2(-1, coordinate.y);
                if (coordinate.x > 1)
                    coordinate = new Vector2(1, coordinate.y);

                //Clamp Y
                if (coordinate.y < -1)
                    coordinate = new Vector2(coordinate.x, -1);
                if (coordinate.y > 1)
                    coordinate = new Vector2(coordinate.x, 1);

                CalculateGraphPoint();
            }
        }

        public Rect Rect
        {
            get
            {
                return rect;
            }

            set
            {
                rect = value;
                CalculateCoordinate();
            }
        }

        public static List<GraphPoint> ToGraphPointList(List<Vector2> vectors, Rect graphRect)
        {
            List<GraphPoint> points = new List<GraphPoint>();

            foreach (Vector2 vector in vectors)
                points.Add(new GraphPoint(vector, graphRect));

            return points;
        }

        public static List<Vector2> ToVector2List(List<GraphPoint> graphPoints)
        {
            List<Vector2> vectors = new List<Vector2>();

            foreach (GraphPoint point in graphPoints)
                vectors.Add(point.Coordinate);

            return vectors;
        }

        public void Draw(string style)
        {
            //Start Drag
            if (GUI.RepeatButton(rect, " ", style))
                drag = true;

            //EventType.Used is raised when button is released
            if (Event.current.type == EventType.Used)
                drag = false;

            //Used to release drag after points x position are switched
            if (Event.current.type == EventType.MouseUp)
                drag = false;

            //Drag
            if (drag)
                rect.position = Event.current.mousePosition - rect.position;

            //Convert point position to coordinate
            CalculateCoordinate();
        }

        public void CalculateGraphPoint()
        {
            //normalize
            float normalX = (coordinate.x + 1) / 2;
            float normalY = (coordinate.y + 1) / 2;

            normalX *= graphRect.width;
            normalY = graphRect.height - (normalY * graphRect.height); //Also invert y

            rect.position = new Vector2(normalX, normalY);
        }

        //Convert point position to coordinate
        public void CalculateCoordinate()
        {
            //Scale to 0-1
            float coordX = rect.position.x / graphRect.width;
            float coordY = 1 - (rect.position.y / graphRect.height); //Also invert y

            //Scale to -1 to 1
            coordX = (coordX / 0.5f) - 1;
            coordY = (coordY / 0.5f) - 1;

            coordinate = new Vector2(coordX, coordY);
        }
    }
}
