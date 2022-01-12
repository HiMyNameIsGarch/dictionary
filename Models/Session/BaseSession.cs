using static System.Console;
using static ConsoleHelper;

public abstract class BaseSession: ISession 
{
    public BaseSession(SessionData data) 
    { 
        Data = data; 
        ResponseTime = new Avarage("Took -> ", " seconds.");
        Accuracy = new Avarage("Accuracy -> ", "%.");
    }

    private const double TypoMystake = 80.0;
    protected int CurrentPair = 0;
    public int Points { get; set; }
    public int TotalPairs { get; set; }
    public Avarage ResponseTime { get; }
    public Avarage Accuracy { get; }

    protected string Delimiter 
    {
        get {
            if(CurrentPair != 0)
                return $"-----------------< {CurrentPair} / {TotalPairs} >-----------------";
            else
                return "------------------------------------------";
        }
    }

    public SessionData Data { get; }
    public Config config { get { return Data.Config; } }
    public Dictionary<string[], string[]> pairs { get { return Data.Pairs; } }

    public void Start(Dictionary<string[], string[]> pairs)
    {
        Data.SetPairs(pairs);
        Data.ResetWrongPairs();
        TotalPairs = pairs.Count;
        CurrentPair = 1;
        foreach(var words in pairs)
        {
            int cursorTop = Console.CursorTop;
            WriteLine(Delimiter);
            int currentPoints = AskQuestion(words.Key, words.Value);

            if(currentPoints == 0)
                Data.WrongPairs.Add(words.Key, words.Value);
            else 
                Points += currentPoints;

            ClearScreen(cursorTop);
            CurrentPair++;
        }
    }

    public abstract int AskQuestion(string[] words, string[] synonyms);

    public abstract void DisplayStatusFor(string logs);

    // Hooks
    public virtual void BeforeSessionHook()
    {
        Points = 0;
        Data.ShufflePairs();
        WriteLine($"\nSession type: {config.FileExtension.ToString()}");
        WriteLine($"Session started on file '{config.CurrentFile}'");
        WriteLine($"Mode type: {config.Mode.ToString()}\n");
    }
    public virtual void AfterSessionHook()
    {
        if(config.DisplayFinalStatistics) 
        {
            WriteLine($"\nWow, you got {Points} points out of {TotalPairs}");
            WriteLine($"Avarage response time -> {ResponseTime.AvarageNum} seconds.");
            WriteLine($"Avarage accuracy -> {Accuracy.AvarageNum}%.");
        }
        Points = 0;
        ResponseTime.ResetValue();
        Accuracy.ResetValue();
    }

    private protected bool IsAnswerRight(int lastAnswers = 1)
    {
        double currentAccuracy = Accuracy.GetLast(lastAnswers);
        if(config.Over80IamCorrect)
        {
            return currentAccuracy > TypoMystake;
        }
        return currentAccuracy == EditDistance.MaxAccuracy;
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
            ResponseTime.Add(timeSpan.TotalSeconds);
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
        if(!config.DisplayOnPairStatistics) return;
        if(isPositive)
        {
            ColorWriteLine("Correct!", ConsoleColor.Green, config.OutputHasColors);
            onPositive();
        }
        else
        {
            ColorWriteLine("Incorrect!", ConsoleColor.Red, config.OutputHasColors);
            onNegative();
        }
        PressKeyToContinue();
    }

    protected void PressKeyToContinue(string prompt = "Press any key to continue -> ")
    {
        if(config.Layout != LayoutType.Card) return;
        if(CurrentPair == pairs.Count) return;
        // Make sure statistics are on screen before clean
        ConsoleHelper.PressKeyToContinue(prompt);
    }

    public void ClearScreen(int cursorBefore)
    {
        if(config.Layout != LayoutType.Card) return;
        if(CurrentPair == pairs.Count) return;

        ConsoleHelper.ClearScreen(cursorBefore);
    }

    public string CombineWords(string[] words, bool useSetting = true)
    {
        if(config.DisplayOneRandomSynonym && useSetting) 
        {
            return GetRandomSynonym(words);
        }

        return words.Combine(" or ");
    }

    public string GetRandomSynonym(string[] synonyms)
    {
        if(synonyms.Length == 0) return "";
        if(synonyms.Length == 1) return synonyms[0];

        Random rand = new Random();
        int currentSynonym = rand.Next(0, synonyms.Length);

        return synonyms[currentSynonym];
    }
}
