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
        int cursorTop = Console.CursorTop;
        WriteLine("Display the evolution for: ");
        WriteLine("1. Statistics for very last session");
        WriteLine("2. Average statistics of all sessions");
        WriteLine("3. Average points accuracy per all files");
        char key = '0';
        do
        {
            Write("Press the number of the option: ");
            key = Console.ReadKey().KeyChar;
        }
        while(char.IsWhiteSpace(key) && !char.IsDigit(key));
        ConsoleHelper.ClearScreen(cursorTop);

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
            case '3':
                AvaragePointsAccuracyForAllFiles(data);
                break;
           default: 
                WriteLine("Invalid Option");
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

        int[] points = data.Select(s => GetAccuracy(s.TotalPoints, s.Points)).ToArray();
        WriteLine("Evolution of points: ");
        Graph pGraph = GetAccuracyGraph(points);
        pGraph.Draw();
    }

    private void AvaragePointsAccuracyForAllFiles(ICollection<StatisticModel> data)
    {
        Dictionary<string, int> pairs = new Dictionary<string, int>();
        foreach(var d in data)
        {
            int accuracy = GetAccuracy(d.TotalPoints, d.Points);
            if(pairs.ContainsKey(d.FileName))
            {
                pairs[d.FileName] = (int)Math.Round((double)(pairs[d.FileName] + accuracy) / 2);
                continue;
            }
            pairs.Add(d.FileName, accuracy);
        }
        Graph graph = GetAccuracyGraph(pairs.Values.ToArray());
        graph.Draw();
        WriteLine("Legend: ");
        int i = 1;
        foreach(var pair in pairs)
        {
            WriteLine(i + " - " + pair.Key);
            i++;
        }
    }

    private int GetAccuracy(int max, int current)
    {
        if(max == current) return 100;
        double num = (double)max / current;
        double accuracy = EditDistance.MaxAccuracy / num;
        if(accuracy < 0) accuracy = accuracy * -1;
        return (int)accuracy;
    }
    private Graph GetResponseTimeGraph(int[] timeResponses)
    {
        var yaxis = new Axis(0, 5, 1);
        var xaxis = new Axis(1, timeResponses.Length, 1);
        var graph = new Graph(yaxis, xaxis, timeResponses, true);
        return graph;
    }
    private Graph GetAccuracyGraph(int[] accuracies)
    {
        var yaxis = new Axis(10, 100, 10);
        var xaxis = new Axis(1, accuracies.Length, 1);
        var graph = new Graph(yaxis, xaxis, accuracies, false);
        return graph;
    }

    private ICollection<StatisticModel> GetAllStatistics()
    {
        return new StatisticParser().ParseFiles();
    }
}
