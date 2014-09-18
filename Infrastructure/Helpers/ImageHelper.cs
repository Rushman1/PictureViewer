using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business;
using Infrastructure.Enums;
using Microsoft.Practices.Prism.Events;

namespace Infrastructure.Helpers {
  public class ImageHelper {
    private readonly IEventAggregator _eventAggregator;

    readonly List<String> _imagesList=new List<string>();

    public List<String> CheckForImagesForDirectory(string directory, Settings settings) {
      if (settings.IncludeSubfolders==null||!(bool)settings.IncludeSubfolders) {
        GetImagesForFolder(directory);
      } else {
        if (Directory.GetDirectories(directory).Any()) {
          foreach (var dir in Directory.GetDirectories(directory)) { GetImagesForFolder(dir); }
        }
        this.GetImagesForFolder(directory);
      }

      return _imagesList;
    }

    private void GetImagesForFolder(string path) {
      var fileInfo=new DirectoryInfo(path);
      GetAllImagesForFolder(fileInfo);
    }

    private void GetAllImagesForFolder(DirectoryInfo fileInfo) {
      foreach (var info in fileInfo.GetFiles()) {
        var ext=info.Extension;
        using (IEnumerator<string> enumerator=Enum.GetNames(typeof(FileExtensionsEnum)).Where<string>(
          delegate(string name) {
            return ((ext.Length>3)&&(ext.ToLower().Substring(1, 3)==name));
          }).GetEnumerator()) {
          while (enumerator.MoveNext()) {
            string current=enumerator.Current;
            _imagesList.Add(info.FullName);
          }
        }
      }
    }

    public List<string> CheckForImagesFromUrl(string imagePath, Settings settings) {
      return new List<string>();
    }
  }
}
