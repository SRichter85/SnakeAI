using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAICore
{
    public class SnakePopulation
    {
        private readonly Snake[] _snakes;

        internal SnakePopulation(Game game, int numSnakes = 4)
        {
            _snakes = new Snake[numSnakes];
            for (int i = 0; i < _snakes.Length; i++)
            {
                _snakes[i] = new Snake(game);
            }
        }

        public IEnumerable<Snake> Snakes { get { return _snakes; } }
        /// <summary>
        /// Activates the first available Snake and returns it. Returns null, if no snakes are available.
        /// </summary>
        /// <returns></returns>
        public Snake? ActivateSnake()
        {
            var snake = _snakes.FirstOrDefault(x => !x.IsActive);
            if(snake != null) snake.IsActive = true;
            return snake;
        }

        public void DeactiveSnake(Snake snake)
        {
            snake.IsActive = false;
        }

        /// <summary>
        /// checks if any of the snakes heads or tails is on <b>Point pt</b>
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        internal bool ContainsPoint(Point pt) => _snakes.Any(s => s.ContainsPoint(pt));

        internal void CheckCollision(Food food)
        {
            foreach(var snake in _snakes) snake.CheckCollision(food);
        }

        internal void CheckCollision()
        {
            for (int i = 0; i < _snakes.Length; i++)
            {
                for (int j = 0; j < _snakes.Length; j++)
                {
                    _snakes[i].CheckCollision(_snakes[j]);
                }
            }
        }

        internal void UpdatePosition()
        {
            foreach (var snake in _snakes) snake.UpdatePosition();
        }

        /// <summary>
        /// Updates the state of the snakes based on previously calculated signal-flags (has been killed, etc.)
        /// </summary>
        internal void UpdateState()
        {
            foreach (var snake in _snakes) snake.UpdateState();
        }

    }
}
