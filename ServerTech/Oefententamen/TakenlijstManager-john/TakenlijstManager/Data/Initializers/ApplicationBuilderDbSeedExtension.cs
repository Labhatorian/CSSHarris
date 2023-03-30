namespace TakenlijstManager.Data.Initializers
{
    public static class ApplicationBuilderDbSeedExtension
    {
            public static IApplicationBuilder UseSeedTakenlijst(this IApplicationBuilder app)
            {
                ArgumentNullException.ThrowIfNull(app, nameof(app));

                using var scope = app.ApplicationServices.CreateScope();
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<TakenManagerDbContext>();
                    TakenlijstDbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                return app;
            }
    }
}
