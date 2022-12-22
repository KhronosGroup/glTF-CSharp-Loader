using g4;
namespace Entities
{
    public class Entity
    {
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";
        public SemanticClasses.SemanticClass SemanticEntityClass { get; set; } = new SemanticClasses.Generic();
        public GeoPose.GeoPose? Pose { get; set; } 
        public SharedGeometry.Mesh[] Meshes { get; set; } = new SharedGeometry.Mesh[0];
        public MeshGenerator? Mesh { get; set; } = null; 
    }
}
