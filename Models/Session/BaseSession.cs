using static System.Console;
using static ConsoleHelper;

public abstract class BaseSession: ISession 
{
    public BaseSession(SessionData data) 
    { 
        _data = data; 
        ResponseTime = new Avarage("Took -> ", " seconds.");
        Accuracy = new Avarage("Accuracy -> ", "%.");
    }

    protected int CurrentPair = 0;
    public int Points { get; set; }
    public Avarage ResponseTime { get; }
    public Avarage Accuracy { get; }

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
    private protected Config config { get { return _data.Config; } }
    private protected Dictionary<string[], string[]> pairs
    { 
        get { return _data.Pairs; }
    }

    public abstract void Start();

    public abstract void DisplayStatusFor(string logs);

    //
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
            WriteLine($"Avarage response time -> {ResponseTime.AvarageNum} seconds.");
            WriteLine($"Avarage accuracy -> {Accuracy.AvarageNum}%.");
        }
    }

    public double CalculateAccuracy(string wanted, string got)
    {
        return wanted.Length;
    }

    public double CalculateAccuracy(string[] wanted, string got)
    {
        return wanted.Length;
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
            ResponseTime.Values.Add(timeSpan.TotalSeconds);
        }
        while(string.IsNullOrWhiteSpace(response));
        return response;
    }

    public virtual void WriteQuestion(string question = "AMA")
    {
        Write(question);
    }

    protected void ShowResponseStatus(bool isPositive, Action onPositive, Action onNegative)
    {
        if(!config.DisplayOnPairStats) return;
        if(isPositive)
        {
            ColorWriteLine("Correct!", ConsoleColor.Green, config.HasColors);
            onPositive();
        }
        else
        {
            ColorWriteLine("Incorrect!", ConsoleColor.Red, config.HasColors);
            onNegative();
        }
        PressKeyToContinue();
    }

    protected void PressKeyToContinue()
    {
        // Make sure statistics are on screen before clean
        if(config.Layout == LayoutType.Card || CurrentPair != pairs.Count) 
        {
            Console.Write("Press any key to continue -> ");
            Console.ReadKey(true);
        }
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
}
