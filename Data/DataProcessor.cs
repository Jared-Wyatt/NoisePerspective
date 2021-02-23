using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoisePerspective.Data
{
    public static class DataProcessor
    {
        public static string WriteToString(Node[] nodes, Connection[] connections)
        {
            string data = "";

            data += WriteNodesToString(nodes);
            data += WriteConnectionsToString(connections);

            return data;
        }

        public static List<NodeData> WriteNodes(Node[] nodes)
        {
            List<NodeData> nodeData = new List<NodeData>();

            foreach (Node node in nodes)
            {
                if (node != null)
                {
                    NodeData data = new NodeData
                    {
                        type = node.GetType().ToString(),
                        id = node.id,
                        name = node.name,
                        windowPosition = node.windowRect.position,
                        showValues = node.showValues,
                        isPerspective = node.isPerspective
                    };

                    if (node.OutputModule != null)
                        data.moduleType = node.OutputModule.GetType().ToString();

                    //Billow Node
                    if (node.GetType() == typeof(Noise.Billow))
                    {
                        Noise.Billow billow = node as Noise.Billow;
                        data.seed = billow.Seed;
                        data.frequency = billow.Frequency;
                        data.lacunarity = billow.Lacunarity;
                        data.octaveCount = billow.OctaveCount;
                        data.persistence = billow.Persistence;
                        data.quality = billow.Quality;
                        data.amplitude = billow.Amplitude;
                    }

                    //Perlin Node
                    else if (node.GetType() == typeof(Noise.Perlin))
                    {
                        Noise.Perlin perlin = node as Noise.Perlin;
                        data.seed = perlin.Seed;
                        data.frequency = perlin.Frequency;
                        data.lacunarity = perlin.Lacunarity;
                        data.octaveCount = perlin.OctaveCount;
                        data.persistence = perlin.Persistence;
                        data.quality = perlin.Quality;
                        data.amplitude = perlin.Amplitude;
                    }

                    //Simplex Node
                    else if (node.GetType() == typeof(Noise.Simplex))
                    {
                        Noise.Simplex simplex = node as Noise.Simplex;
                        data.frequency = simplex.Frequency;
                        data.lacunarity = simplex.Lacunarity;
                        data.octaveCount = simplex.OctaveCount;
                        data.persistence = simplex.Persistence;
                        data.amplitude = simplex.Amplitude;
                    }

                    //RidgedMultifractal Node
                    else if (node.GetType() == typeof(Noise.RidgedMultifractal))
                    {
                        Noise.RidgedMultifractal ridged = node as Noise.RidgedMultifractal;
                        data.seed = ridged.Seed;
                        data.frequency = ridged.Frequency;
                        data.lacunarity = ridged.Lacunarity;
                        data.octaveCount = ridged.OctaveCount;
                        data.quality = ridged.Quality;
                        data.amplitude = ridged.Amplitude;
                    }

                    //Cells Node
                    else if (node.GetType() == typeof(Noise.Cell))
                    {
                        Noise.Cell cell = node as Noise.Cell;
                        data.seed = cell.Seed;
                        data.frequency = cell.Frequency;
                        data.displacement = cell.Displacement;
                        data.cellType = cell.CellType;
                        data.Coefficient = cell.Coefficient;
                        data.useDistance = cell.UseDistance;
                        data.amplitude = cell.Amplitude;
                    }

                    //Constant Node
                    else if (node.GetType() == typeof(Noise.Constant))
                    {
                        Noise.Constant constant = node as Noise.Constant;
                        data.value = constant.Value;
                    }

                    //Cylinders Node
                    else if (node.GetType() == typeof(Noise.Cylinders))
                    {
                        Noise.Cylinders cylinders = node as Noise.Cylinders;
                        data.frequency = cylinders.Frequency;
                        data.amplitude = cylinders.Amplitude;
                    }

                    //Spheres Node
                    else if (node.GetType() == typeof(Noise.Spheres))
                    {
                        Noise.Spheres spheres = node as Noise.Spheres;
                        data.frequency = spheres.Frequency;
                        data.amplitude = spheres.Amplitude;
                    }

                    //Clamp Node
                    else if (node.GetType() == typeof(Operator.Clamp))
                    {
                        Operator.Clamp clamp = node as Operator.Clamp;
                        data.maximum = clamp.Maximum;
                        data.minimum = clamp.Minimum;
                    }

                    //Exponent
                    else if (node.GetType() == typeof(Operator.Exponent))
                    {
                        Operator.Exponent exponent = node as Operator.Exponent;
                        data.value = exponent.Value;
                    }

                    //Curve
                    else if (node.GetType() == typeof(Operator.Curve))
                    {
                        Operator.Curve curve = node as Operator.Curve;
                        data.controlPoints = GraphPoint.ToVector2List(curve.ControlPoints);
                    }

                    //Terrace
                    else if (node.GetType() == typeof(Operator.Terrace))
                    {
                        Operator.Terrace terrace = node as Operator.Terrace;
                        data.isInverted = terrace.IsInverted;
                        data.controlPoints = GraphPoint.ToVector2List(terrace.ControlPoints);
                    }

                    //Rotate
                    else if (node.GetType() == typeof(Operator.Rotate))
                    {
                        Operator.Rotate rotate = node as Operator.Rotate;
                        data.x = rotate.X;
                        data.y = rotate.Y;
                        data.z = rotate.Z;
                    }

                    //Translate
                    else if (node.GetType() == typeof(Operator.Translate))
                    {
                        Operator.Translate translate = node as Operator.Translate;
                        data.x = translate.X;
                        data.y = translate.Y;
                        data.z = translate.Z;
                    }

                    //Scale
                    else if (node.GetType() == typeof(Operator.Scale))
                    {
                        Operator.Scale scale = node as Operator.Scale;
                        data.x = scale.X;
                        data.y = scale.Y;
                        data.z = scale.Z;
                    }

                    //ScaleBias
                    else if (node.GetType() == typeof(Operator.ScaleBias))
                    {
                        Operator.ScaleBias scaleBias = node as Operator.ScaleBias;
                        data.scaleValue = scaleBias.Scale;
                        data.bias = scaleBias.Bias;
                    }

                    //Turbulence
                    else if (node.GetType() == typeof(Operator.Turbulence))
                    {
                        Operator.Turbulence turbulence = node as Operator.Turbulence;
                        data.seed = turbulence.Seed;
                        data.frequency = turbulence.Frequency;
                        data.power = turbulence.Power;
                        data.roughness = turbulence.Roughness;
                    }

                    //Select
                    else if (node.GetType() == typeof(Operator.Select))
                    {
                        Operator.Select select = node as Operator.Select;
                        data.minimum = select.Minimum;
                        data.maximum = select.Maximum;
                        data.fallOff = select.FallOff;
                    }

                    nodeData.Add(data);
                }
            }

            return nodeData;
        }

        public static List<ConnectionData> WriteConnections(Connection[] connections)
        {
            List<ConnectionData> connectionData = new List<ConnectionData>();
            ConnectionData data = new ConnectionData(); 

            foreach (Connection connection in connections)
            {
                data.inputNodeID = connection.inPoint.node.id;
                data.inputConnectorID = connection.inPoint.id;
                data.outputNodeID = connection.outPoint.node.id;
                data.outputConnectorID = connection.outPoint.id;

                connectionData.Add(data);
            }

            return connectionData;
        }

        static string WriteNodesToString(Node[] nodes)
        {
            string data = "Nodes{\n";

            for (int i = 0; i < nodes.Length; i++)
            {
                data += "\tNode\n\t{\n";
                data += "\t\ttype=" + nodes[i].GetType() + ";\n";
                data += "\t\tname=" + nodes[i].name + ";\n";
                data += "\t\tid=" + nodes[i].id + ";\n";
                data += "\t\twindowPosition=" + nodes[i].windowRect.position + ";\n";
                data += "\t\tisPerspective=" + nodes[i].isPerspective + ";\n";
                data += "\t\tshowValues=" + nodes[i].showValues + ";\n";

                //Billow Node
                if (nodes[i].GetType() == typeof(Noise.Billow))
                {
                    Noise.Billow billow = nodes[i] as Noise.Billow;
                    data += "\t\tSeed=" + billow.Seed + ";\n";
                    data += "\t\tFrequency=" + billow.Frequency + ";\n";
                    data += "\t\tLacunarity=" + billow.Lacunarity + ";\n";
                    data += "\t\tOctaveCount=" + billow.OctaveCount + ";\n";
                    data += "\t\tPersistence=" + billow.Persistence + ";\n";
                    data += "\t\tQuality=" + (int)billow.Quality + ";\n";
                    data += "\t\tAmplitude=" + billow.Amplitude + ";\n";
                }

                //Perlin Node
                else if (nodes[i].GetType() == typeof(Noise.Perlin))
                {
                    Noise.Perlin perlin = nodes[i] as Noise.Perlin;
                    data += "\t\tSeed=" + perlin.Seed + ";\n";
                    data += "\t\tFrequency=" + perlin.Frequency + ";\n";
                    data += "\t\tLacunarity=" + perlin.Lacunarity + ";\n";
                    data += "\t\tOctaveCount=" + perlin.OctaveCount + ";\n";
                    data += "\t\tPersistence=" + perlin.Persistence + ";\n";
                    data += "\t\tQuality=" + (int)perlin.Quality + ";\n";
                    data += "\t\tAmplitude=" + perlin.Amplitude + ";\n";
                }

                //Simplex Node
                else if (nodes[i].GetType() == typeof(Noise.Simplex))
                {
                    Noise.Simplex simplex = nodes[i] as Noise.Simplex;
                    data += "\t\tFrequency=" + simplex.Frequency + ";\n";
                    data += "\t\tLacunarity=" + simplex.Lacunarity + ";\n";
                    data += "\t\tOctaveCount=" + simplex.OctaveCount + ";\n";
                    data += "\t\tPersistence=" + simplex.Persistence + ";\n";
                    data += "\t\tAmplitude=" + simplex.Amplitude + ";\n";
                }

                //RidgedMultifractal Node
                else if (nodes[i].GetType() == typeof(Noise.RidgedMultifractal))
                {
                    Noise.RidgedMultifractal ridged = nodes[i] as Noise.RidgedMultifractal;
                    data += "\t\tSeed=" + ridged.Seed + ";\n";
                    data += "\t\tFrequency=" + ridged.Frequency + ";\n";
                    data += "\t\tLacunarity=" + ridged.Lacunarity + ";\n";
                    data += "\t\tOctaveCount=" + ridged.OctaveCount + ";\n";
                    data += "\t\tQuality=" + (int)ridged.Quality + ";\n";
                    data += "\t\tAmplitude=" + ridged.Amplitude + ";\n";
                }

                //Cells Node
                else if (nodes[i].GetType() == typeof(Noise.Cell))
                {
                    Noise.Cell cell = nodes[i] as Noise.Cell;
                    data += "\t\tSeed=" + cell.Seed + ";\n";
                    data += "\t\tFrequency=" + cell.Frequency + ";\n";
                    data += "\t\tDisplacement=" + cell.Displacement + ";\n";
                    data += "\t\tCellType=" + (int)cell.CellType + ";\n";
                    data += "\t\tCoefficient=" + cell.Coefficient + ";\n";
                    data += "\t\tUseDistance=" + cell.UseDistance + ";\n";
                    data += "\t\tAmplitude=" + cell.Amplitude + ";\n";
                }

                //Constant Node
                else if (nodes[i].GetType() == typeof(Noise.Constant))
                {
                    Noise.Constant constant = nodes[i] as Noise.Constant;
                    data += "\t\tValue=" + constant.Value + ";\n";
                }

                //Cylinders Node
                else if (nodes[i].GetType() == typeof(Noise.Cylinders))
                {
                    Noise.Cylinders cylinders = nodes[i] as Noise.Cylinders;
                    data += "\t\tFrequency=" + cylinders.Frequency + ";\n";
                    data += "\t\tAmplitude=" + cylinders.Amplitude + ";\n";
                }

                //Spheres Node
                else if (nodes[i].GetType() == typeof(Noise.Spheres))
                {
                    Noise.Spheres spheres = nodes[i] as Noise.Spheres;
                    data += "\t\tFrequency=" + spheres.Frequency + ";\n";
                    data += "\t\tAmplitude=" + spheres.Amplitude + ";\n";
                }

                //Clamp Node
                else if (nodes[i].GetType() == typeof(Operator.Clamp))
                {
                    Operator.Clamp clamp = nodes[i] as Operator.Clamp;
                    data += "\t\tMaximum=" + clamp.Maximum + ";\n";
                    data += "\t\tMinimum=" + clamp.Minimum + ";\n";
                }

                //Exponent
                else if (nodes[i].GetType() == typeof(Operator.Exponent))
                {
                    Operator.Exponent exponent = nodes[i] as Operator.Exponent;
                    data += "\t\tValue=" + exponent.Value + ";\n";
                }

                //Curve
                else if (nodes[i].GetType() == typeof(Operator.Curve))
                {
                    Operator.Curve curve = nodes[i] as Operator.Curve;
                    data += "\t\tControlPoints=\n\t\t[\n";

                    foreach (Vector2 controlPoint in GraphPoint.ToVector2List(curve.ControlPoints))
                        data += "\t\t\t" + controlPoint + "\n";

                    data += "\t\t];\n";
                }

                //Terrace
                else if (nodes[i].GetType() == typeof(Operator.Terrace))
                {
                    Operator.Terrace terrace = nodes[i] as Operator.Terrace;
                    data += "\t\tIsInverted=" + terrace.IsInverted + ";\n";
                    data += "\t\tControlPoints=\n\t\t[\n";

                    foreach (Vector2 controlPoint in GraphPoint.ToVector2List(terrace.ControlPoints))
                        data += "\t\t\t" + controlPoint + "\n";

                    data += "\t\t];\n";
                }

                //Rotate
                else if (nodes[i].GetType() == typeof(Operator.Rotate))
                {
                    Operator.Rotate rotate = nodes[i] as Operator.Rotate;
                    data += "\t\tX=" + rotate.X + ";\n";
                    data += "\t\tY=" + rotate.Y + ";\n";
                    data += "\t\tZ=" + rotate.Z + ";\n";
                }

                //Translate
                else if (nodes[i].GetType() == typeof(Operator.Translate))
                {
                    Operator.Translate translate = nodes[i] as Operator.Translate;
                    data += "\t\tX=" + translate.X + ";\n";
                    data += "\t\tY=" + translate.Y + ";\n";
                    data += "\t\tZ=" + translate.Z + ";\n";
                }

                //Scale
                else if (nodes[i].GetType() == typeof(Operator.Scale))
                {
                    Operator.Scale scale = nodes[i] as Operator.Scale;
                    data += "\t\tX=" + scale.X + ";\n";
                    data += "\t\tY=" + scale.Y + ";\n";
                    data += "\t\tZ=" + scale.Z + ";\n";
                }

                //ScaleBias
                else if (nodes[i].GetType() == typeof(Operator.ScaleBias))
                {
                    Operator.ScaleBias scaleBias = nodes[i] as Operator.ScaleBias;
                    data += "\t\tScale=" + scaleBias.Scale + ";\n";
                    data += "\t\tBias=" + scaleBias.Bias + ";\n";
                }

                //Turbulence
                else if (nodes[i].GetType() == typeof(Operator.Turbulence))
                {
                    Operator.Turbulence turbulence = nodes[i] as Operator.Turbulence;
                    data += "\t\tSeed=" + turbulence.Seed + ";\n";
                    data += "\t\tFrequency=" + turbulence.Frequency + ";\n";
                    data += "\t\tPower=" + turbulence.Power + ";\n";
                    data += "\t\tRoughness=" + turbulence.Roughness + ";\n";
                }

                //Select
                else if (nodes[i].GetType() == typeof(Operator.Select))
                {
                    Operator.Select select = nodes[i] as Operator.Select;
                    data += "\t\tMinimum=" + select.Minimum + ";\n";
                    data += "\t\tMaximum=" + select.Maximum + ";\n";
                    data += "\t\tFallOff=" + select.FallOff + ";\n";
                }

                data += "\t}\n";
            }
            
            data += "}\n";
            return data;
        }

        static string WriteConnectionsToString(Connection[] connections)
        {
            string data = "Connections{\n";

            for (int i = 0; i < connections.Length; i++)
            {
                data += "\tConnection\n\t{\n";
                data += "\t\tInputNodeID=" + connections[i].inPoint.node.id + ";\n";
                data += "\t\tInputConnectorID=" + connections[i].inPoint.id + ";\n";
                data += "\t\tOutputNodeID=" + connections[i].outPoint.node.id + ";\n";
                data += "\t\tOutputConnectorID=" + connections[i].outPoint.id + ";\n";
                data += "\t}\n";
            }

            data += "}\n";
            return data;
        }

        public static void Read(string saveData)
        {
            if (saveData != null && saveData.Trim() != "")
            {
                saveData = saveData.Trim();

                string[] splitData = saveData.Split(new string[] { "Connections{" }, StringSplitOptions.RemoveEmptyEntries);
                string[] nodeData = splitData[0].Split('{');
                string[] connectionData = splitData[1].Split('{');

                //Node Loop
                for (int currentNode = 0; currentNode < nodeData.Length; currentNode++)
                {
                    //Values parsed from data
                    NodeData savedNodeData = new NodeData();

                    string[] lines = nodeData[currentNode].Split(';');

                    for (int line = 0; line < lines.Length; line++)
                    {
                        string[] valuePair = lines[line].Split('=');

                        if (valuePair[0].Trim() == "type")
                            savedNodeData.type = Type.GetType(valuePair[1]).ToString();

                        else if (valuePair[0].Trim() == "name")
                            savedNodeData.name = valuePair[1];

                        else if (valuePair[0].Trim() == "id")
                            savedNodeData.id = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "windowPosition")
                        {
                            string[] vector = valuePair[1].Split(',');
                            vector[0] = vector[0].Replace("(", "");
                            vector[1] = vector[1].Replace(")", "");
                            savedNodeData.windowPosition = new Vector2(float.Parse(vector[0]), float.Parse(vector[1]));
                        }

                        else if (valuePair[0].Trim() == "ControlPoints")
                        {
                            savedNodeData.controlPoints = new List<Vector2>();

                            valuePair[1] = valuePair[1].Replace("[", "");
                            valuePair[1] = valuePair[1].Replace("]", "");
                            valuePair[1] = valuePair[1].Replace("(", "");
                            valuePair[1] = valuePair[1].Replace(")", "");
                            string[] vectors = valuePair[1].Trim().Split('\n');

                            foreach (string vector in vectors)
                            {
                                string[] splitVector = vector.Trim().Split(',');
                                savedNodeData.controlPoints.Add(new Vector2(float.Parse(splitVector[0]), float.Parse(splitVector[1])));
                            }
                        }

                        else if (valuePair[0].Trim() == "isPerspective")
                            savedNodeData.isPerspective = bool.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "showValues")
                            savedNodeData.showValues = bool.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Seed")
                            savedNodeData.seed = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Amplitude")
                            savedNodeData.amplitude = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Frequency")
                            savedNodeData.frequency = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Lacunarity")
                            savedNodeData.lacunarity = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "OctaveCount")
                            savedNodeData.octaveCount = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Persistence")
                            savedNodeData.persistence = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Quality")
                            savedNodeData.quality = (SharpNoise.NoiseQuality)int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Power")
                            savedNodeData.power = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Roughness")
                            savedNodeData.roughness = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Displacement")
                            savedNodeData.displacement = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "UseDistance")
                            savedNodeData.useDistance = bool.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "CellType")
                            savedNodeData.cellType = (SharpNoise.Modules.Cell.CellType)int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Coefficient")
                            savedNodeData.Coefficient = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Value")
                            savedNodeData.value = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "IsInverted")
                            savedNodeData.isInverted = bool.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Maximum")
                            savedNodeData.maximum = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Minimum")
                            savedNodeData.minimum = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "FallOff")
                            savedNodeData.fallOff = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Scale")
                            savedNodeData.scaleValue = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Bias")
                            savedNodeData.bias = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "X")
                            savedNodeData.x = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Y")
                            savedNodeData.y = float.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "Z")
                            savedNodeData.z = float.Parse(valuePair[1]);
                    }

                    //Create Node
                    if (savedNodeData.type != null)
                    {
                        NodeManager.Instance.InstantiateGenericNode(savedNodeData);
                    }
                }

                //Connection Loop
                for (int currentConnection = 0; currentConnection < connectionData.Length; currentConnection++)
                {
                    //Values parsed from data
                    ConnectionData savedConnectionData = new ConnectionData();

                    string[] lines = connectionData[currentConnection].Split(';');

                    for (int line = 0; line < lines.Length; line++)
                    {
                        string[] valuePair = lines[line].Split('=');

                        if (valuePair[0].Trim() == "InputNodeID")
                            savedConnectionData.inputNodeID = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "InputConnectorID")
                            savedConnectionData.inputConnectorID = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "OutputNodeID")
                            savedConnectionData.outputNodeID = int.Parse(valuePair[1]);

                        else if (valuePair[0].Trim() == "OutputConnectorID")
                            savedConnectionData.outputConnectorID = int.Parse(valuePair[1]);
                    }

                    //Create Connection and add to connections list via Instantiate method
                    if (savedConnectionData.inputNodeID != savedConnectionData.outputNodeID) //ensures data has been read
                    {
                        ConnectionManager.Instance.InstantiateConnection(savedConnectionData);
                    }
                }
            }
        }
    }
}
