namespace try_to_make_app.Database_things;

public class AppDay
{
    public int AppId { get; set; }
    public AppModel AppModel { get; set; }

    public int DayId { get; set; }
    public DayModel Day { get; set; }
    
    public double WorkTimeToDay { get; set; }

    public AppDay(DayModel dayModel, AppModel appModel)
    {
        AppModel = appModel;
        Day = dayModel;
    }
    public AppDay()
    {
    }
}
