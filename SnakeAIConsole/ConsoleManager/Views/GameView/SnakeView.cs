using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeAIConsole
{
    public class SnakeView : IGameObjectView
    {

        private Snake _snake;

        private Dictionary<Point, CharCache> _data = new Dictionary<Point, CharCache>();

        public SnakeView(Snake snake, ConsoleArea view)
        {
            _snake = snake;
            View = view;
        }

        public IGameObject GameObject { get { return _snake; } }

        public ConsoleArea View { get; }

        public void Cleanup()
        {
            foreach (var d in _data.Keys) View.Write(d.X, d.Y, Theme.Board, ' ');
        }

        public void Refresh()
        {
            // Mark all old position for overwritting
            foreach (var kvp in _data) { kvp.Value.New = ' '; }

            // Fill new information
            var head = _snake.Head;
            if (_data.ContainsKey(head)) _data[head].New = 'X';
            else _data[head] = new CharCache() { New = 'X' };

            var tails = _snake.Tails;
            foreach (var t in tails)
            {
                if (_data.ContainsKey(t)) _data[t].New = '+';
                else _data[t] = new CharCache() { New = '+' };
            }

            // write to console
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var kvp in _data)
            {
                if (kvp.Value.Old != kvp.Value.New)
                {
                    kvp.Value.Old = kvp.Value.New;
                    var theme = kvp.Value.New == 'X' ? Theme.SnakeHead :
                        kvp.Value.New == '+' ? Theme.SnakeTail :
                        Theme.Board;
                    View.Write(kvp.Key.X, kvp.Key.Y, theme, kvp.Value.New);
                }
            }

            // remove empty chars
            foreach (var kvp in _data.ToArray())
            {
                if (kvp.Value.Old == ' ') _data.Remove(kvp.Key);
            }
        }

        private class CharCache
        {
            public char Old { get; set; } = ' ';
            public char New { get; set; } = ' ';
        }
    }
}
