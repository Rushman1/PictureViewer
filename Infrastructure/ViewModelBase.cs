using System.ComponentModel;
using System.Runtime.CompilerServices;
using Infrastructure.Annotations;

namespace Infrastructure {
  public class ViewModelBase : INotifyPropertyChanged {
    private bool _isBusy;
    private string _busyMessage;

    public bool IsBusy {
      get { return _isBusy; }
      set {
        if (value.Equals(_isBusy)) return;
        _isBusy = value;
        RaisePropertyChanged();
      }
    }

    public string BusyMessage {
      get { return _busyMessage; }
      set {
        if (value == _busyMessage) return;
        _busyMessage = value;
        RaisePropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
