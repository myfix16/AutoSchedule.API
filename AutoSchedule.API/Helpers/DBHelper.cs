using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Cosmos;
using Azure.Cosmos.Fluent;
using Azure.Cosmos.Serialization;

namespace AutoSchedule.API.Helpers
{
    public static class DBHelper
    {
        public static CosmosDatabase GetDB()
        {
            var cosmosClient = new CosmosClientBuilder
                    ("AccountEndpoint=https://cosmosdb-for-autoschedule.documents.azure.com:443/;AccountKey=DOL6pQKH9MoFrwMk2o29arqXy9uO4tksv83jtfohGK6m9Gkwlwq8oejl2SiyJRsNaNZvol0sAkBBAUUIHV2jqQ==;")
                .WithSerializerOptions(new CosmosSerializationOptions { Indented = true })
                .Build();
            return cosmosClient.GetDatabase("SessionsData");
        }

        public static async Task<IEnumerable<string>> GetTables(CosmosDatabase db)
        {
            var iterator = db.GetContainerQueryIterator<ContainerProperties>();
            List<string> tables = new();
            await foreach (var container in iterator)
            {
                tables.Add(container.Id);
            }

            return tables;
        }
    }
}
