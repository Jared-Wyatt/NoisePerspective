using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective.Output
{
    public class TerrainOutput : OutputNode
    {
		public TerrainOutput(Scene scene, Vector2 position) : base(scene, position)
		{

		}

		public override void RefreshName()
		{
			name = "Output: Terrain - " + id;
		}

        public override void Refresh()
        {
            Manager.Instance.asset.terrainOutput = Data.DataProcessor.WriteNodes(GetOutputChain().ToArray());

            if (inputNodes[0] != null)
                Manager.Instance.asset.terrainOutputLastID = inputNodes[0].id;
            else
                Manager.Instance.asset.terrainOutputLastID = -1;
        }
    }
}
