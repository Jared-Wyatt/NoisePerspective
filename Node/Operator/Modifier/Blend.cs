using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Blend : Operator
        {
            public SharpNoise.Modules.Blend outputModule = new SharpNoise.Modules.Blend();

            public Blend(Scene scene, Vector2 position) : base(scene, position)
            {
                inputConnectionPoints = new List<ConnectionPoint>(3);
                inputNodes = new Node[3];
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public override void RefreshName()
            {
                name = "Blend - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.Source1 = inputModules[1];
                outputModule.Control = inputModules[2];

                base.Refresh();
            }
        }
    }
}

