using static System.Console;
using static ConsoleHelper;

public abstract class BaseSession: ISession 
{
    public BaseSession(SessionData data) 
    { 
        Data = data; 
    }

    private const double TypoMystake = 80.0;
    protected int CurrentPair = 0;
    public SessionData Data { get; }

    private void DisplayDelimiter()
    {
        if(CurrentPair != 0)
        {
            Write("-----------------< ");
            ColorWrite(CurrentPair.ToString(), CurrentPair != Data.TotalPoints ? ConsoleColor.DarkCyan : ConsoleColor.DarkBlue);
            Write(" / ");
            ColorWrite(Data.TotalPoints.ToString(), ConsoleColor.DarkBlue);
            Write(" >-----------------\n");
        }
        else WriteLine("------------------------------------------");
    }

    public void Start(Dictionary<string[], string[]> pairs)
    {
        Data.SetPairs(pairs);
        Data.ResetWrongPairs();
        Data.TotalPoints = pairs.Count;
        CurrentPair = 1;
        foreach(var words in pairs)
        {
            int cursorTop = Console.CursorTop;
            DisplayDelimiter();
            int currentPoints = AskQuestion(words.Key, words.Value);

            if(currentPoints < Data.GetExpectedPoints(words.Key, words.Value))
            {
                Data.WrongPairs.Add(words.Key, words.Value);
            }
            Data.Points += currentPoints;

            ClearScreen(cursorTop);
            CurrentPair++;
        }
    }

    public abstract int AskQuestion(string[] words, string[] synonyms);

    public abstract void DisplayStatusFor(string logs);

    // Hooks
    public virtual void BeforeSessionHook()
    {
        Data.Points = 0;
        Data.ShufflePairs();
        Write("\nSession type: ");
        ColorWrite(Data.Config.FileExtension.ToString(), ConsoleColor.DarkBlue);
        Write("\nSession started on file: ");
        ColorWrite(Data.Config.CurrentFile, ConsoleColor.DarkCyan);
        Write("\nSession type: ");
        ColorWrite(Data.Config.Mode.ToString() + "\n\n", ConsoleColor.DarkMagenta);
    }
    public virtual void AfterSessionHook()
    {
        if(Data.Config.DisplayFinalStatistics) 
        {
            if(Data.Config.Mode != ModeType.LearnAndAnswer || Data.Config.Layout == LayoutType.List) Write("\n");
            Write("You got ");
            ColorWrite(Data.Points.ToString(), Data.Points != Data.TotalPoints ? ConsoleColor.DarkGreen : ConsoleColor.Green);
            Write(" points out of ");
            ColorWrite(Data.TotalPoints.ToString(), ConsoleColor.Green);
            Write("\n");

            Write("Average response time -> ");
            ColorWrite(Data.ResponseTime.AvarageNum.ToString(), ConsoleColor.DarkCyan);
            Write(" seconds.\n");

            Write("Average accuracy -> ");
            ColorWrite(Data.Accuracy.AvarageNum.ToString(), ConsoleColor.DarkYellow);
            Write("%.\n");
        }
        Data.Points = 0;
        Data.ResponseTime.ResetValue();
        Data.Accuracy.ResetValue();
    }

    private protected bool IsAnswerRight(int lastAnswers = 1)
    {
        double currentAccuracy = Data.Accuracy.GetLast(lastAnswers);
        if(Data.Config.Over80IamCorrect)
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
            Data.ResponseTime.Add(timeSpan.TotalSeconds);
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

    protected void PressKeyToContinue(string prompt = "Press any key to continue -> ")
    {
        if(Data.Config.Layout != LayoutType.Card) return;
        if(Data.Config.Mode != ModeType.LearnAndAnswer && CurrentPair == Data.Pairs.Count) return;
        // Make sure statistics are on screen before clean
        ConsoleHelper.PressKeyToContinue(prompt);
    }

    public void ClearScreen(int cursorBefore)
    {
        if(Data.Config.Layout != LayoutType.Card) return;
        if(Data.Config.Mode != ModeType.LearnAndAnswer && CurrentPair == Data.Pairs.Count) return;

        ConsoleHelper.ClearScreen(cursorBefore);
    }

    public string CombineWords(string[] words, bool useSetting = true)
    {
        if(Data.Config.DisplayOneRandomSynonym && useSetting) 
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
