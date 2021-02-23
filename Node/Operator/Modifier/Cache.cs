using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Cache : Operator
        {
            public SharpNoise.Modules.Cache outputModule = new SharpNoise.Modules.Cache();
                
            public Cache(Scene scene, Vector2 position) : base(scene, position)
            {
                
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public override void RefreshName()
            {
                name = "Cache - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];

                base.Refresh();
            }
        }
    }
}

