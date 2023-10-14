using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace try_to_make_app.Database_things;
[Serializable]
public class AppModel: INotifyPropertyChanged
{
    private string name;
    private double worktimetoday;
    private DateTime worktimeonweek;

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
            OnPropertyChanged("Name");
        }
    }

    public double  WorkTimeToDay
    {
        get
        {
            return worktimetoday;
        }
        set
        {
            worktimetoday = value;
            OnPropertyChanged("DatatimeToDay");
        }
    }

    public DateTime WorkTimeOnWeek
    {
        get
        {
            return worktimeonweek;
        }
        set
        {
            worktimeonweek = value;
            OnPropertyChanged("DatatimeOnWeek");
        }
    }

    public AppModel(string name,double worktimetoday)
    {
        this.name = name;
        this.worktimetoday = worktimetoday;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}