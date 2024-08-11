using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace try_to_make_app.Database_things;

static public class DataWorker
{
    public static void CreateDayModel()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            DayModel dayModel = new DayModel();
            db.Days.Add(dayModel);
            db.SaveChanges();
        }
    }

    public static void CreateAddNewApps(List<Process> runningprocesses)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            DayModel dayModel = db.Days.OrderByDescending(d => d.ID).FirstOrDefault();
            foreach (var runpr in runningprocesses)
            {
                if (!db.Apps.Any(a => a.Name == runpr.ProcessName))
                {
                    db.Apps.Add(new AppModel(runpr.ProcessName) { Days = new List<DayModel>() { dayModel } });
                }
            }

            db.SaveChanges();
        }
    }

    public static void UpdateApps(DayModel dayModel, List<Process> runningapps)
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
                        app.WorkTimeOnWeek.AddSeconds(1.0);
                        if (app.AppDays != null)
                        {
                            foreach (var ad in app.AppDays)
                            {
                                if (ad.DayId == dayModel.ID)
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

    public static void UpdateAddNewAppDayModel(DayModel dayModel, List<Process> runproocess)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            if (dayModel.AppModels != null && dayModel.AppModels.Count > 0)
            {
                foreach (var runpr in runproocess)
                {
                    if (!dayModel.AppModels.Any(ap => ap.Name == runpr.ProcessName))
                    {
                        AppModel appModel = db.Apps.Where(ap => ap.Name == runpr.ProcessName).FirstOrDefault();
                        dayModel.AppDays.Add(new AppDay()
                            { AppModel = appModel, AppId = appModel.ID, Day = dayModel, DayId = dayModel.ID });
                        db.Days.Update(dayModel);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                foreach (var runpr in runproocess)
                {
                    AppModel appModel = new AppModel(runpr.ProcessName);
                    appModel.Days.Add(dayModel);
                    db.Apps.Update(appModel);
                    db.SaveChanges();
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