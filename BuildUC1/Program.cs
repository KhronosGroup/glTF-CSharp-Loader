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
myBackgroundFrame.Position.Coordinates[0] = lat;
myBackgroundFrame.Position.Coordinates[1] = lon;
myBackgroundFrame.Position.Coordinates[2] = h;
myBackground.FramePose = myBackgroundFrame;

// add entities to background
Entity earthSurface = new Entity();
earthSurface.Name = "Planet Surface";

GeoPose.Basic myTerrainFrame = new GeoPose.Basic("Terrain Frame");
myTerrainFrame.Position.Coordinates[0] = lat;
myTerrainFrame.Position.Coordinates[1] = lon;
myTerrainFrame.Position.Coordinates[2] = h;
earthSurface.Pose = myTerrainFrame;

earthSurface.SemanticEntityClass = new SemanticClasses.LandSurface();
myBackground.AddEntity(earthSurface);
for (int nBuilding = 0; nBuilding < nBuildings; nBuilding++)
{
    Entity aBuilding = new Entity();
    aBuilding.Name = "Building " + (nBuilding + 0).ToString();

    GeoPose.Basic myBuildingFrame = new GeoPose.Basic("VirtualParts Frame");
    myBuildingFrame.Position.Coordinates[0] = lat;
    myBuildingFrame.Position.Coordinates[1] = lon;
    myBuildingFrame.Position.Coordinates[2] = h;
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
    myStreetFrame.Position.Coordinates[0] = lat;
    myStreetFrame.Position.Coordinates[1] = lon;
    myStreetFrame.Position.Coordinates[2] = h;
    aStreet.Pose = myStreetFrame;
    // add sidewalks
    aStreet.SemanticEntityClass = new SemanticClasses.Road();
    myBackground.AddEntity(aStreet);
    Entity aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 0).ToString();

    GeoPose.Basic mySidewalkFrame = new GeoPose.Basic("Walkway Frame " + aSidewalk.Name);
    mySidewalkFrame.Position.Coordinates[0] = lat;
    mySidewalkFrame.Position.Coordinates[1] = lon;
    mySidewalkFrame.Position.Coordinates[2] = h;
    aSidewalk.Pose = mySidewalkFrame;

    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
    aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 1).ToString();

    mySidewalkFrame = new GeoPose.Basic("Walkway Frame " + aSidewalk.Name);
    mySidewalkFrame.Position.Coordinates[0] = lat;
    mySidewalkFrame.Position.Coordinates[1] = lon;
    mySidewalkFrame.Position.Coordinates[2] = h;
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
    mySignalFrame.Position.Coordinates[0] = lat;
    mySignalFrame.Position.Coordinates[1] = lon;
    mySignalFrame.Position.Coordinates[2] = h;
    aSignal.Pose = mySignalFrame;

    aSignal.SemanticEntityClass = new SemanticClasses.Signal();
    myBackground.AddEntity(aSignal);
}
myWorld.AddWorld(myBackground);

// ====== add foreground world
DynamicWorld myForeground = new DynamicWorld();
myForeground.Name = "Foreground";

GeoPose.Basic myForegroundFrame = new GeoPose.Basic("Foreground Frame");
myForegroundFrame.Position.Coordinates[0] = lat;
myForegroundFrame.Position.Coordinates[1] = lon;
myForegroundFrame.Position.Coordinates[2] = h;
myForeground.FramePose = myForegroundFrame;
// === add rider
Entity riderPerson = new Entity();
riderPerson.Name = "Rider";

GeoPose.Basic myRiderFrame = new GeoPose.Basic("Rider Frame " + riderPerson.Name);
myRiderFrame.Position.Coordinates[0] = lat;
myRiderFrame.Position.Coordinates[1] = lon;
myRiderFrame.Position.Coordinates[2] = h;
riderPerson.Pose = myRiderFrame;

riderPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(riderPerson);

// === add driver
Entity driverPerson = new Entity();
driverPerson.Name = "Driver";

GeoPose.Basic myDriverFrame = new GeoPose.Basic("Driver Frame " + driverPerson.Name);
myDriverFrame.Position.Coordinates[0] = lat;
myDriverFrame.Position.Coordinates[1] = lon;
myDriverFrame.Position.Coordinates[2] = h;
driverPerson.Pose = myDriverFrame;

driverPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(driverPerson);
// === add random people
for (int nPerson = 0; nPerson < nPersons; nPerson++)
{
    Entity aPerson = new Entity();
    aPerson.Name = "Non-participant Person " + (nPerson + 1).ToString();

    GeoPose.Basic myPersonFrame = new GeoPose.Basic("Person Frame " + aPerson.Name);
    myPersonFrame.Position.Coordinates[0] = lat;
    myPersonFrame.Position.Coordinates[1] = lon;
    myPersonFrame.Position.Coordinates[2] = h;
    aPerson.Pose = myPersonFrame;

    aPerson.SemanticEntityClass = new SemanticClasses.Person();
    myForeground.AddEntity(aPerson);
}
// === add cars
Entity rideCar = new Entity();
rideCar.Name = "Ride Car";

GeoPose.Basic myRideCarFrame = new GeoPose.Basic("Ride Car Frame " + rideCar.Name);
myRideCarFrame.Position.Coordinates[0] = lat;
myRideCarFrame.Position.Coordinates[1] = lon;
myRideCarFrame.Position.Coordinates[2] = h;
rideCar.Pose = myRideCarFrame;

rideCar.SemanticEntityClass = new SemanticClasses.Car();
myForeground.AddEntity(rideCar);

for(int nCar = 0;nCar < nCars; nCar++)
{
    Entity aCar = new Entity();
    aCar.Name = "Non-participant Car " + (nCar+1).ToString();

    GeoPose.Basic myCarFrame = new GeoPose.Basic("Car Frame " + aCar.Name);
    myCarFrame.Position.Coordinates[0] = lat;
    myCarFrame.Position.Coordinates[1] = lon;
    myCarFrame.Position.Coordinates[2] = h;
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
myVirtualPartsFrame.Position.Coordinates[0] = lat;
myVirtualPartsFrame.Position.Coordinates[1] = lon;
myVirtualPartsFrame.Position.Coordinates[2] = h;
myVirtualParts.FramePose = myVirtualPartsFrame;

Entity carSign = new Entity();
carSign.Name = "Virtual Sign Over Ride Car";

GeoPose.Basic myCarSignFrame = new GeoPose.Basic("CarSign Frame ");
myCarSignFrame.Position.Coordinates[0] = lat;
myCarSignFrame.Position.Coordinates[1] = lon;
myCarSignFrame.Position.Coordinates[2] = h;
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
