using Microsoft.AspNetCore.Components;

namespace UI.Components;

/// <summary>
/// A base class for components that should not render when an event is triggered in their template.
/// </summary>
public abstract class IgnoreUpdateComponentBase : ComponentBase, IHandleEvent
{
    public Task HandleEventAsync(EventCallbackWorkItem item, object? arg) => item.InvokeAsync(arg);
}