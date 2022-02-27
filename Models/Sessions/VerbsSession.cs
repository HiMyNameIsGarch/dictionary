using static System.Console;
using static ConsoleHelper;

public class VerbsSession : BaseSession
{

    public VerbsSession(SessionData sessionData) : base(sessionData) {}

    public override Tuple<int,int> AskQuestion(string[] values, string[] verbs) 
    {
        string prompt = CombineWords(values);
        var verbsArray = verbs.ToArray();

        var firstVerb = GetValue(1, prompt, verbsArray);
        var secondVerb = GetValue(2, prompt, verbsArray);
        var thirdVerb = GetValue(3, prompt, verbsArray);

        var currentInputVerbs = new IrregularVerbs(firstVerb, secondVerb,
                thirdVerb);
        var correctVerbs = new IrregularVerbs(verbs[0], verbs[1], verbs[2]);

        int currentPoints = correctVerbs.GetPointsFrom(currentInputVerbs);
        bool isCorrect = IsAnswerRight(3);

        ShowResponseStatus(isCorrect, OnPositiveResponse, 
                () => OnNegativeResponse(correctVerbs, currentInputVerbs,
                                         currentPoints));

        return new Tuple<int,int>(currentPoints, IrregularVerbs.MaxVerbs);
    }

    private string GetValue(int num, string prompt, string[] values)
    {
        string verb = GetForm(num, prompt);
        double accuracy = EditDistance.GetAccuracy(values[num - 1], verb);
        AddAccuracy(accuracy);
        return verb;
    }

    private string GetForm(int num, string prompt)
    {
        string termination = num == 1 ? "ST" : num == 2 ? "ND" : num == 3 ? "RD" : "";
        return GetUserResponse($"Write <{num}{termination}> form of -> {prompt}: ");
    }

    private void OnPositiveResponse()
    {
        if(Data.Config.Layout == LayoutType.Card)
        {
            Data.ResponseTime.DisplayTextOnLast(3, Data.Config.DisplayAvarageStatistics);
        }
    }

    private void OnNegativeResponse(IrregularVerbs currentVerbs, IrregularVerbs
            inputVerbs, int currentPoints)
    {
        DisplayCorrectAnswer(currentVerbs, inputVerbs);
        Data.Accuracy.DisplayTextOnLast(3, Data.Config.DisplayAvarageStatistics);
        Write("You got ");
        ColorWrite(currentPoints.ToString(), Data.Points != Data.TotalPoints ? ConsoleColor.DarkGreen : ConsoleColor.Green);
        Write(" points out of ");
        ColorWrite(IrregularVerbs.MaxVerbs.ToString(), ConsoleColor.Green);
        Write("\n");
    }

    private void DisplayCorrectAnswer(IrregularVerbs wanted, IrregularVerbs got)
    {
        Write("The answer is: ");
        ColorOutput(wanted.First, got.First);
        Write(" | ");
        ColorOutput(wanted.Second, got.Second);
        Write(" | ");
        ColorOutput(wanted.Third, got.Third);
        Write("\n");
    }

    private void ColorOutput(string wanted, string got)
    {
        ColorWrite(wanted, wanted == got ? ConsoleColor.Green :
                ConsoleColor.Red);
    }

    public override void BeforeSessionHook()
    {
        base.BeforeSessionHook();
        foreach(var verbs in Data.Pairs)
        {
            if(!IrregularVerbs.CanBuildVerbPairs(verbs.Value))
            {
                Data.Pairs.Remove(verbs.Key);
            }
        }
    }
}
