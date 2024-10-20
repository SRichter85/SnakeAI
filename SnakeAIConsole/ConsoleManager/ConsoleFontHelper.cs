using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

public class ConsoleFontHelper {

    public enum Font {
        LucidaConsole,
        CourierNew,
        Consolas
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern Int32 SetCurrentConsoleFontEx(
        IntPtr ConsoleOutput,
        bool MaximumWindow,
        ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx);

    private enum StdHandle {
        OutputHandle = -11
    }

    [DllImport("kernel32")]
    private static extern IntPtr GetStdHandle(StdHandle index);

    private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
    public static void SetFont(Font font, short height, short width = 0, bool bold = false) {

        string fontName = font == Font.LucidaConsole ? "Lucida Console" :
            font == Font.CourierNew ? "Courier New" :
            font == Font.Consolas ? "Consolas" :
            "Lucida Console";
        // Instantiating CONSOLE_FONT_INFO_EX and setting its size (the function will fail otherwise)
        CONSOLE_FONT_INFO_EX fontInfo = new CONSOLE_FONT_INFO_EX();
        fontInfo.cbSize = (uint)Marshal.SizeOf(fontInfo);
        fontInfo.FaceName = fontName;
        fontInfo.dwFontSize.X = width;
        fontInfo.dwFontSize.Y = height;
        fontInfo.FontWeight = bold ? 700 : 400;
        SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref fontInfo);

    }
    private static void Test(string[] args) {

        // Setting the font and fontsize
        // Other values can be changed too

        Console.WriteLine("Before. Press any key to continue");
        Console.ReadKey();

        // Instantiating CONSOLE_FONT_INFO_EX and setting its size (the function will fail otherwise)
        CONSOLE_FONT_INFO_EX ConsoleFontInfo = new CONSOLE_FONT_INFO_EX();
        ConsoleFontInfo.cbSize = (uint)Marshal.SizeOf(ConsoleFontInfo);

        // Optional, implementing this will keep the fontweight and fontsize from changing
        // See notes
        // GetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);

        ConsoleFontInfo.FaceName = "Lucida Console";
        ConsoleFontInfo.dwFontSize.X = 0;
        ConsoleFontInfo.dwFontSize.Y = 32;
        SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);

        Console.WriteLine("After 1: Lucida Console 32");
        Console.ReadKey();

        ConsoleFontInfo.FaceName = "Lucida Console";
        ConsoleFontInfo.dwFontSize.X = 0;
        ConsoleFontInfo.dwFontSize.Y = 8;
        SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);

        Console.WriteLine("After 2: Lucida Console 8");
        Console.ReadKey();

        ConsoleFontInfo.FaceName = "Lucida Console";
        ConsoleFontInfo.dwFontSize.X = 8;
        ConsoleFontInfo.dwFontSize.Y = 8;
        SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);

        Console.WriteLine("After 3: Lucida Console 8x8");
        Console.ReadKey();

        ConsoleFontInfo.FaceName = "Raster Fonts";
        ConsoleFontInfo.dwFontSize.X = 8;
        ConsoleFontInfo.dwFontSize.Y = 8;

        SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);
        Console.WriteLine("After 4: Raster Fonts 8x8");
        Console.ReadKey();


        Console.Clear();
        Console.Write("After. Notice how the font type and size changed?");
        Console.ReadKey();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD {
        public short X;
        public short Y;

        public COORD(short X, short Y) {
            this.X = X;
            this.Y = Y;
        }
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CONSOLE_FONT_INFO_EX {
        public uint cbSize;
        public uint nFont;
        public COORD dwFontSize;
        public int FontFamily;
        public int FontWeight;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
        public string FaceName;
    }
}
