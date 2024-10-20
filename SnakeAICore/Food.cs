using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAICore;

public class Food : IGameObject {

    private bool _hasBeenEaten = false;

    internal Food(Game game) {
        Game = game;
        Position = Game.Board.CreatePoint();
    }

    public Point Position { get; private set; }

    public Game Game { get; }

    internal void Eat() => _hasBeenEaten = true;

    public void UpdateState() {
        if(_hasBeenEaten) {
            _hasBeenEaten = false;
            Position = Game.Board.CreatePoint();
        }
    }
}
