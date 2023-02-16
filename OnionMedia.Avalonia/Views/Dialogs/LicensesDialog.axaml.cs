using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using OnionMedia.Avalonia.States;
using OnionMedia.Avalonia.Views.Dialogs.LicenseDialogPages;

namespace OnionMedia.Avalonia.Views.Dialogs;

public partial class LicensesDialog : ContentDialog, IStyleable
{
    Type IStyleable.StyleKey => typeof(ContentDialog);
    private Frame NavFrame => this.FindControl<Frame>("licenseNavFrame");
    private LicenseDialogState state;
    
    public LicensesDialog()
    {
        InitializeComponent();
        state = App.DefaultServiceProvider.GetService<LicenseDialogState>();
        state.PropertyChanged += (o, e) =>
        {
            if (e.PropertyName == nameof(state.SelectedLibrary))
                NavFrame.Navigate(typeof(LicensesInfoPage), null,
                    new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        };
        NavFrame.Navigate(typeof(LicensesListPage));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void licenseNavFrame_Navigated(object sender, NavigationEventArgs e)
    {
        var back_btn = this.FindControl<Button>("backButton");
        if (back_btn is null) return;
        back_btn.IsVisible = e.SourcePageType == typeof(LicensesInfoPage);
    }

    private void backButton_Click(object sender, RoutedEventArgs e)
    {
        NavFrame?.GoBack();
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var btn = e.NameScope.Find<Button>("PrimaryButton");
        btn?.Classes.Add("AccentButtonStyle");
    }
}