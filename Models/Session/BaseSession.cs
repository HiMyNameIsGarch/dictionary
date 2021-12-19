using static System.Console;

public abstract class BaseSession: ISession 
{
    public BaseSession(SessionData data) { _data = data; }

    protected int CurrentPair = 0;

    protected string Delimiter 
    {
        get {
            if(CurrentPair != 0)
                return $"-----------------< {CurrentPair} / {pairs.Count} >-----------------";
            else
                return "----------------------------------";
        }
    }

    private readonly SessionData _data;

    public int Points { get; set; }

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
        WriteLine($"\nSession type: {config.FileExtension.ToString()}");
        WriteLine($"Session started on file '{config.CurrentFile}'\n");
    }

    public virtual void DisplayAfterSession()
    {
        if(config.DisplayFinalStats) 
            WriteLine($"\nWow, you got {Points} points out of {pairs.Count}");
    }

    public string GetUserResponse(string question)
    {
        string? response = "";
        do 
        {
            WriteQuestion(question);
            response = ReadLine()?.Trim();
        }
        while(string.IsNullOrWhiteSpace(response));
        return response;
    }

    public virtual void WriteQuestion(string question = "AMA")
    {
        Write(question);
    }

    public string CombineWords(string[] words)
    {
        string combinedWords = "";
        for(int i = 0; i < words.Length; i++) 
        {
            combinedWords += words[i];
            if(i + 1 != words.Length) combinedWords += " or ";
        }
        return combinedWords;
    }

    public abstract void Start();

    public abstract void DisplayStatusFor(string logs);
}
