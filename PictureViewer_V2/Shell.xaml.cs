using System.Configuration;
using System.Windows;
using System.Windows.Input;
using Infrastructure.Interfaces;

namespace PictureViewer_V2 {
  /// <summary>
  /// Interaction logic for Shell.xaml
  /// </summary>
  public partial class Shell : Window, IView {
    public Shell(IShellViewModel viewModel) {
      InitializeComponent();
      ViewModel = viewModel;
    }
    private void menuKOT_Click(object sender, RoutedEventArgs e) {
      Topmost=Topmost!=true;
    }

    private void menuClose_Click(object sender, RoutedEventArgs e) {
      App.Current.Shutdown();
      Close();
    }

    private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
      // TODO: Make either the picture viewer full screen or
      //       make the image open in default viewer

      WindowState=WindowState==WindowState.Maximized?WindowState.Normal:WindowState.Maximized;
    }

    private void DragWindow(object sender, MouseButtonEventArgs e) {
      DragMove();
    }

    public IViewModel ViewModel {
      get { return (IShellViewModel) DataContext; }
      set { DataContext = value; }
    }
  }
}
