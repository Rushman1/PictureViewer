using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Services.Models {
  public class SettingWindowViewModel : ViewModelBase, ISettingsWindow {
    public void Close() {
      throw new NotImplementedException();
    }

    public bool? ShowDialog() {
      throw new NotImplementedException();
    }

    public void SetOwner(object window) {
      throw new NotImplementedException();
    }

    public bool? DialogResult {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }
  }
}
