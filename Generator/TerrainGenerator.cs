using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoisePerspective.Generator
{
    class TerrainGenerator : MonoBehaviour
    {
        public NoiseAsset noiseAsset = null; //Assigned in inspector
        List<SharpNoise.Modules.Module> outputModules;
        SharpNoise.Modules.Module lastOutputModule;

        GameObject terrain;

        //Terrain Settings
        public int terrainSegments = 2;
        public float segmentSize = 500;
        readonly float terrainHeight = 600;
        public int heightResolution = 513;
        public int detailResolution = 512;
        float heightStrength;
        TerrainData[,] terrainDatas;

        // Start is called before the first frame update
        void Start()
        {
            heightStrength = noiseAsset.globalHeightStrength;
            
            BuildOutputModules();

            //Find module at end of output chain
            foreach (SharpNoise.Modules.Module module in outputModules)
            {
                if (module.NodeId == noiseAsset.terrainOutputLastID)
                    lastOutputModule = module;
            }

            CreateTerrain();
            SetHeights();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BuildOutputModules()
        {
            //Create Modules
            outputModules = new List<SharpNoise.Modules.Module>();

            foreach (Data.NodeData nodeData in noiseAsset.terrainOutput)
            {
                SharpNoise.Modules.Module module = NodeManager.Instance.InstantiateGenericModule(nodeData);
                module.SetData(nodeData);
                outputModules.Add(module);
            }

            //Set Source Modules
            foreach (SharpNoise.Modules.Module module in outputModules)
            {
                SharpNoise.Modules.Module[] sourceModules = new SharpNoise.Modules.Module[4];

                foreach (Data.ConnectionData connectionData in noiseAsset.connections)
                {
                    if (connectionData.inputNodeID == module.NodeId)
                    {

                        //Assign to index of connectorID
                        foreach (SharpNoise.Modules.Module sourceModule in outputModules)
                        {
                            if (sourceModule.NodeId == connectionData.outputNodeID)
                                sourceModules[connectionData.inputConnectorID] = sourceModule;
                        }
                    }
                }

                module.SetSourceModules(sourceModules);
            }
        }

        //Terrain
        public void CreateTerrain()
        {
            terrain = new GameObject(name + ": Terrains");

            terrainDatas = new TerrainData[terrainSegments, terrainSegments];

            //Create Terrain Segments
            for (int x = 0; x < terrainSegments; x++)
            {
                for (int y = 0; y < terrainSegments; y++)
                {
                    TerrainData terrainData = new TerrainData();

                    terrainData.SetDetailResolution(detailResolution, 32);
                    terrainData.heightmapResolution = heightResolution;
                    terrainData.size = new Vector3(segmentSize, terrainHeight, segmentSize);
                    terrainData.name = "TerrainData-" + x.ToString("D3") + "-" + y.ToString("D3");

                    GameObject terrainSegment = Terrain.CreateTerrainGameObject(terrainData);
                    terrainSegment.name = "Terrain-" + x.ToString("D3") + "-" + y.ToString("D3");
                    terrainSegment.transform.parent = terrain.transform;
                    terrainSegment.transform.localPosition = new Vector3(x * segmentSize, 0, y * segmentSize);
                    terrainDatas[x, y] = terrainData;
                }
            }

            Terrain.SetConnectivityDirty();
            terrain.transform.position = new Vector3((-segmentSize * terrainSegments) / 2, 0, (-segmentSize * terrainSegments) / 2); //Center Terrain
        }

        public virtual void SetHeights()
        {
            float[,][,] combinedHeights = new float[terrainSegments, terrainSegments][,];

            //Initialize matrix of arrays for heightmaps
            for (int segmentX = 0; segmentX < combinedHeights.GetLength(0); segmentX++)
            {
                for (int segmentY = 0; segmentY < combinedHeights.GetLength(1); segmentY++)
                {
                    float[,] heights = new float[heightResolution, heightResolution];

                    //Fill heightmap for segment
                    for (int x = 0; x < heightResolution; x++)
                    {
                        for (int y = 0; y < heightResolution; y++)
                        {
                            //Get height positions and subtract segment iteration to align seams
                            float heightX = x + (segmentX * heightResolution) - segmentX;
                            float heightY = y + (segmentY * heightResolution) - segmentY;


                            //Normalize to height resolution (0, 1)
                            heightX /= heightResolution;
                            heightY /= heightResolution;
                            //heightY = (heightY - -1) / (1 - -1); //Normalize to (-1, 1)
                            //heightY = (heightY - -1) / (1 - -1); //Normalize to (-1, 1)

                            //Read in heights from heightmap
                            heights[y, x] = (float)lastOutputModule.GetValue(heightX, heightY, 0) * heightStrength; //heights x and y value need to be swapped for terrain to line up (loop order issue?)
                        }
                    }

                    combinedHeights[segmentX, segmentY] = heights; //Assign heights to segment
                }
            }

            Terrain.SetConnectivityDirty();

            //Set Heights
            for (int x = 0; x < terrainSegments; x++)
            {
                for (int y = 0; y < terrainSegments; y++)
                {
                    terrainDatas[x, y].SetHeights(0, 0, combinedHeights[x, y]);
                }
            }
        }
    }
}
