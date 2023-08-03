using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;

namespace try_to_make_app.Database_things;

[Serializable]
class App 
{
    public Process AppProcess;
    public double WorkToDayTime;
    public double WorkWeekTime;
    public App (Process process )
    {
        AppProcess = process;
    }

    public App(Process process, double workToDayTime)
    {
        AppProcess = process;
        WorkToDayTime = workToDayTime;
    }
}

[Serializable]
class Database
{
    public List<App> Apps = new List<App>();
    private DateTime LastRun;
    private List<string> SystemProcess = new List<string>() {"TextInputHost","ApplicationFrameHost","SystemSettings" };
    public List<App> GetApps()
    {
        List<App> apps = new List<App>();
        Process[] processes = Process.GetProcesses();
        foreach (var pr in processes)
        {

            if (pr.MainWindowTitle !="")
            {
                int StateOfChek = 0;
                foreach (var SysPr in SystemProcess)
                {
                    if (pr.ProcessName != SysPr)
                    {
                        StateOfChek = 0;
                    }
                    else
                    {
                        StateOfChek = 1;
                        break;
                    }
                } 
                if (StateOfChek == 0)
                {
                    var ProcessWorkToday = Math.Round((DateTime.Now - pr.StartTime).TotalMinutes);
                    App app = new App(pr,ProcessWorkToday);
                            
                            
                    apps.Add(app);
                }
            }
        }

        return apps;
    }
    
}
