using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoisePerspective.Data
{
    //Used to create a node from saved data
    [Serializable]
    public struct NodeData
    {
        //Values every node has
        public string name;
        public int id;
        public string type;
        public string moduleType;
        public Vector2 windowPosition;
        public bool isPerspective;
        public bool showValues;

        //Different nodes can have different values
        public int seed;
        public float frequency;
        public float lacunarity;
        public int octaveCount;
        public float persistence;
        public float power;
        public int roughness;
        public float displacement;
        public float value;
        public float maximum;
        public float minimum;
        public float fallOff;
        public float scaleValue;
        public float bias;
        public int Coefficient;
        public bool useDistance;
        public bool isInverted;
        public float exponent;
        public float x;
        public float y;
        public float z;
        public List<Vector2> controlPoints;
        public SharpNoise.NoiseQuality quality;
        public SharpNoise.Modules.Cell.CellType cellType;
        public float amplitude;
    }
}
