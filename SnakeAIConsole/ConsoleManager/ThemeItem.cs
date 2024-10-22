namespace SnakeAIConsole;

public static class Theme {

    public static ThemeItem Default { get; } = new ThemeItem(ConsoleColor.Black, ConsoleColor.White);

    public static ThemeItem Board { get; } = new ThemeItem(ConsoleColor.White, ConsoleColor.Black);
    public static ThemeItem Food { get; } = new ThemeItem(ConsoleColor.DarkGreen, ConsoleColor.Green);

    public static ThemeItem[] SnakeHead { get; } = {
        new ThemeItem(ConsoleColor.DarkRed, ConsoleColor.Yellow),
        new ThemeItem(ConsoleColor.DarkBlue, ConsoleColor.Cyan),
        new ThemeItem(ConsoleColor.DarkMagenta, ConsoleColor.Yellow),
        new ThemeItem(ConsoleColor.DarkCyan, ConsoleColor.Blue),
    };
    public static ThemeItem[] SnakeTail { get; } = {
        new ThemeItem(ConsoleColor.DarkRed, ConsoleColor.DarkYellow),
        new ThemeItem(ConsoleColor.DarkBlue, ConsoleColor.DarkCyan),
        new ThemeItem(ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow),
        new ThemeItem(ConsoleColor.DarkCyan, ConsoleColor.DarkBlue)
    };

    public static ThemeItem MenuItem { get; } = new ThemeItem(ConsoleColor.DarkGray, ConsoleColor.White);

    public static ThemeItem MenuItemSelected { get; } = new ThemeItem(ConsoleColor.DarkBlue, ConsoleColor.White);
}

public class ThemeItem {


    public ThemeItem(ConsoleColor background, ConsoleColor foreground) {
        Background = background;
        Foreground = foreground;
    }

    public ConsoleColor Background { get; }

    public ConsoleColor Foreground { get; }
}
