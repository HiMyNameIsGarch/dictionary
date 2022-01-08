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
            // Display pairs and remove them of key
            DisplayPairs(pair, session);
            Console.WriteLine($"( Session {n} of {pairs.Count} )");
            session.Start(session.Data.ShufflePairs(pair));
            session.AfterSessionHook();
            Console.WriteLine();
            n++;
        }
    }

    private void DisplayPairs(Dictionary<string[], string[]> pairs, ISession session)
    {
        int initialCursor = Console.CursorTop;
        session.Data.DisplayPairs(pairs);
        PressKeyToContinue("Press any key to begin -> ");
        ClearScreen(initialCursor);
    }
}
