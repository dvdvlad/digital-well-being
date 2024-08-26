using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace try_to_make_app.Database_things;

[Serializable]
public class AppModel : INotifyPropertyChanged 
{
    public int ID { get; set; }
    public List<AppDay> AppDays { get; set; }
    public List<DayModel> Days { get; set; } = new ();
    private string name = "нет имени";
    private DateTime worktimeonweek = DateTime.MinValue;

    public string Name
    {
        get { return name; }
        set
        {
            name = value;
            OnPropertyChanged("Name");
        }
    }
    public DateTime WorkTimeOnWeek
    {
        get { return worktimeonweek; }
        set
        {
            worktimeonweek = value;
            OnPropertyChanged("DatatimeOnWeek");
        }
    }

    public AppModel(string name)
    {
        this.name = name;
    }

    public AppModel()
    {
        AppDays = new List<AppDay>();
    }
public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

}