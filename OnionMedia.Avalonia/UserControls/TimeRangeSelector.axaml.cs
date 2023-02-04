using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using OnionMedia.Avalonia.ViewModels;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.UserControls;

public sealed partial class TimeRangeSelector : UserControl
{
    public TimeRangeSelector()
    {
        InitializeComponent();
        DataContext = new TimeRangeSelectorViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public TimeSpanGroup TimeSpanGroup
    {
        get => ((TimeRangeSelectorViewModel)DataContext).TimeSpanGroup;
        set => ((TimeRangeSelectorViewModel)DataContext).TimeSpanGroup = value;
    }

    public bool IsReadOnly
    {
        get => ((TimeRangeSelectorViewModel)DataContext).IsReadOnly;
        set => ((TimeRangeSelectorViewModel)DataContext).IsReadOnly = value;
    }

    //Remove focus from the textbox on Enter.
    private void TextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key is Key.Enter)
            this.Focus();
    }
}