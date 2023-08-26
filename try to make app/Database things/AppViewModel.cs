using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace try_to_make_app.Database_things;

public class  AppViewModel
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
    public void UpdateList()
    {
        AppModel temp;
        int HowMannyAdd = 0;
        List <AppModel> appList = WorkGetApps();
        AppModel[] appArray = appList.ToArray();
        double OtherTodayWorkTime = 0;
        AppModel OtherApps = new AppModel(name: "OherApp", OtherTodayWorkTime);
        
        for (int i = 0; i < appArray.Length; i++)
        {
            for (int j = 0;j < appArray.Length; j++)
            {
                if (appArray[i].WorkTimeToDay > appArray[j].WorkTimeToDay)
                {
                    temp = appArray[i];
                    appArray[i] = appArray[j];
                    appArray[j] = temp;
                }
            }
        }

        appArray[5] = OtherApps;
        int count = appArray.Length;
        for (int i = 6; i < count; i++)
        {
            try
            {
                OtherApps.WorkTimeToDay = OtherApps.WorkTimeToDay + appArray[i].WorkTimeToDay;
            }
            catch (Exception e)
            {
                List<AppModel> Catchlist = appArray.ToList();
                int lastindex = appArray.Length - 1;
                Catchlist.Remove(appArray[lastindex]);
                appArray = Catchlist.ToArray();
                break;
            }
            List<AppModel> list = appArray.ToList();
            list.Remove(appArray[i]);
            appArray = list.ToArray();

        }

        Apps = new ObservableCollection<AppModel>(appArray.ToList());
    }
    private List<AppModel> WorkGetApps()
    {
        List<AppModel> Apps = new List<AppModel>();

        List<string> SystemProcess = new List<string>()
            { "TextInputHost", "ApplicationFrameHost", "SystemSettings", "Taskmgr", "NVIDIA Share" };
        Process[] processes = Process.GetProcesses();
        foreach (var pr in processes)
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