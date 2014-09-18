﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace Infrastructure.Interfaces {
  public interface IDebugWindowViewModel : IViewModel {
    DelegateCommand SendCloseMessageCommand { get; }
  }
}
