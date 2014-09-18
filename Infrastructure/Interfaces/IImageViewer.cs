using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;

namespace Infrastructure.Interfaces {
  public interface IImageViewer : IViewModel {
    DelegateCommand NextImageCommand { get; }
    DelegateCommand PreviousImageCommand { get; }
    DelegateCommand PauseCommand { get; }
    LinearGradientBrush BorderColor { get; set; }
    bool IsPaused { get; }
    bool IsBorderColorChanged { get; set; }
  }
}
