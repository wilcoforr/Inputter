namespace Inputter
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Input
    {
        private const uint KEY_UP = 0x02;
        private static Random _random { get; set; }
        private bool _useSleepFeature { get; set; }

        //for user32 dll stuff
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="useSleepFeature"></param>
        public Input(bool useSleepFeature = true)
        {
            _useSleepFeature = useSleepFeature;
            _random = new Random();
        }

        /// <summary>
        /// Send a key press
        /// </summary>
        /// <param name="key"></param>
        public void Send(Key key)
        {
            PressKeyDown(key);

            if (_useSleepFeature)
            {
                RandomSleepKeyPress();
            }
            
            PressKeyUp(key);
        }

        /// <summary>
        /// Send a key press with modifiers held down.
        /// Example: Shift+R, CTRL+SHIFT+R, etc
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        public void Send(Key key, Key modifier)
        {
            PressKeyDown(modifier);
            Send(key);
            PressKeyUp(modifier);
        }

        /// <summary>
        /// Send a key press with a modifier
        /// Example: Shift+R, CTRL+R, etc
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        public void Send(Key key, IEnumerable<Key> modifiers)
        {
            foreach (var mod in modifiers)
            {
                PressKeyDown(mod);
            }
            
            Send(key);

            foreach (var mod in modifiers)
            {
                PressKeyUp(mod);
            }
        }

        /// <summary>
        /// Send a List of Keys to press
        /// </summary>
        /// <param name="keys"></param>
        public void SendList(IEnumerable<Key> keys)
        {
            foreach (var key in keys)
            {
                Send(key);
            }
        }

        /// <summary>
        /// Send a List of Keys to press with a single Modifier
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="modifier"></param>
        public void SendList(IEnumerable<Key> keys, Key modifier)
        {
            foreach (var key in keys)
            {
                Send(key, modifier);
            }
        }

        /// <summary>
        /// Send a List of Keys to press with a List of modifiers
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="modifiers"></param>
        public void SendList(IEnumerable<Key> keys, IEnumerable<Key> modifiers)
        {
            foreach (var key in keys)
            {
                Send(key, modifiers);
            }
        }

        /// <summary>
        /// Move the mouse to a specified mouse position on the user's primary monitor
        /// </summary>
        /// <remarks>
        /// x,y coords represented on like all computers are like
        ///    [0,0, 1,0, 2,0, 3,0, 4,0, 5,0]
        ///    [0,1, 1,1, 2,1, 3,1, 4,1 , 5,1]
        ///    etc
        /// </remarks>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        public void MoveMouse(int xPosition, int yPosition)
        {
            SetCursorPos(xPosition, yPosition);
        }

        /// <summary>
        /// Click the Left Mouse at a specified X,Y coordinate
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        public void ClickLeftMouse(int xPosition, int yPosition)
        {
            MoveMouse(xPosition, yPosition);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xPosition, yPosition, 0, 0);
            
            if (_useSleepFeature)
            {
                RandomSleepKeyPress();
            }

            mouse_event(MOUSEEVENTF_LEFTUP, xPosition, yPosition, 0, 0);
        }

        /// <summary>
        /// Click the Right Mouse at a specified X,Y coordinate
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        public void ClickRightMouse(int xPosition, int yPosition)
        {
            MoveMouse(xPosition, yPosition);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, xPosition, yPosition, 0, 0);
            
            if (_useSleepFeature)
            {
                RandomSleepKeyPress();
            }

            mouse_event(MOUSEEVENTF_RIGHTUP, xPosition, yPosition, 0, 0);
        }

        /// <summary>
        /// Press a specified Key, down
        /// </summary>
        /// <param name="key"></param>
        public void PressKeyDown(Key key)
        {
            keybd_event( (byte) key, 0, 0, 0);
        }

        /// <summary>
        /// Press a specified Key, up
        /// </summary>
        /// <param name="key"></param>
        public void PressKeyUp(Key key)
        {
            keybd_event( (byte) key, 0, KEY_UP, 0);
        }

        // force the program to sleep for a random time
        internal void RandomSleepKeyPress(int min = 50, int max = 300)
        {
            if (min < 0 || max > 1000 * 60)
            {
                throw new Exception("Lol what are you doing");
            }

            int randomSleepTimeMs = _random.Next(min, max);
            System.Threading.Thread.Sleep(randomSleepTimeMs);
        }

        [DllImport("user32.dll")]
        internal static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        internal static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
    }

    //https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes?redirectedfrom=MSDN
    public enum Key
    {
        LBUTTON = 0x01,
        RBUTTON = 0x02,
        CANCEL = 0x03,
        MBUTTON = 0x04,
        XBUTTON1 = 0x05,
        XBUTTON2 = 0x06,
        //- = 0x07,
        BACK = 0x08,
        TAB = 0x09,
        //- = 0x0A-0B,
        CLEAR = 0x0C,
        RETURN = 0x0D,
        //- = 0x0E-0F,
        SHIFT = 0x10,
        CONTROL = 0x11,
        MENU = 0x12,
        PAUSE = 0x13,
        CAPITAL = 0x14,
        KANA = 0x15,
        HANGUEL = 0x15,
        HANGUL = 0x15,
        //- = 0x16,
        JUNJA = 0x17,
        FINAL = 0x18,
        HANJA = 0x19,
        KANJI = 0x19,
        //- = 0x1A,
        ESCAPE = 0x1B,
        CONVERT = 0x1C,
        NONCONVERT = 0x1D,
        ACCEPT = 0x1E,
        MODECHANGE = 0x1F,
        SPACE = 0x20,
        PRIOR = 0x21,
        NEXT = 0x22,
        END = 0x23,
        HOME = 0x24,
        LEFT = 0x25,
        UP = 0x26,
        RIGHT = 0x27,
        DOWN = 0x28,
        SELECT = 0x29,
        PRINT = 0x2A,
        EXECUTE = 0x2B,
        SNAPSHOT = 0x2C,
        INSERT = 0x2D,
        DELETE = 0x2E,
        HELP = 0x2F,
        ZERO = 0x30,

        ONE = 0x31,
        TWO = 0x32,
        THREE = 0x33,
        FOUR = 0x34,
        FIVE = 0x35,
        SIX = 0x36,
        SEVEN = 0x37,
        EIGHT = 0x38,
        NINE = 0x39,

        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        LWIN = 0x5B,
        RWIN = 0x5C,
        APPS = 0x5D,
        //- = 0x5E,
        SLEEP = 0x5F,
        NUMPAD0 = 0x60,
        NUMPAD1 = 0x61,
        NUMPAD2 = 0x62,
        NUMPAD3 = 0x63,
        NUMPAD4 = 0x64,
        NUMPAD5 = 0x65,
        NUMPAD6 = 0x66,
        NUMPAD7 = 0x67,
        NUMPAD8 = 0x68,
        NUMPAD9 = 0x69,
        MULTIPLY = 0x6A,
        ADD = 0x6B,
        SEPARATOR = 0x6C,
        SUBTRACT = 0x6D,
        DECIMAL = 0x6E,
        DIVIDE = 0x6F,

        //function keys
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,

        //- = 0x88-8F,
        NUMLOCK = 0x90,
        SCROLL = 0x91,
        //= 0x92 - 96,
        //- = 0x97-9F,
        LSHIFT = 0xA0,
        RSHIFT = 0xA1,
        LCONTROL = 0xA2,
        RCONTROL = 0xA3,
        LMENU = 0xA4,
        RMENU = 0xA5,

        COMMA = 0xBC,
        PERIOD = 0xBE,
        QUESTION_MARK = 0xBF, //For the US standard keyboard, the '/?' key
        LEFT_BRACKET = 0xDB, //For the US standard keyboard, the '[{' key
        RIGHT_BRACKET = 0xDD, // For the US standard keyboard, the ']}' key
        QUOTE = 0xDE, // For the US standard keyboard, the 'single-quote/double-quote ' " ' key
    }
}
