// See https://aka.ms/new-console-template for more information
using Affordances;
using Verses;
using Entities;
const int nCars = 4;
const int nPersons = 8;
const int nBuildings = 16;
const int nStreets = 8;
const int nSignals = 7;
const double lat = 51.955005;
const double lon = -8.403096;
const double h = -114.0;

IntegratedWorld myWorld = new IntegratedWorld("Use Case 1");
myWorld.OmniVerse = new OutsideOfAnyWorld();
// add interfaces to OmniVerse

StaticWorld myBackground = new StaticWorld();
myBackground.Name = "Background";

GeoPose.Basic myBackgroundFrame = new GeoPose.Basic("Background Frame");
myBackgroundFrame.Position.lat = lat;
myBackgroundFrame.Position.lon = lon;
myBackgroundFrame.Position.h = h;
myBackground.FramePose = myBackgroundFrame;

// add entities to background
Entity earthSurface = new Entity();
earthSurface.Name = "Planet Surface";

GeoPose.Basic myTerrainFrame = new GeoPose.Basic("Terrain Frame");
myTerrainFrame.Position.lat = lat;
myTerrainFrame.Position.lon = lon;
myTerrainFrame.Position.h = h;
earthSurface.Pose = myTerrainFrame;

earthSurface.SemanticEntityClass = new SemanticClasses.LandSurface();
myBackground.AddEntity(earthSurface);
for (int nBuilding = 0; nBuilding < nBuildings; nBuilding++)
{
    Entity aBuilding = new Entity();
    aBuilding.Name = "Building " + (nBuilding + 0).ToString();

    GeoPose.Basic myBuildingFrame = new GeoPose.Basic("VirtualParts Frame");
    myBuildingFrame.Position.lat = lat;
    myBuildingFrame.Position.lon = lon;
    myBuildingFrame.Position.h = h;
    aBuilding.Pose = myBuildingFrame;

    aBuilding.SemanticEntityClass = new SemanticClasses.Building();
    myBackground.AddEntity(aBuilding);
}
for (int nStreet = 0; nStreet < nStreets; nStreet++)
{
    // add streets
    Entity aStreet = new Entity();
    aStreet.Name = "Street " + (nStreet + 1).ToString();

    GeoPose.Basic myStreetFrame = new GeoPose.Basic("Street Frame " + aStreet.Name);
    myStreetFrame.Position.lat = lat;
    myStreetFrame.Position.lon = lon;
    myStreetFrame.Position.h = h;
    aStreet.Pose = myStreetFrame;
    // add sidewalks
    aStreet.SemanticEntityClass = new SemanticClasses.Road();
    myBackground.AddEntity(aStreet);
    Entity aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 0).ToString();

    GeoPose.Basic mySidewalkFrame = new GeoPose.Basic("Walkway Frame " + aSidewalk.Name);
    mySidewalkFrame.Position.lat = lat;
    mySidewalkFrame.Position.lon = lon;
    mySidewalkFrame.Position.h = h;
    aSidewalk.Pose = mySidewalkFrame;

    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
    aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 1).ToString();

    mySidewalkFrame = new GeoPose.Basic("Walkway Frame " + aSidewalk.Name);
    mySidewalkFrame.Position.lat = lat;
    mySidewalkFrame.Position.lon = lon;
    mySidewalkFrame.Position.h = h;
    aSidewalk.Pose = mySidewalkFrame;

    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
}
// add signals
for (int nSignal = 0; nSignal < nSignals; nSignal++)
{
    Entity aSignal = new Entity();
    aSignal.Name = "Signal " + (nSignal + 1).ToString();

    GeoPose.Basic mySignalFrame = new GeoPose.Basic("Signal Frame " + aSignal.Name);
    mySignalFrame.Position.lat = lat;
    mySignalFrame.Position.lon = lon;
    mySignalFrame.Position.h = h;
    aSignal.Pose = mySignalFrame;

    aSignal.SemanticEntityClass = new SemanticClasses.Signal();
    myBackground.AddEntity(aSignal);
}
myWorld.AddWorld(myBackground);

// ====== add foreground world
DynamicWorld myForeground = new DynamicWorld();
myForeground.Name = "Foreground";

GeoPose.Basic myForegroundFrame = new GeoPose.Basic("Foreground Frame");
myForegroundFrame.Position.lat = lat;
myForegroundFrame.Position.lon = lon;
myForegroundFrame.Position.h = h;
myForeground.FramePose = myForegroundFrame;
// === add rider
Entity riderPerson = new Entity();
riderPerson.Name = "Rider";

GeoPose.Basic myRiderFrame = new GeoPose.Basic("Rider Frame " + riderPerson.Name);
myRiderFrame.Position.lat = lat;
myRiderFrame.Position.lon = lon;
myRiderFrame.Position.h = h;
riderPerson.Pose = myRiderFrame;

riderPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(riderPerson);

// === add driver
Entity driverPerson = new Entity();
driverPerson.Name = "Driver";

GeoPose.Basic myDriverFrame = new GeoPose.Basic("Driver Frame " + driverPerson.Name);
myDriverFrame.Position.lat = lat;
myDriverFrame.Position.lon = lon;
myDriverFrame.Position.h = h;
driverPerson.Pose = myDriverFrame;

driverPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(driverPerson);
// === add random people
for (int nPerson = 0; nPerson < nPersons; nPerson++)
{
    Entity aPerson = new Entity();
    aPerson.Name = "Non-participant Person " + (nPerson + 1).ToString();

    GeoPose.Basic myPersonFrame = new GeoPose.Basic("Person Frame " + aPerson.Name);
    myPersonFrame.Position.lat = lat;
    myPersonFrame.Position.lon = lon;
    myPersonFrame.Position.h = h;
    aPerson.Pose = myPersonFrame;

    aPerson.SemanticEntityClass = new SemanticClasses.Person();
    myForeground.AddEntity(aPerson);
}
// === add cars
Entity rideCar = new Entity();
rideCar.Name = "Ride Car";

GeoPose.Basic myRideCarFrame = new GeoPose.Basic("Ride Car Frame " + rideCar.Name);
myRideCarFrame.Position.lat = lat;
myRideCarFrame.Position.lon = lon;
myRideCarFrame.Position.h = h;
rideCar.Pose = myRideCarFrame;

rideCar.SemanticEntityClass = new SemanticClasses.Car();
myForeground.AddEntity(rideCar);

for(int nCar = 0;nCar < nCars; nCar++)
{
    Entity aCar = new Entity();
    aCar.Name = "Non-participant Car " + (nCar+1).ToString();

    GeoPose.Basic myCarFrame = new GeoPose.Basic("Car Frame " + aCar.Name);
    myCarFrame.Position.lat = lat;
    myCarFrame.Position.lon = lon;
    myCarFrame.Position.h = h;
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

GeoPose.Basic myVirtualPartsFrame = new GeoPose.Basic("VirtualParts Frame");
myVirtualPartsFrame.Position.lat = lat;
myVirtualPartsFrame.Position.lon = lon;
myVirtualPartsFrame.Position.h = h;
myVirtualParts.FramePose = myVirtualPartsFrame;

Entity carSign = new Entity();
carSign.Name = "Virtual Sign Over Ride Car";

GeoPose.Basic myCarSignFrame = new GeoPose.Basic("CarSign Frame ");
myCarSignFrame.Position.lat = lat;
myCarSignFrame.Position.lon = lon;
myCarSignFrame.Position.h = h;
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
