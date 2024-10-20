namespace SnakeAIConsole;

public static class Theme {

    public static ThemeItem Default { get; } = new ThemeItem(ConsoleColor.Black, ConsoleColor.White);

    public static ThemeItem Board { get; } = new ThemeItem(ConsoleColor.White, ConsoleColor.Black);
    public static ThemeItem Food { get; } = new ThemeItem(ConsoleColor.DarkGreen, ConsoleColor.Green);

    public static ThemeItem SnakeHead { get; } = new ThemeItem(ConsoleColor.DarkRed, ConsoleColor.Yellow);
    public static ThemeItem SnakeTail { get; } = new ThemeItem(ConsoleColor.DarkRed, ConsoleColor.DarkYellow);

    public static ThemeItem MenuItem { get; } = new ThemeItem(ConsoleColor.DarkGray, ConsoleColor.White);

    public static ThemeItem MenuItemSelected { get; } = new ThemeItem(ConsoleColor.DarkGreen, ConsoleColor.White);
}

public class ThemeItem {


    public ThemeItem(ConsoleColor background, ConsoleColor foreground) {
        Background = background;
        Foreground = foreground;
    }

    public ConsoleColor Background { get; }

    public ConsoleColor Foreground { get; }
}
