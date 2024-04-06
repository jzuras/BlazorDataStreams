using Microsoft.AspNetCore.Components;

namespace BlazorDataStreams.Shared;

public partial class DataDisplay
{
    // note to self - add localhost as an allowed origin via Azure Portal, then remove when finished.

    private StreamViewModel? Model { get; set; }

    [Parameter, EditorRequired]
    public string ClientOrServer { get; set; } = "";

    // Injection can be done here or in razor code like MessageService
    [Inject]
    protected IDataService? DataService { get; set; }

    private bool IsTimerRunning { get; set; } = false;
    private string ButtonText { get; set; } = "Start Timer";
    private Timer? Timer { get; set; }
    private int ElapsedTime { get; set; } = 0;
    private int UpdateInterval { get; set; } = 10;
    private string ApiResponse { get; set; } = "";
    private int ApiCallsCount { get; set; }
    private StreamViewModel ViewModel { get; set; } = default!;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        /* Save for future reference:
         *
         * This method can be used to do something based on the value of a parameter
         * which was set by the parent of this component via razor code.
         * ("rendermode" was not in the list, but "CleintOrServer" was.)
         *
         * The SetParametersAsync is only called the first time a Blazor component is rendered.
         * 
         * However, the OnParametersSet and OnParametersSetAsync lifecycle methods are triggered 
         * whenever the parent component re-renders, providing a different value for a child component’s parameters.
         * */

        //if (parameters.TryGetValue<string>("rendermode", out var value))
        //{
        //    if (value is null)
        //    {
        //        TitleLength = 0;
        //    }
        //    else
        //    {
        //        TitleLength = Title.Length;
        //    }
        //}

        await base.SetParametersAsync(parameters);
    }

    protected override async Task OnInitializedAsync()
    {
        await this.MessageService.SubscribeAsync<Stream>(HandleDisplay);
    }

    private async Task RunRapidUpdate()
    {
        // Count how many calls can be made in 3 seconds.

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int callCount = 0;
        this.IsTimerRunning = true;

        while (stopwatch.Elapsed.TotalSeconds < 3)
        {
            await this.DataService!.GetValueAsync(
                this.ViewModel.IdAssignedByApi, (IDataService.ApiType)this.ViewModel.ApiType, this.ViewModel.Version);

            callCount++;
        }
        this.ApiCallsCount = callCount;
        this.IsTimerRunning = false;
    }

    private async Task HandleDisplay(Stream item)
    {
        string name = item.Name;
        var apiType = this.DataService!.GetApiType(name);
        var version = this.DataService.GetVersion(name);
        var id = await this.DataService.InitializeAsync(item);

        this.ViewModel = new StreamViewModel();
        this.ViewModel.ApiType = (int)apiType;
        this.ViewModel.Version = version;
        this.ViewModel.IdAssignedByApi = id;
        this.ViewModel.CurrentlySelectedStream = name;
        this.ViewModel.IsStreamSelected = true;
        this.Model = this.ViewModel;
        StateHasChanged();
    }

    private void ToggleTimer()
    {
        if (this.IsTimerRunning)
        {
            this.StopTimer();
        }
        else
        {
            this.StartTimer();
        }
    }

    private void StartTimer()
    {
        this.IsTimerRunning = true;
        this.ButtonText = "Stop Timer";
        this.ElapsedTime = 0;
        this.Timer = new Timer(state => Task.Run(() => TimerCallback(state)), null, 0, this.UpdateInterval * 100);
    }

    private void StopTimer()
    {
        this.IsTimerRunning = false;
        this.ButtonText = "Start Timer";
        this.Timer?.Dispose();
    }

    private async Task TimerCallback(object? state)
    {
        if (this.ElapsedTime >= 60)
        {
            this.StopTimer();
        }
        else
        {
            var response = await this.DataService!.GetValueAsync(
                this.ViewModel.IdAssignedByApi, (IDataService.ApiType)this.ViewModel.ApiType, this.ViewModel.Version);
            this.ApiResponse = response;

            this.ElapsedTime += this.UpdateInterval;
        }

        // tell UI to update because this was not called by the UI
        await InvokeAsync(StateHasChanged);
    }

    private void UpdateTimerInterval(ChangeEventArgs e)
    {
        this.UpdateInterval = Convert.ToInt32(e.Value);
        if (this.IsTimerRunning)
        {
            // If the timer is running, update the interval
            this.Timer?.Change(0, this.UpdateInterval * 100);
        }
    }
}
