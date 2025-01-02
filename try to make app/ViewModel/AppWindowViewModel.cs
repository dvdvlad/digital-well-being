using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using try_to_make_app.Database_things;
using try_to_make_app.View;

namespace try_to_make_app.ViewModel;

public class AppWindowViewModel : BaseViewModel, IObserver
{
    public Timer TextCHTimer;

    public DateTime AllowedTime
    {
        get => _alloweDateTime;
        set
        {
            _alloweDateTime = value;
            OnPropertyChanged("AllowedTime");
        }
    }

    public int AllowedTimeHour
    {
        get => _alloweDateTime.Hour;
        set
        {
            _alloweDateTime = DateTime.MinValue.AddHours(value);
        }
    }

    public int AllowedTimeMinute
    {
        get => _alloweDateTime.Minute;
        set
        {
            _alloweDateTime = DateTime.MinValue.AddMinutes(value);
        }
    }

    private DateTime currentDate;

    private DateTime _alloweDateTime = DateTime.MinValue;

    private List<DateTime> _weekusadgetime = new List<DateTime>();

    public List<DateTime> WeekUsadgeTime
    {
        get => _weekusadgetime;
        set
        {
            _weekusadgetime = value;
            OnPropertyChanged("WeekUsadgeTime");
        }
    }

    private string _appname = "noname";

    public string AppName
    {
        get => _appname;
        set
        {
            _appname = value;
            OnPropertyChanged("AllowedTime");
        }
    }

    private RelayComand _backToAppsViewCommand;

    public RelayComand BackToAppsViewCommand
    {
        get => _backToAppsViewCommand;
        private set
        {
            _backToAppsViewCommand = value;
            OnPropertyChanged(nameof(BackToAppsViewCommand));
        }
    }

    public RelayComand PreviusDayCommand =>
        _previusDayCommand ??= new RelayComand(execute => PreviusDayMethod(), canExecute => { return true; });

    private RelayComand _previusDayCommand;

    private void PreviusDayMethod()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            try
            {
                this.currentDate = this.currentDate.AddDays(-1);
                Update();
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "" + currentDate);
            }
        }
    }

    public RelayComand NextDayCommand =>
        _nextDayCommand ??= new RelayComand(execute => NexDayMethod(), canExecute => { return true; });

    private RelayComand _nextDayCommand;

    private void NexDayMethod()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            try
            {
                this.currentDate = this.currentDate.AddDays(-1);

                Update();
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "test" + currentDate);
            }
        }
    }

    public int AppID = 1;

    private DateTime getAlloWedTime(int AppID)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Apps.Where(ap => ap.ID == this.AppID).FirstOrDefault().AllowedTime;
        }
    }

    private string getAppName(int AppID)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            return db.Apps.Where(ap => ap.ID == AppID).FirstOrDefault().Name;
        }
    }

    private void getWeekUsadgeTime(int AppID)
    {
        List<DateTime> weekusadgetime = new List<DateTime>();
        using (ApplicationContext db = new ApplicationContext())
        {
            List<AppDay> Appdays = db.Apps
                .Where(ap => ap.ID == AppID)
                .Include(am => am.AppDays)
                .Include(am => am.Days)
                .FirstOrDefault()
                ?.AppDays
                .ToList() ?? new List<AppDay>();

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = currentDate.AddDays(-i);
                AppDay appDay = Appdays
                    .FirstOrDefault(ad => ad.Day?.Today.Date == date.Date);

                weekusadgetime.Add(appDay?.WorkTimeToDay ?? currentDate.AddDays(-i));
            }
        }

        WeekUsadgeTime = weekusadgetime;
    }

    public AppWindowViewModel(string appname, RelayComand backViewCommand)
    {
        TextCHTimer = new Timer(3000);
        TextCHTimer.Elapsed += textCHTimerOnElapsed;
        TextCHTimer.AutoReset = false;
        currentDate = DateTime.Now;
        AppModel appModel = new AppModel();
        BackToAppsViewCommand = backViewCommand;
        using (ApplicationContext db = new ApplicationContext())
        {
            appModel = db.Apps.Include(ap => ap.AppDays).Where(ap => ap.Name == appname).FirstOrDefault();
        }

        AppID = appModel.ID;
        AppName = appModel.Name;
        AllowedTime = appModel.AllowedTime;
    }

    private void textCHTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            db.Apps.FirstOrDefault(ap => ap.ID== this.AppID).AllowedTime = this.AllowedTime;
            db.SaveChanges();
        }
    }

    public void Update()
    {
        getWeekUsadgeTime(AppID);
        getAppName(AppID);
    }
}