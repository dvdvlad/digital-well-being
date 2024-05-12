using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace try_to_make_app.Database_things;

public class DayViewModel
{
    private AppModel _selectapps = null!;
    private ObservableCollection<AppModel> _apps = null!;
    private double _usadgetime;
    public DateTime today;
    private AppModel _temp;
    public ObservableCollection<AppModel> Apps
    {
        get => _apps;
        set
        {
            _apps = value;
            OnPropertyChanged();
        }
    }

    public AppModel SelctedApp
    {
        get => _selectapps;
        set
        {
            _selectapps = value;
            OnPropertyChanged();
        }
    }
    public double UsadgeTimeToDay
    {
        get
        {
            _usadgetime = 0;
            foreach (var app in this.Apps)
            {
                _usadgetime += app.WorkTimeToDay;
            }

            return _usadgetime;
        }
    }

    public DayViewModel()
    {
        this.Apps = new ObservableCollection<AppModel>();
        this.today = DateTime.Today;
    }
    
    public void UpdateList(double HowManyAdd)
    {
        AddNewApps();
        ObservableCollection<AppModel> tempAppColection = this.Apps;
        List<Process> workingapps = GetRunningAppList();
        AppModel[] appArray = tempAppColection.ToArray();
        Array.Sort(appArray);
        foreach (var app in tempAppColection)
        {
            foreach (var workingapp in workingapps)
            {
                if (app.Name == workingapp.ProcessName)
                {
                    app.WorkTimeToDay += HowManyAdd;
                }
            }
        }

        this.Apps = tempAppColection;
        double otherTodayWorkTime = 0;
        AppModel otherApps = new AppModel(name: "OtherApp", worktimetoday: otherTodayWorkTime);
        try
        {
            appArray[5] = otherApps;
        }
        catch (Exception e)
        {
            // ignored
        }
        
        int count = appArray.Length;
        for (int i = 6; i <= count; i++)
        {
            try
            {
                otherApps.WorkTimeToDay = otherApps.WorkTimeToDay + appArray[i].WorkTimeToDay;
            }
            catch (Exception e)
            {
                List<AppModel> catchlist = appArray.ToList();
                int lastindex = appArray.Length - 1;
                catchlist.Remove(item: appArray[lastindex]);
                appArray = catchlist.ToArray();
                break;
            }

            List<AppModel> list = appArray.ToList();
            list.Remove(item: appArray[i]);
            appArray = list.ToArray();
        }
        
        // this.Apps = new ObservableCollection<AppModel>(list: appArray.ToList());
    }
    private void AddNewApps()
    {
        List<Process> runningapps = GetRunningAppList();
        ObservableCollection<AppModel> apps = new ObservableCollection<AppModel>();
        List<string> systemProcess = new List<string>()
        {
            "TextInputHost", "ApplicationFrameHost", "SystemSettings", "Taskmgr", "NVIDIA Share", "WindowsTerminal",
            "try to make app", "explorer"
        };
        foreach (var pr in runningapps)
        {
            if (pr.MainWindowTitle != "")
            {
                bool stateOfCheck = false;
                foreach (var syPr in systemProcess)
                {
                    if (pr.ProcessName != syPr)
                    {
                        stateOfCheck = true;
                    }
                    else
                    {
                        stateOfCheck = false;
                        break;
                    }
                }
                
                if (stateOfCheck)
                {
                    double worktoday = 0.0;
                    AppModel appModel = new AppModel(name: pr.ProcessName, worktimetoday: worktoday);
                    if(this.Apps != null && this.Apps.Count > 0)
                    {
                        foreach (var app in this.Apps)
                        {
                            if (app.Name == appModel.Name )
                            {
                                stateOfCheck = false;                                    
                            }
                        }

                    }

                    if (stateOfCheck)
                    {
                        if (this.Apps == null )
                        {
                            apps.Add(appModel);
                        
                        }
                        apps.Add(appModel);
                    }
                    else
                    {
                        continue;
                    }   
                }
            }
        }

        foreach (var newAppModel in apps)
        {
            this.Apps.Add(newAppModel);
        }
    }

    private List<Process> GetRunningAppList()
    {
        List<Process> runningapps = new List<Process>();

        List<string> systemProcess = new List<string>()
        {
            "TextInputHost", "ApplicationFrameHost", "SystemSettings", "Taskmgr", "NVIDIA Share", "WindowsTerminal",
            "try to make app"
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
                        continue;
                        stateofcheck = false;
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

    public event PropertyChangedEventHandler PropertyChanged = null!;

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (PropertyChanged != null)
        {
            PropertyChanged(sender: this, e: new PropertyChangedEventArgs(propertyName: prop));
        }
    }
}