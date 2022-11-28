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
myBackground.FramePose.Position.Coordinates[0] = lat;
myBackground.FramePose.Position.Coordinates[1] = lon;
myBackground.FramePose.Position.Coordinates[2] = h;
// add entities to background
Entity earthSurface = new Entity();
earthSurface.Name = "Planet Surface";
earthSurface.SemanticEntityClass = new SemanticClasses.LandSurface();
myBackground.AddEntity(earthSurface);
for (int nBuilding = 0; nBuilding < nBuildings; nBuilding++)
{
    Entity aBuilding = new Entity();
    aBuilding.Name = "Building " + (nBuilding + 0).ToString();
    aBuilding.Pose.Position.Coordinates[0] = lat;
    aBuilding.Pose.Position.Coordinates[1] = lon;
    aBuilding.Pose.Position.Coordinates[2] = h;
    aBuilding.SemanticEntityClass = new SemanticClasses.Building();
    myBackground.AddEntity(aBuilding);
}
for (int nStreet = 0; nStreet < nStreets; nStreet++)
{
    Entity aStreet = new Entity();
    aStreet.Name = "Street " + (nStreet + 1).ToString();
    aStreet.SemanticEntityClass = new SemanticClasses.Road();
    myBackground.AddEntity(aStreet);
    Entity aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 0).ToString();
    aSidewalk.Pose.Position.Coordinates[0] = lat;
    aSidewalk.Pose.Position.Coordinates[1] = lon;
    aSidewalk.Pose.Position.Coordinates[2] = h;
    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
    aSidewalk = new Entity();
    aSidewalk.Name = "Walkway " + (nStreet*2 + 1).ToString();
    aSidewalk.Pose.Position.Coordinates[0] = lat;
    aSidewalk.Pose.Position.Coordinates[1] = lon;
    aSidewalk.Pose.Position.Coordinates[2] = h;
    aSidewalk.SemanticEntityClass = new SemanticClasses.WalkWay();
    myBackground.AddEntity(aSidewalk);
}
for(int nSignal = 0; nSignal < nSignals; nSignal++)
{
    Entity aSignal = new Entity();
    aSignal.Name = "Signal " + (nSignal + 1).ToString();
    aSignal.Pose.Position.Coordinates[0] = lat;
    aSignal.Pose.Position.Coordinates[1] = lon;
    aSignal.Pose.Position.Coordinates[2] = h;
    aSignal.SemanticEntityClass = new SemanticClasses.Signal();
    myBackground.AddEntity(aSignal);
}
// add sidewalks

// add signals
myWorld.AddWorld(myBackground);

DynamicWorld myForeground = new DynamicWorld();
myForeground.Name = "Foreground";
myForeground.FramePose.Position.Coordinates[0] = lat;
myForeground.FramePose.Position.Coordinates[1] = lon;
myForeground.FramePose.Position.Coordinates[2] = h;
// add rider
Entity riderPerson = new Entity();
riderPerson.Name = "Rider";
riderPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(riderPerson);

// add driver
Entity driverPerson = new Entity();
driverPerson.Name = "Driver";
driverPerson.Pose.Position.Coordinates[0] = lat;
driverPerson.Pose.Position.Coordinates[1] = lon;
driverPerson.Pose.Position.Coordinates[2] = h;
driverPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(driverPerson);
// add random people
for (int nPerson = 0; nPerson < nPersons; nPerson++)
{
    Entity aPerson = new Entity();
    aPerson.Name = "Non-participant Person " + (nPerson + 1).ToString();
    aPerson.Pose.Position.Coordinates[0] = lat;
    aPerson.Pose.Position.Coordinates[1] = lon;
    aPerson.Pose.Position.Coordinates[2] = h;
    aPerson.SemanticEntityClass = new SemanticClasses.Person();
    myForeground.AddEntity(aPerson);
}
// add cars
Entity car = new Entity();
car.Name = "Ride Car";
car.SemanticEntityClass = new SemanticClasses.Car();
myForeground.AddEntity(car);
for(int nCar = 0;nCar < nCars; nCar++)
{
    Entity aCar = new Entity();
    aCar.Name = "Non-participant Car " + (nCar+1).ToString();
    aCar.Pose.Position.Coordinates[0] = lat;
    aCar.Pose.Position.Coordinates[1] = lon;
    aCar.Pose.Position.Coordinates[2] = h;
    aCar.SemanticEntityClass = new SemanticClasses.Car();
    myForeground.AddEntity(aCar);
}
myWorld.AddWorld(myForeground);

VirtualWorld myVirtualParts = new VirtualWorld();
myVirtualParts.Name = "Virtual";
myVirtualParts.FramePose.Position.Coordinates[0] = lat;
myVirtualParts.FramePose.Position.Coordinates[1] = lon;
myVirtualParts.FramePose.Position.Coordinates[2] = h;
Entity carSign = new Entity();
carSign.Name = "Virtual Sign Over Ride Car";
carSign.Pose.Position.Coordinates[0] = lat;
carSign.Pose.Position.Coordinates[1] = lon;
carSign.Pose.Position.Coordinates[2] = h;
carSign.SemanticEntityClass = new SemanticClasses.Sign();
myVirtualParts.AddEntity(carSign);
myWorld.AddWorld(myVirtualParts);

string fileName = "c:/temp/models/list20.md";
myWorld.ListElementsAsMarkDown(fileName);

