using Microsoft.AspNetCore.Components;

namespace UI.Components;

public class TabViewTab : ChildComponentBase<TabViewTab>
{
    [Parameter]
    public RenderFragment? PagerContent { get; set; }
}
