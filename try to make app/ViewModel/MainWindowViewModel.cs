using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using try_to_make_app.Database_things;
using try_to_make_app.View;

namespace try_to_make_app.ViewModel;

public class MainWindowViewModel : BaseViewModel, IObserver
{
    public DataWorker DataWorker;

    private ObservableCollection<AppModel> _appModels;

    public ObservableCollection<AppModel> AppModels
    {
        get => _appModels;
        set
        {
            _appModels = value;
            OnPropertyChanged("AppModels");
        }
    }

    private UserControl _selectedusercontrol;

    public UserControl SelectedUserControl
    {
        get { return _selectedusercontrol; }
        set
        {
            _selectedusercontrol = value;
            OnPropertyChanged("SelectedUserControl");
        }
    }

    private AppsView _appsView;
    public AppsView AppsView
    {
        set { _appsView = value; }
        get
        {
            if (_appsView == null)
            {
                _appsView = new AppsView(CreateAppWindowComand,AppModels);
            }

            return _appsView;
        }
    }

    public RelayComand CreateAppWindowComand =>
        _createAppWindow ??= new RelayComand(execute => CreateAppWindow(execute as string), canExecute => { return true; });

    private RelayComand _createAppWindow;

    private void CreateAppWindow(string AppName)
    {
        Console.WriteLine("123123213121312112");
       SelectedUserControl = new AppWindow(new AppWindowViewModel(AppName));
    }


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
            OnPropertyChanged("PieLabels");
        }
    }

    private List<double> _horizontallycharyvalues;

    public List<double> HorizontallyCharyValues
    {
        get => _horizontallycharyvalues;
        set
        {
            _horizontallycharyvalues = value;
            OnPropertyChanged("HorizontallyCharyValues");
        }
    }

    private List<string> _horizontallychartlabels;

    public List<string> HorizontallyChartLabels
    {
        get => _horizontallychartlabels;
        set
        {
            _horizontallychartlabels = value;
            OnPropertyChanged("HorizontallyChartLabels");
        }
    }

    private int dayID = 1;

    private int DayID
    {
        get { return dayID; }
        set { dayID = value; }
    }


    public RelayComand NextDayCommand =>
        _nextDayCommand ??= new RelayComand(execute => NexDayMethod(), canExecute => { return true; });

    private RelayComand _nextDayCommand;

    private void NexDayMethod()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            Console.WriteLine("test");
            if (DayID < db.Days.Count())
            {
                this.DayID += 1;
                Update();
            }
        }
    }

    public RelayComand PreviusDayCommand =>
        _previusDayCommand ??= new RelayComand(execute => PreviusDayMethod(), canExecute => { return true; });

    private RelayComand _previusDayCommand;

    private void PreviusDayMethod()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            if (DayID > 0)
            {
                this.DayID -= 1;
                Update();
            }
        }
    }

    public MainWindowViewModel(DataWorker dataWorker)
    {
        DataWorker = dataWorker;
        SelectedUserControl = AppsView;
        using (ApplicationContext db = new ApplicationContext())
        {
            try
            {
                DayID = db.Days.OrderByDescending(d => d.ID).FirstOrDefault().ID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        dataWorker.AddObserver(this);
        Update();
    }

    public MainWindowViewModel()
    {
    }

    public void Update()
    {
        if (DayID != null && DayID != 0)
        {
            HorizontallyChartLabels = GetHorizontallyChartLabels();
            HorizontallyCharyValues = GetHorizontallyChartValues();
            PieLabels = GetAppsLabels(DayID);
            PieValues = GetAppsValues(DayID);
            AppModels = GetAppModels();
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
            if (DayID >= 7)
            {
                for (int i = DayID - 1; i > DayID - 7; i--)
                {
                    DayModel dayModel = db.Days.Include(d => d.AppDays).FirstOrDefault(d => d.ID == i);
                    if (dayModel != null)
                    {
                        lastSevenDayModels.Add(dayModel);
                    }
                }
            }
            else
            {
                lastSevenDayModels = db.Days.Include(d => d.AppDays)
                    .Where(d => d.ID <= DayID)
                    .OrderByDescending(d => d.ID)
                    .ToList();
            }

            return lastSevenDayModels;
        }
    }

    private List<double> GetHorizontallyChartValues()
    {
        List<double> horizontallyChartValues = new List<double>();
        var lastSevenDays = GetLastSevenDayModel();

        using (ApplicationContext db = new ApplicationContext())
        {
            var currentDay = db.Days.FirstOrDefault(d => d.ID == DayID);
            if (currentDay != null)
            {
                var today = currentDay.Today;

                for (int i = 6; i >= 0; i--)
                {
                    var targetDate = today.AddDays(-i);
                    var dayModel = lastSevenDays.FirstOrDefault(d => d.Today.Date == targetDate.Date);

                    if (dayModel != null)
                    {
                        horizontallyChartValues.Add(dayModel.AppDays.Select(ad => ad.WorkTimeToDay).Sum());
                    }
                    else
                    {
                        horizontallyChartValues.Add(0.0);
                    }
                }
            }
        }

        return horizontallyChartValues;
    }

    private ObservableCollection<AppModel> GetAppModels()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return new ObservableCollection<AppModel>(db.Apps.ToList());
        }
    }

    private List<string> GetHorizontallyChartLabels()
    {
        List<string> horizontallyChartLabels = new List<string>();
        var lastSevenDays = GetLastSevenDayModel();
        var today = DateTime.Today;

        for (int i = 6; i >= 0; i--)
        {
            var targetDate = today.AddDays(-i);
            var dayModel = lastSevenDays.FirstOrDefault(d => d.Today.Date == targetDate.Date);

            if (dayModel != null)
            {
                horizontallyChartLabels.Add(dayModel.Today.Day.ToString());
            }
            else
            {
                horizontallyChartLabels.Add(targetDate.Day.ToString());
            }
        }

        return horizontallyChartLabels;
    }
}