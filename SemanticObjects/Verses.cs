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
    public abstract class World
    {
        public string Name { get; set; } = "";
        public string ReferenceFrame { get; set; } = "";
        public Entities.Entity[] Entities = new Entities.Entity[0];
        public void ListElementsAsMarkDown()
        {
            // name
            Console.WriteLine("\r\nName: " + Name);
            // reference frame
            Console.WriteLine("ReferenceFrame: " + ReferenceFrame);
            // entities
            foreach(Entities.Entity e in Entities)
            {
                Console.WriteLine("Entity Name:" + e.Name);
                Console.WriteLine("Entity ID:  " + e.ID);
            }
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
        public OutsideOfAnyWorld OmniVerse { get; set; } = new OutsideOfAnyWorld();
        public StaticWorld Background { get; set; } = new StaticWorld();
        public DynamicWorld Foreground { get; set; } = new DynamicWorld();
        public VirtualWorld VirtualParts { get; set; } = new VirtualWorld();
        public void ListElementsAsMarkDown()
        {
            Console.WriteLine("Use Case 1 World");
            Console.WriteLine("Created: " + OmniVerse.Now.ToString());
            Background.ListElementsAsMarkDown();
            Foreground.ListElementsAsMarkDown();
            VirtualParts.ListElementsAsMarkDown();
        }
    }
}
