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
    }

    private ICollection<StatisticModel> GetAllStatistics()
    {
        return new StatisticParser().ParseFiles();
    }
}
