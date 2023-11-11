using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.Models;
using BlogApp.ElasticSearch.WEB.Repository;
using BlogApp.ElasticSearch.WEB.ViewModels;

namespace BlogApp.ElasticSearch.WEB.Services
{
    public class BlogService : IBlogService
    {
        //Depedency Inversion +  Inversion Of Control(IoC) = Dependency Injection (DP)
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            Blog newBlog = new Blog();

            newBlog.Title = model.Title;
            newBlog.Content = model.Content;
            newBlog.Tags = model.Tags.ToArray();
            newBlog.UserId = Guid.NewGuid();

            var isCreatedBlog = await _blogRepository.SaveAsync(newBlog);

            return isCreatedBlog != null; //Eğer Repository'deki  SaveAsync metodunda  Blog başarılı bir şekilde
                                          //Index(blog) eklenmişse isCreatedBlog'a null dönmez,
                                          //isCreatedBlog null dönmezse burasıda(Service), Controller tarafına true döner,
                                          //eğer tersi olursa false döner Controllera
        }

       
    }
}
