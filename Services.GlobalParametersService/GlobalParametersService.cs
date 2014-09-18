using System;
using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using System.Configuration;
using Settings = Business.Settings;

namespace Services.GlobalParametersService {
  public class GlobalParametersService : IGlobalParametersService {
    public Settings GlobalSettings { get; set; }
    public ProgramStatesEnum ProgramState { get; set; }
    public List<String> ImageList { get; set; }
    public bool ShowDebugMessages { get; set; }
    public string LogDirectoryName { get; set; }
    public string LogFileName { get; set; }
    public string PathListDirectoryName { get; set; }
    public string PathListFileName { get; set; }

    public GlobalParametersService() {
      GlobalSettings = new Settings();
      ImageList = new List<string>();
      ProgramState = ProgramStatesEnum.running;
      GetStaticImageList();
      var appSettings = ConfigurationManager.AppSettings;
      ShowDebugMessages = appSettings[0] == "true";
      LogDirectoryName = appSettings[1];
      LogFileName = appSettings[2];
      PathListDirectoryName = appSettings[3];
      PathListFileName = appSettings[4];}

    private void GetStaticImageList() {
      var il = new ImageHelper();
      GlobalSettings.ImagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
      ImageList = il.CheckForImagesForDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),GlobalSettings);
    }
  }
}
