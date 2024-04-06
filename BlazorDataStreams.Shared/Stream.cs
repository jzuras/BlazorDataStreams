namespace BlazorDataStreams.Shared;

public class Stream
{
    public string Name { get; set; } = string.Empty;
    public int? MinValue { get; set; } = null;
    public int? MaxValue { get; set; } = null;
}

public class StreamViewModel
{
    public Stream EmptyStream { get; set; } = new Stream();
    public IEnumerable<Stream> StreamList { get; set; } = new List<Stream>();
    public string CurrentlySelectedStream { get; set; } = "[Click 'Display' on a Stream above to get started.]";
    public bool IsStreamSelected { get; set; } = false;
    public int IdAssignedByApi { get; set; } = 0;
    public int ApiType { get; set; } = 0;
    public int Version { get; set; } = 0;
}
