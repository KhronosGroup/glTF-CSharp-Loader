
namespace Entities
{
    public class Entity
    {
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";
        public SemanticClasses.SemanticClass SemanticEntityClass { get; set; } = new SemanticClasses.Generic();
        public Geometry.GeoPose Pose { get; set; } = new Geometry.GeoPose();
        public Geometry.Mesh[] Meshes { get; set; } = new Geometry.Mesh[0]; 
    }
}
