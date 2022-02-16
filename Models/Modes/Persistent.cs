public class Persistent: IMode 
{
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
            if(pairs.Count != 0)
            {
                Console.WriteLine("\nOops, not finished yet, you still got some words that you don't know yet\n");
            }
        }
        while(pairs.Count > 0);
    }
}
