// See https://aka.ms/new-console-template for more information
using Affordances;
using Verses;
using Entities;
using g4;

const int nCars = 4;
const int nPersons = 8;
const int nBuildings = 16;
const int nStreets = 8;
const int nSignals = 7;
const double lat = 51.955005;
const double lon = -8.403096;
const double h = -114.0;
const double yaw = 0.0;
const double pitch = -90.0;
const double roll = 0.0;
const double size = 200.0;

IntegratedWorld myWorld = new IntegratedWorld("Use Case 1");
GeoPose.BasicYPR myIntegratedFrame = new GeoPose.BasicYPR("Integrated Frame");
myIntegratedFrame.position.lat = lat;
myIntegratedFrame.position.lon = lon;
myIntegratedFrame.position.h = h;
myIntegratedFrame.angles.yaw = yaw;
myIntegratedFrame.angles.pitch = pitch;
myIntegratedFrame.angles.roll = roll;
myWorld.OmniVerse = new OutsideOfAnyWorld();
myWorld.FramePose = myIntegratedFrame;
myWorld.Size = new SharedGeometry.Distance();
myWorld.Size.Value = size;
// add interfaces to OmniVerse

StaticWorld myBackground = new StaticWorld();
myBackground.Name = "Background";

GeoPose.BasicYPR myBackgroundFrame = new GeoPose.BasicYPR("Background Frame");
myBackgroundFrame.position.lat = lat;
myBackgroundFrame.position.lon = lon;
myBackgroundFrame.position.h = h;
myBackground.FramePose = myBackgroundFrame;

// add entities to background
Entity boundingSphere = new Entity();
boundingSphere.Pose = myBackgroundFrame;
boundingSphere.Name = "The One and Only Bounding Sphere";
boundingSphere.world = myBackground;
boundingSphere.SemanticEntityClass = new SemanticClasses.BoundingSphere();
boundingSphere.Meshes.Add(SemanticClasses.BoundingSphere.Generate(new Tuple<double, double, double>(0.0, 0.0, 0.0), 200.0));
boundingSphere.Material = new SemanticObjects.Material();
boundingSphere.Material.PBRMetallicRougness = new SemanticObjects.PBRMetallicRoughness();
myBackground.AddEntity(boundingSphere);

Entity earthSurface = new Entity();
earthSurface.Name = "Planet Surface";

GeoPose.BasicYPR myTerrainFrame = new GeoPose.BasicYPR("Terrain Frame");
myTerrainFrame.position.lat = lat;
myTerrainFrame.position.lon = lon;
myTerrainFrame.position.h = h;
earthSurface.Pose = myTerrainFrame;

earthSurface.SemanticEntityClass = new SemanticClasses.LandSurface();
myBackground.AddEntity(earthSurface);
for (int nBuilding = 0; nBuilding < nBuildings; nBuilding++)
{
    Entity aBuilding = new Entity();
    aBuilding.Name = "Building " + (nBuilding + 0).ToString();

    GeoPose.BasicYPR myBuildingFrame = new GeoPose.BasicYPR("VirtualParts Frame");
    myBuildingFrame.position.lat = lat;
    myBuildingFrame.position.lon = lon;
    myBuildingFrame.position.h = h;
    aBuilding.Pose = myBuildingFrame;

    aBuilding.SemanticEntityClass = new SemanticClasses.Building();
    myBackground.AddEntity(aBuilding);
}
for (int nStreet = 0; nStreet < nStreets; nStreet++)
{
    // add streets
    Entity aStreet = new Entity();
    aStreet.Name = "Street " + (nStreet + 1).ToString();

    GeoPose.BasicYPR myStreetFrame = new GeoPose.BasicYPR("Street Frame " + aStreet.Name);
    myStreetFrame.position.lat = lat;
    myStreetFrame.position.lon = lon;
    myStreetFrame.position.h = h;
    aStreet.Pose = myStreetFrame;
    // add sidewalks
    aStreet.SemanticEntityClass = new SemanticClasses.Road();
    myBackground.AddEntity(aStreet);
    Entity aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 0).ToString();

    GeoPose.BasicYPR mySidewalkFrame = new GeoPose.BasicYPR("Walkway Frame " + aSidewalk.Name);
    mySidewalkFrame.position.lat = lat;
    mySidewalkFrame.position.lon = lon;
    mySidewalkFrame.position.h = h;
    aSidewalk.Pose = mySidewalkFrame;

    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
    aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 1).ToString();

    mySidewalkFrame = new GeoPose.BasicYPR("Walkway Frame " + aSidewalk.Name);
    mySidewalkFrame.position.lat = lat;
    mySidewalkFrame.position.lon = lon;
    mySidewalkFrame.position.h = h;
    aSidewalk.Pose = mySidewalkFrame;

    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
}
// add signals
for (int nSignal = 0; nSignal < nSignals; nSignal++)
{
    Entity aSignal = new Entity();
    aSignal.Name = "Signal " + (nSignal + 1).ToString();

    GeoPose.BasicYPR mySignalFrame = new GeoPose.BasicYPR("Signal Frame " + aSignal.Name);
    mySignalFrame.position.lat = lat;
    mySignalFrame.position.lon = lon;
    mySignalFrame.position.h = h;
    aSignal.Pose = mySignalFrame;

    aSignal.SemanticEntityClass = new SemanticClasses.Signal();
    myBackground.AddEntity(aSignal);
}
myWorld.AddWorld(myBackground);

// ====== add foreground world
DynamicWorld myForeground = new DynamicWorld();
myForeground.Name = "Foreground";

GeoPose.BasicYPR myForegroundFrame = new GeoPose.BasicYPR("Foreground Frame");
myForegroundFrame.position.lat = lat;
myForegroundFrame.position.lon = lon;
myForegroundFrame.position.h = h;
myForeground.FramePose = myForegroundFrame;
// === add rider
Entity riderPerson = new Entity();
riderPerson.Name = "Rider";

GeoPose.BasicYPR myRiderFrame = new GeoPose.BasicYPR("Rider Frame " + riderPerson.Name);
myRiderFrame.position.lat = lat;
myRiderFrame.position.lon = lon;
myRiderFrame.position.h = h;
riderPerson.Pose = myRiderFrame;

riderPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(riderPerson);

// === add driver
Entity driverPerson = new Entity();
driverPerson.Name = "Driver";

GeoPose.BasicYPR myDriverFrame = new GeoPose.BasicYPR("Driver Frame " + driverPerson.Name);
myDriverFrame.position.lat = lat;
myDriverFrame.position.lon = lon;
myDriverFrame.position.h = h;
driverPerson.Pose = myDriverFrame;

driverPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(driverPerson);
// === add random people
for (int nPerson = 0; nPerson < nPersons; nPerson++)
{
    Entity aPerson = new Entity();
    aPerson.Name = "Non-participant Person " + (nPerson + 1).ToString();

    GeoPose.BasicYPR myPersonFrame = new GeoPose.BasicYPR("Person Frame " + aPerson.Name);
    myPersonFrame.position.lat = lat;
    myPersonFrame.position.lon = lon;
    myPersonFrame.position.h = h;
    aPerson.Pose = myPersonFrame;

    aPerson.SemanticEntityClass = new SemanticClasses.Person();
    myForeground.AddEntity(aPerson);
}
// === add cars
Entity rideCar = new Entity();
rideCar.Name = "Ride Car";

GeoPose.BasicYPR myRideCarFrame = new GeoPose.BasicYPR("Ride Car Frame " + rideCar.Name);
myRideCarFrame.position.lat = lat;
myRideCarFrame.position.lon = lon;
myRideCarFrame.position.h = h;
rideCar.Pose = myRideCarFrame;

rideCar.SemanticEntityClass = new SemanticClasses.Car();
myForeground.AddEntity(rideCar);

for(int nCar = 0;nCar < nCars; nCar++)
{
    Entity aCar = new Entity();
    aCar.Name = "Non-participant Car " + (nCar+1).ToString();

    GeoPose.BasicYPR myCarFrame = new GeoPose.BasicYPR("Car Frame " + aCar.Name);
    myCarFrame.position.lat = lat;
    myCarFrame.position.lon = lon;
    myCarFrame.position.h = h;
    aCar.Pose = myCarFrame;

    aCar.SemanticEntityClass = new SemanticClasses.Car();
    myForeground.AddEntity(aCar);
}
myWorld.AddWorld(myForeground);

// ====== add earth inertial world - NASA SPICE J2000
DynamicWorld myEarthCenteredInertial = new DynamicWorld();
myEarthCenteredInertial.Name = "EarthCenteredInertial";

GeoPose.Advanced myEarthCenteredInertialFrame = new GeoPose.Advanced("EarthCenteredInertial Frame");
myEarthCenteredInertialFrame.frameSpecification.authority = "https://naif.jpl.nasa.gov/naif/";
myEarthCenteredInertialFrame.frameSpecification.id = "J2000";
myEarthCenteredInertialFrame.frameSpecification.parameters = "";
myEarthCenteredInertial.FramePose = myEarthCenteredInertialFrame;
myWorld.AddWorld(myEarthCenteredInertial);

// ====== add virtual world for virtual props
VirtualWorld myVirtualParts = new VirtualWorld();
myVirtualParts.Name = "Virtual";

GeoPose.BasicYPR myVirtualPartsFrame = new GeoPose.BasicYPR("VirtualParts Frame");
myVirtualPartsFrame.position.lat = lat;
myVirtualPartsFrame.position.lon = lon;
myVirtualPartsFrame.position.h = h;
myVirtualParts.FramePose = myVirtualPartsFrame;

Entity carSign = new Entity();
carSign.Name = "Virtual Sign Over Ride Car";

GeoPose.BasicYPR myCarSignFrame = new GeoPose.BasicYPR("CarSign Frame ");
myCarSignFrame.position.lat = lat;
myCarSignFrame.position.lon = lon;
myCarSignFrame.position.h = h;
carSign.Pose = myCarSignFrame;

carSign.SemanticEntityClass = new SemanticClasses.Sign();
myVirtualParts.AddEntity(carSign);
myWorld.AddWorld(myVirtualParts);
DateTime now = DateTime.Now;
string baseName = "c:/temp/models/world" + now.Year.ToString("d4") + "." + now.Month.ToString("d2") + "." + now.Day.ToString("d2") + "." +
    now.Hour.ToString("d2") + "." + now.Minute.ToString("d2") + "." + now.Second.ToString("d2");
string mdName = baseName + ".md";
string jsonName = baseName + ".json";
string gltfName = baseName + ".gltf";
myWorld.ListElementsAsMarkDown(mdName);
myWorld.SaveAsJSON(jsonName);
myWorld.GenerateglTF(gltfName);
//myWorld.Render2glTF("r_" + gltfName);
