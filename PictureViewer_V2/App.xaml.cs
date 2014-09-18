using System;
using System.Windows;

namespace PictureViewer_V2 {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    protected override void OnStartup(StartupEventArgs e) {
      base.OnStartup(e);
      AppDomain.CurrentDomain.UnhandledException+=CurrentDomain_UnhandledException;

      var bootStrapper=new Bootstrapper();
      bootStrapper.Run();
    }

    void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
      HandleException(e.ExceptionObject as Exception);
    }

    private static void HandleException(Exception ex) {
      if (ex==null)
        return;

      // TODO: Log errors
      var error=string.Format("Error: {0}, Trace: {1}, InnerException: {2}", ex.Message, ex.StackTrace,
        ex.InnerException);

      MessageBox.Show(error);
      Environment.Exit(1);
    }
  }
}
