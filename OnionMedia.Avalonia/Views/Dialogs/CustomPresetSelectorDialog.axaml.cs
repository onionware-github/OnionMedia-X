using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using DynamicData;
using FluentAvalonia.UI.Controls;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.Views.Dialogs;

public sealed partial class CustomPresetSelectorDialog : ContentDialog, IStyleable
{
    Type IStyleable.StyleKey => typeof(ContentDialog);

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = this;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var btn = e.NameScope.Find<Button>("PrimaryButton");
        btn?.Classes.Add("AccentButtonStyle");
    }

    public CustomPresetSelectorDialog()
    {
        ConversionPreset = new();
        InitializeComponent();
    }

    public CustomPresetSelectorDialog(ConversionPreset conversionPreset)
    {
        if (conversionPreset == null)
            throw new ArgumentNullException(nameof(conversionPreset));
        
        ConversionPreset = conversionPreset.Clone();
        UseCustomOptions = true;
        InitializeComponent();
    }

    public ConversionPreset ConversionPreset { get; }
    public bool UseCustomOptions { get; set; }
}