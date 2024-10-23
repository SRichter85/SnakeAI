using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAICore
{

    public class Snake : IGameObject
    {
        private readonly Queue<Point> _tails = new Queue<Point>();
        private object _tailsLock = new object();

        private bool _signalHasEaten = false;
        private bool _signalKilled = false;
        private DirectionChange _signalDirectionChange = DirectionChange.None;

        /// <summary>
        /// describes wether the snake IS active. The property IsActive describes, wether the snake SHOULD be active.
        /// basically this is the internal state and will be used for CheckCollision etc.
        /// </summary>
        private bool _isActive = false;

        private Point _oldHeadPosition = new Point(0, 0);

        public EventHandler? OnKilled;

        internal Snake(Game game)
        {
            Game = game;
            Head = game.Board.CreatePoint();
            CreatedAtFrame = Game.FrameCount;
        }

        public int CreatedAtFrame { get; private set; }

        public Game Game { get; }

        public Direction MoveDirection { get; private set; }

        public Point Head { get; private set; }

        public IEnumerable<Point> Tails
        {
            get { lock (_tailsLock) return new List<Point>(_tails); }
        }

        public int TailLength { get; private set; } = 0;

        public bool IsActive { get; set; } = false;

        public void TurnLeft() => _signalDirectionChange = DirectionChange.Left;

        public void TurnRight() => _signalDirectionChange = DirectionChange.Right;

        internal void CheckCollision(Food food)
        {
            if (!_isActive) return;

            if (Head == food.Position)
            {
                _signalHasEaten = true;
                food.Eat();
            }
        }

        /// <summary>
        /// Checks if this snake's head is on the same position as another snake's tail or head. If yes, signals the snake to be killed.<br />
        /// If the snake given in the parameter is the same as the caller snake, the 'tail'-check is still performed but not the 'head'-check.
        /// </summary>
        /// <param name="snake"></param>
        internal void CheckCollision(Snake snake)
        {
            if (!_isActive || !snake._isActive) return;

            lock (_tailsLock)
            {
                if (snake.Tails.Any(x => x == Head))
                {
                    _signalKilled = true;
                }
            }

            if (snake != this && snake.Head == Head)
            {
                _signalKilled = true;
            }
        }

        internal bool ContainsPoint(Point pt)
        {
            if (!_isActive) return false;
            if (Head == pt) return true;
            var tails = _tails.Any(t => t == pt);
            return tails;
        }

        internal void UpdatePosition()
        {
            if (!_isActive) return;

            _oldHeadPosition = Head;

            int movDir = (int)MoveDirection;
            switch (_signalDirectionChange)
            {
                case DirectionChange.Left:
                    movDir = movDir == 3 ? 0 : movDir + 1;
                    _signalDirectionChange = DirectionChange.None;
                    break;
                case DirectionChange.Right:
                    movDir = movDir == 0 ? 3 : movDir - 1;
                    _signalDirectionChange = DirectionChange.None;
                    break;
            }

            MoveDirection = (Direction)movDir;

            int nX = MoveDirection == Direction.Left ? Head.X - 1 :
                MoveDirection == Direction.Right ? Head.X + 1 :
                Head.X;
            int nY = MoveDirection == Direction.Up ? Head.Y - 1 :
                MoveDirection == Direction.Down ? Head.Y + 1 :
                Head.Y;


            Head = Game.Board.CreatePoint(nX, nY);
        }

        /// <summary>
        /// Updates the state of the snake based on previously calculated signal-flags (has been killed, etc.)
        /// </summary>
        internal void UpdateState()
        {
            bool shouldBeActive = IsActive;
            if (!_isActive)
            {
                if (shouldBeActive)
                {
                    _isActive = true;
                    ResetSnake();
                }

                return;
            }

            if (!shouldBeActive)
            {
                _isActive = false;
                ResetSnake();
                return;
            }

            if (_signalKilled)
            {
                OnKilled?.Invoke(this, EventArgs.Empty);
                ResetSnake();
                return;
            }

            lock (_tailsLock)
            {
                _tails.Enqueue(_oldHeadPosition);
            }

            if (_signalHasEaten)
            {
                _signalHasEaten = false;
                TailLength++;
            }
            else
            {
                lock (_tailsLock) { _tails.Dequeue(); }
            }
        }

        private void ResetSnake()
        {
            _signalHasEaten = false;
            _signalKilled = false;
            lock (_tailsLock)
            {
                TailLength = 0;
                _tails.Clear();
            }

            CreatedAtFrame = Game.FrameCount;
            Head = Game.Board.CreatePoint();
            MoveDirection = Direction.Left;
        }

        public class SnakeEventArgs : EventArgs
        {
            public SnakeEventArgs(Snake snake)
            {
                Snake = snake;
            }

            public Snake Snake { get; }
        }
    }
}
