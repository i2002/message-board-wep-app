using MessageBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MessageBoardContext(
                serviceProvider.GetRequiredService<DbContextOptions<MessageBoardContext>>());
            if (context.Topic.Any())
            {
                return; // DB has been seeded
            }

            // Seed Users
            context.User.Add(new User { Name = "Admin", Email = "admin@example.com", Password = "password" });
            context.SaveChanges();
            int userId = context.User.First().Id;

            // Seed Topics
            context.Topic.AddRange(
                new Topic
                {
                    Title = "Primul meu topic",
                    Content = "Buna tuturor, Acesta este primul meu topic.\nImi face mare placere sa fiu alaturi de voi in aceasta comunitate minunata.",
                    Category = "welcome",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UserId = userId
                },
                new Topic
                {
                    Title = "Cele mai interesante site-uri",
                    Content = "Hei, am alcatuit o lista cu cele mai interesante site-uri pe care le-am descoperit.",
                    Category = "cool",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UserId = userId
                },
                new Topic
                {
                    Title = "Ramas bun",
                    Content = "Cu mare tristete va anunt ca voi fi nevoit sa parasesc acest site intru-cat actiunile unora dintre utilizatori sunt de-a dreptul revoltatoare. Pana cand echipa de administrare nu va introduce solutii de moderare, nu mai am ce cauta aici.",
                    Category = "welcome",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    UserId = userId
                }
            );
            context.SaveChanges();
        }
    }
}
