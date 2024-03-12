using FitJarus.Models;
using FitJarus.Models.Dtos;

using Microsoft.AspNetCore.Components;

namespace FitJarus.Pages.Routines;

public partial class EditRoutine
{
    [Parameter]
    public string Id { get; set; }

    private Routine FormData = new();

    protected override async Task OnInitializedAsync()
    {
        if(string.IsNullOrEmpty(Id))
        {
            return;
        }

        var RoutineListRequest = await RequestHelper.Get<Routine>($"routines/{Id}.json");

        if(RoutineListRequest.Success)
        {
            FormData = RoutineListRequest.Content;
        }
    }

    private async void HandleSubmit()
    {
        if(string.IsNullOrEmpty(FormData.Name))
        {
            return;
        }

        if (string.IsNullOrEmpty(Id))
        {
            var request = await RequestHelper.Post<AddItemResponse>("routines.json", FormData);

            if (request.Success)
            {
                NavigationManager.NavigateTo("/routines");
            }
        } 
        else
        {
            var request = await RequestHelper.Put<AddItemResponse>($"routines/{Id}.json", FormData);

            if (request.Success)
            {
                NavigationManager.NavigateTo("/routines");
            }
        }
    }

    private void OnCancel() {
        NavigationManager.NavigateTo("/routines");
    }
}