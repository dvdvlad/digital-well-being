using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace try_to_make_app.Database_things;

public class SecondThread
{
    public DataWorker DataWorker;

    public SecondThread(DataWorker dataWorker)
    {
        DataWorker = dataWorker;
    }

    public void MainTwo()
    {
        while (true)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Process> getrunprocess = DataWorker.GetRunningProcesses();
                DayModel dayModel = db.Days.Include(d => d.AppModels).OrderByDescending(d => d.ID).FirstOrDefault();
                if (dayModel == null || dayModel.Today != DateTime.Today)
                {
                    DataWorker.CreateDayModel();
                    dayModel = db.Days.Include(d => d.AppModels).OrderByDescending(d => d.ID).FirstOrDefault();
                }

                DataWorker.CreateAddNewApps(getrunprocess, dayModel.ID);
                DataWorker.UpdateApps(dayModel.ID, getrunprocess);
                foreach (var runpr in getrunprocess)
                {
                    AppModel appModelrun = db.Apps.Include(ap => ap.AppDays).FirstOrDefault(ap => ap.Name == runpr.ProcessName);
                    var appModelrunAppDay = appModelrun.AppDays.FirstOrDefault(ad => ad.Day.Today == DateTime.Today);
                    if (appModelrunAppDay != null && appModelrun.AllowedTime.TimeOfDay <= appModelrunAppDay.WorkTimeToDay.TimeOfDay)
                    {
                        runpr.CloseMainWindow();
                    }
                }
            }

            DataWorker.Notify();
            Thread.Sleep(100);
        }
    }
}