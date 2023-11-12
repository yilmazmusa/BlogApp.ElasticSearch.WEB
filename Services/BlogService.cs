using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.Models;
using BlogApp.ElasticSearch.WEB.Repository;
using BlogApp.ElasticSearch.WEB.ViewModels;

namespace BlogApp.ElasticSearch.WEB.Services
{
    public class BlogService : IBlogService
    {
        //Depedency Inversion +  Inversion Of Control(IoC) = Dependency Injection (DP)
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            Blog newBlog = new Blog();

            newBlog.Title = model.Title;
            newBlog.Content = model.Content;
            newBlog.Tags = model.Tags.Split(","); // Gelen datayı , göre parçaladık ve array tipinde olan newBlog.Tags a atadık.
            newBlog.UserId = Guid.NewGuid();

            var isCreatedBlog = await _blogRepository.SaveAsync(newBlog);

            return isCreatedBlog != null; //Eğer Repository'deki  SaveAsync metodunda  Blog başarılı bir şekilde
                                          //Index(blog) eklenmişse isCreatedBlog'a null dönmez,
                                          //isCreatedBlog null dönmezse burasıda(Service), Controller tarafına true döner,
                                          //eğer tersi olursa false döner Controllera
        }


        public async Task<List<BlogSearchViewModel>> SearchAsync(string searchRequest)
        {
            var blogList = await _blogRepository.SearchAsync(searchRequest);

            if (blogList == null )
            {
                return null;
            }

            return blogList.Select(b => new BlogSearchViewModel()
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                Created = b.Created.ToShortDateString(),
                Tags = String.Join(",", b.Tags),
                UserId = b.UserId.ToString()

            }).ToList(); ;
        }

    }
}
