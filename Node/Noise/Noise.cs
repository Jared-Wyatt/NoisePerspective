using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Noise
    {
        public abstract class Noise : Node
        {
            public Noise(Scene scene, Vector2 position) : base(scene, position)
            {
                
            }
        }
    }
}

