using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace UI.Components;

public class ChildComponentBase<T> : ComponentBase, IDisposable
    where T : ComponentBase
{
    private bool isAdded = false;

    [CascadingParameter]
    public ChildContainer<T>? Container { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public void Dispose()
    {
        if (isAdded)
        {
            if (Container == null)
            {
                throw new Exception($"No parent container is provided. Please provide a cascading value of {nameof(ChildContainer<T>)}<{typeof(T).Name}>");
            }
            Container.Remove((T)(object)this);
            isAdded = false;
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (typeof(T) != this.GetType())
        {
            throw new Exception($"Invalid generic parameter. The generic parameter of {nameof(ChildContainer<T>)} must be the type deriving from {nameof(ChildContainer<T>)}");
        }

        if (isAdded != false)
        {
            throw new Exception("The component is already added to a parent. Can not add one component to two or more parents.");
        }

        if (Container == null)
        {
            throw new Exception($"No parent container is provided. Please provide a cascading value of {nameof(ChildContainer<T>)}<{typeof(T).Name}>");
        }

        Container.Add((T)(object)this);

        isAdded = true;
    }

    /// <summary>
    /// Template childs should not render content, only carry them via <see cref="ChildContent"/>.
    /// </summary>
    /// <param name="builder">The tree builder.</param>
    protected sealed override void BuildRenderTree(RenderTreeBuilder builder)
    {
    }
}
