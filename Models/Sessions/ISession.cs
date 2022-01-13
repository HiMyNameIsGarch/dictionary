public interface ISession
{
    SessionData Data { get; }

    int Points { get; set; }

    int TotalPairs { get; set; }

    Average ResponseTime { get; }

    Average Accuracy { get; }

    void Start(Dictionary<string[], string[]> pairs);

    void BeforeSessionHook();

    void AfterSessionHook();

    void DisplayStatusFor(string logs);
}
