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
        public void ListElementsAsMarkDown(StreamWriter sw)
        {
            // name
            sw.WriteLine("\r\n## World\r\n \r\nName: " + Name);
            // reference frame
            sw.WriteLine("\r\n \r\nReferenceFrame: " + ReferenceFrame);
            // entities
            foreach(Entities.Entity e in Entities)
            {
                sw.WriteLine("\r\n### Entity Name:" + e.Name + "\r\n ");
                sw.WriteLine("\r\nEntity ID:  " + e.ID + "\r\n ");
                sw.WriteLine("\r\nSemantic Class: " + e.SemanticEntityClass.GetType().Name + "\r\n ");
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
            string fileName = "c:/temp/models/list.md";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine("# Integrated World: " + Name);
            sw.WriteLine("Created: " + OmniVerse.Now.ToString());
            sw.WriteLine("\r\n# Background World\r\n");
            Background.ListElementsAsMarkDown(sw);
            sw.WriteLine("\r\n# Foreground World\r\n");
            Foreground.ListElementsAsMarkDown(sw);
            sw.WriteLine("\r\n# Virtual World\r\n");
            VirtualParts.ListElementsAsMarkDown(sw);
            sw.Close();
        }
    }
}
