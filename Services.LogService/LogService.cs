using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Services.LogService {
  public class LogService : ILogService {
    private string ERROR_LOG_LOCATION = System.IO.Path.Combine(Environment.SpecialFolder.Personal.ToString(),"PictureViewer");
    private const string ERROR_LOG_FILE_NAME= "ErrorLog.log";
    private const string FOLDER_LIST_FILE_NAME = "FolderList.txt";

    public LogService() {
      CheckForErrorDirectoryAndLogExists();
      CheckForFolderListDirectoryAndFileExists();
    }

    public void LogError(string message, ErrorTypesEnum errorType, string imagePath) {
      using (var writer = new StreamWriter(Path.Combine(ERROR_LOG_LOCATION,ERROR_LOG_FILE_NAME), true,Encoding.UTF8,0x1000)) {
        writer.WriteLine(string.Concat(new object[]{"\r\n",DateTime.Now,",",errorType,",",message,",",imagePath}));
      }
    }

    public void AddToFolderlist(StringBuilder folderList) {
      using (var writer=new StreamWriter(Path.Combine(ERROR_LOG_LOCATION, FOLDER_LIST_FILE_NAME), true, Encoding.UTF8, 0x1000)) {
        writer.WriteLine(folderList.ToString());
      }
    }

    public Dictionary<int, string> FolderList() {
      throw new NotImplementedException();
    }

    private void CheckForErrorDirectoryAndLogExists() {
      FileStream stream = null;
      if (ErrorFileExists()) return;
      if (!Directory.Exists(ERROR_LOG_LOCATION))
        Directory.CreateDirectory(ERROR_LOG_LOCATION);

      using (stream = File.Create(Path.Combine(ERROR_LOG_LOCATION,ERROR_LOG_FILE_NAME))) {
        var bytes = new UTF8Encoding(true).GetBytes("Date,ErrorType,ErrorMessage,FileName");
        stream.Write(bytes,0,bytes.Length);
      }
    }

    private void CheckForFolderListDirectoryAndFileExists() {
      FileStream stream = null;
      if (ErrorFileExists())
        return;
      if (!Directory.Exists(ERROR_LOG_LOCATION))
        Directory.CreateDirectory(ERROR_LOG_LOCATION);

      using (stream=File.Create(Path.Combine(ERROR_LOG_LOCATION, FOLDER_LIST_FILE_NAME))) {
        var bytes=new UTF8Encoding(true).GetBytes("Position,Path");
        stream.Write(bytes, 0, bytes.Length);
      }
    }

    private bool ErrorFileExists() {
      return File.Exists(Path.Combine(ERROR_LOG_LOCATION,ERROR_LOG_FILE_NAME));
    }

    private bool FolderFileExists() {
      return File.Exists(Path.Combine(ERROR_LOG_LOCATION, FOLDER_LIST_FILE_NAME));
    }
  }
}
