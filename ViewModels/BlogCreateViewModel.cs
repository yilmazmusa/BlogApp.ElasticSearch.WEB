using System.ComponentModel.DataAnnotations;

namespace BlogApp.ElasticSearch.WEB.ViewModels
{
    public class BlogCreateViewModel 
    {
        [Display(Name = "Blog Title")] //Bunu  Save.html sayfasında label'ın içerisinde girdiğimiz display değeri görünen değer yani
        [Required]
        public string Title { get; set; } = null!;

        [Display(Name ="Blog Content")]
        [Required]
        public string Content { get; set; } = null!;

        [Display(Name = "Blog Tags")]
        public string Tags { get; set; } = null!; //ASLINDA EŞİTLİĞİN SOL TARAFINDA TİPİ BELLİSYSE SAĞ TARAFINDA BELLİ ETMEYE GEREK YOK sadece new(); desekte olurdu.
      
    }
}


//public List<string> Tags { get; set; } = new List<string>(); //ASLINDA EŞİTLİĞİN SOL TARAFINDA TİPİ BELLİSYSE SAĞ TARAFINDA BELLİ ETMEYE GEREK YOK sadece new(); desekte olurdu.
