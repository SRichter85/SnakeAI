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
        private DirectionChange _dirChange = DirectionChange.None;

        private Point _oldHead = new Point(0, 0);

        public EventHandler OnKilled;

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

        public int TailLength { get; private set; }

        public void TurnLeft() => _dirChange = DirectionChange.Left;

        public void TurnRight() => _dirChange = DirectionChange.Right;

        public void CheckCollision(Food food)
        {
            if (Head == food.Position)
            {
                _signalHasEaten = true;
                food.Eat();
            }
        }

        public void CheckCollision(Snake snake)
        {
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

            if (_signalKilled) OnKilled?.Invoke(this, EventArgs.Empty);
        }

        public void UpdatePosition()
        {
            _oldHead = Head;

            int movDir = (int)MoveDirection;
            switch (_dirChange)
            {
                case DirectionChange.Left:
                    movDir = movDir == 3 ? 0 : movDir + 1;
                    _dirChange = DirectionChange.None;
                    break;
                case DirectionChange.Right:
                    movDir = movDir == 0 ? 3 : movDir - 1;
                    _dirChange = DirectionChange.None;
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
        public void UpdateState()
        {
            if (_signalKilled)
            {
                _signalKilled = false;
                lock (_tailsLock)
                {
                    TailLength = 0;
                    _tails.Clear();
                }

                CreatedAtFrame = Game.FrameCount;
                Head = Game.Board.CreatePoint();
                return;
            }

            lock (_tailsLock)
            {
                _tails.Enqueue(_oldHead);
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
