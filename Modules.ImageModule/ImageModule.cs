using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Modules.ImageModule.Models;
using Modules.ImageModule.Views;

namespace Modules.ImageModule {
  [Module(ModuleName = "ImageModule", OnDemand = false)]
  [ModuleDependency("GlobalParametersModule")]
  public class ImageModule : IModule {
    private readonly IRegionManager _manager;
    private readonly IUnityContainer _container;

    public ImageModule(IRegionManager manager, IUnityContainer container) {
      _manager = manager;
      _container = container;
    }

    public void Initialize() {
      _container.RegisterType<IImageViewer, ImageViewModel>(new ContainerControlledLifetimeManager());
      _manager.RegisterViewWithRegion(RegionNames.MainRegion, typeof (ImageViewer));
    }
  }
}
