using static System.Console;
using static ConsoleHelper;

public class WordsSession : BaseSession
{
    public WordsSession(SessionData sessionData) : base(sessionData) { }

    public override Tuple<int,int> AskQuestion(string[] words, string[] synonyms)
    {
        var consolePosition = Console.CursorTop;

        int maxPoints = 0;
        int currentPoints = 0;
        if(Data.Config.AskMeSynonyms)
        {
            maxPoints = synonyms.Length;
            currentPoints += GetPointsFromSynonyms(words, synonyms);
        }
        else
        {
            maxPoints = 1;
            currentPoints += GetWord(words, synonyms).Item2 ? 1 : 0;
        }

        return new Tuple<int,int>(currentPoints, maxPoints);
    }

    private void DisplaySynonymCount(int current, int max)
    {
        Console.Write("Synonym Count: ( ");
        ConsoleHelper.ColorWrite(current.ToString(), current == max ? ConsoleColor.DarkBlue : ConsoleColor.Cyan);
        Console.Write(" out of ");
        ConsoleHelper.ColorWrite(max.ToString(), ConsoleColor.DarkBlue);
        Console.Write(" )\n");
    }

    private int GetPointsFromSynonyms(string[] words, string[] synonyms)
    {
        int points = 0;
        int maxSynonyms = synonyms.Length;
        while(synonyms.Length != 0)
        {
            var consolePosition = Console.CursorTop;
            if(maxSynonyms > 1) DisplaySynonymCount((points + 1), maxSynonyms);
            var data = GetWord(words, synonyms);
            if(data.Item2) // if question is correct
            {
                points++;
                synonyms = RemoveElement(synonyms, data.Item1);
                ClearScreen(consolePosition);
            }
            else break;
        }
        return points;
    }

    private Tuple<string, bool> GetWord(string[] words, string[] synonyms)
    {
        string response = GetUserResponse(GetQuestionString(words));

        var acc = EditDistance.GetAccuracy(synonyms, response);
        Data.Accuracy.Add(acc.Item2);

        bool isCorrect = synonyms.Any(response.Equals);
        if(!isCorrect) isCorrect = IsAnswerRight();

        ShowResponseStatus(isCorrect, OnPositiveResponse, 
                () => OnNegativeResponse(synonyms, acc.Item1));

        return new Tuple<string, bool>(acc.Item1, isCorrect);
    }

    private void OnPositiveResponse()
    {
        if(Data.Config.Layout == LayoutType.Card)
        {
            Data.ResponseTime.DisplayText(Data.ResponseTime.LastValue, ConsoleColor.Cyan);
        }
    }

    private void OnNegativeResponse(string[] correctWords, string mostAccurate)
    {
        if(correctWords.Length > 1)
        {
            ColorWrite("Most accurate word: ", ConsoleColor.Blue);
            Write(mostAccurate + "\n");
        }
        ColorWrite("The answer can be: ", ConsoleColor.Blue);
        Write(CombineWords(correctWords, false) + "\n");

        Data.Accuracy.DisplayText(Data.Accuracy.LastValue, ConsoleColor.Cyan);
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

}
