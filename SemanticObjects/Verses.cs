using System.Collections;
using SharedGeometry;
using glTFInterface;
using g4;

namespace Verses
/// <summary>
/// The Omniverse is the source of all externally defined information and universal relationships and interactions between entities
/// there are Oracles that provide information and Animators that alter conditions in the real world. Both operate without any explanation or known mechanism.
/// </summary>
{
    public class OutsideOfAnyWorld
    {
        public DateTime Now { get { return DateTime.UtcNow; } }

    }
    // Each type of world behaves as a single space
    // An integrated world follows this rule as well, all parts pulled into a single space as a default
    //    but it can be treated as a composite of the contaained worlds when appropriate
    // Any type of world should be serializable independently
    // It should be possible to serialize an Integrated world as a 
    public class World
    {
        public string Name { get; set; } = "";
        public string ReferenceFrame { get; set; } = "Default";
        public GeoPose.GeoPose? FramePose { get; set; }
        public SharedGeometry.Distance Size { get; set; }
        public List<Entities.Entity> Entities { get; set; } = new List<Entities.Entity>();
        public void SaveAsJSON(StreamWriter jsw)
        {

        }
        public void ListElementsAsMarkDown(StreamWriter sw)
        {
            // name
            //sw.WriteLine("|" + Name);
            // reference frame
            //sw.WriteLine("\r\n \r\nReferenceFrame: " + ReferenceFrame);
            // entities
            foreach(Entities.Entity e in Entities)
            {
                string poseString = e.Pose.ToJSON();
                sw.WriteLine("|" + e.Name + "|" + e.ID + "|" + e.SemanticEntityClass.GetType().Name + "|" + poseString + "|");
                //string poseString = (e.Pose.GetType().ToString() == "GeoPose.BasicYPR") ? ((GeoPose.BasicYPR)(e.Pose)).ToJSON() : ((GeoPose.Advanced)(e.Pose)).ToJSON();
                //sw.WriteLine("|" + e.Name + "|" + e.ID + "|" + e.SemanticEntityClass.GetType().Name + "|" + poseString + "|");
                //sw.WriteLine("\r\nEntity ID:  " + e.ID + "\r\n ");
                //sw.WriteLine("\r\nSemantic Class: " + e.SemanticEntityClass.GetType().Name + "\r\n ");
            }
        }
        public void AddEntity(Entities.Entity newEntity)
        {
            Entities.Add(newEntity);
        }

    }
    public class StaticWorld : World
    {
        public const string Moniker  = "static"; 
    }

    public class DynamicWorld : World
    {
        public const string Moniker = "dynamic";
    }
    public class VirtualWorld : World
    {
        public const string Moniker = "virtual";
        public SemanticClasses.Sign SignOverRide { get; set; } = new SemanticClasses.Sign();
    }

    public class PlanetLike : StaticWorld
    {

    }

    public class UrbanPatch : PlanetLike
    {

    }
    public class IntegratedWorld
    {
        public IntegratedWorld(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; } = "";
        public string ReferenceFrame { get; set; } = "Default";
        public GeoPose.BasicYPR FramePose { get; set; }
        // Size is nominally the diameter of a sphere centered on the origin of the Frame Pose
        public SharedGeometry.Distance Size { get; set; }
        public OutsideOfAnyWorld OmniVerse { get; set; } = new OutsideOfAnyWorld();

        public List<Verses.World> WorldSet = new List<World>();

        public void AddWorld(World aWorld)
        {
            WorldSet.Add(aWorld);   
        }

        public void SaveAsJSON()
        {
            string fileName = "c:/temp/models/integrated.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            // write header
            //    the name, frame, geometry and material for sphere of radius size centered at ltp-enu frame origin
            sw.WriteLine("{\r\n\"Integrated-World\": " + "\"" + Name + "\"");
            sw.WriteLine("Created: " + OmniVerse.Now.ToString());
            sw.WriteLine("\"Worlds\":\r\n[\r\n");
            // loop through worlds
            foreach(World w in WorldSet)
            {
                sw.WriteLine("\r\n# " + w.Name + ": " + w.GetType().ToString());
                // bring into same frame
                // add nodes for entities
                // add to 
            }
            //sw.WriteLine("\r\n# Background World\r\n");
            //Background.SaveAsJSON(sw);
            //sw.WriteLine("\r\n# Foreground World\r\n");
            //Foreground.SaveAsJSON(sw);
            //sw.WriteLine("\r\n# Virtual World\r\n");
            //VirtualParts.SaveAsJSON(sw);
            // write footer with maybe just a closing }
            sw.Close();

        }

        /*
         * 
# Integrated World: Use Case 1
Created: 11/23/2022 11:54:10 PM UTC

---

## Integrated Sub-Worlds

---

**World Name**: \"Background\": 

**World Type**: Verses.StaticWorld

**ReferenceFrame**:  Default

**Contained Entities**:

| Name | ID | Semantic Class |GeoPose |
| ----------- | ----------- | -------- | ----- |
| Planet Surface | 33 | LandSurface | {\"position\": {\"lat\": 48,\"lon\":-122,\"h\": 0},\"quaternion\":{"x":0.00774503,"y":0.00451,"z":0.86391,"w":-0.50356  }}
  |||||

---

         * 
         */
        public void ListElementsAsMarkDown(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("# Integrated World: \"" + Name + "\"");
            sw.WriteLine("Created: " + OmniVerse.Now.ToString() + " UTC");
            sw.WriteLine("\r\n---\r\n");
            sw.WriteLine("## Component Worlds");
            sw.WriteLine("\r\n---\r\n");
            // loop through worlds
            foreach (World w in WorldSet)
            {
                sw.WriteLine("\r\n**Name:** " + w.Name);
                sw.WriteLine("\r\n**Type:** " + w.GetType().ToString());
                sw.WriteLine("\r\n**Reference Frame:** " + w.ReferenceFrame);
                string frameType = (w.FramePose != null) ? w.FramePose.GetType().Name : "Unspecified";
                if (w.FramePose != null && frameType == "BasicYPR")
                {
                    sw.WriteLine("\r\n**Frame Pose:** " + ((GeoPose.BasicYPR)w.FramePose).ToJSON());
                }
                else if (w.FramePose != null && frameType == "Advanced")
                {
                    sw.WriteLine("\r\n**Frame Pose:** " + ((GeoPose.Advanced)w.FramePose).ToJSON());
                }
                else
                {
                    sw.WriteLine("\r\n**Frame Pose:** " + "\"unrecognize type\"" + frameType); ;
                }
                sw.WriteLine("\r\n**Contained Entities:** ");
                sw.WriteLine("\r\n| Name | ID | Semantic Class |GeoPose |");
                sw.WriteLine("| ----------- | ----------- | ----------- | ----------- |");
                w.ListElementsAsMarkDown(sw);
                sw.WriteLine("\r\n---\r\n");
            }
            //sw.WriteLine("\r\n# Background World\r\n");
            //Background.ListElementsAsMarkDown(sw);
            //sw.WriteLine("\r\n# Foreground World\r\n");
            //Foreground.ListElementsAsMarkDown(sw);
            //sw.WriteLine("\r\n# Virtual World\r\n");
            //VirtualParts.ListElementsAsMarkDown(sw);
            sw.Close();
        }
        public void SaveAsJSON(string fileName)
        {
            string header = " {" + " {\r\n";
            string footer = "}\r\n";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(header);
            /*
            sw.WriteLine("# Integrated World: \"" + Name + "\"");
            sw.WriteLine("Created: " + OmniVerse.Now.ToString() + " UTC");
            sw.WriteLine("\r\n---\r\n");
            sw.WriteLine("## Component Worlds");
            sw.WriteLine("\r\n---\r\n");
            // loop through worlds
            foreach (World w in WorldSet)
            {
                sw.WriteLine("\r\n**Name:** " + w.Name);
                sw.WriteLine("\r\n**Type:** " + w.GetType().ToString());
                sw.WriteLine("\r\n**Reference Frame:** " + w.ReferenceFrame);
                string frameType = (w.FramePose != null) ? w.FramePose.GetType().Name : "Unspecified";
                if (w.FramePose != null && frameType == "Basic")
                {
                    sw.WriteLine("\r\n**Frame Pose:** " + ((GeoPose.Basic)w.FramePose).ToJSON());
                }
                else if (w.FramePose != null && frameType == "Advanced")
                {
                    sw.WriteLine("\r\n**Frame Pose:** " + ((GeoPose.Advanced)w.FramePose).ToJSON());
                }
                else
                {
                    sw.WriteLine("\r\n**Frame Pose:** " + "\"unrecognize type\"" + frameType); ;
                }
                sw.WriteLine("\r\n**Contained Entities:** ");
                sw.WriteLine("\r\n| Name | ID | Semantic Class |GeoPose |");
                sw.WriteLine("| ----------- | ----------- | ----------- | ----------- |");
                w.ListElementsAsMarkDown(sw);
                sw.WriteLine("\r\n---\r\n");
            }
            //sw.WriteLine("\r\n# Background World\r\n");
            //Background.ListElementsAsMarkDown(sw);
            //sw.WriteLine("\r\n# Foreground World\r\n");
            //Foreground.ListElementsAsMarkDown(sw);
            //sw.WriteLine("\r\n# Virtual World\r\n");
            //VirtualParts.ListElementsAsMarkDown(sw);
            */
            sw.Write(footer);
            sw.Close();
        }
        /*
         * 
 {
	"extensionsUsed": [
		"OGC_Geo_Semantic_Replica"
	],
	"extensionsRequired": [
		"OGC_Geo_Semantic_Replica"
	],
	"asset": {
		"generator": "SGR00",
		"version": "2.0"
	},

         * 
         */
        public float[] CopyVec3DArray2FloatArray(List<Tuple<double, double, double>> triples,
            out double minX, out double maxX, out double minY, out double maxY, out double minZ, out double maxZ)
        {
            int nTriples = triples.Count();
            float[] fVTemp = new float[nTriples * 3];
            int nFloat = 0;
            minX = 1.0e10;
            minY = 1.0e10;
            minZ = 1.0e10;
            maxX = -1.0e10;
            maxY = -1.0e10;
            maxZ = -1.0e10;
            for (int nVertex = 0; nVertex < nTriples; nVertex++)
            {
                var u = triples[nVertex];
                if (u.Item1 < minX)
                {
                    minX = u.Item1;
                }
                if (u.Item1 > maxX)
                {
                    maxX = u.Item1;
                }
                if (u.Item2 < minY)
                {
                    minY = u.Item2;
                }
                if (u.Item2 > maxY)
                {
                    maxY = u.Item2;
                }
                if (u.Item3 < minZ)
                {
                    minZ = u.Item3;
                }
                if (u.Item3 > maxZ)
                {
                    maxZ = u.Item3;
                }
                fVTemp[nFloat++] = (float)u.Item1;
                fVTemp[nFloat++] = (float)u.Item2;
                fVTemp[nFloat++] = (float)u.Item3;
            }
            return fVTemp;
        }
        public float[] CopyVec3FArray2FloatArray(List<Tuple<float, float, float>> triples,
            out double minX, out double maxX, out double minY, out double maxY, out double minZ, out double maxZ)
        {
            int nTriples = triples.Count();
            float[] fVTemp = new float[nTriples * 3];
            int nFloat = 0;
            minX = 1.0e10;
            minY = 1.0e10;
            minZ = 1.0e10;
            maxX = -1.0e10;
            maxY = -1.0e10;
            maxZ = -1.0e10;
            for (int nTuple = 0; nTuple < nTriples; nTuple++)
            {
                var u = triples[nTuple];
                if (u.Item1 < minX)
                {
                    minX = u.Item1;
                }
                if (u.Item1 > maxX)
                {
                    maxX = u.Item1;
                }
                if (u.Item2 < minY)
                {
                    minY = u.Item2;
                }
                if (u.Item2 > maxY)
                {
                    maxY = u.Item2;
                }
                if (u.Item3 < minZ)
                {
                    minZ = u.Item3;
                }
                if (u.Item3 > maxZ)
                {
                    maxZ = u.Item3;
                }
                fVTemp[nFloat++] = (float)u.Item1;
                fVTemp[nFloat++] = (float)u.Item2;
                fVTemp[nFloat++] = (float)u.Item3;
            }
            return fVTemp;
        }
        public ushort[] CopyUShortTripleArray2UShortArray(List<Tuple<ushort, ushort, ushort>> triples,
            out int minIndex, out int maxIndex)
        {
            int nTriples = triples.Count();
            ushort[] iTemp = new ushort[nTriples * 3];
            int nFloat = 0;
            minIndex = 100000000;
            maxIndex = -1;
            for (int nVertex = 0; nVertex < nTriples; nVertex++)
            {
                var u = triples[nVertex];
                if (u.Item1 < minIndex)
                {
                    minIndex = u.Item1;
                }
                if (u.Item1 > maxIndex)
                {
                    maxIndex = u.Item1;
                }
                if (u.Item2 < minIndex)
                {
                    minIndex = u.Item2;
                }
                if (u.Item2 > maxIndex)
                {
                    maxIndex = u.Item2;
                }
                if (u.Item3 < minIndex)
                {
                    minIndex = u.Item3;
                }
                if (u.Item3 > maxIndex)
                {
                    maxIndex = u.Item3;
                }
                iTemp[nFloat++] = (ushort)u.Item1;
                iTemp[nFloat++] = (ushort)u.Item2;
                iTemp[nFloat++] = (ushort)u.Item3;
            }
            return iTemp;
        }

        public void Render2glTF(string sceneName, string fileName)
        {
            // create and populate root
            glTFRoot root = new glTFRoot();
            root.extensionsRequired.Add("OGC_Semantic_Core");
            root.extensionsUsed.Add("OGC_Semantic_Core");
            root.extensionsUsed.Add("KHR_materials_transmission");
            root.asset.generator = "GSR00.0.5.4";
            root.asset.version = "2.0";
            root.scene = 0;
            double radius = this.Size.Value;
            GeoPose.BasicYPR aPose = this.FramePose;
            OGC_SemanticCore semanticCore = new OGC_SemanticCore("Test", "https://citygml.info/OGC-Khronos-Forum/Prototype/Proto.gltf",
            aPose.Position.lat, aPose.Position.lon, aPose.Position.h, aPose.Angles.yaw, aPose.Angles.pitch, aPose.Angles.roll, radius);

            // add base scene
            Scene scene = new Scene();
            scene.name = sceneName;
            scene.extensions = new Dictionary<string, object>();
            scene.extensions.Add("OGC_Semantic_Core", semanticCore);
            // this happens when the node is added in renderentityscene.nodes.Add(0);
            root.scenes.Add(scene);

            // create and render verseglobe to gltf structures
            glTFInterface.Node node = new glTFInterface.Node();
            node.name = "Bounding Sphere";
            node.mesh = 0;
            root.nodes.Add(node);

            glTFInterface.Material material = new glTFInterface.Material();
            material.name = "Transparent Dome";
            material.alphaMode = "BLEND";
            material.doubleSided = true;
            PbrMetallicRoughness pbrMetallicRoughness = new PbrMetallicRoughness();
            pbrMetallicRoughness.roughnessFactor = 0.1;
            pbrMetallicRoughness.metallicFactor = 0.1;
            pbrMetallicRoughness.baseColorFactor = new float[4] { 0.4f, 0.4f, 0.4f, 0.45f };
            pbrMetallicRoughness.roughnessFactor = 0.1;
            pbrMetallicRoughness.metallicFactor = 0.1;
            material.pbrMetallicRoughness = pbrMetallicRoughness;
            root.materials.Add(material);




            // foreach world

            //     create flexible byte buffer: create add getcursor getbytearray delete

            //     foreach top-level semantic object, render to gltf structures
            //          
            //          render this
            //              node
            //              material
            //              mesh
            //              vertices, normals, uvs, indices to buffer
            //              accessor
            //              bufferview-s
            //              

            //          render children

            // serialize to JSON and/or binary
            // save binary buffer data
        }
        public int RenderEntity(World world, glTFRoot root, Entities.Entity entity)
        {
            int nMeshes = 0;
            foreach(SharedGeometry.Mesh eMesh in entity.Meshes)
            {
                // add a node
                glTFInterface.Node node = new glTFInterface.Node();
                node.name = entity.Name;
                node.mesh = root.meshes.Count;
                root.nodes.Add(node);
                root.scenes[0].nodes.Add(root.nodes.Count - 1); 

                // get vertices, normals, indices, uvs

                // make a material
                glTFInterface.Material material = new glTFInterface.Material();
                material.name = entity.Material.Name;
                material.alphaMode = entity.Material.AlphaMode;
                material.doubleSided = entity.Material.DoubleSided;
                PbrMetallicRoughness pbrMetallicRoughness = new PbrMetallicRoughness();
                pbrMetallicRoughness.roughnessFactor = entity.Material.PBRMetallicRoughness.RoughnessFactor;
                pbrMetallicRoughness.metallicFactor = entity.Material.PBRMetallicRoughness.MetallicFactor;
                pbrMetallicRoughness.baseColorFactor = entity.Material.PBRMetallicRoughness.BaseColorFactor; // new double[4] { 0.4, 0.4, 0.4, 0.45 };
                material.pbrMetallicRoughness = pbrMetallicRoughness;
                root.materials.Add(material);

                // add a mesh
                glTFInterface.Mesh mesh = new glTFInterface.Mesh();
                mesh.name = eMesh.Name;
                MeshPrimitive meshPrimitive = new MeshPrimitive();
                meshPrimitive.attributes.Add("POSITION", root.binChunks.NumberOfChunks);
                meshPrimitive.attributes.Add("NORMAL", root.binChunks.NumberOfChunks + 1);
                meshPrimitive.indices = root.binChunks.NumberOfChunks + 2;
                meshPrimitive.material = root.materials.Count - 1; ;
                mesh.primitives.Add(meshPrimitive);
                int nMesh = root.meshes.Count;
                root.meshes.Add(mesh);
                //nMeshes++;

                //    store binchunks
                int nVertices = eMesh.Vertices.Count;
                int nVerticesBytes = nVertices * 4 * 3;
                // assemble normal info, get start and end bytes
                int nNormals = eMesh.Normals.Count;
                int nNormalsBytes = nNormals * 4 * 3;
                // assemble index info
                int nIndices = eMesh.Indices.Count;
                int nIndicesBytes = nIndices * 2 * 3;
                // allocate a single buffer for this mesh
                // add vertices
                // ***** following is a little awkward but will sort out and refactor when it's working
                double minVertexX, minVertexY, minVertexZ, maxVertexX, maxVertexY, maxVertexZ;
                float[] fVTemp = CopyVec3DArray2FloatArray(eMesh.Vertices,
                    out minVertexX, out maxVertexX, out minVertexY, out maxVertexY, out minVertexZ, out maxVertexZ);
                /* //float[] fVTemp = new float[nVertices * 3];
                int nFloat = 0;
                for (int nVertex = 0; nVertex < nVertices; nVertex++)
                {
                    var u = eMesh.Vertices[nVertex];
                    if (u.Item1 < minVertexX)
                    {
                        minVertexX = u.Item1;
                    }
                    if (u.Item1 > maxVertexX)
                    {
                        maxVertexX = u.Item1;
                    }
                    if (u.Item2 < minVertexY)
                    {
                        minVertexY = u.Item2;
                    }
                    if (u.Item2 > maxVertexY)
                    {
                        maxVertexY = u.Item2;
                    }
                    if (u.Item3 < minVertexZ)
                    {
                        minVertexZ = u.Item3;
                    }
                    if (u.Item3 > maxVertexZ)
                    {
                        maxVertexZ = u.Item3;
                    }
                    fVTemp[nFloat++] = (float)u.Item1;
                    fVTemp[nFloat++] = (float)u.Item2;
                    fVTemp[nFloat++] = (float)u.Item3;
                }
                */
                int nVertexStart = root.binChunks.ByteOffset;
                root.binChunks.AddChunk(fVTemp);

                // add normals
                double minNormalX, minNormalY, minNormalZ, maxNormalX, maxNormalY, maxNormalZ;
                float[] fNTemp = CopyVec3FArray2FloatArray(eMesh.Normals,
                    out minNormalX, out maxNormalX, out minNormalY, out maxNormalY, out minNormalZ, out maxNormalZ);
                /*
                float[] fNTemp = new float[nNormals * 3];
                int nFloat = 0;
                double minNormalX = 1000.0;
                double minNormalY = 1000.0;
                double minNormalZ = 1000.0; ;
                double maxNormalX = -1000.0;
                double maxNormalY = -1000.0; 
                double maxNormalZ = -1000.0;
                for (int nNormal = 0; nNormal < nNormals; nNormal++)
                {
                    var u = eMesh.Normals[nNormal];
                    if (u.Item1 < minNormalX)
                    {
                        minNormalX = u.Item1;
                    }
                    if (u.Item1 > maxNormalX)
                    {
                        maxNormalX = u.Item1;
                    }
                    if (u.Item2 < minNormalY)
                    {
                        minNormalY = u.Item2;
                    }
                    if (u.Item2 > maxNormalY)
                    {
                        maxNormalY = u.Item2;
                    }
                    if (u.Item3 < minNormalZ)
                    {
                        minNormalX = u.Item3;
                    }
                    if (u.Item3 > maxNormalZ)
                    {
                        maxNormalX = u.Item3;
                    }
                    fNTemp[nFloat++] = (float)u.Item1;
                    fNTemp[nFloat++] = (float)u.Item2;
                    fNTemp[nFloat++] = (float)u.Item3;
                }*/
                int nNormalsStart = root.binChunks.ByteOffset;
                root.binChunks.AddChunk(fNTemp);

                // add indices
                int minIndex, maxIndex;
                ushort[] iTemp = CopyUShortTripleArray2UShortArray(eMesh.Indices, out minIndex, out maxIndex);
                /*
                ushort[] iTemp = new ushort[nIndices * 3];
                int nUShort = 0;
                for (int nIndex = 0; nIndex < nIndices; nIndex++)
                {
                    var u = eMesh.Indices[nIndex];
                    iTemp[nUShort++] = (ushort)u.Item1;
                    iTemp[nUShort++] = (ushort)u.Item2;
                    iTemp[nUShort++] = (ushort)u.Item3;
                }
                */
                int nIndicesStart = root.binChunks.ByteOffset;
                root.binChunks.AddChunk(iTemp);

                //    create accessors
                glTFInterface.Accessor accessor = new glTFInterface.Accessor();
                accessor.name = "vertices";
                accessor.bufferView = root.bufferViews.Count;
                accessor.componentType = 5126;
                accessor.count = nVertices;
                accessor.type = "VEC3";
                // calculate minima and maxima

                accessor.max.Add(maxVertexX);
                accessor.max.Add(maxVertexY);
                accessor.max.Add(maxVertexZ);
                accessor.min.Add(minVertexX);
                accessor.min.Add(minVertexY);
                accessor.min.Add(minVertexZ);
                root.accessors.Add(accessor);

                accessor = new glTFInterface.Accessor();
                accessor.name = "normals";
                accessor.bufferView = root.bufferViews.Count + 1;
                accessor.componentType = 5126;
                accessor.count = nNormals;
                accessor.type = "VEC3";
                root.accessors.Add(accessor);

                accessor = new glTFInterface.Accessor();
                accessor.name = "indices";
                accessor.bufferView = root.bufferViews.Count  + 2;
                accessor.componentType = 5123;
                accessor.count = nIndices * 3;
                accessor.type = "SCALAR";
                root.accessors.Add(accessor);


                //    create bufferviews
                glTFInterface.BufferView bufferView = new glTFInterface.BufferView();
                bufferView.name = "vertices";
                bufferView.buffer = root.buffers.Count ;
                bufferView.target = 34962;
                bufferView.byteOffset = nVertexStart;
                bufferView.byteLength = nVerticesBytes;
                root.bufferViews.Add(bufferView);

                bufferView = new glTFInterface.BufferView();
                bufferView.name = "normals";
                bufferView.buffer = root.buffers.Count;
                bufferView.target = 34962;
                bufferView.byteOffset = nNormalsStart;
                bufferView.byteLength = nNormalsBytes;
                root.bufferViews.Add(bufferView);

                bufferView = new glTFInterface.BufferView();
                bufferView.name = "indices";
                bufferView.buffer = root.buffers.Count;
                bufferView.target = 34963;
                bufferView.byteOffset = nIndicesStart;
                bufferView.byteLength = nIndicesBytes;
                root.bufferViews.Add(bufferView);
            }
            Console.WriteLine("\tRendering Entity \"" + entity.Name + "\": Meshes: " + nMeshes.ToString());
            return nMeshes;
        }
        public void RenderWorld(World world, glTFRoot root)
        {
            Console.WriteLine("Rendering World \"" + world.Name + "\"");
            // clear binchunks
            root.binChunks.Clear();
            // render each entity to glTF interface
            foreach (Entities.Entity entity in world.Entities)
            {
                //if (entity.Name != "Bounding Sphere")
                {
                    RenderEntity(world, root, entity);
                }
            }
            if (root.binChunks.ByteOffset < 1)
            {
                Console.WriteLine("\"" + world.Name + "\" is empty");
                return;
            }
            // create new buffer - one buffer per world
            glTFInterface.Buffer buffer = new glTFInterface.Buffer();
            root.buffers.Add(buffer);
            // set buffer bytelength
            buffer.byteLength = root.binChunks.ByteOffset;
            // write buffer
            buffer.name = world.Name + " buffer";
            buffer.uri = world.Name + "." + root.instanceID + ".bin"; 
                //(world.Name + "." + Path.GetFileName( root.uri).Replace("world","").Replace("gltf", "bin") + ".bin").Replace(" ", "_");
            root.binChunks.WriteChunks(root.uri, world.Name, root.instanceID);
        }

        //   Render each world and contained entities
        public void GenerateglTF(string basePath, string instanceID)
        {
            // *** Create glTF root ***
            // Remove any extension
            string uriBase = basePath; // this is either an http(s) address ending in a '/' or a filesystem path ending in '/' or '\'
            glTFRoot root = new glTFRoot();
            root.uri = uriBase;
            root.instanceID = instanceID;
            root.extensionsRequired.Add("OGC_Semantic_Core");
            root.extensionsUsed.Add("OGC_Semantic_Core");
            root.extensionsUsed.Add("KHR_materials_transmission");
            root.asset.generator = "GSR00.0.5.4";
            root.asset.version = "2.0";
            root.scene = 0;
            GeoPose.BasicYPR aPose = this.FramePose;
            double radius = this.Size.Value;
            Scene scene = new Scene();
            scene.name = "Scene";
            // this index gets added for each added node in renderentityscene.nodes.Add(0);
            OGC_SemanticCore semanticCore = new OGC_SemanticCore("Test", "https://citygml.info/OGC-Khronos-Forum/Prototype/Proto.gltf",
             aPose.Position.lat, aPose.Position.lon, aPose.Position.h, aPose.Angles.yaw, aPose.Angles.pitch, aPose.Angles.roll, radius);
            scene.extensions = new Dictionary<string, object>();
            scene.extensions.Add("OGC_Semantic_Core", semanticCore);
            root.scenes.Add(scene);

            // Render each world
            foreach (World aWorld in WorldSet)
            {
                RenderWorld(aWorld, root);
            }
            root.WriteglTF();
        }

    }
}
