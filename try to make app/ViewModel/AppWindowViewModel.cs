using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using try_to_make_app.Database_things;
using try_to_make_app.View;

namespace try_to_make_app.ViewModel;

public class AppWindowViewModel : BaseViewModel, IObserver
{
    public DateTime AllowedTime
    {
        get => _alloweDateTime;
        set
        {
            _alloweDateTime = value;
            OnPropertyChanged("AllowedTime");
        }
    }

    private DateTime _alloweDateTime = DateTime.MinValue;

    private List<double> _weekusadgetime = new List<double>();

    public List<double> WeekUsadgeTime
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

    private List<DateTime> getWeekUsadgeTime(int AppID)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            var appModels = db.Apps.Include(ap => ap.AppDays);
            List<DateTime> allowedTimes = appModels.Where(ap => ap.ID == AppID).Select(ap => ap.AllowedTime).ToList();
            return allowedTimes;
        }
    }

    public AppWindowViewModel(string appname, RelayComand backViewCommand)
    {
        AppModel appModel = new AppModel();
        BackToAppsViewCommand = backViewCommand;
        using (ApplicationContext db = new ApplicationContext())
        {
            appModel = db.Apps.Include(ap => ap.AppDays).Where(ap => ap.Name == appname).FirstOrDefault();
        }

        AppID = appModel.ID;
        AppName = appModel.Name;
        AllowedTime = appModel.AllowedTime;
        WeekUsadgeTime = appModel.AppDays.Select(am => am.WorkTimeToDay).ToList();
    }

    public void Update()
    {
        getWeekUsadgeTime(AppID);
        getAppName(AppID);
        getWeekUsadgeTime(AppID);
    }
}