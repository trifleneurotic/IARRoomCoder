using Microsoft.AspNetCore.Components;

namespace RoomCoder.Pages.Shared;

public partial class ConfirmPopup
{
    public bool Show { get; set; }

    [Parameter]
    public string Title { get; set; } = "Delete";

    [Parameter]
    public string Class { get; set; } = "btn btn-danger";

    [Parameter]
    public string YesCssClass { get; set; } = "btn btn-success";

    [Parameter]
    public string NoCssClass { get; set; } = "btn btn-warning";

    [Parameter]
    public string Message { get; set; } = "Are you sure?";

    [Parameter]
    public EventCallback<bool> ConfirmedChanged { get; set; }

    [Parameter]
    public RenderFragment ButtonContent { get; set; }

    [Parameter]
    public RenderFragment MessageTemplate { get; set; }

    public async Task Confirmation(bool value)
    {
        Show = false;
        if (!value)
        {
            return;
        }
        await ConfirmedChanged.InvokeAsync(value);
    }

    public void ShowPop()
    {
        Show = true;
    }
}
