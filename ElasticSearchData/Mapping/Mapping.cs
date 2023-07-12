using ElasticSearchData.Entity;
using Nest;

namespace ElasticSearchData
{
    public static class Mapping
    {
        public static CreateIndexDescriptor CitiesMapping(this CreateIndexDescriptor createIndexDescriptor)
        {
            return createIndexDescriptor.Map<Cities>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.Id))
                .Text(t => t.Name(n => n.City))
                .Text(t => t.Name(n => n.Region))
                .Text(t => t.Name(n => n.Phone))
                .Number(t => t.Name(n => n.Population))
                .Text(t => t.Name(n => n.Country))
                .Text(t => t.Name(n => n.PostalCode)))
            );
        }
    }
}
