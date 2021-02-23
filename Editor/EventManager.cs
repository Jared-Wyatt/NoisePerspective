using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace NoisePerspective
{
    public sealed class EventManager : IDisposable
    {
        public Vector2 mousePosition = Vector2.zero;
        public Vector2 mouseDragDelta = Vector2.zero;

        private EventManager()
        {
            
        }

        private static EventManager instance = null;
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventManager();

                return instance;
            }
        }

        //Events
        public void ProcessEvents()
        {
            Event e = Event.current;

            mousePosition = e.mousePosition;
            mouseDragDelta = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                        ContextMenuManager.Instance.mainMenu.ShowAsContext(); //Right Click Menu
                    if (e.button == 0)
                        ConnectionManager.Instance.ClearConnectionSelection();
                    break;

                case EventType.MouseDrag:
                    if (e.button == 2)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        //Drag Canvas
        private void OnDrag(Vector2 delta)
        {
            mouseDragDelta = delta;

            //Drag Each Node
            if (NodeManager.Instance.nodes != null)
            {
                for (int i = 0; i < NodeManager.Instance.nodes.Count; i++)
                    NodeManager.Instance.nodes[i].windowRect.position += delta;
            }

            GUI.changed = true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            instance = null;
        }
    }
}
