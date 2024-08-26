using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Documents;
using try_to_make_app.Database_things;

namespace try_to_make_app.ViewModel;

public class MainWindowViewModel : IObserver
{
    public DataWorker DataWorker;
    public List<double> PieValues { get; set; }
    public List<string> PieLabels { get; set; }
    public List<double> HorizontallyCharyValues { get; set; }
    public List<string> HorizontallyChartLabels { get; set; }

    private int DayID
    {
        get
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    return db.Days.OrderByDescending(d => d.ID).FirstOrDefault().ID;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return 0;
            }    
        }
    }

    public MainWindowViewModel(DataWorker dataWorker)
    {
        DataWorker = dataWorker; 
        dataWorker.AddObserver(this);
        Update();
    }
    public void Update()
    {
        if (DayID != null && DayID != 0)
        {
            PieValues = GetAppsValues(DayID);
            PieLabels = GetAppsLabels(DayID);
            HorizontallyCharyValues = GetHorizontallyChartValues();
            HorizontallyChartLabels = GetHorizontallyChartLabels();
        }
    }

    private List<double> GetAppsValues(int dayID)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            List<AppDay> appDays = db.Apps.SelectMany(ap => ap.AppDays.Where(ad => ad.DayId == dayID)).ToList();
            List<double> appValues = appDays.Select(ad => ad.WorkTimeToDay).ToList();
            return appValues;
        }
    }
    private List<string> GetAppsLabels(int dayID)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            List<AppModel> appModels = db.Apps.Where(ap => ap.AppDays.Any(ad => ad.DayId == dayID)).ToList();
            List<string> applabels = appModels.Select(ap => ap.Name).ToList();
            return applabels;
        }    
    }
    private List<DayModel> GetLastSevenDayModel() 
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            List<DayModel> lastSevenDayModels= new List<DayModel>();
            if (db.Days.Count() >= 7)
            {
                for (int i = db.Days.Count();i > db.Days.Count() - 7; i--) 
                {
                    DayModel dayModel = db.Days.ToArray()[i]; 
                    lastSevenDayModels.Add(dayModel);
                }
            }
            else
            {
                lastSevenDayModels = db.Days.ToList();
            }
            return lastSevenDayModels;
        } 
    }
    private List<double> GetHorizontallyChartValues()
    {
        List<double> HorizontallyChartValues = new List<double>();
        foreach (var daymodel in GetLastSevenDayModel())
        {
            HorizontallyChartValues.Add(daymodel.Usadgetime);
        }
        return HorizontallyChartValues;
    }
    private List<string> GetHorizontallyChartLabels()
    {
        List<string> HorizontallyChartLabels = new List<string>();
        foreach (var dayModel in GetLastSevenDayModel())
        {
            HorizontallyChartLabels.Add(dayModel.Today.ToString());     
        }
        return HorizontallyChartLabels;
    }
}