using System.Collections.Generic;
using System.Windows;
using try_to_make_app.Database_things;
using try_to_make_app.View;

namespace try_to_make_app.ViewModel;

public class ApppsViewModel: BaseViewModel
{
   private ICollection<AppModel> _appModels = new List<AppModel> { new AppModel("Нет приложений") };
   public ICollection<AppModel> AppModels
   {
      get
      {
         return _appModels;
      }
      set
      {
         _appModels = value;
         OnPropertyChanged("AppModel");
      }
   }

   public RelayComand CreateAppWindowComand =>
      _createAppWindow ??= new RelayComand(execute => CreateAppWindow("test"), canExecute => { return true; });
   private RelayComand _createAppWindow;

   private void CreateAppWindow(string AppName)
   {
      AppWindow appWindow = new AppWindow(new AppWindowViewModel(AppName));
      
   }

}