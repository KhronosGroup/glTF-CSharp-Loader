// See https://aka.ms/new-console-template for more information
using Affordances;
using Verses;

IntegratedWorld myWorld = new IntegratedWorld();
myWorld.OmniVerse = new OutsideOfAnyWorld();
// add interfaces to OmniVerse

// 

myWorld.Background = new StaticWorld();
// add entities to background
myWorld.Foreground = new DynamicWorld();
// add entities to foreground
myWorld.VirtualParts = new VirtualWorld();
// add entities to virtualparts
myWorld.ListElementsAsMarkDown();

