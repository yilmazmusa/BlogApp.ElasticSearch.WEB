using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.ElasticSearch.WEB.ViewModels
{
    public class BlogSearchViewModel
    {
        public string Id { get; set; } = null!; //Burdaki null sadece design time ve compiler esnasında geçerli, run time da geçerli değil.

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? Tags { get; set; } 

        public string UserId { get; set; } = null!;  //Tipi Guid ti string yaptık çünkü kullanıcıya datayı gösterirken çok bi önemi yok

        public string Created { get; set; } = null!; //Tipi DateTime dı string yaptık çünkü kullanıcıya datayı gösterirken çok bi önemi yok
    }
}
