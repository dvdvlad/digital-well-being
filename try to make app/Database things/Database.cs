using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace try_to_make_app.Database_things;

[Serializable]
class App: Process
{
    private DateTime PersonalLimitation;
}

[Serializable]
class Database
{
    public List<App> Apps;
    private DateTime LastRun;
    
}


