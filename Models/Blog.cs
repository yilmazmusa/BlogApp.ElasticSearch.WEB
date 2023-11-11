using System.Text.Json.Serialization;

namespace BlogApp.ElasticSearch.WEB.Models
{
    public class Blog
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = null!; //Burdaki null sadece design time ve compiler esnasında geçerli, run time da geçerli değil.

        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;


        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;


        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = null!;


        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }


        [JsonPropertyName("created")]
        public DateTime Created { get; set; }


    }
}   
