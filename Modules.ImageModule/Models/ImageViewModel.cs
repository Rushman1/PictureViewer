using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Transitionals;

namespace Modules.ImageModule.Models {
  public class ImageViewModel : ViewModelBase, IImageViewer {
    private readonly IGlobalParametersService _globalParametersService;
    private readonly IEventAggregator _eventAggregator;
    private LinearGradientBrush _borderColor;
    private DispatcherTimer _timer;
    private string _imageSource;
    private int _currentImageIndexNumber;
    private Transition _selectedTransition;
    private object _transitionContent;
    private string _imageName;
    private bool _isBorderColorChanged;
    
    public LinearGradientBrush BorderColor { get { return _borderColor; } set { if (Equals(value, _borderColor)) return; _borderColor=value; RaisePropertyChanged(); } }
    public String ImageSource { get { return _imageSource; } set { if (value==_imageSource) return; _imageSource=value; RaisePropertyChanged(); } }
    public Transition SelectedTransition { get { return _selectedTransition; } set { if (Equals(value, _selectedTransition)) return; _selectedTransition = value; RaisePropertyChanged(); } }
    public Object TransitionContent { get { return _transitionContent; } set {  if (Equals(value, _transitionContent)) return; _transitionContent = value; RaisePropertyChanged(); } }
    public string ImageName { get { return _imageName; } set { if (value == _imageName) return; _imageName = value; RaisePropertyChanged(); } }
    public bool IsPaused { get; private set; }
    public Queue<string> MessageQueue { get; set; }
    public bool IsBorderColorChanged { get { return _isBorderColorChanged; } set { if (value.Equals(_isBorderColorChanged)) return; _isBorderColorChanged = value; RaisePropertyChanged(); } }

    public DelegateCommand OnLoadCommand { get; private set; }
    public DelegateCommand NextImageCommand { get; private set; }
    public DelegateCommand PreviousImageCommand { get; private set; }
    public DelegateCommand PauseCommand { get; private set; }

    public ImageViewModel(IGlobalParametersService globalParametersService, IEventAggregator eventAggregator) {
      MessageQueue = new Queue<string>();
      _globalParametersService=globalParametersService;
      _eventAggregator=eventAggregator;
      BorderColor=new LinearGradientBrush(Colors.RoyalBlue, Colors.Blue, new Point(0, 0), new Point(1, 1));
      OnLoadCommand=new DelegateCommand(OnLoad);
      NextImageCommand = new DelegateCommand(NextImage);
      PreviousImageCommand = new DelegateCommand(PreviousImage);
      PauseCommand = new DelegateCommand(PauseViewer);
      _eventAggregator.GetEvent<SetProgramStateEvent>().Subscribe(CheckProgramState);
      IsPaused = false;
      MessageQueue.Enqueue("ImageViewModel constructor finished");
    }

    private void OnLoad() {
      MessageQueue.Enqueue("ImageViewModel.OnLoad() started");
      StartViewer();
      BorderColorChanger();
      MessageQueue.Enqueue("ImageViewModel.OnLoad() finished");
    }

    private void StartViewer() {
      MessageQueue.Enqueue("ImageViewModel.StartViewer() started");
      _timer = new DispatcherTimer {
        Interval = new TimeSpan(0, 0, 0, (int) _globalParametersService.GlobalSettings.TransitionSpeed)
      };
      if (_globalParametersService.GlobalSettings.ShufflePictures!=null&&
          (bool)_globalParametersService.GlobalSettings.ShufflePictures) {
        _globalParametersService.ImageList=_globalParametersService.ImageList.Shuffle();
      }
      MessageQueue.Enqueue("Setting first Image. Current Image Index Number : "+_currentImageIndexNumber);
      var firstImagePath = _globalParametersService.ImageList[_currentImageIndexNumber];
      GetImageName(firstImagePath);
      CreateTransitionalContent(firstImagePath);
      
      _timer.Tick+= (sender, args) => {
        System.Diagnostics.Debug.WriteLine(string.Format("Methods: {0}, _currentImageIndexNumber: {1}","_timer.Tick",_currentImageIndexNumber));
        if (_currentImageIndexNumber + 1 > _globalParametersService.ImageList.Count-1)
          _currentImageIndexNumber = -1;

        GetImage(++_currentImageIndexNumber);
        SendQueuedMessages();
      };
      _timer.Start();
      SendQueuedMessages();
    }

    private void BorderColorChanger() {
      MessageQueue.Enqueue("ImageViewModel.BorderColorChanger() started");
      MessageQueue.Enqueue("ImageViewModel.BorderColorChanger() finished");
    }

    private void NextImage() {
      MessageQueue.Enqueue("ImageViewModel.NextImage() started");
      MessageQueue.Enqueue(string.Format("Methods: {0}, _currentImageIndexNumber: {1}", "NextImage", _currentImageIndexNumber));
/*
      _globalParametersService.ProgramState = ProgramStatesEnum.paused;
      IsPaused = true;
      CheckProgramState(ProgramStatesEnum.paused);
*/
      var num = _currentImageIndexNumber++;
      System.Diagnostics.Debug.WriteLine(string.Format("Methods: {0}, _currentImageIndexNumber: {1}","NextImage",_currentImageIndexNumber));

      if (num >= _globalParametersService.ImageList.Count - 1)
        _currentImageIndexNumber = 0;

      GetImage(_currentImageIndexNumber);
      SendQueuedMessages();
    }

    private void PreviousImage() {
/*
      _globalParametersService.ProgramState = ProgramStatesEnum.paused;
      IsPaused = true;
      CheckProgramState(ProgramStatesEnum.paused);
*/
      var num=_currentImageIndexNumber--;
      System.Diagnostics.Debug.WriteLine(string.Format("Methods: {0}, _currentImageIndexNumber: {1}", "PreviousImage", _currentImageIndexNumber));

      if (num<=0)
        _currentImageIndexNumber = _globalParametersService.ImageList.Count - 1;

      GetImage(_currentImageIndexNumber);
      SendQueuedMessages();
    }

    private void PauseViewer() {
      MessageQueue.Enqueue("ImageViewModel.PauseViewer() started");
      MessageQueue.Enqueue("IsPaused : "+IsPaused);
      IsPaused = !IsPaused;
      _globalParametersService.ProgramState = IsPaused ? ProgramStatesEnum.paused : ProgramStatesEnum.running;
      CheckProgramState(_globalParametersService.ProgramState);
      MessageQueue.Enqueue("ImageViewModel.PauseViewer() finished");
      SendQueuedMessages();
    }

    private void GetImage(int imageNumber) {
      MessageQueue.Enqueue("ImageViewModel.GetImage() started");
      if (_globalParametersService.GlobalSettings.TransactionType != 0) { SelectedTransition = GetTransition(); }
      var imagePath = _globalParametersService.ImageList[imageNumber];
      GetImageName(imagePath);
      CreateTransitionalContent(imagePath);
      MessageQueue.Enqueue("ImageViewModel.GetImage() finished");
    }

    private void CreateTransitionalContent(string imageSource) {
      MessageQueue.Enqueue("ImageViewModel.CreateTansitionalContent() started");
      ImageName = _globalParametersService.GlobalSettings.ImageName;
      MessageQueue.Enqueue("ImageName : " + ImageName);
      try {
        var i=new Image { Source=new BitmapImage(new Uri(imageSource, UriKind.RelativeOrAbsolute)) };
        MessageQueue.Enqueue("ImageSource : "+imageSource);
        TransitionContent = i;
        MessageQueue.Enqueue("ImageViewModel.CreateTransitionalContent() finished");
      } catch (Exception ex) {
        // TODO: Log message and continue
      }
    }

    private void GetImageName(string imagePath) {
      var imageName=new FileInfo(imagePath).Name;
      var imageNameShortened=imageName.Substring(0, imageName.Length-4);
      _globalParametersService.GlobalSettings.ImageName=imageNameShortened;
    }

    private void CheckProgramState(ProgramStatesEnum state) {
      MessageQueue.Enqueue("ImageViewModel.CheckProgramState() started");
      MessageQueue.Enqueue(string.Format("CheckProgramState. State is {0}", state));
      System.Diagnostics.Debug.WriteLine(string.Format("CheckProgramState. State is {0}",state));
      switch (state) {
        case ProgramStatesEnum.running:
          if (!_timer.IsEnabled) {
            StartViewer();
          }
          break;
        case ProgramStatesEnum.paused:
          if (_timer.IsEnabled) {
            _timer.Stop();
          }
          break;
        default:
          break;
      }
      MessageQueue.Enqueue("ImageViewModel.CheckProgramState() finished");
    }

    private Transition GetTransition() {
      MessageQueue.Enqueue("ImageViewModel.GetTransition() started");
      var transitionNumber = _globalParametersService.GlobalSettings.TransactionType;
      MessageQueue.Enqueue("Transition Number : " + transitionNumber);

      if (transitionNumber == 16) {
        var random = new Random();
        transitionNumber = random.Next(1, 22);
      }

      switch (transitionNumber) {
        case 0:
          MessageQueue.Enqueue("Returning Transition : \'null\'");
          return null;
        case 1:
          MessageQueue.Enqueue("Returning Transition : \'CheckerboardTransition\'");
          return new Transitionals.Transitions.CheckerboardTransition();
        case 2:
          MessageQueue.Enqueue("Returning Transition : \'DiagonalWipeTransition\'");
          return new Transitionals.Transitions.DiagonalWipeTransition();
        case 3:
          MessageQueue.Enqueue("Returning Transition : \'DiamondsTransition\'");
          return new Transitionals.Transitions.DiamondsTransition();
        case 4:
          MessageQueue.Enqueue("Returning Transition : \'DoorTransition\'");
          return new Transitionals.Transitions.DoorTransition();
        case 5:
          MessageQueue.Enqueue("Returning Transition : \'DotsTransition\'");
          return new Transitionals.Transitions.DotsTransition();
        case 6:
          MessageQueue.Enqueue("Returning Transition : \'DoubleRotateWipeTransition\'");
          return new Transitionals.Transitions.DoubleRotateWipeTransition();
        case 7:
          MessageQueue.Enqueue("Returning Transition : \'ExplosionTransition\'");
          return new Transitionals.Transitions.ExplosionTransition();
        case 8:
          MessageQueue.Enqueue("Returning Transition : \'FadeAndBlurTransition\'");
          return new Transitionals.Transitions.FadeAndBlurTransition();
        case 9:
          MessageQueue.Enqueue("Returning Transition : \'FadeAndGrowTransition\'");
          return new Transitionals.Transitions.FadeAndGrowTransition();
        case 10:
          MessageQueue.Enqueue("Returning Transition : \'FadeTransition\'");
          return new Transitionals.Transitions.FadeTransition();
        case 11:
          MessageQueue.Enqueue("Returning Transition : \'FlipTransition\'");
          return new Transitionals.Transitions.FlipTransition();
        case 12:
          MessageQueue.Enqueue("Returning Transition : \'HorizontalBlindsTransition\'");
          return new Transitionals.Transitions.HorizontalBlindsTransition();
        case 13:
          MessageQueue.Enqueue("Returning Transition : \'HorizontalWipeTransition\'");
          return new Transitionals.Transitions.HorizontalWipeTransition();
        case 14:
          MessageQueue.Enqueue("Returning Transition : \'MeltTransition\'");
          return new Transitionals.Transitions.MeltTransition();
        case 15:
          MessageQueue.Enqueue("Returning Transition : \'PageTransition\'");
          return new Transitionals.Transitions.PageTransition();
        case 17:
          MessageQueue.Enqueue("Returning Transition : \'RollTransition\'");
          return new Transitionals.Transitions.RollTransition();
        case 18:
          MessageQueue.Enqueue("Returning Transition : \'RotateTransition\'");
          return new Transitionals.Transitions.RotateTransition();
        case 19:
          MessageQueue.Enqueue("Returning Transition : \'RotateWipeTransition\'");
          return new Transitionals.Transitions.RotateWipeTransition();
        case 20:
          MessageQueue.Enqueue("Returning Transition : \'StarTransition\'");
          return new Transitionals.Transitions.StarTransition();
        case 21:
          MessageQueue.Enqueue("Returning Transition : \'TranslateTransition\'");
          return new Transitionals.Transitions.TranslateTransition();
        case 22:
          MessageQueue.Enqueue("Returning Transition : \'VerticalBlindsTransition\'");
          return new Transitionals.Transitions.VerticalBlindsTransition();
        case 23:
          MessageQueue.Enqueue("Returning Transition : \'VerticalWipeTransition\'");
          return new Transitionals.Transitions.VerticalWipeTransition();
        default:
          MessageQueue.Enqueue("Returning Transition : \'null\'");
          return null;
      }

    }

    private void SendQueuedMessages() {
      var l = new List<String>();
      for (int i = 0; i < MessageQueue.Count; i++) {
        l.Add(MessageQueue.Dequeue());
      }
      _eventAggregator.GetEvent<SetDebugMessageEvent>().Publish(l);
    }
  }
}
