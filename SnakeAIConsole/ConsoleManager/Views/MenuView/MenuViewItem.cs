using System.Drawing;

namespace SnakeAIConsole {
    public class MenuViewItem {
        private readonly Action? _action;
        private readonly int _line;
        private readonly ConsoleArea _view;
        private bool _lastIsSelected = false;

        public MenuViewItem(int line, ConsoleArea view, string displayText, Action? action)
        {
            DisplayText = " " + displayText;
            _action = action;
            _line = line;
            _view = view;
            _view.WriteLine(_line, Theme.MenuItem, DisplayText);
        }

        public string DisplayText { get; }

        public bool IsSelected { get; set; }

        public void Refresh() {
            if (IsSelected != _lastIsSelected) {
                _lastIsSelected = IsSelected;
                var color = IsSelected ? Theme.MenuItemSelected : Theme.MenuItem;
                _view.WriteLine(_line, color, DisplayText);
            }
        }

        public void Execute() {
            if(_action != null) _action();
        }
    }
}
