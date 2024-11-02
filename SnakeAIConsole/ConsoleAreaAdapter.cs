using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGui;

namespace SnakeAIConsole;

internal class ConsoleAreaAdapter<T> : ConsoleFrame
    where T: ConsoleArea
{

    private readonly Func<Rect, T> _consoleAreaConstructor;

    public ConsoleAreaAdapter(Func<Rect, T> consoleAreaConstructor)
    {
        _consoleAreaConstructor = consoleAreaConstructor;
    }

    public T? ConsoleArea { get; private set; }

    protected override void OnInitializing()
    {
        Rect area = new Rect(); // TODO: Implement logic for calculation of Rect
        ConsoleArea = _consoleAreaConstructor(area);
    }

    protected override void OnRefreshing()
    {
        ConsoleArea.Refresh();
    }
}
