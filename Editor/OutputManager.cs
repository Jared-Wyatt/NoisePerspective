using NoisePerspective.Output;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    public sealed class OutputManager : IDisposable
    {
        private OutputManager()
        {

        }

        private static OutputManager instance = null;
        public static OutputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new OutputManager();

                return instance;
            }
        }

        SharpNoise.Modules.Module terrainOutput;
        public SharpNoise.Modules.Module TerrainOutput
        {
            get
            {
                return terrainOutput;
            }
            set
            {
                if (terrainOutput == null)
                    terrainOutput = value;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            instance = null;
        }
    }
}
