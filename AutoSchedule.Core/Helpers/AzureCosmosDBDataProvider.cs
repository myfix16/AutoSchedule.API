using System.Collections.Generic;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;
using Azure.Cosmos;
using Azure.Cosmos.Fluent;
using Azure.Cosmos.Serialization;

namespace AutoSchedule.Core.Helpers
{
    /// <summary>
    /// A helper class that renders data from data source. 
    /// </summary>
    public class AzureCosmosDBDataProvider : IDataProviderAsync<IEnumerable<Session>>
    {
        const string ReadOnlyConnectionString = "AccountEndpoint=https://cosmosdb-for-autoschedule.documents.azure.com:443/;AccountKey=j5rHnPcv8ZpWLhFbOFBVxz6G5QgZaIAm5lX6yNDZhifJKtVepwEUEMFHd5DblXukEodgrbXHbJQB2CgLONC2bA==;";

        public async Task<IEnumerable<Session>> GetDataAsync()
        {
            var cosmosClient = new CosmosClientBuilder(ReadOnlyConnectionString)
                .WithSerializerOptions(new CosmosSerializationOptions { Indented = true })
                .Build();
            var container = cosmosClient.GetDatabase("SessionsData").GetContainer("2021-2022-Term1");

            const string sqlQueryText = "SELECT * FROM c";
            var queryIterator = container.GetItemQueryIterator<Session>(new QueryDefinition(sqlQueryText));

            // * v3 version of Azure Cosmos SDK
            // //Asynchronous query execution
            // while (queryIterator.HasMoreResults)
            // {
            //     foreach (var item in await queryIterator.ReadNextAsync())
            //         sessions.Add(item);
            // }

            // Fetch session data from data base.
            List<Session> sessions = new();
            await foreach (Session session in queryIterator)
            {
                sessions.Add(session);
            }

            return sessions;
        }
    }
}
