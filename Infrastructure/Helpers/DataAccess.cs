using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Interfaces;

namespace Infrastructure.Helpers {
  public class DataAccess {
    private readonly IGlobalParametersService _globalParametersService;
    private string _pathFile;

    public DataAccess(IGlobalParametersService globalParametersService) {
      _globalParametersService = globalParametersService;
      _pathFile = Path.Combine(_globalParametersService.PathListDirectoryName, _globalParametersService.PathListFileName);
    }

    // TODO: Get list of selected paths from textfile
    // TODO: Order list of selected paths from textfile based 
    // TODO: Write list of selected paths to textfile

    public void WriteListOfPathsToFile(Dictionary<int, string> paths) {
      if (!DoesPathListFileExist()) {
        File.Create(_pathFile);
      }

      using (var writer = File.CreateText(_pathFile)) {
        for (int i = 0; i < paths.Count; i++) {
          writer.WriteLine(string.Concat(i,",",paths[i]));
        }
      }
    }

    private bool DoesPathListFileExist() {
      var d = File.Exists(_pathFile);
      return d;
    }

    public Dictionary<int, string> GetListOfPathsFromFile() {
      var d = new Dictionary<int, string>();

      if (!DoesPathListFileExist()) return d;

      using (var reader = new StreamReader(_pathFile)) {
        string line;
        while ((line = reader.ReadLine()) != null) {
          var split = line.Split(',');
          d.Add(Convert.ToInt32(split[0]),split[1]);

        }
      }
      return d;
    }
  }
}
