using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoisePerspective
{
    [Serializable]
    public class Connection
    {
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;

            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Vector3[] points = CreateConnectorPoints();
            Handles.color = ConnectionManager.Instance.connectionColor;
                
            if (ConnectionManager.Instance.useBezierConnections)
                    Handles.DrawBezier(points[0], points[3], points[1], points[2], Handles.color, null, 2);
            else
                Handles.DrawAAPolyLine(points);

            Handles.color = Color.white;

            if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }

        Vector3[] CreateConnectorPoints()
        {
            Vector2 inputCenter = inPoint.rect.center + new Vector2(0, 3);
            Vector2 outputCenter = outPoint.rect.center + new Vector2(0, 3);
            
            float angelPointOffset = Mathf.Abs(inputCenter.x - outputCenter.x) * GetTangentFactor();

            Vector3[] points = new Vector3[4];

            points[0] = inputCenter;
            points[1] = inputCenter + Vector2.left * angelPointOffset;
            points[2] = outputCenter - Vector2.left * angelPointOffset;
            points[3] = outputCenter;

            return points;
        }

        public static float GetTangentFactor()
        {
            if (ConnectionManager.Instance.useBezierConnections)
                return ConnectionManager.Instance.bezierTangentFactor;
            else
                return ConnectionManager.Instance.lineTangentFactor;
        }
    }
}