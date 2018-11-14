using MHser.DAL.Contexts;
using MHser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MHser.DAL.Data
{
    public class DbInitializer : CreateDatabaseIfNotExists<MessoyahaContext>
    {
        protected override void Seed( MessoyahaContext context )
        {
            var categories = new List<Category>()
            {
                new Category
                {
                    CategoryId = 1,
                    Name = "Промышленная безопасность",
                    ShortName = "ПБ",
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Пожарная безопасность",
                    ShortName = "ПожБ",
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Электробезопасность",
                    ShortName = "ЭлБ",
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "Работа с ПС",
                    ShortName = "ПС",
                },
                new Category
                {
                    CategoryId = 5,
                    Name = "Транспортная безопасность",
                    ShortName = "ТрБ",
                },
                new Category
                {
                    CategoryId = 6,
                    Name = "Экологическая безопасность",
                    ShortName = "ООС",
                },
                new Category
                {
                    CategoryId = 7,
                    Name = "Работы на высоте",
                    ShortName = "РнВ",
                },
                new Category
                {
                    CategoryId = 8,
                    Name = "Огневые работы",
                    ShortName = "ОР",
                },
                new Category
                {
                    CategoryId = 9,
                    Name = "Другое",
                    ShortName = "Другое",
                },
            };
            using ( var transaction = context.Database.BeginTransaction() )
            {
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Categories] ON" );
                context.Categories.AddRange( categories );
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Categories] OFF" );
                transaction.Commit();
            }

            var characters = new List<Character>()
            {
                new Character
                {
                    CharacterId = 1,
                    Name = "Опасные условия",
                    ShortName = "ОУ",
                },
                new Character
                {
                    CharacterId = 2,
                    Name = "Опасные действия",
                    ShortName = "ОД",
                },
                new Character
                {
                    CharacterId = 3,
                    Name = "Происшествия без последствий",
                    ShortName = "ПбП",
                },
            };
            using ( var transaction = context.Database.BeginTransaction() )
            {
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Characters] ON" );
                context.Characters.AddRange( characters );
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Characters] OFF" );
                transaction.Commit();
            }

            var Users = new List<User>()
            {
                new User
                {
                    UserId = 1,
                    Name = @"Денисевич Юрий Владимирович",
                    AdName = @"GAZPROM-NEFT\Denisevich.YuV",
                    Position = "Главный специалист",
                },
                new User
                {
                    UserId = 2,
                    Name = @"Камальтинов Рафаэль Рустемович",
                    AdName = @"Gazprom-neft\Kamaltinov.RR",
                    Position = "Ведущий специалист",
                },
                new User
                {
                    UserId = 3,
                    Name = @"Костарев Сергей Юрьевич",
                    AdName = @"GAZPROM-NEFT\Kostarev.SYu",
                    Position = "Главный специалист ПБ,ЭБ,ОТ и ГЗ",
                },
                new User
                {
                    UserId = 4,
                    Name = @"Тимергазин Ринат Римович",
                    AdName = @"GAZPROM-NEFT\Timergazin.RRi",
                    Position = "Главный специалист ПБ,ЭБ,ОТ и ГЗ",
                },
            };
            using ( var transaction = context.Database.BeginTransaction() )
            {
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Users] ON" );
                context.Users.AddRange( Users );
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Users] OFF" );
                transaction.Commit();
            }

            var locations = new List<Location>()
            {
                new Location
                {
                    LocationId = 1,
                    Name = @"testLocation",
                    TypeOfObject = Constants.Messoyaha,
                },
                new Location
                {
                    LocationId = 2,
                    Name = @"ИСК ""Ямал Альянс""",
                    TypeOfObject = Constants.Contractor,
                },
                new Location
                {
                    LocationId = 3,
                    Name = @"ИСК ""Ямал Альянс"" Жилой городок",
                    TypeOfObject = Constants.Contractor,
                },
            };
            using ( var transaction = context.Database.BeginTransaction() )
            {
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Locations] ON" );
                context.Locations.AddRange( locations );
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Locations] OFF" );
                transaction.Commit();
            }

            var responsables = new List<Responsable>()
            {
                new Responsable
                {
                    ResponsableId = 1,
                    Name = @"TestName",
                    Position = "testPostition",
                    LocationId = 1,
                },
                new Responsable
                {
                    ResponsableId = 2,
                    Name = @"Раимгулов М.М.",
                    Position = "Зам. главного инженера",
                    LocationId = 2,
                },
                new Responsable
                {
                    ResponsableId = 3,
                    Name = @"Хомченко В.Н.",
                    Position = "Зам. главного инженера",
                    LocationId = 3,
                },
            };
            using ( var transaction = context.Database.BeginTransaction() )
            {

                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Responsables] ON" );
                context.Responsables.AddRange( responsables );
                context.SaveChanges();
                //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Responsables] OFF" );
                transaction.Commit();
            }

            //var disruptions = new List<Disruption>()
            //{
            //    new Disruption
            //    {
            //        DisruptionId = 1,
            //        DetectionTime = DateTime.Parse("Oct 22 2018  1:43PM"),
            //        ExecuteUntil = DateTime.Parse("Oct 30 2018  4:40PM"),
            //        Description = @"Прилегающая территория к нефтеналивному парку ЦДНГ. Клети для хранения балонов с кислородом и пропаном не заземлены, отсутсвуют молниеотводы.",
            //        Documentation = @"ПУЭ гл. 1.7. п. 7.4.13. ",
            //        UserId = 1,
            //        LocationId = 3,
            //        CategoryId = 3,
            //        CharacterId = 1,
            //        ResponsableId = 1,
            //        IsDone = false,
            //        CreationDate = DateTime.UtcNow,
            //    },
            //};
            //using ( var transaction = context.Database.BeginTransaction() )
            //{

            //    //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Disruptions] ON" );
            //    context.Disruptions.AddRange( disruptions );
            //    context.SaveChanges();
            //    //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Disruptions] OFF" );
            //    transaction.Commit();
            //}

            //var disruptionsLog = new List<DisruptionLog>()
            //{
            //    new DisruptionLog
            //    {
            //        DisruptionId = 1,
            //        DetectionTime = DateTime.Parse("Oct 22 2018  1:43PM"),
            //        ExecuteUntil = DateTime.Parse("Oct 30 2018  4:40PM"),
            //        Description = @"Прилегающая территория к нефтеналивному парку ЦДНГ. Клети для хранения балонов с кислородом и пропаном не заземлены, отсутсвуют молниеотводы.",
            //        Documentation = @"ПУЭ гл. 1.7. п. 7.4.13. ",
            //        UserId = 1,
            //        LocationId = 3,
            //        CategoryId = 3,
            //        CharacterId = 1,
            //        ResponsableId = 1,
            //        IsDone = false,
            //        CreationDate = DateTime.UtcNow,
            //    }
            //};

            //using ( var transaction = context.Database.BeginTransaction() )
            //{

            //    //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Disruptions] ON" );
            //    context.DisruptionsLog.AddRange( disruptionsLog );
            //    context.SaveChanges();
            //    //context.Database.ExecuteSqlCommand( "SET IDENTITY_INSERT [dbo].[Disruptions] OFF" );
            //    transaction.Commit();
            //}
        }
    }
}