using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Business;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace Services.SettingsWindowService.Models {
  public class SettingWindowViewModel : ViewModelBase, ISettingsWindow {
    private readonly IChildWindow _childWindow;
    private readonly IEventAggregator _eventAggregator;
    private readonly IGlobalParametersService _globalParametersService;
    private readonly IUnityContainer _container;
    private readonly ILogService _logService;
    private int _recentFolderListSelectedIndex;
    private int _timeToShowImageSelectedIndex;
    private int _transitionSelectedIndex;
    private string _selectedPath = string.Empty;
    private Dictionary<int, string> _recentFolderList;
    private Dictionary<int, string> _timeToShowImage;
    private Dictionary<int, string> _transitionSelection;
    private int _selectedRecentFolder;
    private int _selectedTimeToShowImage;
    private int _selectedTransition;
    private bool _shufflePictures;
    private bool _includeSubfolders;
    private DataAccess _d;
    private Dictionary<int, string> _listOfPaths;
    private Visibility _errorVisibility;
    private string _errorMessage;
    
    public Dictionary<int, string> RecentFolderList { get { return _recentFolderList; } set { if (Equals(value, _recentFolderList)) return; _recentFolderList = value; RaisePropertyChanged(); } }
    public Dictionary<int, string> TimeToShowImage { get { return _timeToShowImage; } set { if (Equals(value, _timeToShowImage)) return; _timeToShowImage = value; RaisePropertyChanged(); } }
    public Dictionary<int, string> TransitionSelection { get { return _transitionSelection; } set { if (Equals(value, _transitionSelection)) return; _transitionSelection = value; RaisePropertyChanged(); } }
    public int SelectedRecentFolder { get { return _selectedRecentFolder; } set { if (value == _selectedRecentFolder) return; _selectedRecentFolder = value; RaisePropertyChanged(); } }
    public int SelectedTimeToShowImage { get { return _selectedTimeToShowImage; } set { if (value == _selectedTimeToShowImage) return; _selectedTimeToShowImage = value; RaisePropertyChanged(); } }
    public int SelectedTransition { get { return _selectedTransition; } set { if (value == _selectedTransition) return; _selectedTransition = value; RaisePropertyChanged(); } }
    public int RecentFolderListSelectedIndex { get { return _recentFolderListSelectedIndex; } set { if (value == _recentFolderListSelectedIndex) return; _recentFolderListSelectedIndex = value; RaisePropertyChanged(); } }
    public int TimeToShowImageSelectedIndex { get { return _timeToShowImageSelectedIndex; } set { if (value == _timeToShowImageSelectedIndex) return; _timeToShowImageSelectedIndex = value; RaisePropertyChanged(); } }
    public int TransitionSelectedIndex { get { return _transitionSelectedIndex; } set { if (value == _transitionSelectedIndex) return; _transitionSelectedIndex = value; RaisePropertyChanged(); } }
    public bool ShufflePictures { get { return _shufflePictures; } set { if (value.Equals(_shufflePictures)) return; _shufflePictures = value; RaisePropertyChanged(); } }
    public bool IncludeSubfolders { get { return _includeSubfolders; } set { if (value.Equals(_includeSubfolders)) return; _includeSubfolders = value; RaisePropertyChanged(); } }
    public Visibility ErrorVisibility { get { return _errorVisibility; } set { if (value == _errorVisibility) return; _errorVisibility = value; RaisePropertyChanged(); } }
    public string ErrorMessage { get { return _errorMessage; } set { if (value == _errorMessage) return; _errorMessage = value; RaisePropertyChanged(); } }

    public object Owner { get; set; }
    public Settings NewSettings { get; set; }

    public DelegateCommand OkCommand { get; private set; }
    public DelegateCommand CancelCommand { get; private set; }
    public DelegateCommand FolderSelectCommand { get; private set; }
    public DelegateCommand OnLoadCommand { get; private set; }

    public SettingWindowViewModel(IChildWindow childWindow,IEventAggregator eventAggregator, IGlobalParametersService globalParametersService, IUnityContainer container, ILogService logService) {
      _eventAggregator = eventAggregator;
      _globalParametersService = globalParametersService;
      _container = container;
      _logService = logService;
      _childWindow = childWindow;
      NewSettings = new Settings();
      OkCommand=new DelegateCommand(Ok);
      CancelCommand=new DelegateCommand(Cancel);
      FolderSelectCommand=new DelegateCommand(FolderSelect);
      ErrorVisibility = Visibility.Collapsed;
      ErrorMessage = string.Empty;
      OnLoadCommand = new DelegateCommand(OnLoad);
    }

    private void OnLoad() {

      _d = new DataAccess(_globalParametersService);
      _listOfPaths = _d.GetListOfPathsFromFile();
      PopulateListBoxes(_listOfPaths);
      
      if (RecentFolderList!=null&&RecentFolderList.Count>0)
        RecentFolderListSelectedIndex=0;

      TransitionSelectedIndex=0;
      TimeToShowImageSelectedIndex=4;

      IncludeSubfolders=(bool)_globalParametersService.GlobalSettings.IncludeSubfolders;
      ShufflePictures = (bool) _globalParametersService.GlobalSettings.ShufflePictures;
      TimeToShowImageSelectedIndex = (int)_globalParametersService.GlobalSettings.TransitionSpeed;
      TransitionSelectedIndex = (int) _globalParametersService.GlobalSettings.TransactionType;

      _eventAggregator.GetEvent<SetProgramStateEvent>().Publish(ProgramStatesEnum.paused);
      _globalParametersService.ProgramState=ProgramStatesEnum.paused;
      
    }

    private void Ok() {
      // TODO: Pass information to set the global settings...
      NewSettings.ShufflePictures = ShufflePictures;
      NewSettings.IncludeSubfolders = IncludeSubfolders;
      NewSettings.TransactionType = SelectedTransition;
      NewSettings.TransitionSpeed = SelectedTimeToShowImage;
      NewSettings.ImagePath = RecentFolderList[SelectedRecentFolder];
      ReorderRecentPathList();
      _globalParametersService.GlobalSettings = NewSettings;
      var list = new ImageHelper();
      var imageList = list.CheckForImagesForDirectory(NewSettings.ImagePath, NewSettings);
      if (imageList.Any()) {
        _globalParametersService.ImageList = imageList;
        CloseWindow();
      } else {
        ErrorVisibility = Visibility.Visible;
        ErrorMessage = "There are no images in that folder";
      }
    }

    private void Cancel() {
      CloseWindow();
    }

    private void FolderSelect() {
      var folder = new FolderBrowserDialog();
      var result = folder.ShowDialog();
      if (result == DialogResult.OK) {
        _selectedPath = folder.SelectedPath;

        _d.WriteListOfPathsToFile(AddToList(_selectedPath));

        _listOfPaths.Clear();
        _listOfPaths=_d.GetListOfPathsFromFile();

        var pathSplit = _selectedPath.Split('\\');
        // TODO: Add to RecentFolderList combobox
        // TODO: And add to recent file list (keep 5)

        RecentFolderList = _listOfPaths;
        RecentFolderListSelectedIndex = 0;
      }
    }

    private Dictionary<int, string> AddToList(string path) {
      var d = new Dictionary<int, string>();
      var i = 0;
      d.Add(i,path);
      foreach (var listOfPath in _listOfPaths) {
        d.Add(++i,listOfPath.Value);
      }
      return d;
    }

    private void ReorderRecentPathList() {
      var d = new Dictionary<int, string>();
      var i = 0;
      d.Add(i,RecentFolderList[SelectedRecentFolder]);
      _listOfPaths.Remove(SelectedRecentFolder);
      foreach (var listOfPath in _listOfPaths) {
        d.Add(++i,listOfPath.Value);
      }
      _d.WriteListOfPathsToFile(d);
    }

    private void PopulateListBoxes(Dictionary<int, string> list) {
      
      var orderedList = list.OrderBy(x => x.Key);
      var d = new Dictionary<int, string>();
      orderedList.ForEach(x=> d.Add(x.Key, x.Value));
      RecentFolderList = d;

      _selectedPath = d.FirstOrDefault().Value;


      var time=new Dictionary<int, string>
                                  {
                                      {1,"1 Second"},
                                      {5,"5 Seconds"},
                                      {10, "10 Seconds"},
                                      {15, "15 Seconds"},
                                      {20, "20 Seconds"},
                                      {25, "25 Seconds"},
                                      {30, "30 Seconds"},
                                      {45, "45 Seconds"},
                                      {60, "1 Minute"},
                                      {180, "3 Minutes"},
                                      {300, "5 Minutes"},
                                      {600, "10 Minutes"},
                                      {1200, "20 Minutes"},
                                      {1800, "30 Minutes"}
                                  };

      TimeToShowImage=time;

      var transitions=new Dictionary<int, string>
                                      {
                                        {0, "None"},
                                        {1,"Checker Board"},
                                        {2,"Diagonal Wipe"},
                                        {3,"Diamond Wipe"},
                                        {4,"Door"},
                                        {5,"Dots"},
                                        {6,"Double Rotate"},
                                        {7,"Explosion"},
                                        {8,"Fade and Blur"},
                                        {9, "Fade and Grow"},
                                        {10, "Fade"},
                                        {11,"Flip"},
                                        {12, "Horizontal Blinds"},
                                        {13, "Horizontal Wipe"},
                                        {14,"Melt"},
                                        {15,"Page"},
                                        {16,"Random"},
                                        {17,"Roll"},
                                        {18,"Rotate"},
                                        {19,"Rotate Wipe"},
                                        {20,"Star"},
                                        {21,"Translation"},
                                        {22,"Vertical Blinds"},
                                        {23,"Vertical Wipe"}
                                      };

      TransitionSelection=transitions;
    }

    private void CloseWindow() {
      _eventAggregator.GetEvent<SetProgramStateEvent>().Publish(ProgramStatesEnum.running);
      _globalParametersService.ProgramState = ProgramStatesEnum.running;
      _childWindow.Close();
    }
  }
}
