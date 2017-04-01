using System.Collections.Generic;
using System.Threading;
using BeardBook.Entities;
using BeardBook.Identity;
using Microsoft.AspNet.Identity;

namespace BeardBook.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BeardBookDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BeardBookDbContext context)
        {
            const string loremIpsum = "Lorem ipsum dolor sit amet enim. Etiam ullamcorper. Suspendisse a pellentesque dui, non felis. Maecenas malesuada elit lectus felis, malesuada ultricies. Curabitur et ligula. Ut molestie a, ultricies porta urna. Vestibulum commodo volutpat a, convallis ac, laoreet enim. Phasellus fermentum in, dolor. Pellentesque facilisis. Nulla imperdiet sit amet magna. Vestibulum dapibus, mauris nec malesuada fames ac turpis velit, rhoncus eu, luctus et interdum adipiscing wisi. Aliquam erat ac ipsum. Integer aliquam purus.";
            const string loremIpsum2 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam lacinia massa nisi, nec venenatis turpis hendrerit vel. Nulla facilisi. Nunc feugiat odio nulla, interdum vestibulum arcu viverra quis. Fusce vitae lacus maximus, volutpat erat vitae, lobortis diam. Vestibulum sit amet turpis eu elit semper aliquam.";
            var userManager = new AppUserManager(new CustomUserStore(new BeardBookDbContext("DefaultConnection")));
            
            SeedUser(userManager, "jon@skeet.com", "Jon", "Skeet", new []
            {
                loremIpsum,
                loremIpsum2,
                "I work for Google",
                "But I love .NET :)",
                "I\'d be happy to join the DotNext conference",
                "...and i don\'t cost 15k$ + business class travel :P"
            });
            SeedUser(userManager, "bill@gates.com", "Bill", "Gates", new []
            {
                loremIpsum,
                loremIpsum2,
                "Skeet, komm zu mir!"
            });
            SeedUser(userManager, "scott@hanselman.com", "Scott", "Hanselman", new []
            {
                loremIpsum,
                loremIpsum2,
                "GIMME MORE BUSINESS CLASS TRAVEL"
            });
            SeedUser(userManager, "anonymous@hacker.ru", "Definitely Not", "Russian", new []
            {
                loremIpsum,
                loremIpsum2,
                "im da anonymous hacker )))))",
                "liberals are scared of me ))))))))))))))))))",
                "<b>huehue )))</b>",
                "<script>alert(\'XXS attack!\');</script>",
            });
        }

        private static void SeedUser(AppUserManager userManager, string email, string firstName, string lastName, IEnumerable<string> posts)
        {
            if (userManager.FindByEmail(email) != null) return;
            var user = new User
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName
            };
            userManager.Create(user, "Asdqwe1");
            user = userManager.FindByEmail(email);
            user.Posts = new List<Post>();
            foreach (var post in posts)
            {
                user.Posts.Add(new Post
                {
                    UserId = user.Id,
                    Text = post
                });
                Thread.Sleep(50);
            }
            userManager.Update(user);
        }
    }
}
