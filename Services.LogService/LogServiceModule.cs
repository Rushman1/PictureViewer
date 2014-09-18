using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Services.LogService {
  [Module(ModuleName = "LogService", OnDemand = false)]
  public class LogServiceModule : IModule {
    private readonly IUnityContainer _container;

    public LogServiceModule(IUnityContainer container) {
      _container = container;
    }

    public void Initialize() {
      _container.RegisterType<ILogService, LogService>(new ContainerControlledLifetimeManager());
    }
  }
}
