using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SnakeAIConsole {
    public class MenuView : ConsoleArea {

        private const int PADDING = 2;
        private readonly ConsoleManager _consoleManager;
        private List<MenuViewItem> _items = new List<MenuViewItem>();
        private int _selectedIndex = 0;
        private int _currentLine = 0;

        public MenuView(ConsoleManager consoleManager, Point topLeft, Size size) : base(topLeft, size) {

            FillBackground(Theme.MenuItem);
            _consoleManager = consoleManager;

            Write(0, 0, Theme.MenuItem, new string(' ', size.Width));
            _currentLine += 2;
            AddMenuViewItem("Schneller", ActionSchneller);
            AddMenuViewItem("Langsamer", ActionLangsamer);
            _currentLine += 2;
            AddMenuViewItem("Füge Futter hinzu", ActionFutterAdd);
            AddMenuViewItem("Entferne Futter", ActionFutterRemove);
            _currentLine += 2;
            AddMenuViewItem("Beende", ActionBeende);

            SelectedItem = _items.First();
            SelectedItem.IsSelected = true;
        }

        public MenuViewItem SelectedItem { get; private set; }

        public override void Refresh() {
            SetSelectedItem();
            foreach (var item in _items) item.Refresh();
            base.Refresh();
        }

        public void MoveUp() {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = 0;
        }

        public void MoveDown() {

            _selectedIndex++;
            if (_selectedIndex >= _items.Count) _selectedIndex = _items.Count - 1;
        }


        private void SetSelectedItem() {
            var idx = _selectedIndex;
            if (SelectedItem != _items[idx]) {
                SelectedItem.IsSelected = false;
                SelectedItem = _items[idx];
                SelectedItem.IsSelected = true;
            }
        }

        private void AddMenuViewItem(string text, Action action) {
            if (action == null) action = () => { };

            int maxTextLength = Size.Width - 2 * PADDING;
            if (text.Length > Size.Width - 2 * PADDING) {
                text = text.Substring(0, maxTextLength);
            } else {
                text += new string(' ', maxTextLength - text.Length);
            }

            text = $"  {text}  ";

            var item = new MenuViewItem(
                displayText: text,
                line: _currentLine,
                view: this,
                action: action);

            _currentLine++;
            _items.Add(item);
        }

        private void ActionSchneller() { _consoleManager.Game.FramesPerSecond += _consoleManager.Game.FramesPerSecond < 100 ?  5 : 0; }
        private void ActionLangsamer() { _consoleManager.Game.FramesPerSecond -= _consoleManager.Game.FramesPerSecond > 5 ? 5 : 0; }

        private void ActionFutterAdd() { _consoleManager.Game.SetFoodCount(_consoleManager.Game.Food.Count() + 1); }
        private void ActionFutterRemove() { _consoleManager.Game.SetFoodCount(_consoleManager.Game.Food.Count() - 1); }

        private void ActionBeende() { _consoleManager.RaiseOnProgramStop(); }
    }
}
