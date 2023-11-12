using BlogApp.ElasticSearch.WEB.Models;

namespace BlogApp.ElasticSearch.WEB.Interfaces
{
    public interface IBlogRepository
    {
        public  Task<Blog> SaveAsync(Blog blog);
    }
}
