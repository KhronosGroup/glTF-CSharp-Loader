namespace BB
{
    public class RealityEnabler
    {

    }
    public class RealityPlayer
    {

    }
    public class Shared
    {

    }
    public class PubSubBroker : RealityEnabler
    {

    }
    public class MappingAndLocalization: RealityEnabler
    {

    }
    public class DigitalTwinAuthoring: RealityEnabler
    {

    }
    public class RealityModel : RealityEnabler
    {

    }
    public class BaseMap : RealityEnabler
    {

    }
    public class SharedPointCloud : RealityEnabler
    {

    }
    public class SemanticUnderstanding : RealityEnabler
    {

    }
    public class RealWorldSensor : RealityPlayer
    {

    }
    public class RealWorldActuator : RealityPlayer
    {

    }
    public class RealWorldAgent : RealityPlayer
    {
        
    }
    public class IDPrivacySecurityOwnershipMetadata : Shared
    {

    }
    public class ServiceDiscovery : Shared
    {

    }
    public class ExperienceDiscovery : Shared
    {

    }
    public class MetaverseExperience : Shared
    {

    }
}

namespace Affordances
{
    public class Affordance
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Afforder { get; set; } = "None";
    }
}

namespace Behaviors
{
    public class Behavior
    {
        public string Name { get; set; } = "None";
        public string Desee { get; set; } = "All";
    }
}

namespace Physics
{

}

namespace Attributes
{

}

namespace Sensors
{
    public class Sensor
    {

    }
    public class Camera : Sensor
    {

    }
    public class Binocular : Sensor
    {
        public Binocular()
        {

        }
        public Binocular(Camera leftCamera, Camera rightCamera)
        {

        }
        public Camera LeftCamera { get; set; } = new Camera();
        public Camera RightCamera { get; set; } = new Camera(); 
    }

}

namespace Virtual
{

}
///
///
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
namespace SemanticObjects
{


    public class SemanticClass
    {
        // parent 
        // name
        // ID
        // description
        // affordances
        // behaviors
        //     affordances used
        //     interfaces used
        // physics
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
    public class Way : SemanticClass
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

    public class Person : SemanticClass
    {
        public Sensors.Binocular BothEyes { get; set; } = new Sensors.Binocular();

    }
}
namespace Entities
{
    public class Entity
    {
        // name
        // id
        // location, orpientation
        public SemanticObjects.SemanticClass SemanticEntityClass { get; set; } = new SemanticObjects.SemanticClass();
    }
}
namespace Verses
/// <summary>
/// The Omniverse is the source of all externally defined information and universal relationships and interactions between entities
/// there are Oracles that provide information and Animators that alter conditions in the real world. Both operate without any explanation or known mechanism.
/// </summary>
{
    public class OutsideOfAnyWorld
    {

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
        public SemanticObjects.Sign SignOverRide { get; set; } = new SemanticObjects.Sign();
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
namespace ServiceInterfaces
{
    public class ServiceInterface
    {

    }
    public class InformationRequest : ServiceInterface
    {

    }
    public class InformationResponse : ServiceInterface
    {

    }
    public class PositionRequest : InformationRequest
    {

    }
    public class PositionResponse : InformationResponse
    {

    }
    public class Operation : ServiceInterface
    {

    }
}
