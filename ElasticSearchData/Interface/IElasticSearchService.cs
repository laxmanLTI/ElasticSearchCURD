using ElasticSearchData.Entity;
using ElasticSearchData.Interface;

namespace ElasticSearchData.Interface
{
    public interface IElasticSearchService
    {
        Task CheckIndex(string IndexName);
        Task InsertDocument(string IndexName, Cities cities);
        Task DeleteIndex(string IndexName);
        Task DeleteByIdDocument(string IndexName, Cities cities);
        Task InsertBulkDocument(string  IndexName, List<Cities> cities);
        Task<Cities> GetDocument(string IndexName, string Id);
        Task<List<Cities>> GetBulkDocument(string IndexName);
    }
}
