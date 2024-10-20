using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole {
    internal class FoodView : IGameObjectView {

        private Food _food;

        private Point _old;

        public FoodView(Food food, ConsoleArea view) {
            _food = food;
            View = view;
        }

        public IGameObject GameObject { get { return _food; } }

        public ConsoleArea View { get; }

        public void Cleanup() {
            View.Write(_old.X, _old.Y, Theme.Board, " ");
        }

        public void Refresh() {
            var np = _food.Position;
            if(_old != np) {
                // View.Write(_old.X, _old.Y, Theme.Board, " ");
                View.Write(np.X, np.Y, Theme.Food, "$");
                _old = np;
            }
        }
    }
}
