using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace try_to_make_app.Database_things;

public class DataWorker : IObservable
{
    public List<IObserver> Observers = new List<IObserver>();

    public void AddObserver(IObserver o)
    {
        Observers.Add(o);
    }

    public void RemoveObser(IObserver o)
    {
        Observers.Remove(o);
    }

    public void Notify()
    {
        foreach (var observer in Observers)
        {
            observer.Update();
        }
    }

    public static void CreateDayModel()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            DayModel dayModel = new DayModel();
            db.Days.Add(dayModel);
            db.SaveChanges();
        }
    }

    public static void CreateAddNewApps(List<Process> runningprocesses, int DayID)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            DayModel dayModel = db.Days.OrderByDescending(d => d.ID).FirstOrDefault();
            foreach (var runpr in runningprocesses)
            {
                if (!db.Apps.Any(a => a.Name == runpr.ProcessName))
                {
                    db.Apps.Add(new AppModel(runpr.ProcessName) { Days = new List<DayModel>() { dayModel } });
                    db.SaveChanges();
                }
                else
                {
                    List<AppModel> appModels = db.Apps
                        .Include(am => am.Days)
                        .ToList();
                    AppModel appModel = appModels.Find(ap => ap.Name == runpr.ProcessName);
                     if (!appModel.Days.Any(dm => dm.ID == DayID))
                    {
                        if (appModel.AppDays != null)
                        {
                            appModel.AppDays.Add(new AppDay(dayModel,appModel));
                            db.SaveChanges();
                        }
                    }
                }
            }

            db.SaveChanges();
        }
    }

    public static void  UpdateApps(int DayID, List<Process> runningapps)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            List<AppModel> appModels = new List<AppModel>();
            foreach (var runpr in runningapps)
            {
                foreach (var app in db.Apps.Include(ap => ap.Days))
                {
                    if (runpr.ProcessName == app.Name)
                    {
                        if (app.AppDays != null)
                        {
                            foreach (var ad in app.AppDays)
                            {
                                if (ad.DayId ==DayID)
                                {
                                    ad.WorkTimeToDay += 1.0;
                                        db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public static List<Process> GetRunningProcesses()
    {
        List<Process> runningapps = new List<Process>();

        List<string> systemProcess = new List<string>()
        {
            "TextInputHost", "ApplicationFrameHost", "SystemSettings", "Taskmgr", "NVIDIA Share", "WindowsTerminal",
            "try to make app", "explorer", "TextInputHost", "ApplicationFrameHost"
        };
        List<Process> processes = new List<Process>(Process.GetProcesses());
        foreach (var pr in processes)
        {
            bool stateofcheck = false;
            if (pr.MainWindowTitle != "")
            {
                foreach (var syPr in systemProcess)
                {
                    if (pr.ProcessName == syPr)
                    {
                        stateofcheck = false;
                        
                        break;
                    }
                    else
                    {
                        stateofcheck = true;
                    }
                }

                if (stateofcheck)
                {   
                    runningapps.Add(pr);
                }
            }
        }

        return runningapps;
    }
}