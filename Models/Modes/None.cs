public class None: IMode 
{
    public void Start(ISession session)
    {
        session.BeforeSessionHook();
        session.Start(session.Data.Pairs);
        session.Stats.Store(session.Data);
        session.AfterSessionHook();
    }
}
