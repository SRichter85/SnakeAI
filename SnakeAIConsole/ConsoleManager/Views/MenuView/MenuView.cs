using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SnakeAIConsole;

public class MenuView : ConsoleArea
{

    private List<MenuViewItem> _items = new List<MenuViewItem>();
    private int _selectedIndex = 0;
    private bool[] _isPlayerActive = new bool[2];

    public MenuView(SnakeAiConfiguration configuration, Point topLeft, int width) : base(topLeft, new Size(width, 19))
    {

        Configuration = configuration;
        FillBackground(Theme.MenuItem);

        Write(0, 1, Theme.MenuItem, new string(' ', width / 2 - 2) + "MENÜ");

        _items.Add(new MenuViewItem(3, this, "Schneller", ActionSchneller));
        _items.Add(new MenuViewItem(4, this, "Langsamer", ActionLangsamer));

        _items.Add(new MenuViewItem(6, this, "Füge Futter hinzu", ActionFutterAdd));
        _items.Add(new MenuViewItem(7, this, "Entferne Futter", ActionFutterRemove));

        _items.Add(new MenuViewItem(9, this, "Zoom In", ZoomIn));
        _items.Add(new MenuViewItem(10, this, "Zoom Out", ZoomOut));

        _items.Add(new MenuViewItem(12, this, "Aktiviere Spieler 1", item => ActivatePlayer(item, 0)));
        _items.Add(new MenuViewItem(13, this, "Aktiviere Spieler 2", item => ActivatePlayer(item, 1)));

        _items.Add(new MenuViewItem(16, this, "Beende", ActionBeende));

        SelectedItem = _items.First();
        SelectedItem.IsSelected = true;
    }

    public MenuViewItem SelectedItem { get; private set; }

    public SnakeAiConfiguration Configuration { get; }

    public Game Game => Configuration.Game;

    public override void Refresh()
    {
        SetSelectedItem();
        foreach (var item in _items) item.Refresh();
        base.Refresh();
    }

    public void MoveUp()
    {
        _selectedIndex--;
        if (_selectedIndex < 0) _selectedIndex = 0;
    }

    public void MoveDown()
    {

        _selectedIndex++;
        if (_selectedIndex >= _items.Count) _selectedIndex = _items.Count - 1;
    }


    private void SetSelectedItem()
    {
        var idx = _selectedIndex;
        if (SelectedItem != _items[idx])
        {
            SelectedItem.IsSelected = false;
            SelectedItem = _items[idx];
            SelectedItem.IsSelected = true;
        }
    }

    private void ActionSchneller(MenuViewItem src)
    {
        Game.MillisecondsPerFrame = Game.MillisecondsPerFrame == 0 ? 0 :
            Game.MillisecondsPerFrame / 2;
        Configuration.Settings.GameSpeed = Game.MillisecondsPerFrame;
    }

    private void ActionLangsamer(MenuViewItem src)
    {
        Game.MillisecondsPerFrame = Game.MillisecondsPerFrame == 0 ? 1 :
            Game.MillisecondsPerFrame >= 1024 ? 2048 :
            Game.MillisecondsPerFrame * 2;
        Configuration.Settings.GameSpeed = Game.MillisecondsPerFrame;
    }

    private void ActionFutterAdd(MenuViewItem src)
    {
        Game.SetFoodCount(Game.FoodCount + 1);
        Configuration.Settings.FoodCount = Game.FoodCount;
    }

    private void ActionFutterRemove(MenuViewItem src)
    {
        Game.SetFoodCount(Game.FoodCount - 1);
        Configuration.Settings.FoodCount = Game.FoodCount;
    }

    private void ZoomIn(MenuViewItem src)
    {
        if (Configuration.Settings.FontSize < 32)
        {
            Configuration.Settings.FontSize++;
            Configuration.Console.SetFontSize(Configuration.Settings.FontSize);
        }
    }

    private void ZoomOut(MenuViewItem src)
    {
        if (Configuration.Settings.FontSize > 5)
        {
            Configuration.Settings.FontSize--;
            Configuration.Console.SetFontSize(Configuration.Settings.FontSize);
        }
    }

    private void ActionBeende(MenuViewItem src)
    {
        Configuration.Stop();
    }

    private void ActivatePlayer(MenuViewItem item, int playerId)
    {
        if (_isPlayerActive[playerId])
        {
            Configuration.Control.DeactivateSnake(playerId);
            _isPlayerActive[playerId] = false;
            item.DisplayText = $"Aktiviere Spieler {playerId+1}";
            item.ForceRefresh();
        }
        else
        {
            var snake = Game.Snakes.ActivateSnake();
            if (snake != null)
            {
                Configuration.Control.ActivateSnake(playerId, snake);
                _isPlayerActive[playerId] = true;
                item.DisplayText = $"Deaktiviere Spieler {playerId + 1}";
                item.ForceRefresh();
            }
        }
    }
}
