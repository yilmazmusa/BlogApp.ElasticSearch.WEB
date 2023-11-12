using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.AspNetCore.Http.HttpResults;

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


        //public  async Task<Blog?> SaveAsync(Blog newBlog)
        //{
        //    newBlog.Created = DateTime.Now;

        //    var response = await _elasticsearchClient.IndexAsync(newBlog, i => i.Index(indexName));

        //    if (!response.IsValidResponse) //IsValidResponse İŞLEMİN BAŞARILI BİR ŞEKİLDE YAPILIP YAPILMADIPINI DÖNER.
        //    {
        //        return null;
        //    }

        //    newBlog.Id = response.Id;

        //    return newBlog; 

        //}

        public async Task<Blog?> SaveAsync(Blog newBlog)
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

        public async Task<List<Blog>> SearchAsync(string searchRequest)
        {//hem title hemde contentine göre arama yapmak istiyorum mesela ben.
         //title => full textarama yapıcam
         //content =Z full text arama yapıcam
         //NOT:Should un içerisinde birden fazla sorgu girerken . ile yaparsak onu AND olarak algılar o yüzden OR yapmak i.in , kullanmamız gerekir Should sorguları içeririsinde.

            var response = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .Bool(b => b
            .Should(s => s
            .Match(m => m
            .Field(f => f.Content).Query(searchRequest)),
            s => s.MatchBoolPrefix(p => p
            .Field(f => f.Title).Query(searchRequest))))));


            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToList();
        }


    }
}
