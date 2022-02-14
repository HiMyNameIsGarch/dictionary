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
        WriteLine("2. Average Statistics of all sessions");
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
            case '2':
                AvarageStatisticsForAllSessions(data);
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
        Graph graph = GetResponseTimeGraph(timeResponses);
        graph.Draw();

        var accuracies = lastSession.Accuracies.Select(a => (int)a).ToArray();
        WriteLine("Evolution of accuracy: ");
        Graph agraph = GetAccuracyGraph(accuracies);
        agraph.Draw();

    }

    private void AvarageStatisticsForAllSessions(ICollection<StatisticModel> data)
    {
        int[] times = data.Select(s => (int)s.AverageResponseTime).ToArray();
        WriteLine("\nEvolution of response time: ");
        Graph graph = GetResponseTimeGraph(times);
        graph.Draw();

        int[] acc = data.Select(s => (int)s.AverageAccuracy).ToArray();
        WriteLine("Evolution of accuracy: ");
        Graph agraph = GetAccuracyGraph(acc);
        agraph.Draw();
    }

    private Graph GetResponseTimeGraph(int[] timeResponses)
    {
        var yaxis = new Bar(0, 5, 1);
        var xaxis = new Bar(1, timeResponses.Length, 1);
        var graph = new Graph(yaxis, xaxis, timeResponses, true);
        return graph;
    }

    private Graph GetAccuracyGraph(int[] accuracies)
    {
        var yaxis = new Bar(10, 100, 10);
        var xaxis = new Bar(1, accuracies.Length, 1);
        var graph = new Graph(yaxis, xaxis, accuracies, false);
        return graph;
    }

    private ICollection<StatisticModel> GetAllStatistics()
    {
        return new StatisticParser().ParseFiles();
    }
}
