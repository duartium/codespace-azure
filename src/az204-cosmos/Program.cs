using Microsoft.Azure.Cosmos;

 string EndpointUri = "https://ejercicios-cosmos-db-learn.documents.azure.com:443/";
 string PrimaryKey = "YOUR_API_KEY";
 CosmosClient cosmosClient;
 Database database = null;
 Container container;
 string databaseId = "az204Database";
 string containerId = "az204Container";

await CosmosAsync();


async Task CosmosAsync(){
    cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

    await CreateDatabaseAsync();

    await CreateContainerAsync();

    Console.WriteLine("Cosmos DB setup complete.");
}

async Task CreateDatabaseAsync(){
    database = await cosmosClient.CreateDatabaseAsync(databaseId);
    Console.WriteLine("Created Database: {0}\n", database.Id);
}

async Task CreateContainerAsync(){
    container = await database.CreateContainerIfNotExistsAsync(
        containerId,
        "/LastName"
    );
    Console.WriteLine("Created Container: {0}\n", container.Id);
}

