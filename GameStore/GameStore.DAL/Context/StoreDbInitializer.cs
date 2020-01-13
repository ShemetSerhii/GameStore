using GameStore.Domain.Entities;
using GameStore.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace GameStore.DAL.Context
{
    public class StoreDbInitializer : CreateDatabaseIfNotExists<SqlContext>
    {
        protected override void Seed(SqlContext db)
        {
            var platformTypes = new List<PlatformType>
            {
                new PlatformType { Type = "mobile" },
                new PlatformType { Type = "browser" },
                new PlatformType { Type = "desktop" },
                new PlatformType { Type = "console" },
            };

            var genres = new List<Genre>
            {
                new Genre { Name = "Strategy" },
                new Genre { Name = "RTS", ParentId = 1 },
                new Genre { Name = "TBS", ParentId = 1},
                new Genre { Name = "RPG"  },
                new Genre { Name = "Sports" },
                new Genre { Name = "Races" },
                new Genre { Name = "Rally", ParentId = 6},
                new Genre { Name = "Arcade", ParentId = 6 },
                new Genre { Name = "Formula", ParentId = 6},
                new Genre { Name = "Off-road", ParentId = 6 },
                new Genre { Name = "Action"  },
                new Genre { Name = "FPS", ParentId = 11 },
                new Genre { Name = "TPS", ParentId = 11 },
                new Genre { Name = "Misc.", ParentId = 11 },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Puzzle & Skill" }
            };

            var publishers = new List<Publisher>
            {
                new Publisher() {CompanyName = "Ubisoft", Description = "Info", HomePage = "Text", UserLogin = "Publisher"},
                new Publisher() {CompanyName = "Nintendo", Description = "Info", HomePage = "Test"}
            };

            var games = new List<Game>();

            for (int i = 1; i <= 200; i++)
            {
                games.Add(new Game()
                {
                    Price = i * 10,
                    Key = i.ToString(),
                    Name = "Game" + i,
                    Description = "Info" + i,
                    DateAdded = DateTime.UtcNow,
                    DatePublication = DateTime.UtcNow.AddDays(-1),
                    Publisher = publishers[i % 2],
                    PlatformTypes = new List<PlatformType>{platformTypes[i % 4]},
                    Genres = new List<Genre> { genres[i % 16]}
                });
            }

            var roles = new List<Role>
            {
                new Role {Name = "Administrator"},
                new Role {Name = "Manager"},
                new Role {Name = "Moderator"},
                new Role {Name = "User"},
                new Role {Name = "Guest"},
                new Role {Name = "Publisher"},
            };

            var users = new List<User>
            {
                new User
                {
                    Name = "Admin",
                    Login = "Admin",
                    Password = "123456".GetHashCode(),
                    Roles = new List<Role>
                    {
                        roles[0]
                    }
                },
                new User
                {
                    Name = "Manager",
                    Login = "Manager",
                    Password = "123456".GetHashCode(),
                    Roles = new List<Role>
                    {
                        roles[1]
                    }
                },
                new User
                {
                    Name = "Moderator",
                    Login = "Moderator",
                    Password = "123456".GetHashCode(),
                    Roles = new List<Role>
                    {
                        roles[2]
                    }
                },
                new User
                {
                    Name = "User",
                    Login = "User",
                    Password = "123456".GetHashCode(),
                    Roles = new List<Role>
                    {
                        roles[3]
                    }
                },
                new User
                {
                    Name = "Publisher",
                    Login = "Publisher",
                    Password = "123456".GetHashCode(),
                    Roles = new List<Role>
                    {
                        roles[5]
                    }
                },
            };

            foreach (var game in games)
            {
                db.Games.Add(game);
            }


            foreach (var platform in platformTypes)
            {
                db.PlatformTypes.Add(platform);
            }


            foreach (var genre in genres)
            {
                db.Genres.Add(genre);
            }


            foreach (var publisher in publishers)
            {
                db.Publishers.Add(publisher);
            }

            foreach (var user in users)
            {
                db.Users.Add(user);
            }

            foreach (var role in roles)
            {
                db.Roles.Add(role);
            }
                
            db.SaveChanges();
            base.Seed(db);
        }
    }
}
