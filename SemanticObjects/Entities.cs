
namespace Entities
{
    public class Entity
    {
        // name
        // id
        // location, orpientation
        // geometry
        // appearance
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";
        public Geometry.GeoPose GeoPose { get; set; } = new Geometry.GeoPose();
        public Geometry.Mesh GeometryCollection { get; set; }
        public SemanticClasses.SemanticClass SemanticEntityClass { get; set; } = new SemanticClasses.Generic();
    }
}
