using g4;

namespace Entities
{
    public class Entity
    {
        // what world does this entity live in?
        public Verses.World? world { get; set; } = null;
        // Friendly name of entity
        public string Name { get; set; } = "";
        // Unique name of entity
        public string ID { get; set; } = "";
        // Parent entity - null only for root
        public Entity? parentEntity { get; set; } = null;
        // Parent entity - null only for root
        public List<Entity>? childEntities { get; set; } = new List<Entity>();
        // Semantic class of entity
        public SemanticClasses.SemanticClass? SemanticEntityClass { get; set; } = null;
        // pose inside of world frame
        public GeoPose.GeoPose? Pose { get; set; }
        // mesh(es) with geometry of this entity
        // in many graphics environments, each mesh maps to a distinct node in the scenegraph
        // Semantic classes have a generator that can create the mesh(es) from a template and parameters
        public List<SharedGeometry.Mesh> Meshes { get; set; } = new List<SharedGeometry.Mesh>();
        public SemanticObjects.Material Material { get; set; } = new SemanticObjects.Material(); 
        //public MeshGenerator? Mesh { get; set; } = null; 
    }
}
