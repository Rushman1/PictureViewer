using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Services.SettingsWindowService.Models;

namespace Services.SettingsWindowService.Views {
  /// <summary>
  /// Interaction logic for SettingsWindow.xaml
  /// </summary>
  public partial class SettingsWindow : Window, IChildWindow {
    private readonly IEventAggregator _eventAggregator;
    private readonly IGlobalParametersService _globalParametersService;
    private readonly IUnityContainer _container;
    private readonly ILogService _logService;

    public SettingsWindow(IEventAggregator eventAggregator, IGlobalParametersService globalParametersService, IUnityContainer container, ILogService logService) {
      _eventAggregator = eventAggregator;
      _globalParametersService = globalParametersService;
      _container = container;
      _logService = logService;
      InitializeComponent();
      Closing += SettingsWindowClosing;
      DataContext = new SettingWindowViewModel(this,_eventAggregator, _globalParametersService, _container, _logService);
    }

    public void SetOwner(object window) {
      Owner = window as Window;
    }

    private void SettingsWindowClosing(object sender, CancelEventArgs e) {
      e.Cancel = true;
      Visibility = Visibility.Hidden;
    }

    private void DragWindow(object sender, MouseButtonEventArgs e) {
      DragMove();
    }
  }
}
