using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Business;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces {
  public interface IGlobalParametersService {
    Settings GlobalSettings { get; set; }
    ProgramStatesEnum ProgramState { get; set; }
    List<String> ImageList { get; set; }
    bool ShowDebugMessages { get; set; }
    string LogDirectoryName { get; set; }
    string LogFileName { get; set; }
    string PathListDirectoryName { get; set; }
    string PathListFileName { get; set; }
  }
}
