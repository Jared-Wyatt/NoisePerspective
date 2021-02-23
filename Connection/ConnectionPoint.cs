using System;
using UnityEngine;

public enum ConnectionPointType { Input, Output }

namespace NoisePerspective
{
    public class ConnectionPoint
    {
        public string name;
        public int id;

        public Rect rect = new Rect(0, 0, 10f, 13f);

        public ConnectionPointType type;
        readonly string style;

        public Node node;
        public Connection connection;

        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint(Node node, int id, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint)
        {
            this.node = node;
            this.id = id;
            this.type = type;
            this.OnClickConnectionPoint = OnClickConnectionPoint;

            if (type == ConnectionPointType.Output)
                style = "outputPoint";
            if (type == ConnectionPointType.Input)
                style = "InputPoint";
        }

        public void Draw()
        {
            if (GUILayout.Button("", style, GUILayout.Width(rect.width), GUILayout.Height(rect.height)))
            {
                OnClickConnectionPoint?.Invoke(this);
            }

            if (Event.current.type == EventType.Repaint)
                rect.position = node.windowRect.position + GUILayoutUtility.GetLastRect().position;
        }

        public void InputConnected(Node connectedNode)
        {
            node.OnInputConnected(connectedNode, this);
        }
    }
}