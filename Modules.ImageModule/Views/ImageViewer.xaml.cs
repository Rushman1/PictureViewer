using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using Infrastructure.Interfaces;

namespace Modules.ImageModule.Views {
  /// <summary>
  /// Interaction logic for ImageViewer.xaml
  /// </summary>
  public partial class ImageViewer : UserControl, IView {
    public ImageViewer(IImageViewer viewModel) {
      InitializeComponent();
      ViewModel = viewModel;
      SetBorderColor();
    }

    private void SetBorderColor() {
      var dt=new DispatcherTimer();
      dt.Interval=new TimeSpan(0, 0, 0, 5);
      dt.Tick+=(sender, args) => {
        var randomNumber=new Random();
        var startColor=Color.FromArgb((byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(9, 255));
        var endColor=Color.FromArgb((byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(9, 255));
        var borderColor = (IImageViewer)ViewModel;
        borderColor.BorderColor=new LinearGradientBrush(startColor, endColor, 
          new Point(randomNumber.NextDouble(), randomNumber.NextDouble()), 
          new Point(randomNumber.NextDouble(), randomNumber.NextDouble()));
        borderColor.IsBorderColorChanged = true;
      };
      dt.Start();
    }

    public IViewModel ViewModel {
      get { return (IImageViewer) DataContext; }
      set { DataContext = value; }
    }

    private void Grid_MouseEnter(object sender, MouseEventArgs e) {
      AnimateControls(true);
      brdControlsBottom.Visibility = Visibility.Visible;
    }

    private void Grid_MouseLeave(object sender, MouseEventArgs e) {
      AnimateControls(false);
    }

    private void imgPrev_MouseDown(object sender, MouseButtonEventArgs e) {
      var vm = (IImageViewer) DataContext;
      vm.PreviousImageCommand.Execute();
    }

    private void imgPausePlay_MouseDown(object sender, MouseButtonEventArgs e) {
      var vm = (IImageViewer) DataContext;
      vm.PauseCommand.Execute();
      if (vm.IsPaused) {
        var b = new BitmapImage();
        var i = new Uri("pack://application:,,,/Resources;component/Images/play_rest.png");
        b.BeginInit();
        b.UriSource = i;
        b.EndInit();

        imgPausePlay.Source = b;
      } else {
        var b=new BitmapImage();
        var i=new Uri("pack://application:,,,/Resources;component/Images/pause_rest.png");
        b.BeginInit();
        b.UriSource=i;
        b.EndInit();

        imgPausePlay.Source=b;
      }
    }

    private void imgNext_MouseDown(object sender, MouseButtonEventArgs e) {
      var vm = (IImageViewer) DataContext;
      vm.NextImageCommand.Execute();
    }

    private void AnimateControls(bool showControls) {
      var sbBottom = showControls
        ? (Storyboard) FindResource("FadeInBottom")
        : (Storyboard) FindResource("FadeOutBottom");
      sbBottom.Begin(this);
    }

    private void FadeOutBorder_OnCompleted(object sender, EventArgs e) {
      var borderBlah = (Storyboard) FindResource("FadeInBorder");
      borderBlah.Begin();
      var vm = (IImageViewer) DataContext;
      vm.IsBorderColorChanged = false;
    }
  }
}
