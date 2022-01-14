using static System.Console;
using static ConsoleHelper;

public abstract class BaseSession: ISession 
{
    public BaseSession(SessionData data) 
    { 
        Data = data; 
        ResponseTime = new Average("Took -> ", " seconds.");
        Accuracy = new Average("Accuracy -> ", "%.");
    }

    private const double TypoMystake = 80.0;
    protected int CurrentPair = 0;
    public int Points { get; set; }
    public int TotalPairs { get; set; }
    public Average ResponseTime { get; }
    public Average Accuracy { get; }

    private void DisplayDelimiter()
    {
        if(CurrentPair != 0)
        {
            Write("-----------------< ");
            ColorWrite(CurrentPair.ToString(), CurrentPair != TotalPairs ? ConsoleColor.DarkCyan : ConsoleColor.DarkBlue);
            Write(" / ");
            ColorWrite(TotalPairs.ToString(), ConsoleColor.DarkBlue);
            Write(" >-----------------\n");
        }
        else WriteLine("------------------------------------------");
    }
    public SessionData Data { get; }
    public ConfigOptions config { get { return Data.Config; } }
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
            DisplayDelimiter();
            int currentPoints = AskQuestion(words.Key, words.Value);

            if(currentPoints == 0 || (config.AskMeSynonyms && currentPoints < words.Value.Length))
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
        Write("\nSession type: ");
        ColorWrite(config.FileExtension.ToString(), ConsoleColor.DarkBlue);
        Write("\nSession started on file: ");
        ColorWrite(config.CurrentFile, ConsoleColor.DarkCyan);
        Write("\nSession type: ");
        ColorWrite(config.Mode.ToString() + "\n\n", ConsoleColor.DarkMagenta);
    }
    public virtual void AfterSessionHook()
    {
        if(config.DisplayFinalStatistics) 
        {
            if(config.Mode != ModeType.LearnAndAnswer || config.Layout == LayoutType.List) Write("\n");
            Write("You got ");
            ColorWrite(Points.ToString(), Points != TotalPairs ? ConsoleColor.DarkGreen : ConsoleColor.Green);
            Write(" points out of ");
            ColorWrite(TotalPairs.ToString(), ConsoleColor.Green);
            Write("\n");

            Write("Average response time -> ");
            ColorWrite(ResponseTime.AvarageNum.ToString(), ConsoleColor.DarkCyan);
            Write(" seconds.\n");

            Write("Average accuracy -> ");
            ColorWrite(Accuracy.AvarageNum.ToString(), ConsoleColor.DarkYellow);
            Write("%.\n");
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
            ColorWriteLine("Correct!", ConsoleColor.Green);
            onPositive();
        }
        else
        {
            ColorWriteLine("Incorrect!", ConsoleColor.Red);
            onNegative();
        }
        PressKeyToContinue();
    }

    protected void PressKeyToContinue(string prompt = "Press any key to continue -> ")
    {
        if(config.Layout != LayoutType.Card) return;
        if(config.Mode != ModeType.LearnAndAnswer && CurrentPair == pairs.Count) return;
        // Make sure statistics are on screen before clean
        ConsoleHelper.PressKeyToContinue(prompt);
    }

    public void ClearScreen(int cursorBefore)
    {
        if(config.Layout != LayoutType.Card) return;
        if(config.Mode != ModeType.LearnAndAnswer && CurrentPair == pairs.Count) return;

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
