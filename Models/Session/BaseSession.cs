using static System.Console;

public abstract class BaseSession: ISession 
{
    public BaseSession(DataSession data) { _data = data; }

    public int Points { get; set; }

    private readonly DataSession _data;

    private protected Config config
    { 
        get { return _data.Config; } 
    }
    private protected Dictionary<string[], string[]> pairs
    { 
        get { return _data.Pairs; }
    }

    public virtual void DisplayBeforeSession() 
    {
        Points = 0;
        _data.ShufflePairs();
        WriteLine($"Session type: {config.FileExtension.ToString()}");
        WriteLine($"Session started on file '{config.CurrentFile}'");
    }

    public virtual void DisplayAfterSession()
    {
        WriteLine($"\nWow, you got {Points} points out of {pairs.Count}");
    }

    public abstract void Start();

    public abstract void DisplayStatusFor(string logs);
}
