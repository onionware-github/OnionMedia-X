using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OnionMedia.Avalonia.States;
using System.IO;

namespace OnionMedia.Avalonia.Views.Dialogs.LicenseDialogPages;

public sealed partial class LicensesInfoPage : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private LicenseDialogState state;

    public string LicenseText { get; set; }

    public LicensesInfoPage()
    {
        state = App.DefaultServiceProvider.GetService<LicenseDialogState>();
        state.PropertyChanged += UpdateDataContext;
        InitializeComponent();
        DataContext = state.SelectedLibrary;
        if (!File.Exists(state.SelectedLibrary.LicensePath)) return;
        LicenseText = File.ReadAllText(state.SelectedLibrary.LicensePath)?.Replace("{year}", state.SelectedLibrary.Year > 0 ? state.SelectedLibrary.Year.ToString() : string.Empty).Replace("{author}", state.SelectedLibrary.Author ?? string.Empty);;
        PropertyChanged?.Invoke(this, new(nameof(LicenseText)));
    }

    private void UpdateDataContext(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(state.SelectedLibrary)) return;
        DataContext = state.SelectedLibrary;
        if (!File.Exists(state.SelectedLibrary.LicensePath)) return;
        LicenseText = File.ReadAllText(state.SelectedLibrary.LicensePath)?.Replace("{year}", state.SelectedLibrary.Year > 0 ? state.SelectedLibrary.Year.ToString() : string.Empty).Replace("{author}", state.SelectedLibrary.Author ?? string.Empty);
        PropertyChanged?.Invoke(this, new(nameof(LicenseText)));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}