public interface ISession
{
    SessionData Data { get; }

    int Points { get; set; }

    int TotalPairs { get; set; }

    Avarage ResponseTime { get; }

    Avarage Accuracy { get; }

    void Start(Dictionary<string[], string[]> pairs);

    void BeforeSessionHook();

    void AfterSessionHook();

    void DisplayStatusFor(string logs);
}
