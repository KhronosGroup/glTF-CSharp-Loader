using g4;

namespace Entities
{
    public class Entity
    {
        public Entity()
        {

        }
        public Entity(Verses.World myWorld, GeoPose.ENUPose pose, string name, SemanticClasses.SemanticClass semanticClass)
        {

        }
        public Entity(Verses.World world, double east, double north, double up, double yaw, double pitch, double roll,
            string name, SemanticClasses.SemanticClass semanticClass)
        {
            World = world;
            Name = name;
            SemanticEntityClass = semanticClass;
            GeoPose.ENUPose pose = new GeoPose.ENUPose();
            pose.Position.East = 0.0;
            pose.Position.North = 0.0;
            pose.Position.Up = 0.0;
            pose.Angles.yaw = 0.0;
            pose.Angles.pitch = 0.0;
            pose.Angles.roll = 0.0;
            Pose = pose;
        }
        // what world does this entity live in?
        public Verses.World? World { get; set; } = null;
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
        public GeoPose.ENUPose? Pose { get; set; }
        // mesh(es) with geometry of this entity
        // in many graphics environments, each mesh maps to a distinct node in the scenegraph
        // Semantic classes have a generator that can create the mesh(es) from a template and parameters
        public List<SharedGeometry.Mesh> Meshes { get; set; } = new List<SharedGeometry.Mesh>();
        public Entities.Material Material { get; set; } = new Entities.Material(); 
        //public MeshGenerator? Mesh { get; set; } = null; 
    }
}
