using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Power : Operator
        {
            public SharpNoise.Modules.Power outputModule = new SharpNoise.Modules.Power();

            public Power(Scene scene, Vector2 position) : base(scene, position)
            {
                inputConnectionPoints = new List<ConnectionPoint>(2);
                inputNodes = new Node[2];
            }

            public override SharpNoise.Modules.Module OutputModule
            {
                get { return outputModule; }
            }

            public override void RefreshName()
            {
                name = "Power - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();


                outputModule.Source0 = inputModules[0];
                outputModule.Source1 = inputModules[1];


                base.Refresh();
            }
        }
    }
}

