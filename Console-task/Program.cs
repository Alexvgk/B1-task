// See https://aka.ms/new-console-template for more information
using Console_Test.Generation;

Console.WriteLine("Hello, World!");
IGenerator generator = new TxtGenerator();
if (generator.GenerateFile())
{
    Console.WriteLine("Success");
}
