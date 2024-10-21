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

    public MenuView(SnakeAiConfiguration configuration, Point topLeft, int width) : base(topLeft, new Size(width, 14))
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

        _items.Add(new MenuViewItem(13, this, "Beende", ActionBeende));

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

    private void ActionSchneller()
    {
        Game.MillisecondsPerFrame = Game.MillisecondsPerFrame == 0 ? 0 :
            Game.MillisecondsPerFrame / 2;
        Configuration.Settings.GameSpeed = Game.MillisecondsPerFrame;
    }

    private void ActionLangsamer()
    {
        Game.MillisecondsPerFrame = Game.MillisecondsPerFrame == 0 ? 1 :
            Game.MillisecondsPerFrame >= 1024 ? 2048 :
            Game.MillisecondsPerFrame * 2;
        Configuration.Settings.GameSpeed = Game.MillisecondsPerFrame;
    }

    private void ActionFutterAdd()
    {
        Game.SetFoodCount(Game.FoodCount + 1);
        Configuration.Settings.FoodCount = Game.FoodCount;
    }

    private void ActionFutterRemove()
    {
        Game.SetFoodCount(Game.FoodCount - 1);
        Configuration.Settings.FoodCount = Game.FoodCount;
    }

    private void ZoomIn()
    {
        if (Configuration.Settings.FontSize < 32)
        {
            Configuration.Settings.FontSize++;
            Configuration.Console.SetFontSize(Configuration.Settings.FontSize);
        }
    }

    private void ZoomOut()
    {
        if (Configuration.Settings.FontSize > 5)
        {
            Configuration.Settings.FontSize--;
            Configuration.Console.SetFontSize(Configuration.Settings.FontSize);
        }
    }

    private void ActionBeende()
    {
        Configuration.Stop();
    }
}
