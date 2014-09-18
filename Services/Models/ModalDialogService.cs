using System;
using System.Windows;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Services.Models {
  public class ModalDialogService : IDialogBoxService {

    #region Constructor
    public ModalDialogService() { }

    #endregion

    #region Methods
    MessageBoxButton GetButton(DialogButton button) {
      switch (button) {
        case DialogButton.OK:
          return MessageBoxButton.OK;
        case DialogButton.OKCancel:
          return MessageBoxButton.OKCancel;
        case DialogButton.YesNo:
          return MessageBoxButton.YesNo;
        case DialogButton.YesNoCancel:
          return MessageBoxButton.YesNoCancel;
      }
      throw new ArgumentOutOfRangeException("button", "Invalid button");
    }
    MessageBoxImage GetImage(DialogImage image) {
      switch (image) {
        case DialogImage.Asterisk:
          return MessageBoxImage.Asterisk;
        case DialogImage.Error:
          return MessageBoxImage.Error;
        case DialogImage.Exclamation:
          return MessageBoxImage.Exclamation;
        case DialogImage.Hand:
          return MessageBoxImage.Hand;
        case DialogImage.Information:
          return MessageBoxImage.Information;
        case DialogImage.None:
          return MessageBoxImage.None;
        case DialogImage.Question:
          return MessageBoxImage.Question;
        case DialogImage.Stop:
          return MessageBoxImage.Stop;
        case DialogImage.Warning:
          return MessageBoxImage.Warning;
      }
      throw new ArgumentOutOfRangeException("image", "Invalid image");
    }
    DialogResponse GetResponse(MessageBoxResult result) {
      switch (result) {
        case MessageBoxResult.Cancel:
          return DialogResponse.Cancel;
        case MessageBoxResult.No:
          return DialogResponse.No;
        case MessageBoxResult.None:
          return DialogResponse.None;
        case MessageBoxResult.OK:
          return DialogResponse.OK;
        case MessageBoxResult.Yes:
          return DialogResponse.Yes;
      }
      throw new ArgumentOutOfRangeException("result", "Invalid result");
    }

    public DialogResponse ShowException(String message, DialogImage image=DialogImage.Error) {
      MessageBox.Show(message, "Error", MessageBoxButton.OK);
      return DialogResponse.OK;
    }

    public DialogResponse ShowMessage(String message, String caption, DialogButton button, DialogImage image) {
      return GetResponse(MessageBox.Show(message, caption, GetButton(button)));
    }

    #endregion
  }
}
