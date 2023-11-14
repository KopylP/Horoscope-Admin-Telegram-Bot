using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddEnvironmentVariables();
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var firebaseSettings = context.Configuration["FirebaseConfiguration"];
        var firebaseProjectId = context.Configuration["FirebaseProjectId"];
        var credential = GoogleCredential.FromJson(firebaseSettings);
        
        services.AddSingleton(_ => new FirestoreDbBuilder
        {
            ProjectId = firebaseProjectId,
            Credential = credential
        }.Build());
    })
    .Build();

host.Run();
