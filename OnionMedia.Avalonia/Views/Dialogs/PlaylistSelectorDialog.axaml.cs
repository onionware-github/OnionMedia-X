using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using OnionMedia.Core.ViewModels.Dialogs;
using YoutubeExplode.Videos;

namespace OnionMedia.Avalonia.Views.Dialogs;

public partial class PlaylistSelectorDialog : ContentDialog, IStyleable
{
    Type IStyleable.StyleKey => typeof(ContentDialog);
    
    /// <summary> FOR AVALONIA XAML ONLY, DO NOT USE MANUALLY!</summary>
    public PlaylistSelectorDialog() {}

    public PlaylistSelectorDialog(IEnumerable<IVideo> videos)
    {
        DataContext = new PlaylistSelectorViewModel(videos);
        this.InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var btn = e.NameScope.Find<Button>("PrimaryButton");
        btn?.Classes.Add("AccentButtonStyle");
    }
}