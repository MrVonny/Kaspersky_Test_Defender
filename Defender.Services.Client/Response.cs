using Newtonsoft.Json;

namespace Defender.Services.Client;

public class Response<T>
{
    [JsonProperty("success")]
    public bool Success { get; set; }
    [JsonProperty("errors")]
    public List<string> Errors { get; set; } = new();
    [JsonProperty("data")]
    public T Data { get; set; }
}