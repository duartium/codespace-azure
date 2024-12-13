using System.Text;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

Console.WriteLine("Azure blob exercise!");

ProcessAsync().GetAwaiter().GetResult();

static async Task ProcessAsync()
{
    string connectionString = "DefaultEndpointsProtocol=https;AccountName=almacenamientoaz204;AccountKey=;EndpointSuffix=core.windows.net";

    var sb  = new StringBuilder();
    var storageConnectionString = sb.Append(
        connectionString
        )
    .ToString();

    BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

    //Create a unique name for the container
string containerName = "wtblob" + Guid.NewGuid().ToString();

// Create the container and return a container client object
BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
Console.WriteLine("A container named '" + containerName + "' has been created. " +
    "\nTake a minute and verify in the portal." + 
    "\nNext a file will be created and uploaded to the container.");

string localPath = "./data";
string fileName = "wtfile" + Guid.NewGuid().ToString() + ".txt";
string localFilePath = Path.Combine(localPath, fileName);

//write text to the file
await File.WriteAllTextAsync(localFilePath, "Hello world!");
BlobClient blobClient = containerClient.GetBlobClient(fileName);

Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

// Open the file and upload its data
using (FileStream uploadFileStream = File.OpenRead(localFilePath))
{
    await blobClient.UploadAsync(uploadFileStream);
    uploadFileStream.Close();
}

// List blobs in the container
Console.WriteLine("Listing blobs...");
await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
{
    Console.WriteLine("\t" + blobItem.Name);
}

Console.WriteLine("\nYou can also verify by looking inside the " + 
        "container in the portal." +
        "\nNext the blob will be downloaded with an altered file name.");
Console.WriteLine("Press 'Enter' to continue.");
}