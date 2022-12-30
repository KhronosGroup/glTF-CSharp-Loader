using SharedGeometry;
using g4;
/*
 * 
 * Use Case: Urban Person-Vehicle Rendezvous
This is a slightly generalized form of the "shared ride" rendezvous example.

Begin Real World
===================
Universe: there are Oracles that provide information and Animators that alter conditions in the real world. Both operate without any explanation or known mechanism.

Environment: streets, buildings, pedestrian walkways

The [static real] Environment is an urban space with a few streets, buildings, and pedestrian walkways bounding the buildings and bordering the streets. The buildings are stereotypical, with at least four levels and outer walls containing windows and ground-level doors. The streets are sterotypical, with at least one lane of vehicle traffic in each direction, following the right-hand driving convention. The streets have marked pedestrian crossings, traffic signals at some junctions, and signs. The entire horizontal surface of the Environment is partitioned into non-overlapping buildings, pedestrian walkways, and streets.

Actors: vehicles, people, signal lights

The active real environment contains walking Persons [people] and moving Vehicles. The walking Persons are located on the Pedestrian-Walkways or Pedestrian-Crossings. 
The moving Vehicles are located on the Street surface. The moving Vehicles are capable of containing Persons [people]. A Person can Teleport to/from a Vehicle either from/to a Pedestrian-Walkway or from/to a Pedestrian-Crossing if and only if the Vehicle is touching or intersecting a Pedestrian-Crossing or Pedestrian-Walkway and Close to the Person. 
Signal lights have a state controlled by an Oracle. The possible and mutually exclusive signal light states are red, yellow, green. Signal light state may have a visual manifestation.

Sensors: the topmost part of every Person has a pair of Cameras.

=================
End Real World

Begin Virtual World

====================
Virtual Elements: visible ID Tag

An Actor may have a visible ID Tag located above it, visible in all directions, but possibly occluded from view by elements of the Environment or Actors.

 * 
 * The [static real] Environment is an urban space with a few streets, buildings, and pedestrian walkways bounding the buildings and bordering the streets. 
 * The buildings are stereotypical, with at least four levels and outer walls containing windows and ground-level doors. 
 * The streets are sterotypical, with at least one lane of vehicle traffic in each direction, following the right-hand driving convention.
 * The streets have marked pedestrian crossings, traffic signals at some junctions, and signs. 
 * The entire horizontal surface of the Environment is partitioned into non-overlapping buildings, pedestrian walkways, and streets.
 * 
 */

///
namespace SemanticClasses
{


    public abstract class SemanticClass
    {
        // class
        // parent class
        // parent (object, not classes)
        public SemanticClass? ParentClass { get; set; } = null;
        // children (objects, nott classes)
        public List<SemanticClass> ChildrenObjects { get; set; } = null;
        // name
        public string Name { get; set; } = string.Empty;
        // ID
        public string ID { get; set; } = string.Empty;
        // version
        public string version { get; set; } = string.Empty;
        // description
        public string Description { get; set; } = string.Empty;
        // constructors
        // affordances
        public List<Object>? Affordances { get; set; } = null;
        // behaviors
        //     affordances used
        //     interfaces used
        // appearance
        //    material
        //    texture
        // physics
    }
    public class BoundingSphere : SemanticClass
    {
        public static Mesh Generate(Tuple<double, double, double> center, double radius)
        {
            return new SharedGeometry.GeneratedSphere_Cube(radius, new double[3] { center.Item1, center.Item2, center.Item3 }, 8).GetMesh();
        }

    }
    public class Generic : SemanticClass
    {

    }
 
    public class Verse : SemanticClass
    {
        public List<SemanticClass>? Contents { get; set; } = null;

    }
    public class LandSurface : SemanticClass
    {

    }

    /// <summary>
    /// The streets are sterotypical, with at least one lane of vehicle traffic in each direction, following the right-hand driving convention.
    /// The streets have marked pedestrian crossings, traffic signals at some junctions, and signs.    
    /// The streets are sterotypical, with at least one lane of vehicle traffic in each direction, following the right-hand driving convention.
    /// </summary>
    public class Road : SemanticClass
    {
        // ways
        RoadMarking[] RoadMarkings { get; set; } = null;
        Signal[] Signals { get; set; } = null;

    }
    public abstract class Way : SemanticClass
    {
        // a way has a beginning and an end
        // properties may be referred to as left and right as viewed from start to end
        // properties may be ordinally numbered from left to right as viewed from start to end
        // material
    }
    public class RoadWay : Way
    {
        // position, width, direction
    }
    public class RoadMarking : SemanticClass
    {

    }

    public class RoadFurniture : SemanticClass
    {

    }

    public class Signal : RoadFurniture
    {

    }

    public class Sign : RoadFurniture
    {
        // GeoPose
        // height
        // width
        // image
        // text
    }

    public class Building : SemanticClass
    {

    }

    public class ExternalDoor : SemanticClass
    {

    }

    public class ExternalWindow : SemanticClass
    {

    }

    public class WalkWay : Way
    {

    }

    public class Car : SemanticClass
    {
    

    }

    public class RideCar : Car
    {
        public bool IsAvailable { get; set; } = true;
    }

    public class Person : SemanticClass
    {
        public Sensors.Binocular BothEyes { get; set; } = new Sensors.Binocular();

    }
}
