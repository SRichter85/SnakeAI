using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole {
    public class GameView : ConsoleArea {

        private SnakeView _snakeView;

        private List<FoodView> _foodViews = new List<FoodView>();

        private SyncStack<Food> _foodSrc;

        public GameView(Point topLeft, Game game) : base(topLeft, game.Board.Size) {
            Game = game;
            _foodSrc = new SyncStack<Food>(game.Food);
            FillBackground(Theme.Board);
            _snakeView = new SnakeView(Game.Snake, this);
        }

        public Game Game { get; }

        public override void Refresh() {
            // check if new foods have been added or removed
            _foodSrc.Sync();
            if (_foodSrc.Count != _foodViews.Count) {
                List<FoodView> objectsToRemove = new List<FoodView>();
                foreach (var obj in _foodViews) {
                    if (!_foodSrc.Any(x => x == obj.GameObject)) {
                        objectsToRemove.Add(obj);
                    }
                }

                foreach (var obj in objectsToRemove) {
                    obj.Cleanup();
                    _foodViews.Remove(obj);
                }

                foreach (var f in _foodSrc) {
                    if (!_foodViews.Any(x => x.GameObject == f)) {
                        _foodViews.Add(new FoodView(f, this));
                    }
                }
            }

            // refresh views
            _snakeView.Refresh();
            foreach (var obj in _foodViews) obj.Refresh();
            base.Refresh();
        }
    }
}
