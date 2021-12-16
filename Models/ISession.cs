public interface ISession
{
    int Points { get; set; }

    void Start();

    void DisplayBeforeSession();

    void DisplayAfterSession();

    void DisplayStatusFor(string logs);
}
