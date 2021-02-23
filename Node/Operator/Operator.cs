using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public abstract class Operator : Node
        {    
            public Operator(Scene scene, Vector2 position) : base(scene, position)
            {
                inputConnectionPoints = new List<ConnectionPoint>(1);
                inputNodes = new Node[1];
            }

            public override void OnInputConnected(Node connectedNode, ConnectionPoint inputPoint)
            {
                base.OnInputConnected(connectedNode, inputPoint);

                for (int i = 0; i < inputConnectionPoints.Count; i++)
                {
                    if (inputPoint == inputConnectionPoints[i])
                        inputNodes[i] = connectedNode;
                }

                Refresh();
            }

            public SharpNoise.Modules.Module[] NullToConstInputModules()
            {
                SharpNoise.Modules.Module[] modules = new SharpNoise.Modules.Module[inputNodes.Length];

                for (int i = 0; i < inputNodes.Length; i++)
                {
                    if (inputNodes[i] == null)
                        modules[i] = new SharpNoise.Modules.Constant();
                    
                    else
                        modules[i] = inputNodes[i].OutputModule;
                }

                return modules;
            }
        }
    }
}

