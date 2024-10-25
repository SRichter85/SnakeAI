using System.Diagnostics;

namespace TEST_ConsolePrototypes;

class Program
{
    static readonly I2 SIZE = new I2() { x = 140, y = 120 };

    static void Main(string[] args)
    {
        Random rnd = new Random();
        var sw = Stopwatch.StartNew();

        Console.SetWindowSize(SIZE.x, SIZE.y);

        Prototype_Grid testObject = new Prototype_Grid(SIZE.x, SIZE.y);
        var dataS = GenLines();
        var dataC = GenChars();


        sw.Restart();
        Write(dataS);
        sw.Stop();

        sw.Restart();
        Write(dataC);
        sw.Stop();

        sw.Restart();
        testObject.GenerateRandomData(rnd);
        sw.Stop();

        sw.Restart();
        testObject.RefreshPerChar();
        sw.Stop();


        Console.ReadKey();
    }

    static char[,] GenChars()
    {
        char[,] d = new char[SIZE.x, SIZE.y];
        for (int c = 0; c < SIZE.x; c++)
        {
            for (int r = 0; r < SIZE.y; r++)
            {
                d[c, r] = 'c';
            }
        }

        return d;
    }

    static string[] GenLines()
    {
        string[] retval = new string[SIZE.y];
        for (int l = 0; l < SIZE.y; l++)
        {
            retval[l] = new string('c', SIZE.x);
        }

        return retval;
    }

    static void Write(char[,] data)
    {
        Console.SetCursorPosition(0, 0);
        for (int r = 0; r < SIZE.y; r++)
        {
            //Console.SetCursorPosition(0, r);
            for (int c = 0; c < SIZE.x; c++)
            {
                //Console.SetCursorPosition(c, r);
                //Console.ForegroundColor = ConsoleColor.White;
                //Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(data[c, r]);
            }
        }
    }

    static void Write(string[] data)
    {
        Console.SetCursorPosition(0, 0);
        for (int l = 0; l < SIZE.y; l++)
        {
            //Console.SetCursorPosition(0, l);
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(data[l]);
        }
    }

    struct WriteInfo
    {
        public char C;
        public ConsoleColor B;
        public ConsoleColor F;
    }

    struct I2
    {
        public int x;
        public int y;
    }

    class Prototype_Grid
    {
        WriteInfo[,] _data;
        public Prototype_Grid(int width, int height)
        {
            _data = new WriteInfo[width, height];
        }

        public void RefreshPerChar() {
            for (int c = 0; c < _data.GetLength(0); c++)
            {
                for (int r = 0; r < _data.GetLength(1); r++)
                {
                    Console.SetCursorPosition(c, r);
                    Console.ForegroundColor = _data[c, r].F;
                    Console.BackgroundColor = _data[c, r].B;
                    Console.Write(_data[c, r].C);
                }
            }
        }

        public void GenerateRandomData(Random rnd)
        {
            for (int c = 0; c < _data.GetLength(0); c++)
            {
                for (int r = 0; r < _data.GetLength(1); r++)
                {
                    _data[c, r] = new WriteInfo
                    {
                        C = (char)rnd.Next(26, 70),
                        F = (ConsoleColor)rnd.Next(0, 16),
                        B = (ConsoleColor)rnd.Next(0, 16),
                    };
                    //_data[c, r] = new WriteInfo
                    //{
                    //    C = 'c',
                    //    F = ConsoleColor.White,
                    //    B = ConsoleColor.Black,
                    //};
                }
            }
        }
    }
}
