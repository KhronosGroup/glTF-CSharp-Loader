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
        public void GenerateglTF(string fileName)
        {
            // *** render world as glTF
            glTFRoot root = new glTFRoot();
            root.extensionsRequired = (new string[] { "OGC_Semantic_Core" });
            root.extensionsUsed = (new string[] { "OGC_Semantic_Core", "KHR_materials_transmission" });
            root.asset.generator = "GSR00.0.5.4";
            root.asset.version = "2.0";
            root.scene = 0;
            GeoPose.BasicYPR aPose = this.FramePose;
            double radius = this.Size.Value;
            Scene scene = new Scene();
            scene.name = "Scene";
            scene.nodes = (new int[] { 0 });
            OGC_SemanticCore semanticCore = new OGC_SemanticCore("Test", "https://citygml.info/OGC-Khronos-Forum/Prototype/Proto.gltf",
             aPose.position.lat, aPose.position.lon, aPose.position.h, aPose.angles.yaw, aPose.angles.pitch, aPose.angles.roll, radius);
            //scene.extensions = new Extension[1];
            scene.extensions = new Dictionary<string, object>();
            scene.extensions.Add("OGC_Semantic_Core", semanticCore);
            root.scenes = new Scene[1];
            root.scenes[0] = scene;

            glTFInterface.Node node = new glTFInterface.Node();
            node.name = "Bounding Sphere";
            node.mesh = 0;
            root.nodes = new glTFInterface.Node[1];
            root.nodes[0] = node;

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
            root.materials = new Material[1];
            root.materials[0] = material;

            /*
             * 
	"meshes": [
		{
			"name": "OS Southampton Bounds",
			"primitives": [
				{
					"attributes": {
						"POSITION": 0,
						"NORMAL": 1
					},
					"indices": 3,
					"material": 0
				}
			]
		}
	],
             * 
             */

            string wName = this.WorldSet[0].Name;
            MeshGenerator? wMesh = this.WorldSet[0].Entities[0].Mesh;

            // get length of POSITIONs

            // get length of NORMALs

            // get length of indices


            // start a buffer

            // add vertices, get tart end end bytes
            int nVertices = wMesh.vertices.Count;
            int nVerticesBytes = nVertices * 4 * 3;
            int nVertexStart = 0;
            int nVertexEnd = nVertexStart + nVertices * 4 * 3 - 1;
            // add normals, get start and end bytes
            int nNormals = wMesh.normals.Count;
            int nNormalsBytes = nNormals * 4 * 3;
            int nNormalsStart = nVertexEnd + 1;
            int nNormalsEnd = nNormalsStart + nNormals * 4 * 3 - 1;
            // add indices
            int nIndices = wMesh.triangles.Count;
            int nIndicesBytes = nVertices * 2 * 3;
            int nIndicesStart = nNormalsEnd + 1;
            int nIndicesEnd = nIndicesStart + nIndices * 2 * 3;
            byte[] tbuffer = new byte[nIndicesEnd + 1];
            float[] fTemp = new float[nVertices*3];
            int nFloat = 0;
            for(int nVertex = 0; nVertex < nVertices; nVertex++)
            {
                var u = wMesh.vertices[nVertex];
                fTemp[nFloat++] = (float)u[0];
                fTemp[nFloat++] = (float)u[1];
                fTemp[nFloat++] = (float)u[2];
            }
            if (wMesh.vertices != null)
            {
                System.Buffer.BlockCopy(fTemp, 0, tbuffer, 0, nVerticesBytes);
            }
            string newBase64 = System.Convert.ToBase64String(tbuffer, 0, nIndicesEnd + 1, Base64FormattingOptions.None);
            // meshes
            glTFInterface.Mesh mesh = new glTFInterface.Mesh();
            mesh.name = "Bounding Dome";
            MeshPrimitive meshPrimitive = new MeshPrimitive();
            meshPrimitive.attributes.Add("POSITION", 0);
            meshPrimitive.attributes.Add("NORMAL", 1);
            meshPrimitive.indices = 3;
            meshPrimitive.material = 0;
            mesh.primitives = new MeshPrimitive[1];
            mesh.primitives[0] = meshPrimitive;
            root.meshes = new glTFInterface.Mesh[1];
            root.meshes[0] = mesh;

            // accessors
            root.accessors = new glTFInterface.Accessor[4];
            glTFInterface.Accessor accessor = new glTFInterface.Accessor();
            accessor.name = "one";
            accessor.bufferView = 0;
            accessor.componentType = 5126;
            accessor.count = 24;
            accessor.type = "VEC3";
            accessor.max = new double[3] {  140, 60,  160 };
            accessor.min = new double[3] { -140, 10, -160 };
            root.accessors[0] = accessor;

            accessor = new glTFInterface.Accessor();
            accessor.name = "two";
            accessor.bufferView = 1;
            accessor.componentType = 5126;
            accessor.count = 24;
            accessor.type = "VEC3";
            root.accessors[1] = accessor;

            accessor = new glTFInterface.Accessor();
            accessor.name = "three";
            accessor.bufferView = 2;
            accessor.componentType = 5126;
            accessor.count = 24;
            accessor.type = "VEC2";
            root.accessors[2] = accessor;

            accessor = new glTFInterface.Accessor();
            accessor.name = "four";
            accessor.bufferView = 3;
            accessor.componentType = 5123;
            accessor.count = 24;
            accessor.type = "SCALAR";
            root.accessors[3] = accessor;

            // bufferViews
            root.bufferViews = new glTFInterface.BufferView[4];
            glTFInterface.BufferView bufferView = new glTFInterface.BufferView();
            bufferView.name = "one";
            bufferView.buffer = 0;
            bufferView.target = 34962;
            bufferView.byteOffset = 0;
            bufferView.byteLength = 288;
            root.bufferViews[0] = bufferView;

            bufferView = new glTFInterface.BufferView();
            bufferView.name = "two";
            bufferView.buffer = 0;
            bufferView.target = 34962;
            bufferView.byteOffset = 288;
            bufferView.byteLength = 288;
            root.bufferViews[1] = bufferView;

            bufferView = new glTFInterface.BufferView();
            bufferView.name = "three";
            bufferView.buffer = 0;
            bufferView.target = 34962;
            bufferView.byteOffset = 576;
            bufferView.byteLength = 192;
            root.bufferViews[2] = bufferView;

            bufferView = new glTFInterface.BufferView();
            bufferView.name = "four";
            bufferView.buffer = 1;
            bufferView.target = 34963;
            bufferView.byteOffset = 768;
            bufferView.byteLength = 72;
            root.bufferViews[3] = bufferView;

            // buffers
            glTFInterface.Buffer buffer = new glTFInterface.Buffer();
            buffer.name = "Transparent Dome";
            root.buffers = new glTFInterface.Buffer[1];
            root.buffers[0] = buffer;

            //root.scenes[0].extensions = new Extension[1];

            // *** save glTF rendering as file

            string glTF = root.ToJSON();

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            // output invariant part
            sw.Write(glTF);
            // output linkage to parent world

            // foreach object

            //   foreach world


            //     build nodes


            //     build materials


            //      build meshes


            //      build accessors


            //      build bufferviews


            //      build buffers
            /*
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
            //sw.WriteLine(footer);
            sw.Close();
        }
    }
}
