using static System.Console;
using static ConsoleHelper;

public class WordsSession : BaseSession
{

    public WordsSession(SessionData sessionData) : base(sessionData) { }

    public override void Start()
    {
        var cursorTop = Console.CursorTop;
        var values = new Dictionary<string[], string[]>();

        if(config.ReverseWords)
            values = pairs.ToDictionary(k => k.Value, v => v.Key);
        else 
            values = pairs;

        foreach(var words in values)
        {
            CurrentPair++;
            WriteLine(Delimiter);
            Points += AskQuestions(words.Key, words.Value);
            ClearScreen(cursorTop);
        }
    }

    private int AskQuestions(string[] words, string[] synonyms)
    {
        var data = AskQuestion(words, synonyms);
        if(!data.Item2) return 0; // if you don't know one word, don't bother asking the others
        if(synonyms.Length == 1 || !config.AskMeSynonyms) 
            return data.Item2 ? 1 : 0;

        ColorWriteLine("\nIt's show time, I hope you know synonyms!", ConsoleColor.Cyan, config.OutputHasColors);
        Thread.Sleep(2000);

        return GetPointsFromSynonyms(words, 
                synonyms.Where(s => s != data.Item1).ToArray());
    }

    private int GetPointsFromSynonyms(string[] words, string[] synonyms)
    {
        int points = 1; // the user already got a point from the previous question
        while(synonyms.Length != 0)
        {
            var data = AskQuestion(words, synonyms);
            if(data.Item2) // if question is correct
            {
                points++;
                synonyms = RemoveElement(synonyms, data.Item1);
                if(config.Layout == LayoutType.Card) Write("\n");
            }
            else break;
        }
        return points;
    }

    private Tuple<string,bool> AskQuestion(string[] words, string[] synonyms)
    {
        string response = GetUserResponse(GetQuestionString(words));

        var acc = CalculateAccuracy(synonyms, response);
        Accuracy.Add(acc.Item2);

        bool isCorrect = synonyms.Any(response.Equals);
        isCorrect = Over80IamCorrect(isCorrect);

        ShowResponseStatus(isCorrect, OnPositiveResponse, 
                () => OnNegativeResponse(synonyms, acc.Item1));

        return new Tuple<string, bool>(acc.Item1, isCorrect);
    }

    private void OnPositiveResponse()
    {
        if(config.Layout == LayoutType.Card)
        {
            ColorWriteLine(ResponseTime.GetText(), ConsoleColor.Cyan, config.OutputHasColors);
        }
    }

    private void OnNegativeResponse(string[] correctWords, string mostAccurate)
    {
        if(correctWords.Length > 1)
        {
            ColorWrite("Most accurate word: ", ConsoleColor.Blue, config.OutputHasColors);
            Write(mostAccurate + "\n");
        }
        ColorWrite("The answer can be: ", ConsoleColor.Blue, config.OutputHasColors);
        Write(CombineWords(correctWords, false) + "\n");

        ColorWriteLine(Accuracy.GetText(), ConsoleColor.Cyan, config.OutputHasColors);
    }

    private string GetQuestionString(string[] words)
    {
        string question = "What means -> ";
        question += CombineWords(words);
        question += ": ";
        return question;
    }

    private string[] RemoveElement(string[] array, string element)
    {
        return array.Where(s => s != element).ToArray();
    }


    public override void DisplayAfterSession() 
    {
        int totalPoints = 0;
        if(config.AskMeSynonyms)
        {
            foreach(var synonyms in pairs.Values) 
                totalPoints += synonyms.Length;
        } 
        else totalPoints = pairs.Count;
        WriteLine($"Wow, you got {Points} of {totalPoints} points!");
        WriteLine($"Avarage response time -> {ResponseTime.AvarageNum} seconds.");
        WriteLine($"Avarage accuracy -> {Accuracy.AvarageNum}.");
    }

    public override void DisplayStatusFor(string logs)
    {
        throw new NotImplementedException();
    }
}
