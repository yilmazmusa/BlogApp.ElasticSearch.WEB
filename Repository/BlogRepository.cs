using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
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




            #region   Aşağıdaki(75.satırda dinamik olmayan sorguyu) sorguyu Dinamik hale getirelim.Yani mesela searchRequest boş ya da null geldiğinde tüm datayı göstersin bize

            List<Action<QueryDescriptor<Blog>>> ListQuery = new(); // Boş bir List oluştuduk gelen isteğin null olup olmamasına göre aşağıdaki foksiyonlar bu boş Listi dolduracaklar.

            Action<QueryDescriptor<Blog>> matchAll = q => q.MatchAll(); //Boş geldiğinde tüm datayı çekicek sorgu

            Action<QueryDescriptor<Blog>> matchContent = q => q //Gelen istek datasına göre Content alanında/fieldında Match sorgusu yapacak olan sorgu
                                                         .Match(m => m
                                                         .Field(f => f.Content).Query(searchRequest));

            Action<QueryDescriptor<Blog>> matchBoolPrefixTitle = q => q //Gelen istek datasına göre Title alanında/fieldında MatchBoolPrefix sorgusu yapacak olan sorgu
                                                                .MatchBoolPrefix(mb => mb
                                                                .Field(f => f.Title).Query(searchRequest));

            Action<QueryDescriptor<Blog>> termTagLevelQuery = q => q
                                                              .Term(t => t
                                                              .Field(f => f.Tags).Value(searchRequest));


            //NOT: TermLevel Querylerde(yani tipi keyword olan datalarda arama yaparken) Field(f => f.Tags).Value(searchRequest) derken
            //FullText Querylerde(yani tipi text olan datalarda arama yaparken) .Field(f => f.Title).Query(searchRequest)); deriz
            //Yani Term Querylerde Field ten sonra Value derken, Fulltext Querylerde Field ten sonra Query deriz.

            if (string.IsNullOrEmpty(searchRequest))
            {
                ListQuery.Add(matchAll);
            }
            else
            {
                ListQuery.Add(matchContent);
                ListQuery.Add(matchBoolPrefixTitle);
                ListQuery.Add(termTagLevelQuery);

            }



            var response = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .Bool(b => b
            .Should(ListQuery.ToArray())))); // Normalde Sould un içerisine yazmıştık  Match ve MatchBoolPrefix sorgularını şimdi bu şekilde 


            foreach (var hit in response.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response.Documents.ToList();


            #endregion

            #region Burası Sorgunun Dinamişk olmayan hali. Dinamik hali yukarda

            var response1 = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(indexName)
            .Size(50)
            .Query(q => q
            .Bool(b => b
            .Should(s => s
            .Match(m => m
            .Field(f => f.Content).Query(searchRequest)),
            s => s.MatchBoolPrefix(p => p
            .Field(f => f.Title).Query(searchRequest))))));


            foreach (var hit in response1.Hits)
            {
                hit.Source.Id = hit.Id;
            }

            return response1.Documents.ToList();
        }

        #endregion



    }
}
