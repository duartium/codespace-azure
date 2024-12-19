var scopes = new[] { "User.Read" };

// Multi-tenant apps can use "common",
// single-tenant apps must use the tenant ID from the Azure portal
var tenantId = "5403bb85-6bc3-495d-8a5e-1a61c622dd74";

// Value from app registration
var clientId = "ea591b26-052f-4b9c-8288-709115f3d192";

// using Azure.Identity;
var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

// Callback function that receives the user prompt
// Prompt contains the generated device code that you must
// enter during the auth process in the browser
Func<DeviceCodeInfo, CancellationToken, Task> callback = (code, cancellation) => {
    Console.WriteLine(code.Message);
    return Task.FromResult(0);
};

// /dotnet/api/azure.identity.devicecodecredential
var deviceCodeCredential = new DeviceCodeCredential(
    callback, tenantId, clientId, options);

var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);

var user = await graphClient.Me
    .GetAsync();


// GET https://graph.microsoft.com/v1.0/me/messages?
// $select=subject,sender&$filter=subject eq 'Hello world'
var messages = await graphClient.Me.Messages
    .GetAsync(requestConfig =>
    {
        requestConfig.QueryParameters.Select =
            ["subject", "sender"];
        requestConfig.QueryParameters.Filter =
            "subject eq 'Hello world'";
    });

var calendar = new Calendar
{
    Name = "Volunteer",
};

var newCalendar = await graphClient.Me.Calendars
    .PostAsync(calendar);