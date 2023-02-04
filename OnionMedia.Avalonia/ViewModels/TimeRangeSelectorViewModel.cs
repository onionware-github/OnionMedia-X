using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using OnionMedia.Core.Models;

namespace OnionMedia.Avalonia.ViewModels;

public partial class TimeRangeSelectorViewModel : ObservableObject
{
    [ObservableProperty] [AlsoNotifyChangeFor(nameof(IsEditable))]
    private bool isReadOnly;

    // Dont allow to edit videos there are shorter than 2 seconds.
    public bool IsUneditable => TimeSpanGroup == null || TimeSpanGroup.Duration < TimeSpan.FromSeconds(2);
    public bool IsEditable => !IsReadOnly && !IsUneditable;


    public TimeSpanGroup TimeSpanGroup
    {
        get => timeSpanGroup ?? new(TimeSpan.Zero);
        set
        {
            if (!SetProperty(ref timeSpanGroup, value)) return;

            if (StartValue != value.StartTime / value.Duration)
                StartValue = value.StartTime / value.Duration;
            if (EndValue != value.EndTime / value.Duration)
                EndValue = value.EndTime / value.Duration;

            OnPropertyChanged(nameof(StartTimeString));
            OnPropertyChanged(nameof(EndTimeString));
            OnPropertyChanged(nameof(IsEditable));
        }
    }
    private TimeSpanGroup timeSpanGroup;

    public string StartTimeString
    {
        get => TimeSpanGroup.Duration switch
        {
            { Days: var days } when days > 0 => TimeSpanGroup.StartTime.ToString(@"dd\:hh\:mm\:ss"),
            { TotalHours: var tHours } when (int)tHours > 0 => TimeSpanGroup.StartTime.ToString(@"hh\:mm\:ss"),
            _ => TimeSpanGroup.StartTime.ToString(@"mm\:ss")
        };
        set
        {
            try
            {
                if (StartTimeString == value) return;
                var newTime = ParseTime(value, TimeSpanGroup.EndTime);
                if (newTime >= TimeSpanGroup.EndTime)
                    throw new ArgumentException();

                StartValue = newTime / TimeSpanGroup.Duration;
            }
            catch (ArgumentException)
            {
                OnPropertyChanged();
            }
        }
    }

    public string EndTimeString
    {
        get => TimeSpanGroup.Duration switch
        {
            { Days: var days } when days > 0 => TimeSpanGroup.EndTime.ToString(@"dd\:hh\:mm\:ss"),
            { TotalHours: var tHours } when (int)tHours > 0 => TimeSpanGroup.EndTime.ToString(@"hh\:mm\:ss"),
            _ => TimeSpanGroup.EndTime.ToString(@"mm\:ss")
        };
        set
        {
            try
            {
                if (EndTimeString == value) return;
                var newTime = ParseTime(value, TimeSpanGroup.Duration);
                if (newTime <= TimeSpanGroup.StartTime || newTime > TimeSpanGroup.Duration)
                    throw new ArgumentException();

                EndValue = newTime / TimeSpanGroup.Duration;
            }
            catch (ArgumentException)
            {
                OnPropertyChanged();
            }
        }
    }

    public double StartValue
    {
        get => startValue;
        set
        {
            if (!double.IsNormal(value)) value = 0;
            SetProperty(ref startValue, value);
            TimeSpanGroup.StartTime = TimeSpanGroup.Duration / 100 * (value * 100);
            OnPropertyChanged(nameof(StartTimeString));
        }
    }

    private double startValue;

    public double EndValue
    {
        get => endValue;
        set
        {
            if (!double.IsNormal(value) && value != 0) value = 1;
            SetProperty(ref endValue, value);
            TimeSpanGroup.EndTime = TimeSpanGroup.Duration / 100 * (value * 100);
            OnPropertyChanged(nameof(EndTimeString));
        }
    }

    private double endValue;

    static TimeSpan ParseTime(string timespan, TimeSpan maxTime)
    {
        Debug.WriteLine(timespan);
        if (string.IsNullOrWhiteSpace(timespan)) return TimeSpan.Zero;
        timespan = timespan.Trim();
        timespan = timespan.TrimEnd(':');
        timespan = Regex.Replace(timespan, ":{2,}", ":");

        //Throw an exception when the string contains invalid chars.
        if (timespan.Any(c => !char.IsNumber(c) && c != ':'))
            throw new ArgumentException("Input parameter has an invalid format.");

        //Remove overhead
        while (timespan.Any() && timespan[0] is '0' or ':')
            timespan = timespan.Remove(0, 1);

        if (!timespan.Any()) return TimeSpan.Zero;
        string[] timeUnits = timespan.Split(':');
        try
        {
            var result = timeUnits.Length switch
            {
                1 => new TimeSpan(0, 0, int.Parse(timespan)),
                2 => new TimeSpan(0, int.Parse(timeUnits[0]), int.Parse(timeUnits[1])),
                3 => new TimeSpan(int.Parse(timeUnits[0]), int.Parse(timeUnits[1]), int.Parse(timeUnits[2])),
                4 => new TimeSpan(int.Parse(timeUnits[0]), int.Parse(timeUnits[1]), int.Parse(timeUnits[2]),
                    int.Parse(timeUnits[3])),
                _ => throw new ArgumentException("Input parameter has an invalid format.")
            };

            if (result <= maxTime)
                return result;
            throw new ArgumentException("Input parameter has an invalid format.");
        }
        catch (OverflowException)
        {
            throw new ArgumentException("Input parameter has an invalid format.");
        }
    }
}