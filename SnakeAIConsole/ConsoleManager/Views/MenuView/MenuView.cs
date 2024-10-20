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

    public MenuView(SnakeAiConfiguration configuration, Point topLeft, int width) : base(topLeft, new Size(width, 11))
    {

        Configuration = configuration;
        FillBackground(Theme.MenuItem);

        Write(0, 1, Theme.MenuItem, new string(' ', width / 2 - 2) + "MENÜ");

        _items.Add(new MenuViewItem(3, this, "Schneller", ActionSchneller));
        _items.Add(new MenuViewItem(4, this, "Langsamer", ActionLangsamer));

        _items.Add(new MenuViewItem(6, this, "Füge Futter hinzu", ActionFutterAdd));
        _items.Add(new MenuViewItem(7, this, "Entferne Futter", ActionFutterRemove));

        _items.Add(new MenuViewItem(9, this, "Beende", ActionBeende));

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

    private void ActionSchneller() { Game.FramesPerSecond += Game.FramesPerSecond < 100 ? 5 : 0; }

    private void ActionLangsamer() { Game.FramesPerSecond -= Game.FramesPerSecond > 5 ? 5 : 0; }

    private void ActionFutterAdd() { Game.SetFoodCount(Game.FoodCount + 1); }

    private void ActionFutterRemove() { Game.SetFoodCount(Game.FoodCount - 1); }

    private void ActionBeende() { Configuration.Stop(); }
}
