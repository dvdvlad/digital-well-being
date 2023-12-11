using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace try_to_make_app.Database_things;

public class  DayViewModel
{
    private AppModel selectapps;
    private Save _save = new Save();
    private ObservableCollection<AppModel> apps;
    public ObservableCollection<AppModel> Apps
    {
        get { return apps;}
        set
        {
            apps = value;
            OnPropertyChanged("Apps");
        }

    }

    public AppModel SelctedApp
    {
        get { return selectapps; }
        set
        {
            selectapps = value;
            OnPropertyChanged("SelectedApp");
        }
    }
    public  void UpdateList()
    {
        AppModel OtherApp = new AppModel("Другие Приложения", 0);
        List<AppModel> AllWorkAppNow = WorkGetApps().ToList();
        foreach (var WorkingApp in AllWorkAppNow)
        {
            foreach (var AppsInDatabase  in this.Apps)
            {
                if (WorkingApp.Name == AppsInDatabase.Name)
                {
                    this.Apps.Add(WorkingApp);
                    break;
                }   
            }
        }

        bool StateOfCheck = true;
        double AllAppsWorkTIme = 0;
        foreach (var app in this.Apps)
        {
            AllAppsWorkTIme += app.WorkTimeToDay;
        }

        AppModel temp;
        AppModel[] apps = this.Apps.ToArray();
        for (int i = 0; i < apps.Length - 1; i++)
        {
            for (int j = i + 1; j < apps.Length; j++)
            {
                if (apps[i].WorkTimeToDay > apps[j].WorkTimeToDay)
                {
                    temp = apps[i];
                    apps[i] = apps[j];
                    apps[j] = temp;
                }
            }
        }

        this.Apps = new ObservableCollectionListSource<AppModel>(apps.ToList());
        ObservableCollection<AppModel> appstemp = new ObservableCollection<AppModel>(this.Apps);
        foreach (var app in this.Apps)
        {
            if (app.WorkTimeToDay / AllAppsWorkTIme < 5.00)
            {
                OtherApp.WorkTimeToDay += app.WorkTimeToDay;
                appstemp[this.Apps.IndexOf(app)] = OtherApp;
            }
        }

        this.Apps = appstemp;



    }
    private ObservableCollection<AppModel> WorkGetApps()
    {
        
        List<string> SystemProcess = new List<string>()
            { "TextInputHost", "ApplicationFrameHost", "SystemSettings", "Taskmgr", "NVIDIA Share","WindowsTerminal","try to make app" };
        Process[] processes = Process.GetProcesses();
        foreach (var pr in processes)
        {
            if (pr.MainWindowTitle != "")
            {

                bool StateOfCheck = false;
                foreach (var SyPr in SystemProcess)
                {

                    if (pr.ProcessName != SyPr )
                    {
                        if (this.Apps != null)
                        {
                            foreach (var app in Apps)
                            {
                                if (app.Name == pr.ProcessName)
                                {
                                    StateOfCheck = false;
                                    break;
                                }
                                StateOfCheck = true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        StateOfCheck = false;
                        break;
                    }
                }
                

                if (StateOfCheck)
                {
                    double worktoday = (DateTime.Now - pr.StartTime).TotalMinutes;
                    AppModel appModel = new AppModel(pr.ProcessName, worktoday);
                    Apps.Add(appModel);
                }
            }
        }

        return Apps;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}