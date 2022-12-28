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
                string poseString = (e.Pose.GetType().ToString() == "GeoPose.BasicYPR") ? ((GeoPose.BasicYPR)(e.Pose)).ToJSON() : ((GeoPose.Advanced)(e.Pose)).ToJSON();
                sw.WriteLine("|" + e.Name + "|" + e.ID + "|" + e.SemanticEntityClass.GetType().Name + "|" + poseString + "|");
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
            aPose.position.lat, aPose.position.lon, aPose.position.h, aPose.angles.yaw, aPose.angles.pitch, aPose.angles.roll, radius);

            // add base scene
            Scene scene = new Scene();
            scene.name = sceneName;
            scene.extensions = new Dictionary<string, object>();
            scene.extensions.Add("OGC_Semantic_Core", semanticCore);
            scene.nodes.Add(0);
            //root.scenes = new Scene[1];
            root.scenes.Add(scene);

            // create and render verseglobe to gltf structures
            glTFInterface.Node node = new glTFInterface.Node();
            node.name = "Bounding Sphere";
            node.mesh = 0;
            //root.nodes = new glTFInterface.Node[1];
            root.nodes.Add(node);

            glTFInterface.Material material = new glTFInterface.Material();
            material.name = "Transparent Dome";
            material.alphaMode = "BLEND";
            material.doubleSided = true;
            PbrMetallicRoughness pbrMetallicRoughness = new PbrMetallicRoughness();
            pbrMetallicRoughness.roughnessFactor = 0.1;
            pbrMetallicRoughness.metallicFactor = 0.1;
            pbrMetallicRoughness.baseColorFactor = new double[4] { 0.4, 0.4, 0.4, 0.45 };
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
        public void GenerateglTF(string fileName)
        {
            // *** render world as glTF
            // glTFRoot root = RenderRoot(fileName)
            string bufferFileName = fileName.Replace(".gltf", ".bin");
            glTFRoot root = new glTFRoot();
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
            scene.nodes.Add(0);
            OGC_SemanticCore semanticCore = new OGC_SemanticCore("Test", "https://citygml.info/OGC-Khronos-Forum/Prototype/Proto.gltf",
             aPose.position.lat, aPose.position.lon, aPose.position.h, aPose.angles.yaw, aPose.angles.pitch, aPose.angles.roll, radius);
            //scene.extensions = new Extension[1];
            scene.extensions = new Dictionary<string, object>();
            scene.extensions.Add("OGC_Semantic_Core", semanticCore);
            //root.scenes = new Scene[1];
            root.scenes.Add(scene);

            glTFInterface.Node node = new glTFInterface.Node();
            node.name = "Bounding Sphere";
            node.mesh = 0;
            //root.nodes = new glTFInterface.Node[1];
            root.nodes.Add(node);

            glTFInterface.Material material = new glTFInterface.Material();
            material.name = "Transparent Dome";
            material.alphaMode = "BLEND";
            material.doubleSided = true;
            PbrMetallicRoughness pbrMetallicRoughness = new PbrMetallicRoughness();
            pbrMetallicRoughness.roughnessFactor = 0.1;
            pbrMetallicRoughness.metallicFactor = 0.1;
            pbrMetallicRoughness.baseColorFactor = new double[4] { 0.4, 0.4, 0.4, 0.45 };
            pbrMetallicRoughness.roughnessFactor = 0.1;
            pbrMetallicRoughness.metallicFactor = 0.1;
            material.pbrMetallicRoughness = pbrMetallicRoughness;
            root.materials.Add(material);

            string wName = this.WorldSet[0].Name;
            MeshGenerator? wMesh = this.WorldSet[0].Entities[0].Mesh;
            // start a buffer
            BinChunkStore binChunks = new BinChunkStore();
            // assemble vertex info, get start end end bytes
            int nVertices = wMesh.vertices.Count;
            int nVerticesBytes = nVertices * 4 * 3;
            int nVertexStart = 0;
            int nVertexEnd = nVertexStart + nVerticesBytes - 1;
            // assemble normal info, get start and end bytes
            int nNormals = wMesh.normals.Count;
            int nNormalsBytes = nNormals * 4 * 3;
            int nNormalsStart = nVertexEnd + 1;
            int nNormalsEnd = nNormalsStart + nNormalsBytes - 1;
            // assemble index info
            int nIndices = wMesh.triangles.Count;
            int nIndicesBytes = nIndices * 2 * 3;
            int nIndicesStart = nNormalsEnd + 1;
            int nIndicesEnd = nIndicesStart + nIndicesBytes - 1;
            // allocate a single buffer for this mesh
            //byte[] tbuffer = new byte[nVerticesBytes + nNormalsBytes + nIndicesBytes];
            byte[] tbuffer = new byte[nVerticesBytes];
            // add vertices
            // ***** following is a little awkward but will sort out and refactor when it's working
            float[] fVTemp = new float[nVertices * 3];
            int nFloat = 0;
            for (int nVertex = 0; nVertex < nVertices; nVertex++)
            {
                var u = wMesh.vertices[nVertex];
                fVTemp[nFloat++] = (float)u[0];
                fVTemp[nFloat++] = (float)u[1];
                fVTemp[nFloat++] = (float)u[2];
            }
            binChunks.AddChunk(fVTemp);

            // add normals
            tbuffer = new byte[nNormalsBytes];
            float[] fNTemp = new float[nNormals * 3];
            nFloat = 0;
            for (int nNormal = 0; nNormal < nNormals; nNormal++)
            {
                var u = wMesh.normals[nNormal];
                fNTemp[nFloat++] = (float)u[0];
                fNTemp[nFloat++] = (float)u[1];
                fNTemp[nFloat++] = (float)u[2];
            }
            binChunks.AddChunk(fNTemp);

            // add indices
            tbuffer = new byte[nIndicesBytes];
            ushort[] iTemp = new ushort[nIndices * 3];
            double aMin = 1000000000000.0;
            double aMax = -aMin;
            int nInt = 0;
            for (int nIndex = 0; nIndex < nIndices; nIndex++)
            {
                var u = wMesh.triangles[nIndex];
                if (u.Length < 1)
                {
                    int jj = 0;
                }
                iTemp[nInt++] = (ushort)u[0];
                iTemp[nInt++] = (ushort)u[1];
                iTemp[nInt++] = (ushort)u[2];

                double xp = wMesh.vertices[u[0]].x;
                double yp = wMesh.vertices[u[0]].y;
                double zp = wMesh.vertices[u[0]].z;
                double x1 = wMesh.vertices[u[1]].x - xp;
                double y1 = wMesh.vertices[u[1]].y - yp;
                double z1 = wMesh.vertices[u[1]].z - zp;
                double x2 = wMesh.vertices[u[2]].x - xp;
                double y2 = wMesh.vertices[u[2]].y - yp;
                double z2 = wMesh.vertices[u[2]].z - zp;

                double xo = y1 * z2 - y2 * z1;
                double yo = -x1 * z2 + x2 * z1;
                double zo = x1 * y2 - x2 * y1;

                double a2 = xo * xo + yo * yo + zo * zo;
                double area = Math.Sqrt(a2);
                if (area < 0.00001)
                {
                    int jj = 0;
                }
                if (area < aMin)
                {
                    aMin = area;
                }
                if (area > aMax)
                {
                    aMax = area;
                }
            }
            binChunks.AddChunk(iTemp);

            // meshes
            glTFInterface.Mesh mesh = new glTFInterface.Mesh();
            mesh.name = "Bounding Dome";
            MeshPrimitive meshPrimitive = new MeshPrimitive();
            meshPrimitive.attributes.Add("POSITION", 0);
            meshPrimitive.attributes.Add("NORMAL", 1);
            meshPrimitive.indices = 2;
            meshPrimitive.material = 0;
            mesh.primitives.Add(meshPrimitive);
            root.meshes.Add(mesh);

            // accessors
            glTFInterface.Accessor accessor = new glTFInterface.Accessor();
            accessor.name = "one";
            accessor.bufferView = 0;
            accessor.componentType = 5126;
            accessor.count = nVertices;
            accessor.type = "VEC3";
            accessor.max.Add(198.0);
            accessor.max.Add(198.0);
            accessor.max.Add(198.0);
            accessor.min.Add(-198.0);
            accessor.min.Add(-198.0);
            accessor.min.Add(-198.0);
            root.accessors.Add(accessor);

            accessor = new glTFInterface.Accessor();
            accessor.name = "two";
            accessor.bufferView = 1;
            accessor.componentType = 5126;
            accessor.count = nNormals;
            accessor.type = "VEC3";
            root.accessors.Add(accessor);

            accessor = new glTFInterface.Accessor();
            accessor.name = "three";
            accessor.bufferView = 2;
            accessor.componentType = 5123;
            accessor.count = nIndices * 3;
            accessor.type = "SCALAR";
            root.accessors.Add(accessor);

            // bufferViews
            glTFInterface.BufferView bufferView = new glTFInterface.BufferView();
            bufferView.name = "one";
            bufferView.buffer = 0;
            bufferView.target = 34962;
            bufferView.byteOffset = 0;
            bufferView.byteLength = nVerticesBytes;
            root.bufferViews.Add(bufferView);

            bufferView = new glTFInterface.BufferView();
            bufferView.name = "two";
            bufferView.buffer = 0;
            bufferView.target = 34962;
            bufferView.byteOffset = nVerticesBytes;
            bufferView.byteLength = nNormalsBytes;
            root.bufferViews.Add(bufferView);

            bufferView = new glTFInterface.BufferView();
            bufferView.name = "three";
            bufferView.buffer = 0;
            bufferView.target = 34963;
            bufferView.byteOffset = nVerticesBytes + nNormalsBytes;
            bufferView.byteLength = nIndicesBytes;
            root.bufferViews.Add(bufferView);

            // buffers
            glTFInterface.Buffer buffer = new glTFInterface.Buffer();
            buffer.name = "Transparent Dome";
            buffer.uri = Path.GetFileName(bufferFileName);
            buffer.byteLength = nVerticesBytes + nNormalsBytes + nIndicesBytes;
            root.buffers.Add(buffer);
            // end of render root

            // ***** update buffer byte length 
            root.Lock();
            // write binary buffer file
            binChunks.WriteChunks(bufferFileName);
            binChunks.Clear();

            string glTF = root.ToJSON();

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            // output invariant part
            sw.Write(glTF);
            sw.Close();
        }

    }
}
