using System.ComponentModel;
using System.Windows;
using Infrastructure.Interfaces;
using PictureViewer_V2.Models;

namespace PictureViewer_V2 {
  /// <summary>
  /// Interaction logic for DebugWindow.xaml
  /// </summary>
  public partial class DebugWindow : Window, IView {
    public DebugWindow(IDebugWindowViewModel viewModel) {
      InitializeComponent();
      ViewModel = viewModel;
    }

    public IViewModel ViewModel { get { return (IDebugWindowViewModel) DataContext; } set { DataContext = value; } }
    
    protected override void OnClosing(CancelEventArgs e) {
      var d = (DebugWindowViewModel) DataContext;
      d.SendCloseMessageCommand.Execute();
      base.OnClosing(e);
    }
  }
}
