using SnakeAIConsole;
using System.Diagnostics;

namespace TEST_ConsolePrototypes;
class Program
{

    static readonly I2 SIZE = new I2() { x = 100, y = 80 };

    static void Main(string[] args)
    {
        var sw = Stopwatch.StartNew();
        Console.SetWindowSize(SIZE.x, SIZE.y);
        Test(sw);
        Console.ReadKey();
    }

    static void Test(Stopwatch sw)
    {
        ConsoleFrame frameMain = new ConsoleFrame(SIZE.x , SIZE.y);
        frameMain.Write(20, 20, "Hello World");
        frameMain.Refresh();
    }
}
