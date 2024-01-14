using System;
using System.Runtime.InteropServices;
using OnionMedia.Core.Enums;
using OnionMedia.Core.Services;

namespace OnionMedia.Avalonia.Mac.Services;

sealed class TaskbarProgressService : ITaskbarProgressService
{
    private Type currentVmType;

    public ProgressBarState CurrentState { get; private set; }

    public void UpdateProgress(Type senderType, float progress)
    {
        if (senderType != currentVmType) return;

        //TODO: Implement progress
    }

    public void UpdateState(Type senderType, ProgressBarState state)
    {
        if (senderType != currentVmType) return;
        
        //TODO: Implement state
    }

    public void SetType(Type type)
    {
        if (currentVmType == type) return;
        currentVmType = type;
    }
}