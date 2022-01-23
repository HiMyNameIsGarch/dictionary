public class Statistics
{
    public void Store(SessionData data)
    {
        var newData = new StatisticModel();
        newData.TotalPoints = data.TotalPoints;
        StatisticParser sparser = new StatisticParser("test.json");
        sparser.StoreModel(newData);
    }
}
