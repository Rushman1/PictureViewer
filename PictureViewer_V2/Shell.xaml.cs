using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Infrastructure.Interfaces;

namespace PictureViewer_V2 {
  /// <summary>
  /// Interaction logic for Shell.xaml
  /// </summary>
  public partial class Shell : Window, IView {
    public Shell(IShellViewModel viewModel) {
      InitializeComponent();
      ViewModel = viewModel;
/*
      MenuCanvas.SetValue(Canvas.LeftProperty, MainGrid.ActualWidth);
      MenuCanvas.SetValue(Canvas.TopProperty, 15.0);
*/

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

    private async void DockPanel_MouseLeave(object sender, MouseEventArgs e) {
      var wait = Task.Factory.StartNew(() => System.Threading.Thread.Sleep(1000));
      await wait;
      StartLeaveAnimation();
    }

    private void DockPanel_MouseEnter(object sender, MouseEventArgs e) {
      var ta = new ThicknessAnimation();
/*
      ta.From = MenuCanvas.Margin;
      ta.To = new Thickness(0,15,-360,0);
      ta.Duration = new Duration(TimeSpan.FromMilliseconds(500));
      MenuCanvas.BeginAnimation(MarginProperty, ta);
*/
    }

    private void StartLeaveAnimation() {
      var ta=new ThicknessAnimation();
/*
      ta.From=MenuCanvas.Margin;
      ta.To=new Thickness(0, 15, -410, 0);
      ta.Duration=new Duration(TimeSpan.FromMilliseconds(500));
      MenuCanvas.BeginAnimation(MarginProperty, ta);
*/

    }

    private void Shell_OnSizeChanged(object sender, SizeChangedEventArgs e) {
/*
      MenuCanvas.SetValue(Canvas.LeftProperty, MainGrid.ActualWidth);
      MenuCanvas.SetValue(Canvas.TopProperty, 15.0);
*/
    }
  }
}
