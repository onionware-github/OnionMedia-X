using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.DependencyInjection;
using FFMpegCore;
using FluentAvalonia.Styling;
using OnionMedia.Avalonia.ViewModels;
using OnionMedia.Avalonia.Views;
using OnionMedia.Core;
using OnionMedia.Core.Models;
using OnionMedia.Core.Services;
using OnionMedia.Services;
using Color = Avalonia.Media.Color;

namespace OnionMedia.Avalonia
{
    public sealed partial class App : Application
    {
        public static MainWindow MainWindow { get; private set; }
        public static readonly SoftwareVersion Version = new(2, 0, 0, 0);

        internal static readonly ServiceProvider DefaultServiceProvider = new();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                };
                MainWindow = (MainWindow)desktop.MainWindow;
                if (DefaultServiceProvider.GetService<IWindowClosingService>() is WindowClosingService closingService)
                    closingService.RegisterWindow(MainWindow);

                AccentColorChanged();
                UseAccentColorChanged();
                RequestedThemeVariant = CoreToAvaloniaTheme(AppSettings.Instance.SelectedTheme);
                MainWindow.FlowDirection = CoreToAvaloniaFlowDirection(AppSettings.Instance.AppFlowDirection);
                AppSettings.Instance.PropertyChanged += AppSettingsChanged;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void AppSettingsChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AppSettings.CustomAccentColorHex):
                    AccentColorChanged();
                    return;
                
                case nameof(AppSettings.UseCustomAccentColor):
                    UseAccentColorChanged();
                    return;
                
                case nameof(AppSettings.SelectedTheme):
                    RequestedThemeVariant = CoreToAvaloniaTheme(AppSettings.Instance.SelectedTheme);
                    return;
                
                case nameof(AppSettings.AppFlowDirection):
                    MainWindow.FlowDirection = CoreToAvaloniaFlowDirection(AppSettings.Instance.AppFlowDirection);
                    return;
            }
        }

        private void UseAccentColorChanged()
        {
            if (AppSettings.Instance.UseCustomAccentColor)
            {
                try
                {
                    var color = ColorTranslator.FromHtml(AppSettings.Instance.CustomAccentColorHex);
                    AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>().CustomAccentColor = Color.FromArgb(color.A, color.R, color.G, color.B);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
                return;
            }
            AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>().CustomAccentColor = null;
        }

        private void AccentColorChanged()
        {
            if (!AppSettings.Instance.UseCustomAccentColor) return;
            try
            {
                var color = ColorTranslator.FromHtml(AppSettings.Instance.CustomAccentColorHex);
                AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>().CustomAccentColor =
                    Color.FromArgb(color.A, color.R, color.G, color.B);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        ThemeVariant CoreToAvaloniaTheme(ThemeType theme) => theme switch
        {
            ThemeType.Light => ThemeVariant.Light,
            ThemeType.Dark => ThemeVariant.Dark,
            _=> ThemeVariant.Default
        };

        FlowDirection CoreToAvaloniaFlowDirection(AppFlowDirection direction) => direction switch
        {
            AppFlowDirection.LeftToRight => FlowDirection.LeftToRight,
            AppFlowDirection.RightToLeft => FlowDirection.RightToLeft,
            _=> FlowDirection.LeftToRight
        };
    }
}