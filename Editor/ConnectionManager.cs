using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace NoisePerspective
{
    public sealed class ConnectionManager : IDisposable
    {

        public List<Connection> connections;

        ConnectionPoint selectedInPoint;
        ConnectionPoint selectedOutPoint;

        public bool useBezierConnections;

        public readonly float bezierTangentFactor = 0.5f;
        public readonly float lineTangentFactor = 0.25f;

        public readonly Color OutputConnectionColor = Color.magenta;
        public readonly Color InputConnectionColor = Color.cyan;
        public readonly Color connectionColor = Color.yellow;

        private ConnectionManager()
        {

        }

        private static ConnectionManager instance = null;
        public static ConnectionManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConnectionManager();

                return instance;
            }
        }

        public void Initialize()
        {
            if (connections == null)
                connections = new List<Connection>();
        }

        //Used when loading data
        public void InstantiateConnection(Data.ConnectionData data)
        {
            if (connections == null)
                connections = new List<Connection>();

            Node inputNode = NodeManager.Instance.nodes[data.inputNodeID]; 
            Node outputNode = NodeManager.Instance.nodes[data.outputNodeID];

            ConnectionPoint inputPoint = inputNode.inputConnectionPoints[data.inputConnectorID]; 
            ConnectionPoint outputPoint = outputNode.outputConnectionPoints[data.outputConnectorID]; 

            Connection connection = new Connection(inputPoint, outputPoint, RemoveConnection);

            //Used for deleting connections
            inputPoint.connection = connection;
            outputPoint.connection = connection;

            //Required to refresh data
            inputNode.inputNodes[data.inputConnectorID] = outputNode;
            inputNode.Refresh();

            //Debug.Log()

            connections.Add(connection);
        }

        //Draw existing connections
        public void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        //Draw line while selecting new connection
        public void DrawConnectionLine(Event e)
        {
            Vector3[] points = new Vector3[4];
            float angelPointOffset;

            //Get points from Input to mouse
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Vector2 inputCenter = selectedInPoint.rect.center + new Vector2(0, 3);
                angelPointOffset = Mathf.Abs(inputCenter.x - e.mousePosition.x) * Connection.GetTangentFactor();

                points[0] = inputCenter;
                points[1] = inputCenter + Vector2.left * angelPointOffset;
                points[2] = e.mousePosition - Vector2.left * angelPointOffset;
                points[3] = e.mousePosition;

                Handles.color = ConnectionManager.Instance.OutputConnectionColor;
            }

            //Get Points from output to mouse
            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Vector2 outputCenter = selectedOutPoint.rect.center + new Vector2(0, 3);
                angelPointOffset = Mathf.Abs(outputCenter.x - e.mousePosition.x) * Connection.GetTangentFactor();

                points[0] = outputCenter;
                points[1] = outputCenter - Vector2.left * angelPointOffset;
                points[2] = e.mousePosition + Vector2.left * angelPointOffset;
                points[3] = e.mousePosition;

                Handles.color = ConnectionManager.Instance.InputConnectionColor;
            }

            //Draw
            if (ConnectionManager.Instance.useBezierConnections)
                Handles.DrawBezier(points[0], points[3], points[1], points[2], Handles.color, null, 2f);
            else
                Handles.DrawAAPolyLine(points);

            Handles.color = Color.white;
            GUI.changed = true;
        }

        public void OnClickOutputPoint(ConnectionPoint outputPoint)
        {
            selectedOutPoint = outputPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        public void OnClickInputPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            //Remove old connection
            if (selectedInPoint.connection != null)
                connections.Remove(selectedInPoint.connection);

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }

            connections.Add(new Connection(selectedInPoint, selectedOutPoint, RemoveConnection));
            selectedInPoint.connection = connections[connections.Count - 1]; //Used to remove connection when new connection is made and when node deleted
            selectedOutPoint.connection = connections[connections.Count - 1]; //Used to remove connection when node deleted

            selectedInPoint.InputConnected(selectedOutPoint.node); //Updates node
        }

        public void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        public void RemoveConnection(Connection connection)
        {
            //Remove assigned input
            for (int i = 0; i < connection.inPoint.node.inputNodes.Length; i++)
            {
                if (connection.inPoint.node.inputNodes[i] == connection.outPoint.node)
                    connection.inPoint.node.inputNodes[i] = null;
            }

            connections.Remove(connection);
            connection.inPoint.node.Refresh();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            connections = null;
            instance = null;
        }
    }
}



