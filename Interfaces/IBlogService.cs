using BlogApp.ElasticSearch.WEB.ViewModels;

namespace BlogApp.ElasticSearch.WEB.Interfaces
{
    public interface IBlogService
    {
        public  Task<bool> SaveAsync(BlogCreateViewModel model);
    }
}
