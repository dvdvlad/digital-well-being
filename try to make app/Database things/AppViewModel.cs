using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace try_to_make_app.Database_things;

public class  AppViewModel: INotifyPropertyChanged
{
    private AppModel selectapps;
    private Save _save = new Save();
    private  ObservableCollection<AppModel> apps;
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
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}