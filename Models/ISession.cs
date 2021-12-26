public interface ISession
{
    int Points { get; set; }

    Avarage ResponseTime { get; }

    Avarage Accuracy { get; }

    void Start();

    void DisplayBeforeSession();

    void DisplayAfterSession();

    void DisplayStatusFor(string logs);
}
