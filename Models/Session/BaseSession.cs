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

    private protected double _timeSpan;

    private protected string ResponseTimeText 
    {
        get { return "Took -> " + Math.Round(_timeSpan, + _decimals) + " seconds."; }
    }

    private const int _decimals = 2;

    private List<double> _timeResponses = new List<double>();

    private protected double GetAvarageResponseTime 
    {
        get { return _timeResponses.Count > 0 ? 
                Math.Round(_timeResponses.Average(), _decimals)  : 0.0; }
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
        {
            WriteLine($"\nWow, you got {Points} points out of {pairs.Count}");
            WriteLine($"Avarage response time -> {GetAvarageResponseTime} seconds.");
        }
    }

    public string GetUserResponse(string question)
    {
        string? response = "";
        do 
        {
            WriteQuestion(question);
            var before = DateTime.Now;
            response = ReadLine()?.Trim();
            var after = DateTime.Now;
            var timeSpan = after - before;
            _timeSpan = timeSpan.TotalSeconds;
            _timeResponses.Add(_timeSpan);
        }
        while(string.IsNullOrWhiteSpace(response));
        return response;
    }

    public virtual void WriteQuestion(string question = "AMA")
    {
        Write(question);
    }

    public void ClearScreen(int cursorBefore)
    {
        if(config.Layout == LayoutType.Card)
        {
            int currentCursor = Console.CursorTop;
            int linesToClear = cursorBefore - currentCursor - 1;
            linesToClear = linesToClear * -1;
            Console.SetCursorPosition(0, cursorBefore);
            for (int i = 0; i < linesToClear; i++)
            {
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, currentCursor - (linesToClear - 1));
        }
    }

    public void PressKeyToContinue()
    {
        // Make sure statistics are on screen before clean
        if(config.Layout == LayoutType.Card && CurrentPair != pairs.Count) 
        {
            Console.Write("Press any key to continue - ");
            Console.ReadKey(true);
        }
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
