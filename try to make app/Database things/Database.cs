using System.Collections.Generic;
using System.Windows.Documents;

namespace try_to_make_app.Database_things;

public class Database
{
    private List<DayViewModel> _dayViewModels = new List<DayViewModel>();

    public List<DayViewModel> DayViewModels
    {
        get
        {
            return _dayViewModels;

        }
        set
        {
            _dayViewModels = value;
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
        foreach (var day in DayViewModels)
        {
            foreach (var app in day.Apps)
            {
                MethodWorkTime = MethodWorkTime + app.WorkTimeToDay;
            }
        }

        return MethodWorkTime;
    }

    public void NewDay()
    {
        DayViewModel dayViewModel = new DayViewModel();
        dayViewModel.UpdateList(1);
        this.DayViewModels.Add(dayViewModel);
    }
}