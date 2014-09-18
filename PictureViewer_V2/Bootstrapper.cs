using System.Windows;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using PictureViewer_V2.Models;

namespace PictureViewer_V2 {
  public class Bootstrapper : UnityBootstrapper {
    protected override DependencyObject CreateShell() {
      var shell = Container.Resolve<Shell>();
      return shell;
    }

    protected override void ConfigureContainer() {
      base.ConfigureContainer();
      Container.RegisterType<IShellViewModel, ShellViewModel>(new ContainerControlledLifetimeManager());
      Container.RegisterType<IDebugWindowViewModel, DebugWindowViewModel>();
    }

    protected override void InitializeShell() {
      base.InitializeShell();

      App.Current.MainWindow = (Window) Shell;
      App.Current.MainWindow.Show();
    }

    protected override IModuleCatalog CreateModuleCatalog() {
      return new DirectoryModuleCatalog{ModulePath = @".\Modules"};
    }
  }
}
