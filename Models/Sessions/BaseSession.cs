using static System.Console;
using static ConsoleHelper;

public abstract class BaseSession: ISession 
{
    public BaseSession(SessionData data) 
    { 
        Data = data; 
        Stats = new Statistics();
    }

    private const string _separator = " or ";
    private const double _typoMistake= 80.0;
    protected int CurrentPair = 0;
    public SessionData Data { get; }
    public Statistics Stats { get; }

    private void DisplayDelimiter()
    {
        if(CurrentPair != 0)
        {
            Write("─────────────────┤ ");
            ColorWrite(CurrentPair.ToString(), CurrentPair != Data.TotalPairs ? ConsoleColor.DarkCyan : ConsoleColor.DarkBlue);
            Write(" / ");
            ColorWrite(Data.TotalPairs.ToString(), ConsoleColor.DarkBlue);
            Write(" ├─────────────────\n");
        }
        else WriteLine("──────────────────────────────────────────");
    }

    public void Start(Dictionary<string[], string[]> pairs)
    {
        if(pairs.Count < 1)
        {
            ColorWriteLine("No pairs found, abording.", ConsoleColor.Red);
            return;
        }
        Data.SetPairs(pairs);
        Data.ResetWrongPairs();
        Data.TotalPairs = pairs.Count;
        CurrentPair = 1;
        foreach(var words in pairs)
        {
            int cursorTop = Console.CursorTop;
            DisplayDelimiter();
            var points = AskQuestion(words.Key, words.Value);

            if(points.Item1 < points.Item2)
            {
                Data.WrongPairs.Add(words.Key, words.Value);
            }
            Data.Points += points.Item1;
            Data.TotalPoints += points.Item2;

            ClearScreen(cursorTop);
            CurrentPair++;
        }
    }

    //This will return a tuple that will contain the <current points> and the <maximum> the could achive
    public abstract Tuple<int,int> AskQuestion(string[] words, string[] synonyms);
    
    // Hooks
    public virtual void BeforeSessionHook()
    {
        Data.Points = 0;
        Data.ShufflePairs();
        Write("\nSession Type    : ");
        ColorWrite(Data.Config.FileExtension.ToFormattedString(), ConsoleColor.DarkBlue);
        Write("\nStarted on file : ");
        ColorWrite(Data.Config.CurrentFile, ConsoleColor.DarkCyan);
        Write("\nWith mode       : ");
        ColorWrite(Data.Config.Mode.ToFormattedString() + "\n\n", ConsoleColor.DarkMagenta);
    }
    public virtual void AfterSessionHook()
    {
        if(Data.Config.DisplayFinalStatistics) 
        {
            if(Data.Config.Layout == LayoutType.List) Write("\n");
            Write("You got ");
            ColorWrite(Data.Points.ToString(), Data.Points != Data.TotalPoints ? ConsoleColor.DarkGreen : ConsoleColor.Green);
            Write(" points out of ");
            ColorWrite(Data.TotalPoints.ToString(), ConsoleColor.Green);
            Write("\n");

            Write("Average response time -> ");
            ColorWrite(Data.ResponseTime.AvarageNum.ToString(), ConsoleColor.DarkCyan);
            Write(" seconds.\n");

            Write("Average accuracy      -> ");
            ColorWrite(Data.Accuracy.AvarageNum.ToString(), ConsoleColor.DarkYellow);
            Write("%.\n");
        }
        Data.Points = 0;
        Data.TotalPoints = 0;
        Data.ResponseTime.ResetValue();
        Data.Accuracy.ResetValue();
    }

    private protected bool IsAnswerRight(int lastAnswers = 1)
    {
        double currentAccuracy = Data.Accuracy.GetLast(lastAnswers);
        if(Data.Config.Over80IamCorrect)
        {
            return currentAccuracy > _typoMistake;
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
            Data.ResponseTime.Add(timeSpan.TotalSeconds);
        }
        while(string.IsNullOrWhiteSpace(response));
        return response;
    }

    public virtual void WriteQuestion(string question = "AMA")
    {
        string[] parts = question.Split(_separator);
        for(int i = 0; i < parts.Length; i++)
        {
            Write(parts[i]);
            if(i != parts.Length - 1) ColorWrite(_separator, ConsoleColor.Cyan);
        }
    }

    protected void ShowResponseStatus(bool isPositive, Action onPositive, Action onNegative)
    {
        if(!Data.Config.DisplayOnPairStatistics) return;
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

    private bool IsLastQuestion() => 
        Data.Config.Mode != ModeType.LearnAndAnswer && 
        CurrentPair == Data.Pairs.Count;

    protected void PressKeyToContinue(string prompt = "Press any key to continue -> ")
    {
        if(Data.Config.Layout != LayoutType.Card) return;

        ConsoleHelper.PressKeyToContinue(prompt);
    }

    public void ClearScreen(int cursorBefore)
    {
        if(Data.Config.Layout != LayoutType.Card) return;

        ConsoleHelper.ClearScreen(cursorBefore);

        if(IsLastQuestion()) DisplayDelimiter();
    }

    public string CombineWords(string[] words, bool useSetting = true)
    {
        if(Data.Config.DisplayOneRandomSynonym && useSetting) 
        {
            return GetRandomSynonym(words);
        }

        return words.Combine(_separator);
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
