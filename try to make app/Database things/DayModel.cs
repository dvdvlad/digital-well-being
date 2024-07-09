using System;
using System.Collections.Generic;

namespace try_to_make_app.Database_things;

public class DayModel
{
    public int ID { get; set; }
    public DateTime Today { get; set; }
    public double Usadgetime { get; set; }
    
    public List<AppModel> AppModels { get; set; }
    public List<AppDay> AppDays { get; set; }

    public DayModel()
    {
        this.Today = DateTime.Today;
        Usadgetime = 0; 
        this.AppDays = new List<AppDay>();
    }
}