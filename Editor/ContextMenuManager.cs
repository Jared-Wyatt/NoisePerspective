using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace NoisePerspective
{
    public sealed class ContextMenuManager : IDisposable
    {

        public GenericMenu mainMenu = new GenericMenu();

        private ContextMenuManager()
        {
            //===================================================================Main Menu===================================================================
            //Noise Nodes
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Perlin"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Perlin));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Billow"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Billow));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Simplex"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Simplex));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Ridged Multifractal"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.RidgedMultifractal));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Cells"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Cell));
            mainMenu.AddSeparator("Create Node/Noise/");
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Checker"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Checker));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Constant"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Constant));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Cylinders"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Cylinders));
            mainMenu.AddItem(new GUIContent("Create Node/Noise/Spheres"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Noise.Spheres));

            //Math Operator Nodes
            mainMenu.AddItem(new GUIContent("Create Node/Math/Add"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Add));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Multiply"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Multiply));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Subtract"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Subtract));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Exponent"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Exponent));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Power"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Power));
            mainMenu.AddSeparator("Create Node/Math/");
            mainMenu.AddItem(new GUIContent("Create Node/Math/Absolute"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Absolute));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Clamp"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Clamp));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Invert"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Invert));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Max"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Max));
            mainMenu.AddItem(new GUIContent("Create Node/Math/Min"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Min));

            //Modifier Operator Nodes
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Rotate"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Rotate));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Scale"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Scale));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/ScaleBias"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.ScaleBias));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Translate"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Translate));
            mainMenu.AddSeparator("Create Node/Modifier/");
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Blend"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Blend));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Curve"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Curve));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Displace"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Displace));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Select"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Select));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Terrace"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Terrace));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Turbulence"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Turbulence));
            mainMenu.AddItem(new GUIContent("Create Node/Modifier/Cache"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Operator.Cache));

            //OutputNodes
            mainMenu.AddItem(new GUIContent("Create Node/Output/Terrain Output"), false, NodeManager.Instance.InstantiateCreateGenericNode, typeof(Output.TerrainOutput));
        }

        private static ContextMenuManager instance = null;
        public static ContextMenuManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ContextMenuManager();

                return instance;
            }
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            instance = null;
        }
    }
}
