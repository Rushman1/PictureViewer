using System.Configuration;
using Infrastructure;
using Infrastructure.Annotations;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace PictureViewer_V2.Models {
  public class ShellViewModel : ViewModelBase, IShellViewModel {
    private readonly IEventAggregator _eventAggregator;
    private readonly IUnityContainer _container;
    private IChildWindow _childWindow;
    private bool _debugWindowVisible;

    public bool DebugWindowVisible { get { return _debugWindowVisible; } set { if (value.Equals(_debugWindowVisible)) return; _debugWindowVisible = value; RaisePropertyChanged(); } }

    public DelegateCommand ShowSettingsCommand { get; private set; }
    public DelegateCommand ShowDebugWindowCommand { get; private set; }

    public ShellViewModel(IEventAggregator eventAggregator, IUnityContainer container) {
      _eventAggregator = eventAggregator;
      _container = container;
      _eventAggregator.GetEvent<SetIsBusyEvent>().Subscribe(SetBusy);
      _eventAggregator.GetEvent<SetBusyMessageEvent>().Subscribe(SetBusyMessage);
      _eventAggregator.GetEvent<SetDebugWindowVisibilityEvent>().Subscribe(SetDebugWindowVisibilityCheckMark);
      ShowSettingsCommand = new DelegateCommand(ShowSettings);
      ShowDebugWindowCommand = new DelegateCommand(ShowDebugWindow);
      var appSettings=ConfigurationManager.AppSettings;
      if (appSettings[0]!="true")
        return;
      ShowDebugWindow();
    }

    private void ShowSettings() {
      _childWindow = _container.Resolve<IChildWindow>();
      _childWindow.SetOwner(App.Current.MainWindow);
      _childWindow.ShowDialog();
    }

    private void ShowDebugWindow() {
      if(DebugWindowVisible) return;
      var n=new DebugWindow(new DebugWindowViewModel(_eventAggregator));
      n.Show();
      SetDebugWindowVisibilityCheckMark(true);
    }

    private void SetDebugWindowVisibilityCheckMark(bool e) {
      DebugWindowVisible = e;
    }

    private void SetBusy(bool isBusy) {
      IsBusy = isBusy;
    }

    private void SetBusyMessage(string busyMessage) {
      BusyMessage = busyMessage;
    }
   }
}
