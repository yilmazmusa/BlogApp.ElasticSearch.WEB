using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.Models;
using Elastic.Clients.Elasticsearch;

namespace BlogApp.ElasticSearch.WEB.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string? indexName = "blog";

        public BlogRepository(ElasticsearchClient client)
        {
            _elasticsearchClient = client;
        }


        public  async Task<Blog?> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;

            var response = await _elasticsearchClient.IndexAsync(newBlog, i => i.Index(indexName));

            if (!response.IsValidResponse) //IsValidResponse İŞLEMİN BAŞARILI BİR ŞEKİLDE YAPILIP YAPILMADIPINI DÖNER.
            {
                return null;
            }

            newBlog.Id = response.Id;
            
            return newBlog; 

        }

        
    }
}
