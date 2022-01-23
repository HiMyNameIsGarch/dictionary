public interface ISession
{
    SessionData Data { get; }

    Statistics Stats { get; }

    void Start(Dictionary<string[], string[]> pairs);

    void BeforeSessionHook();

    void AfterSessionHook();

    void DisplayStatusFor(string logs);
}
