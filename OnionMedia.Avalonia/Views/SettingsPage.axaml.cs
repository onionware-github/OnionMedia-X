using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OnionMedia.Core.ViewModels;

namespace OnionMedia.Avalonia.Views;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<SettingsViewModel>();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}