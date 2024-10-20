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

        private bool _increased = false;
        private bool _killed = false;
        private DirectionChange _dirChange = DirectionChange.None;

        private Point _oldHead = new Point(0, 0);

        internal Snake(Game game)
        {
            Game = game;
            Head = game.Board.CreatePoint();
        }

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
                _increased = true;
                food.Eat();
            }
        }

        public void CheckCollision(Snake snake)
        {
            lock (_tailsLock)
            {
                if (snake.Tails.Any(x => x == Head))
                {
                    _killed = true;
                }
            }

            if (snake != this && snake.Head == Head)
            {
                _killed = true;
            }
        }

        public bool DoesCollide(Point pt)
        {
            if (Head == pt) return true;
            bool retVal = false;
            lock (_tailsLock)
            {
                if (_tails.Any(x => x == pt)) retVal = true;
            }
            return retVal;
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
        /// Updates the state of the snake based on previously calculated
        /// </summary>
        public void UpdateState()
        {
            if (_killed)
            {
                _killed = false;
                lock (_tailsLock)
                {
                    TailLength = 0;
                    _tails.Clear();
                }

                Head = Game.Board.CreatePoint();
                return;
            }

            lock (_tailsLock)
            {
                _tails.Enqueue(_oldHead);
            }

            if (_increased)
            {
                _increased = false;
                TailLength++;
            }
            else
            {
                lock (_tailsLock) { _tails.Dequeue(); }
            }

        }
    }
}
