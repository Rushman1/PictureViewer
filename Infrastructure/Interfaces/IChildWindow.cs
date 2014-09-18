namespace Infrastructure.Interfaces {
  public interface IChildWindow {
    void Close();
    bool? ShowDialog();
    void SetOwner(object window);
    bool? DialogResult { get; set; }
  }
}
