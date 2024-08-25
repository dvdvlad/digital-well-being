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
                }

                DataWorker.CreateAddNewApps(getrunprocess);
                DataWorker.UpdateApps(db.Days.OrderByDescending(d => d.ID).FirstOrDefault(), getrunprocess);
                DataWorker.UpdateAddNewAppDayModel(db.Days.OrderByDescending(d => d.ID).FirstOrDefault(),
                    getrunprocess);
            }

            Thread.Sleep(100);
        }
    }
}