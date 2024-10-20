using SnakeAICore;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace SnakeAIConsole;

class Program
{
    static void Main(string[] args)
    {
        SnakeAiConfiguration config = new SnakeAiConfiguration();
        config.Start();
        while (config.IsRunning) Thread.Sleep(1000);
    }
}
