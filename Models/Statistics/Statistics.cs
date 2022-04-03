using static System.Console;
using static ConsoleHelper;
using System.Text;

public class Statistics
{
    private List<string> _options = new List<string>()
    {
        "Statistics for very last session",
        "Average statistics of all sessions",
        "Average points accuracy per all files"
    };
    private const int _minimumDataForStatistics = 5;
    private const int _maxValueOnTimeGraph = 50;
    private const int _freqOnTimeGraph = 3;
    private const int _addDecimals = 10;

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
        model.TimeOnSession = data.TimeOnSession;
        return model;
    }

    public void ShowOptions()
    {
        int cursorTop = Console.CursorTop;
        DisplayOptions();
        char key = '0';
        do
        {
            ColorWrite("Press the number of the option: ", ConsoleColor.DarkYellow);
            key = Console.ReadKey().KeyChar;
        }
        while(char.IsWhiteSpace(key) && !char.IsDigit(key));
        ConsoleHelper.ClearScreen(cursorTop);

        DisplayOptionFor(key);
    }

    private void DisplayOptions()
    {
        ColorWriteLine("Display the evolution for: ", ConsoleColor.Yellow);
        int i = 1;
        foreach(var option in _options)
        {
            ConsoleHelper.ColorWrite(i + ". ", ConsoleColor.DarkBlue);
            ConsoleHelper.ColorWrite(option, ConsoleColor.Blue);
            Write("\n");
            i++;
        }
    }

    private void DisplayOptionFor(char key)
    {
        var data = GetAllStatistics();
        if(data.Count < _minimumDataForStatistics)
        {
            WriteLine("> Unfortunately, you have to little data in order to show statistics");
            WriteLine("> You have to have at least {0} sessions completed", _minimumDataForStatistics);
            WriteLine("> Thanks!");
            return;
        }
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

    private int RoundResponseTime(double value) => (int)value * _addDecimals;

    private void StatisticsForLastSession(StatisticModel lastSession)
    {
        WriteLine("Filename: " + lastSession.FileName);
        WriteLine("Mode: " + lastSession.Mode.ToFormattedString());
        WriteLine("Time Spent: " + lastSession.TimeOnSession.TotalMinutes + " minutes");

        var timeResponses = lastSession.TimeResponses.Select(t => RoundResponseTime(t)).ToArray();
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
        int[] times = data.Select(s => RoundResponseTime(s.AverageResponseTime)).ToArray();
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
        StringBuilder sb = new StringBuilder();
        WriteLine("Legend: ");
        int i = 1;
        foreach(var pair in pairs)
        {
            sb.AppendLine(i + " - " + pair.Key);
            i++;
        }
        ConsoleHelper.DisplayColumn(sb.ToString(), '-');
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
        var yaxis = new Axis(0, _maxValueOnTimeGraph, _freqOnTimeGraph);
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
