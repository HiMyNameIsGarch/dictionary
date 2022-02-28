using static ConsoleHelper;

public class LearnAndAnswer: IMode 
{
    public void Start(ISession session)
    {
        session.BeforeSessionHook();
        var pairs = session.Data.SplitPairsIn(10);
        int n = 1;
        foreach(var pair in pairs)
        {
            // Display pairs and remove them from key
            DisplayPairs(pair, session);
            DisplaySessionCount(n, pairs.Count);
            session.Start(session.Data.ShufflePairs(pair));
            session.Stats.Store(session.Data);
            session.AfterSessionHook();
            Console.WriteLine();
            n++;
        }
    }

    private void DisplaySessionCount(int current, int max)
    {
        Console.Write("( Session ");
        ColorWrite(current.ToString(), current != max ? ConsoleColor.DarkCyan : ConsoleColor.DarkBlue);
        Console.Write(" of ");
        ColorWrite(max.ToString(), ConsoleColor.DarkBlue);
        Console.Write(" )\n");
    }

    private void DisplayPairs(Dictionary<string[], string[]> pairs, ISession session)
    {
        int initialCursor = Console.CursorTop;
        session.Data.DisplayPairs(pairs);
        PressKeyToContinue("Press any key to begin -> ");
        ClearScreen(initialCursor);
    }
}
