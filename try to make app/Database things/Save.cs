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

public  class  Save
{
    private static  string PATH =  $"{Environment.CurrentDirectory}\\AppDatabase.json";
    public static  void SaveDatabase(AppViewModel appViewModel)
    {
        if (appViewModel.Apps != null)
        {
            using (StreamWriter sw = File.CreateText(PATH))
            {
                string forsave = JsonConvert.SerializeObject(appViewModel.Apps);
                sw.Write(forsave);
            }
        }
        else
        {
            appViewModel.Apps = null;
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

     public static ObservableCollection<AppModel> LoadDatabase()
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
}