using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    namespace Operator
    {
        public class Subtract : Operator
        {
            public SharpNoise.Modules.Add outputModule = new SharpNoise.Modules.Add();

            public Subtract(Scene scene, Vector2 position) : base(scene, position)
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
                name = "Subtract - " + id;
            }

            public override void Refresh()
            {
                SharpNoise.Modules.Module[] inputModules = NullToConstInputModules();

                //Invert second input so add will subtract
                SharpNoise.Modules.Invert invertedInput = new SharpNoise.Modules.Invert
                {
                    Source0 = inputModules[1]
                };

                outputModule.Source0 = inputModules[0];
                outputModule.Source1 = invertedInput;

               
                base.Refresh();
            }
        }
    }
}

