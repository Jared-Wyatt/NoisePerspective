using NoisePerspective.Output;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoisePerspective
{
    [CreateAssetMenu(fileName = "New NoisePerspective.asset", menuName = "NoisePerspective/Create Noise")]
    
    [Serializable]
    public class NoiseAsset : ScriptableObject
    {
        [HideInInspector] public string saveData;
        [HideInInspector] public bool useBezier;
        [HideInInspector] public float globalHeightStrength = 1;
        [HideInInspector] public Vector2 gridOffset;

        [HideInInspector] public List<Data.ConnectionData> connections;
        [HideInInspector] public List<Data.NodeData> terrainOutput;
        [HideInInspector] public int terrainOutputLastID;
    }
}
