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
            WriteLine($"\nWow, you got {Points} points out of {pairs.Count}");
            WriteLine($"Avarage response time -> {ResponseTime.AvarageNum} seconds.");
            WriteLine($"Avarage accuracy -> {Accuracy.AvarageNum}%.");
        }
    }

    private protected bool IsAnswerRight(int lastAnswers = 1)
    {
        double currentAccuracy = Accuracy.GetLast(lastAnswers);
        if(config.Over80IamCorrect)
        {
            return currentAccuracy > 80;
        }
        return currentAccuracy == (double)100;
    }

    public double CalculateAccuracy(string wanted, string got)
    {
        if(wanted == got) return 100;
        char[] wantedArray = FillArray(wanted);
        char[] gotArray = FillArray(got);
        int[,] matrix = new int[gotArray.Length, wantedArray.Length];
        int k = 0;
        foreach(char c in gotArray)
        {
            matrix[k, 0] = k;
            k++;
        }
        k = 0;
        foreach(char c in wantedArray)
        {
            matrix[0, k] = k;
            k++;
        }
        // Calculate edit distance
        double editDistance = EditDistance(gotArray, wantedArray, matrix);
        if(editDistance == 0) return 100;
        // Display matrix
        double num = (double)wanted.Length / editDistance;
        return 100 - (100 / num);
    }

    private char[] FillArray(string word)
    {
        char[] array = new char[word.Length + 1];
        array[0] = ' ';
        int i = 1;
        foreach(char c in word)
        {
            array[i] = c;
            i++;
        }
        return array;
    }

    private double EditDistance(char[] wanted, char[] got, int[,] matrix)
    {
        for (int i = 1; i < wanted.Length; i++)
        {
            for (int j = 1; j < got.Length; j++)
            {
                int min = Min(matrix, i, j);
                if(wanted[i] == got[j])
                    matrix[i,j] = min;
                else
                    matrix[i,j] = min + 1;
            }
        }
        return matrix[wanted.Length - 1, got.Length - 1];
    }

    private int Min(int[,] matrix, int i, int j)
    {
        int insert = matrix[i, j - 1];
        int delete = matrix[i - 1, j];
        int replace = matrix[i - 1, j - 1];
        if(insert < delete && insert < replace)
            return insert;
        else if (delete < insert && delete < replace)
            return delete;
        else 
            return replace;
    }

    private void DisplayMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i,j] + "   ");
            }
            Console.WriteLine("\n");
        }
    }

    public Tuple<string,double> CalculateAccuracy(string[] wanted, string got)
    {
        double mostAccurate = -1;
        string correctWord = "";
        foreach(var word in wanted)
        {
            double acc = CalculateAccuracy(word, got);
            if(acc > mostAccurate)
            {
                mostAccurate = acc;
                correctWord = word;
            }
        }
        return new Tuple<string, double>(correctWord, mostAccurate);
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
        Console.Write(prompt);
        Console.ReadKey(true);
    }

    public void ClearScreen(int cursorBefore)
    {
        if(config.Layout != LayoutType.Card) return;
        if(CurrentPair == pairs.Count) return;
        // 
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

    public string CombineWords(string[] words, bool useSetting = true)
    {
        if(config.DisplayOneRandomSynonym && useSetting) 
        {
            return GetRandomSynonym(words);
        }
        string combinedWords = "";
        for(int i = 0; i < words.Length; i++) 
        {
            combinedWords += words[i];
            if(i + 1 != words.Length) combinedWords += " or ";
        }
        return combinedWords;
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
