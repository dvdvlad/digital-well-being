using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace try_to_make_app.Database_things;

[Serializable]
public class AppModel  
{
    public int ID { get; set; }
    public List<AppDay> AppDays { get; set; }
    public List<DayModel> Days { get; set; }
    private string name = "нет имени";
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private DateTime allowedtitme = DateTime.MinValue;

    public DateTime AllowedTime
    {
        get => allowedtitme;
        set
        {
            allowedtitme = value;
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
}