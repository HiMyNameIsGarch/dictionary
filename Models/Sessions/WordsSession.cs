using static System.Console;
using static ConsoleHelper;

public class WordsSession : BaseSession
{

    public WordsSession(SessionData sessionData) : base(sessionData) { }

    public override Tuple<int,int> AskQuestion(string[] words, string[] synonyms)
    {
        if(Data.Config.ReverseWords)
        {
            var tempWords = words;
            words = synonyms;
            synonyms = tempWords;
        }
        int maxPoints = (Data.Config.AskMeSynonyms) ? synonyms.Length : 1;
        var data = GetSynonym(words, synonyms);
        
        // if you don't know one word, don't bother asking the others
        if(!data.Item2) return new Tuple<int,int>(0, maxPoints); 
        if(synonyms.Length == 1 || !Data.Config.AskMeSynonyms) 
        {
            int currentPoints = data.Item2 ? synonyms.Length : 0;
            return new Tuple<int, int>(currentPoints, maxPoints);
        }

        ColorWriteLine("\nIt's show time, I hope you know synonyms!", ConsoleColor.Cyan);
        Thread.Sleep(1500);

        int pointsWithSynonyms = GetPointsFromSynonyms(words, 
                synonyms.Where(s => s != data.Item1).ToArray());
        return new Tuple<int,int>(pointsWithSynonyms, maxPoints);
    }

    private int GetPointsFromSynonyms(string[] words, string[] synonyms)
    {
        int points = 1; // the user already got a point from the previous question
        while(synonyms.Length != 0)
        {
            var data = GetSynonym(words, synonyms);
            if(data.Item2) // if question is correct
            {
                points++;
                synonyms = RemoveElement(synonyms, data.Item1);
                if(Data.Config.Layout == LayoutType.Card) Write("\n");
            }
            else break;
        }
        return points;
    }

    private Tuple<string,bool> GetSynonym(string[] words, string[] synonyms)
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

    public override void DisplayStatusFor(string logs)
    {
        throw new NotImplementedException();
    }
}
