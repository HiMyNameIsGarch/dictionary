public class StatisticModel
{
    private string _fileName = "";

    public string FileName { 
        get { return _fileName; }
        set { _fileName = value; }
    }

    public ModeType Mode { get; set; }

    public int Points { get; set; }

    public int TotalPoints { get; set; }

    public double AverageResponseTime { get; set; }

    public double AverageAccuracy { get; set; }

    private List<double> _timeResponses = new List<double>();
    private List<double> _accuracies = new List<double>();

    public List<double> TimeResponses { 
        get { return _timeResponses; }
        set { _timeResponses = value; }
    }

    public List<double> Accuracies {
        get { return _accuracies; }
        set { _accuracies = value; }
    }

}
