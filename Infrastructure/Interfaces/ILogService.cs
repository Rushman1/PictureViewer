using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces {
  public interface ILogService {
    void LogError(string message, ErrorTypesEnum errorType, string imagePath);
    void AddToFolderlist(StringBuilder folderList);
    Dictionary<int,String> FolderList();
  }
}
