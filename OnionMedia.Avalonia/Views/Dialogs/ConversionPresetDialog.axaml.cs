using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.UI.Controls;
using OnionMedia.Core;
using OnionMedia.Core.Extensions;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.Views.Dialogs;

public sealed partial class ConversionPresetDialog : ContentDialog, IStyleable, INotifyPropertyChanged
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
    
    /// <summary> FOR AVALONIA XAML ONLY, DO NOT USE MANUALLY!</summary>
    public ConversionPresetDialog() {}

    public ConversionPresetDialog(IEnumerable<string> forbiddenNames = null)
    {
        ConversionPreset = new();
        title = "titleNewPreset".GetLocalized(RESOURCEPATH);
        primaryButtonText = "primarybuttoncreate".GetLocalized(RESOURCEPATH);
        this.forbiddenNames = forbiddenNames;
        InitializeComponent();
	}

    public ConversionPresetDialog(ConversionPreset conversionPreset, IEnumerable<string> forbiddenNames = null)
    {
        if (conversionPreset == null)
            throw new ArgumentNullException(nameof(conversionPreset));
        
        ConversionPreset = conversionPreset.Clone();
        title = "titleEditPreset".GetLocalized(RESOURCEPATH);
        primaryButtonText = "primarybuttonapply".GetLocalized(RESOURCEPATH);
        if (forbiddenNames != null)
            this.forbiddenNames = forbiddenNames.Where(n => n != conversionPreset.Name);

        if (GlobalResources.FFmpegCodecs.Videocodecs.Any(c => c.Name.Equals(conversionPreset.VideoCodec.Name)))
            ConversionPreset.VideoCodec =
                GlobalResources.FFmpegCodecs.Videocodecs.First(c => c.Name.Equals(conversionPreset.VideoCodec.Name));

        if (GlobalResources.FFmpegCodecs.Audiocodecs.Any(c => c.Name.Equals(conversionPreset.AudioCodec.Name)))
            ConversionPreset.AudioCodec =
                GlobalResources.FFmpegCodecs.Audiocodecs.First(c => c.Name.Equals(conversionPreset.AudioCodec.Name));

        if (ConversionPreset.VideoCodec.Encoders.Any(e => e.Equals(conversionPreset.VideoEncoder)))
            ConversionPreset.VideoEncoder =
                ConversionPreset.VideoCodec.Encoders.First(e => e.Equals(conversionPreset.VideoEncoder));

        if (ConversionPreset.AudioCodec.Encoders.Any(e => e.Equals(conversionPreset.AudioEncoder)))
            ConversionPreset.AudioEncoder =
                ConversionPreset.AudioCodec.Encoders.First(e => e.Equals(conversionPreset.AudioEncoder));
        InitializeComponent();
	}

    public string PresetName
    {
        get => ConversionPreset?.Name;
        set
        {
            if (ConversionPreset.Name == value) return;
            ConversionPreset.Name = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(NameAlreadyInUse));
            OnPropertyChanged(nameof(NameIsEmpty));
            OnPropertyChanged(nameof(ValidName));
        }
    }

    public bool NameAlreadyInUse => forbiddenNames?.Contains(PresetName) is true;
    public bool NameIsEmpty => string.IsNullOrWhiteSpace(PresetName);
    public bool ValidName => !NameIsEmpty && !NameAlreadyInUse;

    public ConversionPreset ConversionPreset { get; private set; }

    public string title { get; private set; }
    public string primaryButtonText { get; private set; }
    public IEnumerable<string> forbiddenNames  { get; private set; }
    const string RESOURCEPATH = "ConversionPresetDialog";
    
    #region NotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string propname = "") => PropertyChanged?.Invoke(this, new(propname));

    #endregion
}