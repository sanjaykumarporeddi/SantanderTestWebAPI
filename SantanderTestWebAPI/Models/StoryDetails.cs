using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class StoryDetails
{
    [JsonProperty("by")]
    public string PostedBy { get; set; }

    [JsonProperty("descendants")]
    public int Descendants { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("kids")]
    public List<int> Kids { get; set; }

    [JsonProperty("score")]
    public int Score { get; set; }

    [JsonProperty("time")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Time { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}
