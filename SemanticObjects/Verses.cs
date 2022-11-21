namespace Verses
/// <summary>
/// The Omniverse is the source of all externally defined information and universal relationships and interactions between entities
/// there are Oracles that provide information and Animators that alter conditions in the real world. Both operate without any explanation or known mechanism.
/// </summary>
{
    public class OutsideOfAnyWorld
    {

    }
    public class World
    {
        public string Name { get; set; } = "";
        public string ReferenceFrame { get; set; } = "";
    }
    public class StaticWorld
    {
        public Entities.Entity[] StaticEntities = new Entities.Entity[0];
        public Entities.Entity[] DynamicEntities = new Entities.Entity[0];
        public Entities.Entity[] VirtualEntities = new Entities.Entity[0];
    }

    public class DynamicWorld
    {

    }
    public class VirtualWorld
    {
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
        public OutsideOfAnyWorld OmniVerse { get; set; } = new OutsideOfAnyWorld();
        public StaticWorld Background { get; set; } = new StaticWorld();
        public DynamicWorld Foreground { get; set; } = new DynamicWorld();
        public VirtualWorld VirtualParts { get; set; } = new VirtualWorld();
    }
}
