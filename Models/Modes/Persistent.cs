using static ConsoleHelper;

public class Persistent: IMode 
{
    private const string _errorMessage = "Oops, not finished yet, you still got some words that you don't know yet";

    public void Start(ISession session)
    {
        session.BeforeSessionHook();
        var pairs = session.Data.Pairs;
        do
        {
            session.Start(pairs);
            pairs = session.Data.WrongPairs;
            if(pairs.Count == 0) session.Stats.Store(session.Data);
            session.AfterSessionHook();
            ContinueWithWrongPairs(pairs.Count);
        }
        while(pairs.Count > 0);
    }

    private void ContinueWithWrongPairs(int pairsCount)
    {
        if(pairsCount == 0) return;

        int cursorTop = Console.CursorTop;
        ColorWriteLine(_errorMessage, ConsoleColor.Red);
        PressKeyToContinue("Press any key to continue: ");
        ClearScreen(cursorTop);
        Console.Write("\n");
    }
}
