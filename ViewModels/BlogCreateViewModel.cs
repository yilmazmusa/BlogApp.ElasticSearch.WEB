using System.ComponentModel.DataAnnotations;

namespace BlogApp.ElasticSearch.WEB.ViewModels
{
    public class BlogCreateViewModel 
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public List<string> Tags { get; set; } = new List<string>(); //ASLINDA EŞİTLİĞİN SOL TARAFINDA TİPİ BELLİSYSE SAĞ TARAFINDA BELLİ ETMEYE GEREK YOK sadece new(); desekte olurdu.
      
    }
}
