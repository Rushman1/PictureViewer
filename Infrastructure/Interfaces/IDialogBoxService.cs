using System;
using Infrastructure.Enums;

namespace Infrastructure.Interfaces {
    public interface IDialogBoxService {
      DialogResponse ShowException(String message, DialogImage image = DialogImage.Error);
      DialogResponse ShowMessage(String message, String caption, DialogButton button, DialogImage image);
    }
  }