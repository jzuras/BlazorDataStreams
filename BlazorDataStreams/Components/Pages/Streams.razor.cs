using BlazorDataStreams.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace BlazorDataStreams.Components.Pages;

public partial class Streams
{
    private List<BlazorDataStreams.Shared.Stream>? streamList;
    private StreamViewModel model = new StreamViewModel();

    protected override async Task OnInitializedAsync()
    {
        // Simulate asynchronous loading to demonstrate streaming rendering
        //await Task.Delay(500);

        // above???

        streamList = new List<Shared.Stream>();
        streamList.Add(new Shared.Stream { Name = "RestAPI (default version)", MinValue = 12, MaxValue = 50 });
        streamList.Add(new Shared.Stream { Name = "RestAPI-v1", MinValue = 22, MaxValue = 55 });
        streamList.Add(new Shared.Stream { Name = "RestAPI-v2", MinValue = 32, MaxValue = 60 });
    }

    private async Task DisplayHandler(Shared.Stream stream)
    {
        // todo - get model for real

        this.model = new StreamViewModel();
        this.model.CurrentlySelectedStream = stream.Name;

        base.StateHasChanged();
    }
}
