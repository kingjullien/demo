using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace SBISCCMWeb.Models
{
    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static DocumentClient client;

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            //CreateDatabaseIfNotExistsAsync().Wait();
            //CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            try
            {
                Initialize();
                return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            Initialize();
            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), option)
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<int> GetItemsCount(Expression<Func<T, bool>> predicate)
        {
            Initialize();
            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            int query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), option)
                  .Where(predicate)
                .Count();


            return query;
        }
    }
}