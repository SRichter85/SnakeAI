using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole {
    public class GameView : ConsoleArea {

        private List<SnakeView> _snakeViews = new List<SnakeView>();

        private List<FoodView> _foodViews = new List<FoodView>();

        public GameView(Point topLeft, Game game) : base(topLeft, game.Board.Size) {
            Game = game;
            FillBackground(Theme.Board);

            foreach (var s in game.Snakes.Snakes) _snakeViews.Add(new SnakeView(s, this));

            int numColors = Math.Min(_snakeViews.Count, Math.Min(Theme.SnakeHead.Length, Theme.SnakeTail.Length));
            for (int idx = 0; idx < numColors; idx++)
            {
                _snakeViews[idx].HeadColor = Theme.SnakeHead[idx];
                _snakeViews[idx].TailColor = Theme.SnakeHead[idx];
            }
        }

        public Game Game { get; }

        public override void Refresh() {
            // check if new foods have been added or removed
            var foods = Game.Food;
            if (foods.Count() != _foodViews.Count) {
                List<FoodView> objectsToRemove = new List<FoodView>();
                foreach (var obj in _foodViews) {
                    if (!foods.Any(x => x == obj.GameObject)) {
                        objectsToRemove.Add(obj);
                    }
                }

                foreach (var obj in objectsToRemove) {
                    obj.Cleanup();
                    _foodViews.Remove(obj);
                }

                foreach (var f in foods) {
                    if (!_foodViews.Any(x => x.GameObject == f)) {
                        _foodViews.Add(new FoodView(f, this));
                    }
                }
            }

            // refresh views
            _snakeViews.ForEach(x => x.Refresh());
            foreach (var obj in _foodViews) obj.Refresh();
            base.Refresh();
        }
    }
}
