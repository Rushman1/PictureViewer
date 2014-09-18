using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Services.GlobalParametersService {
  [Module(ModuleName = "GlobalParametersModule")]
  [ModuleDependency("LogService")]
  public class GlobalParametersModule : IModule {
    private readonly IUnityContainer _container;
    private readonly IEventAggregator _eventAggregator;

    public GlobalParametersModule(IUnityContainer container, IEventAggregator eventAggregator) {
      _container = container;
      _eventAggregator = eventAggregator;
    }

    public void Initialize() {
      _container.RegisterType<IGlobalParametersService, GlobalParametersService>(
        new ContainerControlledLifetimeManager());
      _eventAggregator.GetEvent<LoadMessageUpdateEvent>().Publish("Global Parameters Service loaded");
    }
  }
}
