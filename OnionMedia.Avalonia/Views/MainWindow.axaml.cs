using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Media;
using OnionMedia.Avalonia.ViewModels;

namespace OnionMedia.Avalonia.Views
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if WINDOWS
            Application.Current.ActualThemeVariantChanged += ApplicationActualThemeVariantChanged;
#endif
        }

#if DEBUG
        public bool Debug => true;
#else
        public bool Debug => false;
#endif

#if WINDOWS
        public bool IsWindows11 => Environment.OSVersion.Version.Build >= 22000;

        private void TryEnableMicaEffect()
        {
            // The background colors for the Mica brush are still based around SolidBackgroundFillColorBase resource
            // BUT since we can't control the actual Mica brush color, we have to use the window background to create
            // the same effect. However, we can't use SolidBackgroundFillColorBase directly since its opaque, and if
            // we set the opacity the color become lighter than we want. So we take the normal color, darken it and 
            // apply the opacity until we get the roughly the correct color
            // NOTE that the effect still doesn't look right, but it suffices. Ideally we need access to the Mica
            // CompositionBrush to properly change the color but I don't know if we can do that or not
            if (ActualThemeVariant == ThemeVariant.Dark)
            {
                var color = this.TryFindResource("SolidBackgroundFillColorBase",
                    ThemeVariant.Dark, out var value)
                    ? (Color2)(Color)value
                    : new Color2(24, 24, 24);

                color = color.LightenPercent(0);

                Background = new ImmutableSolidColorBrush(color, 0.8);
            }
            else if (ActualThemeVariant == ThemeVariant.Light)
            {
                // Similar effect here
                var color = this.TryFindResource("SolidBackgroundFillColorBase",
                    ThemeVariant.Light, out var value)
                    ? (Color2)(Color)value
                    : new Color2(243, 243, 243);

                color = color.LightenPercent(0.8f);

                Background = new ImmutableSolidColorBrush(color, 0.8);
            }
        }

        private void ApplicationActualThemeVariantChanged(object sender, EventArgs e)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO: add Windows version to CoreWindow
                if (IsWindows11 && ActualThemeVariant != FluentAvaloniaTheme.HighContrastTheme)
                {
                    TryEnableMicaEffect();
                }
                else if (ActualThemeVariant != FluentAvaloniaTheme.HighContrastTheme)
                {
                    // Clear the local value here, and let the normal styles take over for HighContrast theme
                    SetValue(BackgroundProperty, AvaloniaProperty.UnsetValue);
                }
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            var thm = ActualThemeVariant;

            // Enable Mica on Windows 11
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO: add Windows version to CoreWindow
                if (IsWindows11 && thm != FluentAvaloniaTheme.HighContrastTheme)
                {
                    TransparencyBackgroundFallback = Brushes.Transparent;
                    TransparencyLevelHint = WindowTransparencyLevel.Mica;

                    TryEnableMicaEffect();
                }
            }
        }
#endif
    }
}