namespace SpellWork.Services.RunOnce;

public class DBCLoaderService(IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        // TODO: this should actually be async
        await DBC.DBC.Load(configuration.GetValue<string>("DbcPath"), configuration.GetValue<string>("Locale"), configuration.GetValue<string>("GtPath"));
    }
}