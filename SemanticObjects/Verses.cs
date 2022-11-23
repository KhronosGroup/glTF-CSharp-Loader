using System.Collections;
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
        public List<Entities.Entity> Entities = new List<Entities.Entity>();
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
                Console.WriteLine("Semantic Class: " + e.SemanticEntityClass.GetType().Name);
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
        public OutsideOfAnyWorld OmniVerse { get; set; } = new OutsideOfAnyWorld();
        public StaticWorld Background { get; set; } = new StaticWorld();
        public DynamicWorld Foreground { get; set; } = new DynamicWorld();
        public VirtualWorld VirtualParts { get; set; } = new VirtualWorld();
        public void ListElementsAsMarkDown()
        {
            Console.WriteLine("Integrated World: " + Name);
            Console.WriteLine("Created: " + OmniVerse.Now.ToString());
            Background.ListElementsAsMarkDown();
            Foreground.ListElementsAsMarkDown();
            VirtualParts.ListElementsAsMarkDown();
        }
    }
}
