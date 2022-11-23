// See https://aka.ms/new-console-template for more information
using Affordances;
using Verses;
using Entities;

IntegratedWorld myWorld = new IntegratedWorld("Use Case 1");
myWorld.OmniVerse = new OutsideOfAnyWorld();
// add interfaces to OmniVerse

// 

myWorld.Background = new StaticWorld();
// add entities to background
Entity earthSurface = new Entity();
earthSurface.Name = "OneAndOnly Planet Surface";
earthSurface.SemanticEntityClass = new SemanticClasses.LandSurface();
myWorld.Background.AddEntity(earthSurface);
myWorld.Foreground = new DynamicWorld();
// add entities to foreground
myWorld.VirtualParts = new VirtualWorld();
// add entities to virtualparts
myWorld.ListElementsAsMarkDown();

