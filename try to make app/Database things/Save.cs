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
    public static  void SaveDatabase(Database database)
    {
        if (database != null)
        {
            using (StreamWriter sw = File.CreateText(PATH))
            {
                string forsave = JsonConvert.SerializeObject(database);
                sw.Write(forsave);
            }
        }
        else
        {
            database = new Database();
            using (StreamWriter sw = File.CreateText(PATH))
            {
                string forsave = JsonConvert.SerializeObject(database);
                sw.Write(forsave);
            }
        }
    }

     public static Database LoadDatabase()
    {
        bool fileexist = File.Exists(PATH);
        if (fileexist)
        {
            using (var reader = File.OpenText(PATH))
            {
                string filetxt = reader.ReadToEnd();
                Database database =  JsonConvert.DeserializeObject<Database>(filetxt);
                if (database == null)
                {
                    Database IfNUlLdatabase = new Database();
                    DayViewModel dayViewModel = new DayViewModel();
                    dayViewModel.UpdateList();
                    IfNUlLdatabase.DayViewModels.Add(dayViewModel);
                    return IfNUlLdatabase;
                }
                else
                {
                    return database;
                }
            }
        }
        else
        {
            File.CreateText(PATH).Dispose();
            Database database = new Database();
            return database;
        }
    }
}