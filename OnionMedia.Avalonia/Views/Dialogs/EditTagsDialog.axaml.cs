using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.Views.Dialogs;

public partial class EditTagsDialog : ContentDialog, IStyleable
{
    Type IStyleable.StyleKey => typeof(ContentDialog);
    
    public EditTagsDialog()
    {
        FileTags = new();
        InitializeComponent();
    }
    
    public EditTagsDialog(FileTags fileTags)
    {
        FileTags = fileTags.Clone() ?? throw new ArgumentNullException(nameof(fileTags));
        InitializeComponent();
    }
    public FileTags FileTags { get; }

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
}