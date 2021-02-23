using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Displace : Operator
        {
            public SharpNoise.Modules.Displace outputModule = new SharpNoise.Modules.Displace();

            public Displace(Scene scene, Vector2 position) : base(scene, position)
            {
                inputConnectionPoints = new List<ConnectionPoint>(4);
                inputNodes = new Node[4];
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public override void RefreshName()
            {
                name = "Displace - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module [] inputModules = NullToConstInputModules();

                outputModule.Source0 = inputModules[0];
                outputModule.SetDisplacementModules(inputModules[1], inputModules[2], inputModules[3]);

                base.Refresh();
            }
        }
    }
}

