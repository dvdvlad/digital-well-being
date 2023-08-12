using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Threading;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace try_to_make_app.Database_things;

public class Save
{
    delegate void ChangedDatabase();
    private event ChangedDatabase Notify;
    private string PATH =  $"{Environment.CurrentDirectory}\\AppDatabase.json";
    public AppModel CheckOnExisting(AppModel ProcessOnCheck,ObservableCollection<AppModel> SavedApps)
    {
        AppModel Result = null;
        foreach (var savedApp in SavedApps)
        {
            if (savedApp.Name == ProcessOnCheck.Name)
            {
                Result = savedApp;
                break;
            }
            else
            {
                Result = null;
            }
        }

        return Result;
    }
    
    public void SaveDatabase(AppViewModel appViewModel)
    {
        ObservableCollection<AppModel> WorkProcess = WorkGetApps();
        if (appViewModel.Apps != null)
        {
            foreach (var WorkPr in WorkProcess)
            {
                AppModel App = CheckOnExisting(WorkPr, appViewModel.Apps);
                if (App != null)
                {
                    
                    appViewModel.Apps.Remove(App);
                    appViewModel.Apps.Add(WorkPr);
                }
                else
                {
                    appViewModel.Apps.Add(WorkPr);
                }
                
            }
            
            using (StreamWriter sw = File.CreateText(PATH))
            {
                string forsave = JsonConvert.SerializeObject(appViewModel.Apps);
                sw.Write(forsave);
            }
        }
        else
        {
            appViewModel.Apps = WorkProcess;
            using (StreamWriter sw = File.CreateText(PATH))
            {
                string forsave = JsonConvert.SerializeObject(appViewModel.Apps);
                sw.Write(forsave);
            }
        }
        
        
        
        using (StreamWriter sw = File.CreateText(PATH))
        {
            string forsave = JsonConvert.SerializeObject(appViewModel.Apps);
            sw.Write(forsave);
        }

    }

    public  ObservableCollection<AppModel> LoadDatabase()
    {
        bool fileexist = File.Exists(PATH);
        if (fileexist)
        {
            using (var reader = File.OpenText(PATH))
            {
                string filetxt = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<AppModel>>(filetxt);

            }
        }
        else
        {
            File.CreateText(PATH).Dispose();
            return null;
        }
    }
     private  ObservableCollection<AppModel> WorkGetApps()
    {
        ObservableCollection<AppModel> Apps = new ObservableCollection<AppModel>();

        List<string> SystemProcess = new List<string>()
        { "TextInputHost", "ApplicationFrameHost", "SystemSettings", "Taskmgr", "NVIDIA Share" };
        Process[] processes = Process.GetProcesses();
        foreach (var pr in processes)
        {
            if (pr.ProcessName != "")
            {
                if (pr.MainWindowTitle != "")
                {

                    bool StateOfCheck = false;
                    foreach (var SyPr in SystemProcess)
                    {

                        if (pr.ProcessName != SyPr)
                        {
                            StateOfCheck = true;
                        }
                        else
                        {
                            StateOfCheck = false;
                            break;
                        }
                    }

                    if (StateOfCheck)
                    {
                        AppModel appModel = new AppModel(pr, pr.ProcessName);
                        Apps.Add(appModel);
                    }
                }
            }
        }

        return Apps;
    }
}