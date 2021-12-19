using static System.Console;
public class VerbsSession : BaseSession
{
    public VerbsSession(DataSession sessionData) : base(sessionData) {}

    public override void Start()
    {

        foreach(var verbs in pairs)
        {
            if(CanBuildVerbsPairs(verbs.Value))
            {
                var verbsPairs = new Tuple<string, string, string>(
                            verbs.Value[0], 
                            verbs.Value[1],
                            verbs.Value[2]);
                WriteLine("-----------------------");
                if(AskQuestion(verbs.Key, verbsPairs)) Points++;
            }
        }
    }
    public bool CanBuildVerbsPairs(string[] verbs) { return verbs.Length == 3; }

    public bool AskQuestion(string[] values, Tuple<string, string, string> verbs) 
    {
        string firstVerb = GetUserInput($"Write <1ST> form of -> {values[0]}: ");
        string secondVerb = GetUserInput($"Write <2ND> form of -> {values[0]}: ");
        string thirdVerb = GetUserInput($"Write <3RD> form of -> {values[0]}: ");
        bool IsCorrect = verbs.Item1 == firstVerb && verbs.Item2 == secondVerb && verbs.Item3 == thirdVerb;
        ShowResponseStatus(verbs, GetVerbsPairs(firstVerb, secondVerb, thirdVerb), IsCorrect);
        return IsCorrect;
    }

    private void ShowResponseStatus(Tuple<string,string,string> verbs, Tuple<string,string,string> input, bool IsPositive)
    {
        if(IsPositive)
        {
            WriteLine("Correct");
        }
        else
        {
            int correctVerbs = 0;
            if(verbs.Item1 == input.Item1) correctVerbs++;
            if(verbs.Item2 == input.Item2) correctVerbs++;
            if(verbs.Item3 == input.Item3) correctVerbs++;
            WriteLine("Incorrect");
            WriteLine($"You got {correctVerbs} of 3");
            WriteLine($"The answer is: {verbs.Item1} | {verbs.Item2} | {verbs.Item3}\n");
        }
    }

    private Tuple<string, string, string> GetVerbsPairs(string first, string second, string third)
    {
        return new Tuple<string, string, string>(first,second, third);
    }

    private string GetUserInput(string question)
    {
        string? response = "";
        do 
        {
            Write(question);
            response = ReadLine()?.Trim();
        }
        while(string.IsNullOrWhiteSpace(response));
        return response;
    }
    public override void DisplayAfterSession() 
    {
        WriteLine($"omg I can't belive this you got {Points} of {pairs.Count} points!");
    }
    public override void DisplayBeforeSession() { base.DisplayBeforeSession(); }
    public override void DisplayStatusFor(string logs) { throw new NotImplementedException(); }
}
