using System.Drawing;

namespace SnakeAIConsole {
    public class MenuViewItem {
        private readonly Action<MenuViewItem>? _action; // todo: use real events
        private readonly int _line;
        private readonly ConsoleArea _view;
        private bool _lastIsSelected = false;
        private bool _forceRefresh = false;

        public MenuViewItem(int line, ConsoleArea view, string displayText, Action<MenuViewItem>? action)
        {
            DisplayText = displayText;
            _action = action;
            _line = line;
            _view = view;
            _view.WriteLine(_line, Theme.MenuItem, DisplayText);
        }

        public string DisplayText { get; set; } = string.Empty;

        public bool IsSelected { get; set; }

        public void ForceRefresh()
        {
            _forceRefresh = true;
        }

        public void Refresh() {
            if (IsSelected != _lastIsSelected || _forceRefresh) {
                _forceRefresh = false;
                _lastIsSelected = IsSelected;
                var color = IsSelected ? Theme.MenuItemSelected : Theme.MenuItem;
                _view.WriteLine(_line, color, DisplayText);
            }
        }

        public void Execute() {
            if(_action != null) _action(this);
        }
    }
}
