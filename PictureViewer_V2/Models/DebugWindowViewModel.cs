using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace PictureViewer_V2.Models {
  public class DebugWindowViewModel : ViewModelBase, IDebugWindowViewModel {
    private readonly IEventAggregator _eventAggregator;
    private ObservableCollection<string> _debugStrings;
    private string _debugString;

    public ObservableCollection<string> DebugStrings {
      get { return _debugStrings; }
      set {
        if (Equals(value, _debugStrings)) return;
        _debugStrings = value;
        RaisePropertyChanged();
      }
    }

    public DelegateCommand SendCloseMessageCommand { get; private set; }
    public DelegateCommand ExportListCommand { get; private set; }

    public DebugWindowViewModel(IEventAggregator eventAggregator) {
      _eventAggregator = eventAggregator;
      DebugStrings = new ObservableCollection<string>();
      _eventAggregator.GetEvent<SetDebugMessageEvent>().Subscribe(SetMessage);
      SendCloseMessageCommand = new DelegateCommand(SendCloseMessage);
      ExportListCommand = new DelegateCommand(ExportList);
    }

    private void SendCloseMessage() {
      _eventAggregator.GetEvent<SetDebugWindowVisibilityEvent>().Publish(false);
    }

    private void ExportList() {
      var folder=new FolderBrowserDialog();
      var result=folder.ShowDialog();
      if (result==DialogResult.OK) {
        var selectedPath=folder.SelectedPath;

        // TODO: Create download file and name it 'DebugWindowList.log'
        // TODO: and copy it into the selected path
        var fileName = string.Format("{0}_{1}-{2}-{3} {4}-{5}.log", "DebugWindowList", DateTime.Now.Day,DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour,DateTime.Now.Minute);
        var blah = fileName.Replace('/', '-');
        var filePath = Path.Combine(selectedPath, blah);
        using(var sw = new StreamWriter(filePath,false)){
          DebugStrings.ForEach(x=>sw.WriteLine(x));
        }
      }
    }

    private void SetMessage(List<String> message) {
      //App.Current.Dispatcher.BeginInvoke(new Action(() => {
        DebugStrings.Insert(0, "***************************************************");
        message.ForEach(x => DebugStrings.Insert(0, string.Format("--->> {0}", x)));
        DebugStrings.Insert(0, DateTime.Now.ToShortDateString() + " / " + DateTime.Now.ToLongTimeString());
        DebugStrings.Insert(0, "***************************************************");
      //}));
    }
  }
}
