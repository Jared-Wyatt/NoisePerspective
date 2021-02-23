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
    public struct ConnectionData
    {
        //Values every node has
        public int inputNodeID;
        public int inputConnectorID;
        public int outputNodeID;
        public int outputConnectorID;
    }
}
