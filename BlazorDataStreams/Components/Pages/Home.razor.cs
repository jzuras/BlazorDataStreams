using BlazorDataStreams.Shared;

namespace BlazorDataStreams.Components.Pages;

public partial class Home
{
    private List<BlazorDataStreams.Shared.Stream>? streamList;
    private StreamViewModel model = new StreamViewModel();

    protected override Task OnInitializedAsync()
    {
        // Simulate asynchronous loading to demonstrate streaming rendering
        //await Task.Delay(500);

        // above???

        streamList = new List<Shared.Stream>();
        streamList.Add(new Shared.Stream { Name = "RestAPI", MinValue = 12, MaxValue = 50 });

        return Task.CompletedTask;
    }
}
