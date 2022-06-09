using Newtonsoft.Json;

namespace Defender.Services.Api.ViewModels;

public class CreateDefenderTaskViewModel
{
    [JsonProperty("directory")]
    public string Directory { get; set; }
}