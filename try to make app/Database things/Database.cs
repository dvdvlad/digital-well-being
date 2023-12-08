using System.Collections.Generic;
using System.Windows.Documents;

namespace try_to_make_app.Database_things;

public class Database
{
    private List<DayViewModel> _appViewModels = new List<DayViewModel>();

    public List<DayViewModel> AppViewModels
    {
        get
        {
            return _appViewModels;

        }
        set
        {
            _appViewModels = value;
        }
    }

    private double _weekWorkTime = 0;

    public double WeekWorkTime
    {
        get
        {
            return _weekWorkTime;
        }
        set
        {
            _weekWorkTime = value;
        }
    }

    private double CountWeekWorkTime()
    {
        double MethodWorkTime = 0;
        foreach (var day in AppViewModels)
        {
            foreach (var app in day.Apps)
            {
                MethodWorkTime = MethodWorkTime + app.WorkTimeToDay;
            }
        }

        return MethodWorkTime;
    }
    
}