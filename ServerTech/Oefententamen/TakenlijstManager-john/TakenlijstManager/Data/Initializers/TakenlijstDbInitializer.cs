using TakenlijstManager.Data.Entities;

namespace TakenlijstManager.Data.Initializers
{
    public class TakenlijstDbInitializer
    {
        public static void Initialize(TakenManagerDbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();

            if (dbContext.Taken.Any()) return; //hier kun je bepalen om eerst alle data uit de database te gooien alles opnieuw in te voeren

            var taak1 = new Taak() { Naam = "Huiswerk Client", Omvang = 5, Prioriteit = 6 };
            var taak2 = new Taak() { Naam = "Huiswerk Server", Omvang = 3, Prioriteit = 3 };
            var taak3 = new Taak() { Naam = "Voorbereiden Tentamen Server", Omvang = 4, Prioriteit = 10 };
            var taak4 = new Taak() { Naam = "Voorbereiden Tentamen Client", Omvang = 5, Prioriteit = 10 };

            dbContext.Add(taak1);
            dbContext.Add(taak2);
            dbContext.Add(taak3);
            dbContext.Add(taak4);

            dbContext.SaveChanges();

            //var alleTaken = dbContext.Taken.ToList();

            //foreach (var taak in alleTaken)
            //{
            //    Console.WriteLine(taak.Naam);
            //}
        }
    }
}
