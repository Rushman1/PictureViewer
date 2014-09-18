using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Services.SettingsWindowService.Views;

namespace Services.SettingsWindowService {
  public class SettingsWindowModule : IModule {
    private readonly IUnityContainer _container;

    public SettingsWindowModule(IUnityContainer container) {
      _container = container;
    }

    public void Initialize() {
      _container.RegisterType<IChildWindow, SettingsWindow>();
    }
  }
}
