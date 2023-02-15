using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using OnionMedia.Avalonia.ViewModels;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.UserControls;

public partial class TimeRangeSelector : UserControl
{
    private TimeRangeSelectorViewModel ViewModel { get; } = new() {TimeSpanGroup = new(TimeSpan.Zero)};
    public TimeRangeSelector()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    public void UpdateTimeSpanGroup(TimeSpanGroup times)
    {
	    ((TimeRangeSelectorViewModel)DataContext).TimeSpanGroup = times;
    }

    public void UpdateIsReadOnly(bool isReadOnly)
    {
	    ((TimeRangeSelectorViewModel)DataContext).IsReadOnly = isReadOnly;
    }

	private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public static readonly StyledProperty<TimeSpanGroup> TimeSpanGroupProperty =
        AvaloniaProperty.Register<TimeRangeSelector, TimeSpanGroup>(nameof(TimeSpanGroup));
    
    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<TimeRangeSelector, bool>(nameof(IsReadOnly));

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