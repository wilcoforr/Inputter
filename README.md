# Inputter

A library that uses user32.dll to simulate key presses and mouse clicks/positioning

The code below demos sending key pressees for typing "Hello, world!".

Then demostrates some mouse input/moving simulation. Best seen on a blank screen.

```csharp
using Inputter;

namespace InputterDriver
{
    /// <summary>
    /// Driver program to showcase the Inputter library
    /// </summary>
    internal class Program
    {
        static string Seperator = new string('-', 50);
        static void Main(string[] args)
        {
            Console.WriteLine(Seperator);
            Console.WriteLine("Driver program to showcase the Inputter library");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            //Console.WriteLine("Hello, world demo.");
            var inputter = new Input();

            inputter.Send(Key.H, new List<Key> { Key.LSHIFT });
            inputter.Send(Key.E);
            inputter.Send(Key.L);
            inputter.Send(Key.L);
            inputter.Send(Key.O);
            inputter.Send(Key.COMMA);
            inputter.Send(Key.SPACE);

            var worldKeys = new List<Key> { Key.W, Key.O, Key.R, Key.L, Key.D };
            inputter.SendList(worldKeys);

            inputter.Send(Key.ONE, new List<Key> { Key.LSHIFT });
             

            Thread.Sleep(TimeSpan.FromSeconds(5));

            inputter.MoveMouse(100, 100);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            inputter.MoveMouse(500, 500);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            inputter.MoveMouse(300, 500);
            Thread.Sleep(TimeSpan.FromSeconds(1));


            Console.WriteLine(Seperator);
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();

        }
    }
}

```
