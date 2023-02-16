using CommunityToolkit.Mvvm.ComponentModel;
using OnionMedia.Core;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.States;

[INotifyPropertyChanged]
sealed partial class LicenseDialogState
{
    public LibraryInfo[] Libraries { get; } = GlobalResources.LibraryLicenses;

    [ObservableProperty] LibraryInfo selectedLibrary;
}