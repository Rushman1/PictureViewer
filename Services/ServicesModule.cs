using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Services.Models;

namespace Services {
  public class ServicesModule : IModule {
    private readonly IUnityContainer _container;
    private readonly IEventAggregator _eventAggregator;

    public ServicesModule(IUnityContainer container, IEventAggregator eventAggregator) {
      _container = container;
      _eventAggregator = eventAggregator;
    }

    public void Initialize() {
      _container.RegisterType<IGlobalParametersService, GlobalParametersService>(
        new ContainerControlledLifetimeManager());
      _eventAggregator.GetEvent<LoadMessageUpdateEvent>().Publish("Global Parameters Service loaded");
      _container.RegisterType<IDialogBoxService, ModalDialogService>(new ContainerControlledLifetimeManager());
      _eventAggregator.GetEvent<LoadMessageUpdateEvent>().Publish("Modal Box Service loaded");
    }

    
  }
}
