using System.Collections.Generic;

namespace Business {
  public class Settings {
    public string ImageName { get; set; }
    public int ImageNumber { get; set; }
    public bool? IncludeSubfolders { get; set; }
    public bool IsNewSetting { get; set; }
    public List<int> PicNumberList { get; set; }
    public int NumberOfImages { get; set; }
    public bool? ShufflePictures { get; set; }
    public int? TransitionSpeed { get; set; }
    public int? TransactionType { get; set; }
    public string ImagePath { get; set; }

    public Settings() {
      IncludeSubfolders = false;
      ShufflePictures = false;
      TransitionSpeed = 30;
      TransactionType = 0;
    }
  }
}
