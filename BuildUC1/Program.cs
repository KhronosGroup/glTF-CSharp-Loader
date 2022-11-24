// See https://aka.ms/new-console-template for more information
using Affordances;
using Verses;
using Entities;
const int nCars = 4;
const int nPersons = 8;

IntegratedWorld myWorld = new IntegratedWorld("Use Case 1");
myWorld.OmniVerse = new OutsideOfAnyWorld();
// add interfaces to OmniVerse

StaticWorld myBackground = new StaticWorld();
myBackground.Name = "Background";
// add entities to background
Entity earthSurface = new Entity();
earthSurface.Name = "Planet Surface";
earthSurface.SemanticEntityClass = new SemanticClasses.LandSurface();
myBackground.AddEntity(earthSurface);
myWorld.AddWorld(myBackground);

DynamicWorld myForeground = new DynamicWorld();
myForeground.Name = "Foreground";
// add rider
Entity riderPerson = new Entity();
riderPerson.Name = "Rider";
riderPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(riderPerson);

// add driver
Entity driverPerson = new Entity();
driverPerson.Name = "Driver";
driverPerson.SemanticEntityClass = new SemanticClasses.Person();
myForeground.AddEntity(driverPerson);
// add random people
for (int nPerson = 0; nPerson < nPersons; nPerson++)
{
    Entity aPerson = new Entity();
    aPerson.Name = "Random Person " + (nPerson + 1).ToString();
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
    aCar.Name = "Random Car " + (nCar+1).ToString();
    aCar.SemanticEntityClass = new SemanticClasses.Car();
    myForeground.AddEntity(aCar);
}
myWorld.AddWorld(myForeground);

VirtualWorld myVirtualParts = new VirtualWorld();
myVirtualParts.Name = "Virtual";
Entity carSign = new Entity();
carSign.Name = "Virtual Sign Over Ride Car";
carSign.SemanticEntityClass = new SemanticClasses.Sign();
myVirtualParts.AddEntity(carSign);
myWorld.AddWorld(myVirtualParts);

string fileName = "c:/temp/models/list09.md";
myWorld.ListElementsAsMarkDown(fileName);

