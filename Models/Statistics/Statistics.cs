using static System.Console;

public class Statistics
{
    public void Store(SessionData data)
    {
        var model = MapData(data);
        var ts = DateTime.Now - DateTime.UnixEpoch;
        string fileName = (int)ts.TotalSeconds + ".json";
        StatisticParser sparser = new StatisticParser(fileName);
        sparser.StoreModel(model);
    }

    private StatisticModel MapData(SessionData data)
    {
        StatisticModel model = new StatisticModel();
        model.Mode = data.Config.Mode;
        model.FileName = data.Config.CurrentFile;
        model.Points = data.Points;
        model.TotalPoints = data.TotalPoints;
        model.AverageAccuracy = data.Accuracy.AvarageNum;
        model.AverageResponseTime = data.ResponseTime.AvarageNum;
        model.TimeResponses = data.ResponseTime.Values;
        model.Accuracies = data.Accuracy.Values;
        return model;
    }

    public void ShowOptions()
    {
        WriteLine("Display the evolution for: ");
        WriteLine("1. Statistics for very last session");
        Write("Press the number of the option: ");
        char key = Console.ReadKey().KeyChar;
        Write("\n\n");
        DisplayOptionFor(key);
    }

    private void DisplayOptionFor(char key)
    {
        var data = GetAllStatistics();
        switch(key)
        {
            case '1':
                StatisticsForLastSession(data.Last());
                break;
           default: 
                break;
        }
    }

    private void StatisticsForLastSession(StatisticModel lastSession)
    {
        WriteLine("Filename: " + lastSession.FileName);
        WriteLine("Mode: " + lastSession.Mode.ToFormattedString());
        var timeResponses = lastSession.TimeResponses.Select(t => (int)t).ToArray();

        WriteLine("\nEvolution of response time: ");
        var rleft = new Bar(0, 5, 1);
        var rright = new Bar(1, timeResponses.Length, 1);
        var graph = new Graph(rleft, rright, timeResponses, true);
        graph.Draw();

        var accuracies = lastSession.Accuracies.Select(a => (int)a).ToArray();
        WriteLine("Evolution of accuracy: ");
        var aleft = new Bar(10, 100, 10);
        var aright = new Bar(1, accuracies.Length, 1);
        var agraph = new Graph(aleft, aright, accuracies, false);
        agraph.Draw();

    }

    private ICollection<StatisticModel> GetAllStatistics()
    {
        return new StatisticParser().ParseFiles();
    }
}
