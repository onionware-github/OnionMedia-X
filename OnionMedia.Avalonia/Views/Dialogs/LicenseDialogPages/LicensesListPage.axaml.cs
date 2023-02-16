using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OnionMedia.Avalonia.States;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.Views.Dialogs.LicenseDialogPages;

public sealed partial class LicensesListPage : UserControl
{
    public LicensesListPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<LicenseDialogState>();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems?.Count == 0) return;
        ((LicenseDialogState)DataContext).SelectedLibrary = (LibraryInfo)e.AddedItems[0];
    }
}