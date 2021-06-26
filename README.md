# Inputter

A library that uses user32.dll to simulate key presses and mouse clicks/positioning

The code below demos sending "Hello, world!" then pressing Return/Enter key, then "ABC".

```csharp
namespace Inputter
{
    using System.Collections.Generic;
    using System.Threading;

    internal static class Demo
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);

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

            inputter.Send(Key.RETURN);

            inputter.SendList(new List<Key> { Key.A, Key.B, Key.C }, new List<Key> { Key.LSHIFT });
        }
    }
}
```
