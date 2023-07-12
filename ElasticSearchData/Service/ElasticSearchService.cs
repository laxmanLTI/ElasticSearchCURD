using ElasticSearchData.Entity;
using ElasticSearchData.Interface;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticSearchData.Service
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly ElasticClient? _client;
        public ElasticSearchService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = CreateInstance();
        }

        private ElasticClient? CreateInstance()
        {
            //get settings from JSON file 
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value;
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value;
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value;
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value;

            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            //basic authentication
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }

        async Task IElasticSearchService.CheckIndex(string IndexName)
        {
            var chkindex = await _client.Indices.ExistsAsync(IndexName);
            if (chkindex.Exists)
                return;
            var response = await _client.Indices.CreateAsync(IndexName,
                     ci => ci
                    .Index(IndexName)
                    .CitiesMapping()
                    .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                );
            return;
        }

        async Task IElasticSearchService.DeleteByIdDocument(string IndexName, Cities cities)
        {
            var response = await _client.CreateAsync(cities, q => q.Index(IndexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.DeleteAsync(DocumentPath<Cities>.Id(cities.Id).Index(IndexName));
            }
        }

        async Task IElasticSearchService.DeleteIndex(string IndexName)
        {
           //await _client.DeleteAsync(IndexName);
        }

        async Task<List<Cities>> IElasticSearchService.GetBulkDocument(string IndexName)
        {
            #region WildCard
            var response = await _client.SearchAsync<Cities>(s => s
                .From(0)
                .Take(10)
                .Index(IndexName)
                .Query(q => q
                .Bool(b => b
                .Should(m => m
                .Wildcard(w => w
                .Field("city")
                .Value("r*Ze")))))
            );
            #endregion

            #region Fuzzy
            //var response = await _client.SearchAsync<Cities>(s => s
            //    .Index(IndexName)
            //    .Query(q => q
            //    .Fuzzy(fz => fz.Field("city")
            //    .Value("anka").Fuzziness(Fuzziness.EditDistance(4))))

            //);

            //var response = await _client.SearchAsync<Cities>(s => s
            //    .Index(IndexName)
            //    .Query(q => q
            //    .Fuzzy(fz => fz.Field("city")
            //    .Value("rie").Transpositions(true)))
            //);
            #endregion

            #region AnalyzeWildcard
            //var response = await _client.SearchAsync<Cities>(s => s
            //    .Index(IndexName)
            //    .Query(q => q
            //    .QueryString(qs => qs
            //    .AnalyzeWildcard()
            //    .Query("*" + "iz" + "*")
            //    .Fields(fs => fs
            //    .Fields(f1 => f1.City))))
            //);
            #endregion

            #region Match
            //var response = await _client.SearchAsync<Cities>(s => s
            //   .Index(IndexName)
            //   .Size(10000)
            //   .Query(q => q
            //   .Match(m => m.Field("City").Query("pune")))
            //);
            #endregion

            #region Term
            //var response = await _client.SearchAsync<Cities>(s => s
            //     .Index(IndexName)
            //     .Size(10000)
            //     .Query(q => q.Term(f=>f.City, "mumbai"))
            //);
            #endregion

            #region MultiMatch 
            //var response = await _client.SearchAsync<Cities>(s => s
            //      .Index(IndexName)
            //      .Query(q => q
            //      .MultiMatch(mm=>mm
            //      .Fields(f=>f
            //      .Field(ff=>ff.Country)
            //      .Field(ff=>ff.Population))
            //      .Type(TextQueryType.PhrasePrefix)
            //      .Query("pn")
            //      .MaxExpansions(10)))

            //);
            #endregion

            #region MatchPhrasePrefix
            //var response = await _client.SearchAsync<Cities>(s => s
            //    .Index(IndexName)
            //    .Query(q => q.MatchPhrasePrefix(m=>m.Field(f=>f.City).Query("mum").MaxExpansions(10)))

            //);
            #endregion

            return response.Documents.ToList();
        }

        async Task<Cities> IElasticSearchService.GetDocument(string IndexName, string Id)
        {
            var response = await _client.GetAsync<Cities>(Id, q => q.Index(IndexName));
            return response.Source;
        }

        async Task IElasticSearchService.InsertBulkDocument(string IndexName, List<Cities> cities)
        {
            var x = await _client.IndexManyAsync(cities, index: IndexName);
        }

        async Task IElasticSearchService.InsertDocument(string IndexName, Cities cities)
        {
            var response = await _client.CreateAsync(cities, q => q.Index(IndexName));
            if (response.ApiCall?.HttpStatusCode == 409)
            {
                await _client.UpdateAsync<Cities>(cities.Id, a => a.Index(IndexName).Doc(cities));
            }
        }
    }
}
