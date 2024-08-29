using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using try_to_make_app.Database_things;
using try_to_make_app.View;

namespace try_to_make_app.ViewModel;

public class MainWindowViewModel : BaseViewModel, IObserver
{
    public DataWorker DataWorker;
    private List<double> _pievalues;

    public List<double> PieValues
    {
        get => _pievalues;
        set
        {
            _pievalues = value;
            OnPropertyChanged("PieValues");
        }
    }

    private List<string> _pielabels;

    public List<string> PieLabels
    {
        get => _pielabels;
        set
        {
            _pielabels = value;
            // OnPropertyChanged("PieLabels");
        }
    }

    private List<double> _horizontallycharyvalues;

    public List<double> HorizontallyCharyValues
    {
        get => _horizontallycharyvalues;
        set
        {
            _horizontallycharyvalues = value;
            // OnPropertyChanged("HorizontallyCharyValues");
        }
    }

    private List<string> _horizontallychartlabels;

    public List<string> HorizontallyChartLabels
    {
        get => _horizontallychartlabels;
        set
        {
            _horizontallychartlabels = value;
            // OnPropertyChanged("HorizontallyChartLabels");
        }
    }

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
            HorizontallyChartLabels = GetHorizontallyChartLabels();
            HorizontallyCharyValues = GetHorizontallyChartValues();
            PieLabels = GetAppsLabels(DayID);
            PieValues = GetAppsValues(DayID);
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
            List<DayModel> lastSevenDayModels = new List<DayModel>();
            if (db.Days.Count() >= 7)
            {
                for (int i = db.Days.Count(); i > db.Days.Count() - 7; i--)
                {
                    DayModel dayModel = db.Days.Include(d => d.AppDays).ToArray()[i];
                    lastSevenDayModels.Add(dayModel);
                }
            }
            else
            {
                lastSevenDayModels = db.Days.Include(d => d.AppDays).ToList();
            }

            return lastSevenDayModels;
        }
    }

    private List<double> GetHorizontallyChartValues()
    {
        List<double> HorizontallyChartValues = new List<double>();
        foreach (var daymodel in GetLastSevenDayModel())
        {
            HorizontallyChartValues.Add(daymodel.AppDays.Select(ad => ad.WorkTimeToDay).Sum());
        }

        if (HorizontallyChartValues.Count < 7)
        {
            while (HorizontallyChartValues.Count < 7)
            {
                HorizontallyChartValues.Add(0.0);
            }
        }

        return HorizontallyChartValues;
    }

    private List<string> GetHorizontallyChartLabels()
    {
        List<string> HorizontallyChartLabels = new List<string>();
        foreach (var dayModel in GetLastSevenDayModel())
        {
            string label = dayModel.Today.Day.ToString();
            HorizontallyChartLabels.Add(label);
        }

        if (HorizontallyChartLabels.Count < 7)
        {
            while (HorizontallyChartLabels.Count < 7)
            {
                HorizontallyChartLabels.Add(" ");
            }
        }

        return HorizontallyChartLabels;
    }
}