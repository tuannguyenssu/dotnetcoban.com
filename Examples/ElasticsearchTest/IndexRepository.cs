using Nest;
using System;
using System.Threading.Tasks;

namespace ElasticsearchTest
{
    public class IndexRepository
    {
        private readonly ElasticClient _client;

        public IndexRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<bool> CreateIndexWithTemplateAsync(string indexTemplateName, string indexPatterns, string indexName, string aliasName)
        {
            if (_client.Indices.TemplateExists(indexTemplateName).Exists) return true;
            var createTemplateResponse = await _client.Indices.PutTemplateAsync(indexTemplateName, c => c.
                Version(1).
                IndexPatterns(indexPatterns).
                Settings(s => s.
                    NumberOfShards(1).
                    NumberOfReplicas(0).
                    RefreshInterval(new Time(TimeSpan.FromSeconds(1))).AutoExpandReplicas(AutoExpandReplicas.Disabled).
                    Setting("search.idle.after", "1s").
                    Setting("search.slowlog.level", "warn").
                    Setting("search.slowlog.threshold.query.warn", "10s").
                    Setting("search.slowlog.threshold.fetch.warn", "1s")).
                Map<DeviceLog>(m => m.Dynamic(false).
                    Properties(p => p.
                        Number(t => t.Name(s => s.CustomerId).Type(NumberType.Long)).
                        Date(t => t.Name(s => s.CreatedTime)).
                        Keyword(t => t.Name(s => s.VehiclePlate).Norms(false)).
                        Keyword(t => t.Name(s => s.Message).Index(false).Norms(false).DocValues(false))).
                    SourceField(s => s.Enabled(true))).
                Aliases(a => a.Alias(aliasName)));
            if (!createTemplateResponse.IsValid) return false;

            if (_client.Indices.Exists(indexName).Exists) return true;
            var response = await _client.Indices.CreateAsync(indexName,
                c => c.
                    Aliases(a => a.Alias(aliasName, s => s.IsWriteIndex(true))));
            return response.IsValid;
        }

        public async Task<bool> CreateIndexAsync(string indexName, string aliasName)
        {
            if (_client.Indices.Exists(indexName).Exists) return true;
            var response = await _client.Indices.CreateAsync(indexName, c => c.
                Settings(s => s.
                    NumberOfShards(1).
                    NumberOfReplicas(0).
                    RefreshInterval(new Time(TimeSpan.FromMinutes(2))).AutoExpandReplicas(AutoExpandReplicas.Disabled).
                    Setting("search.idle.after", "300s").
                    Setting("search.slowlog.level", "warn").
                    Setting("search.slowlog.threshold.query.warn", "10s").
                    Setting("search.slowlog.threshold.fetch.warn", "1s")).
                Map<DeviceLog>(m => m.Dynamic(false).
                    Properties(p => p.
                        Number(t => t.Name(s => s.CustomerId).Type(NumberType.Long)).
                        Date(t => t.Name(s => s.CreatedTime)).
                        Keyword(t => t.Name(s => s.VehiclePlate).Norms(false)).
                        Keyword(t => t.Name(s => s.Message).Index(false).Norms(false).DocValues(false))).
                    SourceField(s => s.Enabled(true))).
                Aliases(a => a.Alias(aliasName)));

            return response.IsValid;
        }

        public async Task<bool> UpdateIndexAsync(string indexName)
        {
            var response =
                await _client.Indices.UpdateSettingsAsync(indexName, i => i.IndexSettings(s => s.NumberOfReplicas(1)));
            return response.IsValid;
        }

        public async Task<bool> DeleteIndexAsync(string indexName)
        {
            if (_client.Indices.Exists(indexName).Exists)
            {
                var response = await _client.Indices.DeleteAsync(indexName);
                return response.IsValid;
            }
            return true;
        }

        public async Task<bool> CreateIndexTemplateAsync(string indexTemplateName, string indexPatterns, string aliasName)
        {
            if (_client.Indices.TemplateExists(indexTemplateName).Exists) return true;
            var response = await _client.Indices.PutTemplateAsync(indexTemplateName, c => c.
                Version(1).
                IndexPatterns(indexPatterns).
                Settings(s => s.
                    NumberOfShards(1).
                    NumberOfReplicas(0).
                    RefreshInterval(new Time(TimeSpan.FromMinutes(2))).AutoExpandReplicas(AutoExpandReplicas.Disabled).
                    Setting("search.idle.after", "300s").
                    Setting("search.slowlog.level", "warn").
                    Setting("search.slowlog.threshold.query.warn", "10s").
                    Setting("search.slowlog.threshold.fetch.warn", "1s")).
                Map<DeviceLog>(m => m.Dynamic(false).
                    Properties(p => p.
                        Number(t => t.Name(s => s.CustomerId).Type(NumberType.Long)).
                        Date(t => t.Name(s => s.CreatedTime)).
                        Keyword(t => t.Name(s => s.VehiclePlate).Norms(false)).
                        Keyword(t => t.Name(s => s.Message).Index(false).Norms(false).DocValues(false))).
                    SourceField(s => s.Enabled(true))).
                Aliases(a => a.Alias(aliasName)));
            return response.IsValid;
        }

        public async Task<bool> DeleteIndexTemplateAsync(string indexTemplateName)
        {
            var response = await _client.Indices.DeleteTemplateAsync(indexTemplateName);
            return response.IsValid;
        }
    }
}
