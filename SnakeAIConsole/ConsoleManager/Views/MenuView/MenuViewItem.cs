namespace SnakeAIConsole {
    public class MenuViewItem {
        private readonly Action _action;
        private readonly int _line;
        private readonly ConsoleArea _view;
        private bool _lastIsSelected = false;

        public MenuViewItem(string displayText, int line, ConsoleArea view, Action action) {
            DisplayText = displayText;
            _action = action;
            _line = line;
            _view = view;
            _view.Write(0, _line, Theme.MenuItem, DisplayText);
        }

        public string DisplayText { get; }

        public bool IsSelected { get; set; }

        public void Refresh() {
            if (IsSelected != _lastIsSelected) {
                _lastIsSelected = IsSelected;
                _view.Write(0, _line, IsSelected ? Theme.MenuItemSelected : Theme.MenuItem, DisplayText);
            }
        }

        public void Execute() {
            if(_action != null) _action();
        }
    }
}
