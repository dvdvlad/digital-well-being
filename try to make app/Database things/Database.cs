using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Documents;

namespace try_to_make_app.Database_things;

[Serializable]
class App: Process
{
    private DateTime PersonalLimitation = new DateTime(0,0,0,0,0,0);
}

[Serializable]
class Database
{
    public List<App> Apps;
    private DateTime LastRun;
    public List<App> GetApps()
    {
        List<App> apps = new List<App>();
        Process[] process = App.GetProcesses();
        foreach (Process pr in process)
        {
            foreach (var datapr in Apps)
            {
                if (datapr.ProcessName == pr.ProcessName)
                {
                    Console.WriteLine("есть в бд");
                }
                else
                {
                    App app = (App)pr;
                    
                }
            }
        }
        return apps;
    }
}


