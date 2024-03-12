using FitJarus.Helpers;
using FitJarus.Models.Dtos;

using Microsoft.AspNetCore.Components;

namespace FitJarus.Pages.Routines
{
    public partial class Routines()
    {
        public Dictionary<string, Routine> RoutineList { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await GetRoutineList();
        }

        private async Task GetRoutineList()
        {
            var RoutineListRequest = await RequestHelper.Get<Dictionary<string, Routine>>("routines.json");
            RoutineList = RoutineListRequest.Content;

            StateHasChanged();
        }

        private async void Delete(string id)
        {
            var request = await RequestHelper.Delete($"routines/{id}.json");

            if(request.Success)
            {
                await GetRoutineList();
            }
        }

        private void GoToEditPage(string id = "")
        {
            NavigationManager.NavigateTo($"/routines/edit/{id}");
        }
    }
}