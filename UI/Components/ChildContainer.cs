using Microsoft.AspNetCore.Components;
using System.Collections;

namespace UI.Components;

/// <summary>
/// A container class for Blazor child components.
/// </summary>
/// <typeparam name="T">The type of child component.</typeparam>
public class ChildContainer<T> : IEnumerable<T>
    where T : ComponentBase
{
    private Action<T> onAddListener;
    private Action<T> onRemoveListener;
    private List<T> children = new();

    public ChildContainer(Action<T> onAddListener, Action<T> onRemoveListener)
    {
        this.onAddListener = onAddListener;
        this.onRemoveListener = onRemoveListener;
    }

    public int Count => children.Count;

    public T this[int index] => children[index];

    public void Add(T child)
    {
        children.Add(child);
        onAddListener(child);
    }

    public void Remove(T child)
    {
        children.Remove(child);
        onRemoveListener(child);
    }

    // IEnumerable interface
    public IEnumerator<T> GetEnumerator() => children.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => children.GetEnumerator();
}
