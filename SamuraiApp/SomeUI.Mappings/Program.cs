using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeUI.Mappings
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            #region • Entity Framework Core 2 - Getting Started                         
            //https://app.pluralsight.com/library/courses/entity-framework-core-2-getting-started/table-of-contents

            #region --- Single Entity CRUD --- Module 3
            //InsertSamurai();
            //InsertMultipleSamurais();
            //InsertMultipleDifferenteObjects();
            //SimpleSamuraiQuery();
            //MoreQueries();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();
            //QueryAndUpdateSamurai_Disconnected();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //AddSomeMoreSamurais();
            //DeleteWhileTracked();
            //DeleteMany();
            //DeleteWhileNotTracked();
            //DeleteUsingId(3);
            #endregion

            #region --- Related Data --- Module 4
            //InsertNewPkFkGraph();
            //InsertNewPkFkGraphMultipleChildren();
            //AddChildToExistingObjectWhileTracked();
            //AddChildToExistingObjectWhileNotTracked(1);
            //EagerLoadSamuraiWithQuotes();
            //var dynamicList = ProjectDynamic();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();
            #endregion

            #endregion

            #region • Entity Framework Core 2 - Mappings                                                         
            // https://app.pluralsight.com/library/courses/e-f-core-2-beyond-the-basics-mappings/table-of-contents

            #region --- Many to Many ---
            //PrePopulateSamuraisAndBattles();
            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoABattle();
            //EnlistSamuraiIntoABattleUntracked();
            //AddNewSamuraiViaDisconnectedBattleObject();
            //GetSamuraiWithBattles();
            //GetSamuraiWithBattlesV2();
            //GetBattlesForSamuraiInMemory();
            //RemoveJoinBetweenSamuraiAndBattleSimple();
            //RemoveBattleFromSamurai();
            //RemoveBattleFromSamuraiWhenDisconnected();
            #endregion

            #region --- One to One ---
            //AddNewSamuraiWithSecretIdentity();
            //AddSecretIdentityUsingSamuraiId();
            //AddSecretIdentityToExistingSamurai();
            //EditASecretIdentity();
            //ReplaceASecretIdentity();
            //ReplaceASecretIdentityNotTracked();
            //ReplaceSecretIdentityNotInMemory();
            #endregion

            #region --- Shadow Properties ---
            //CreateSamurai();
            //RetrieveSamuraisCreatedInPastWeek();
            //CreateThenEditSamuraiWithQuote();
            #endregion

            #region --- Owned Types ---
            //GetAllSamurais();

            //CreateSamuraiWithBetterName();
            //RetrieveAndUpdateBetterName();
            //ReplaceBetterName();
            //CreateAndFixUpNullBetterName();
            #endregion

            #region --- Scalar Functions ---
            //User Defined Funcions (UDF)
            //A Scalar Function returns a single scalar value, readyonly and not designed to update database
            //Example: GetDate in database

            //RetrieveScalarResult();
            //FilterWithScalarResult();
            //SortWithScalar();
            //SortWithoutReturningScalar();

            //RetrieveDaysInBattle();
            //RetrieveDaysInBattleWithoutDbFunction();

            //RetrieveYearUsingDbBuiltInFunction();
            #endregion

            #region --- Working with Database Views ---
            //GetStats();
            //Filter();
            //Project();
            #endregion

            #endregion
        }

        #region • Entity Framework Core 2 - Getting Started     

        #region --- Single Entity CRUD --- Module 3

        private static void AddSomeMoreSamurais()
        {
            _context.AddRange(
               new Samurai { Name = "Kambei Shimada" },
               new Samurai { Name = "Shichirōji " },
               new Samurai { Name = "Katsushirō Okamoto" },
               new Samurai { Name = "Heihachi Hayashida" },
               new Samurai { Name = "Kyūzō" },
               new Samurai { Name = "Gorōbei Katayama" }
             );
            _context.SaveChanges();
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Julie" };

            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "Dream" };
            var samuraiSammy = new Samurai { Name = "Sammy" };

            using (var context = new SamuraiContext())
            {
                context.Samurais.AddRange(samurai, samuraiSammy);
                context.SaveChanges();
            }
        }

        private static void InsertMultipleDifferenteObjects()
        {
            var samurai = new Samurai { Name = "Oda Nobunaga" };

            var battle = new Battle
            {
                Name = "Battle of Nagashino",
                StartDate = new DateTime(1575, 06, 16),
                EndDate = new DateTime(1575, 06, 28)
            };

            using (var context = new SamuraiContext())
            {
                context.AddRange(samurai, battle);
                context.SaveChanges();
            }
        }

        private static void SimpleSamuraiQuery()
        {
            using (var context = new SamuraiContext())
            {
                var samurais = context.Samurais.ToList();
            }
        }

        private static void MoreQueries()
        {
            var samurais_NonParameterizedQuery = _context.Samurais.Where(s => s.Name == "Dream").ToList();

            //Faz com que o T-SQL gerado use parâmetros
            var name = "Dream";

            var samurais_ParameterizedQuery = _context.Samurais.Where(s => s.Name == name).ToList();

            var samurai_Object = _context.Samurais.FirstOrDefault(s => s.Name == name);

            var samurais_ObjectFindByKeyValue = _context.Samurais.Find(2);

            var samuraisJ = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "D%")).ToList();

            var lastDream = _context.Samurais.OrderBy(s => s.Id).LastOrDefault(s => s.Name == name);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";

            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "Hiro";
            _context.Samurais.Add(new Samurai { Name = "Kikuchiyo" });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateSamurai_Disconnected()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kikuchiyo");
            samurai.Name += "San";
            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Samurais.Update(samurai);
                newContextInstance.SaveChanges();
            }
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);

            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void DeleteUsingId(int samuraiId)
        {
            //2 Database calls, better use a stored procedure
            var samurai = _context.Samurais.Find(samuraiId);
            _context.Remove(samurai);
            _context.SaveChanges();
            //alternate: call a stored procedure!
            //_context.Database.ExecuteSqlCommand("exec DeleteById {0}", samuraiId);
        }

        private static void DeleteWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kambei Shimada");
            _context.Samurais.Remove(samurai);
            //alternates:
            // _context.Remove(samurai);
            // _context.Entry(samurai).State=EntityState.Deleted;
            // _context.Samurais.Remove(_context.Samurais.Find(1));
            _context.SaveChanges();
        }

        private static void DeleteMany()
        {
            var samurais = _context.Samurais.Where(s => s.Name.Contains("ō"));
            _context.Samurais.RemoveRange(samurais);
            //alternate: _context.RemoveRange(samurais);
            _context.SaveChanges();
        }

        private static void DeleteWhileNotTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Heihachi Hayashida");
            using (var contextNewAppInstance = new SamuraiContext())
            {
                contextNewAppInstance.Samurais.Remove(samurai);
                //contextNewAppInstance.Entry(samurai).State=EntityState.Deleted;
                contextNewAppInstance.SaveChanges();
            }
        }

        #endregion

        #region --- Related Entities --- Module 4

        private static void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                               {
                                 new Quote {Text = "I've come to save you"}
                               }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraphMultipleChildren()
        {
            var samurai = new Samurai
            {
                Name = "Kyūzō",
                Quotes = new List<Quote> {
                  new Quote {Text = "Watch out for my sharp sword!"},
                  new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddChildToExistingObjectWhileTracked()
        {
            var samurai = _context.Samurais.First();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddChildToExistingObjectWhileNotTracked(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?",
                SamuraiId = samuraiId
            };
            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Where(s => s.Name.Contains("Kyūzō"))
                                                     .Include(s => s.Quotes)
                                                     .Include(s => s.SecretIdentity)
                                                     .FirstOrDefault();
        }

        private static List<dynamic> ProjectDynamic()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            return someProperties.ToList<dynamic>();
        }

        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idsAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropertiesWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, s.Quotes.Count })
            //    .ToList();


            var somePropertiesWithSomeQuotes = _context.Samurais
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();

            //Not Working atm (waiting for fix in a future release)
            //var samuraiWithHappyQuotes = _context.Samurais
            //    .Select(s => new
            //    {
            //        Samurai = s,
            //        Quotes = s.Quotes.Where(q => q.Text.Contains("happy")).ToList()
            //    })
            //    .ToList();

            //Workaround
            var samurais = _context.Samurais.ToList();
            var happyQuotes = _context.Quotes.Where(q => q.Text.Contains("happy")).ToList();
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                                   .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                                   .ToList();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            samurai.Quotes[0].Text += " Did you hear that?";
            //_context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text += " Did you hear that?";
            using (var newContext = new SamuraiContext())
            {
                //newContext.Quotes.Update(quote); //atualiza todos os campos sem necessidade
                newContext.Entry(quote).State = EntityState.Modified; //atualiza apenas o que foi alterado
                newContext.SaveChanges();
            }
        }

        #endregion

        #endregion

        #region • Entity Framework Core 2 - Mappings 

        #region --- Many to Many ---
        private static void PrePopulateSamuraisAndBattles()
        {
            //, BetterName = new PersonFullName(null, null) to work with the owned type added later
            _context.AddRange(
             new Samurai { Name = "Kikuchiyo"/*, BetterName = new PersonFullName(null, null)*/ },
             new Samurai { Name = "Kambei Shimada"/*, BetterName = new PersonFullName(null, null)*/ },
             new Samurai { Name = "Shichirōji "/*, BetterName = new PersonFullName(null, null)*/ },
             new Samurai { Name = "Katsushirō Okamoto"/*, BetterName = new PersonFullName(null, null)*/ },
             new Samurai { Name = "Heihachi Hayashida"/*, BetterName = new PersonFullName(null, null)*/ },
             new Samurai { Name = "Kyūzō"/*, BetterName = new PersonFullName(null, null)*/ },
             new Samurai { Name = "Gorōbei Katayama"/*, BetterName = new PersonFullName(null, null)*/ }
           );

            _context.Battles.AddRange(
             new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) },
             new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
             new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
             new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
           );

            _context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            //Kikuchiyo id is 1, Siege of Osaka id is 3
            var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };

            _context.Add(sbJoin);
            _context.SaveChanges();
        }

        private static void EnlistSamuraiIntoABattle()
        {
            var battle = _context.Battles.Find(1);

            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 3 });

            _context.SaveChanges();
        }

        private static void EnlistSamuraiIntoABattleUntracked()
        {
            Battle battle;

            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(1);
            }

            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 2 });

            _context.Battles.Attach(battle);
            _context.ChangeTracker.DetectChanges(); //here to show you debugging info
            _context.SaveChanges();
        }

        private static void AddNewSamuraiViaDisconnectedBattleObject()
        {
            Battle battle;

            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(1);
            }

            //BetterName to work with the owned type added later
            var newSamurai = new Samurai { Name = "SampsonSan"/*, BetterName = new PersonFullName(null, null)*/ };

            battle.SamuraiBattles.Add(new SamuraiBattle { Samurai = newSamurai });

            _context.Battles.Attach(battle);
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle).FirstOrDefault(s => s.Id == 1);

            var battle = samuraiWithBattles.SamuraiBattles.First().Battle;

            var allTheBattles = new List<Battle>();

            foreach (var samuraiBattle in samuraiWithBattles.SamuraiBattles)
            {
                allTheBattles.Add(samuraiBattle.Battle);
            }
        }

        private static void GetSamuraiWithBattlesV2()
        {
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle).FirstOrDefault(s => s.Id == 1);

            var battle = samuraiWithBattles.Battles().First();

            var allTheBattles = samuraiWithBattles.Battles();
        }

        //----------------------------------------------------------------------------------------------------

        private static void GetBattlesForSamuraiInMemory()
        {
            var battle = _context.Battles.Find(1);
            _context.Entry(battle).Collection(b => b.SamuraiBattles).Query().Include(sb => sb.Samurai).Load();

        }

        //----------------------------------------------------------------------------------------------------

        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 1, SamuraiId = 8 };

            _context.Remove(join);
            _context.SaveChanges();
        }

        private static void RemoveBattleFromSamurai()
        {
            //Goal:Remove join between Shichirōji(Id=3) and Battle of Okehazama (Id=1)
            var samurai = _context.Samurais.Include(s => s.SamuraiBattles)
                                           .ThenInclude(sb => sb.Battle)
                                           .SingleOrDefault(s => s.Id == 3);

            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);

            samurai.SamuraiBattles.Remove(sbToRemove); //remove via List<T>

            //_context.Remove(sbToRemove); //remove using DbContext
            _context.ChangeTracker.DetectChanges(); //here for debugging
            _context.SaveChanges();
        }

        private static void RemoveBattleFromSamuraiWhenDisconnected()
        {
            //Goal:Remove join between Shichirōji(Id=3) and Battle of Okehazama (Id=1)
            Samurai samurai;

            using (var separateOperation = new SamuraiContext())
            {
                samurai = separateOperation.Samurais.Include(s => s.SamuraiBattles)
                                                    .ThenInclude(sb => sb.Battle)
                                           .SingleOrDefault(s => s.Id == 3);
            }

            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);

            samurai.SamuraiBattles.Remove(sbToRemove);

            //Não funciona, porque quando volta a dar track again o join e a battle já não fazem parte e o
            //change tracker não vai saber que o samuraibattle foi apagado e como tal não vai apagar na BD

            //_context.Attach(samurai);
            //_context.ChangeTracker.DetectChanges();

            _context.Remove(sbToRemove);
            _context.SaveChanges();
        }
        #endregion

        #region --- One to One ---
        private static void AddNewSamuraiWithSecretIdentity()
        {
            var samurai = new Samurai { Name = "Jina Ujichika"/*, BetterName = new PersonFullName(null, null)*/ };

            samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddSecretIdentityUsingSamuraiId()
        {
            //Note: SamuraiId 1 does not have a secret identity yet!
            var identity = new SecretIdentity { SamuraiId = 1, };

            _context.Add(identity);
            _context.SaveChanges(); 
        }

        private static void AddSecretIdentityToExistingSamurai()
        {
            Samurai samurai;

            using (var separateOperation = new SamuraiContext())
            {
                samurai = _context.Samurais.Find(2);
            }

            samurai.SecretIdentity = new SecretIdentity { RealName = "Julia" };

            _context.Samurais.Attach(samurai);
            _context.SaveChanges();
        }

        private static void EditASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                  .FirstOrDefault(s => s.Id == 1);

            samurai.SecretIdentity.RealName = "T'Challa";

            _context.SaveChanges();
        }

        private static void ReplaceASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                  .FirstOrDefault(s => s.Id == 1);

            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };

            _context.SaveChanges();
        }

        //Não funca
        private static void ReplaceASecretIdentityNotTracked()
        {
            Samurai samurai;

            using (var separateOperation = new SamuraiContext())
            {
                samurai = separateOperation.Samurais.Include(s => s.SecretIdentity)
                                           .FirstOrDefault(s => s.Id == 1);
            }

            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };

            _context.Samurais.Attach(samurai);
            //this will fail...EF Core tries to insert a duplicate samuraiID FK
            _context.SaveChanges();
        }

        //Não funca
        private static void ReplaceSecretIdentityNotInMemory()
        {
            //Does not work
            var samurai = _context.Samurais.FirstOrDefault(s => s.SecretIdentity != null);

            samurai.SecretIdentity = new SecretIdentity { RealName = "Bobbie Draper" };

            _context.SaveChanges();
        }
        #endregion

        #region --- Shadow Properties ---
        private static void CreateSamurai()
        {
            //, BetterName = new PersonFullName(null, null) to work with owned entity added later
            var samurai = new Samurai { Name = "Ronin"/*, BetterName = new PersonFullName(null, null)*/ };

            _context.Samurais.Add(samurai);

            var timestamp = DateTime.Now;

            _context.Entry(samurai).Property("Created").CurrentValue = timestamp;
            _context.Entry(samurai).Property("LastModified").CurrentValue = timestamp;

            _context.SaveChanges();
        }

        private static void RetrieveSamuraisCreatedInPastWeek()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            //var newSamurais = _context.Samurais
            //                          .Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
            //                          .ToList();

            var samuraisCreated = _context.Samurais
                                          .Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
                                          .Select(s => new { s.Id, s.Name, Created = EF.Property<DateTime>(s, "Created") })
                                          .ToList();
        }

        private static void CreateThenEditSamuraiWithQuote()
        {
            //, BetterName = new PersonFullName(null, null) to work with owned entity added later
            var samurai = new Samurai { Name = "Ronin"/*, BetterName = new PersonFullName(null, null)*/ };
            var quote = new Quote { Text = "Aren't I MARVELous?" };

            samurai.Quotes.Add(quote);

            _context.Samurais.Add(samurai);
            _context.SaveChanges();

            quote.Text += " See what I did there?";

            _context.SaveChanges();
        }
        #endregion

        #region --- Owned Types ---
        private static void GetAllSamurais()
        {
            var allsamurais = _context.Samurais.ToList();
        }
        
        private static void CreateSamuraiWithBetterName()
        {
            var samurai = new Samurai
            {
                Name = "Jack le Black",
                BetterName = PersonFullName.Create("Jack", "Black")
            };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        //Não funciona depois de ter tornado PersonFullName em um Value Object
        /*private static void RetrieveAndUpdateBetterName()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.BetterName.SurName == "Black");

            samurai.BetterName.GivenName = "Jill";

            _context.SaveChanges();
        }*/

        //Não funciona, pq o ChangeTracker fica confuso e vai tentar adicionar em vez de dar update para o novo
        private static void ReplaceBetterName_NãoFuncional()
        {
            var samurai = _context.Samurais.FirstOrDefault();

            samurai.BetterName = PersonFullName.Create("Shohreh", "Aghdashloo");

            _context.SaveChanges();
        }

        private static void ReplaceBetterName()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Jack le Bllanc");
            
            samurai.BetterName = PersonFullName.Create("Shohreh", "Aghdashloo");

            _context.Samurais.Update(samurai);
            _context.SaveChanges();
        }

        //Não funciona, e não me apetece perguntar porque (caso comente a partir do new e depois antes funciona
        private static void CreateAndFixUpNullBetterName()
        {
            _context.Samurais.Add(new Samurai { Name = "lol" });
            _context.SaveChanges();
            
            _context = new SamuraiContext();

            var persistedSamurai = _context.Samurais.FirstOrDefault(s => s.Name == "lol");
            if (persistedSamurai is null) { return; }
            if (persistedSamurai.BetterName.IsEmpty())
            {
                persistedSamurai.BetterName = null;
            }
        }
        #endregion

        #region --- Scalar Functions ---
        private static void RetrieveScalarResult()
        {
            var samurais = _context.Samurais
                .Select(s => new
                {
                    s.Name,
                    EarliestBattle = _context.EarliestBattleFoughtBySamurai(s.Id)
                })
                .ToList();
        }

        private static void FilterWithScalarResult()
        {
            var samurais = _context.Samurais
                .Where(s => EF.Functions.Like(_context.EarliestBattleFoughtBySamurai(s.Id), "%Battle%"))
                .Select(s => new
                {
                    s.Name,
                    EarliestBattle = _context.EarliestBattleFoughtBySamurai(s.Id)
                })
                .ToList();
        }
        
        private static void SortWithScalar()
        {
            var samurais = _context.Samurais
                 .OrderBy(s => _context.EarliestBattleFoughtBySamurai(s.Id))
                 .Select(s => new
                 {
                     s.Name,
                     EarliestBattle = _context.EarliestBattleFoughtBySamurai(s.Id)
                 })
                 .ToList();
        }

        private static void SortWithoutReturningScalar()
        {
            var samurais = _context.Samurais
                 .OrderBy(s => _context.EarliestBattleFoughtBySamurai(s.Id))
                 .ToList();
        }

        private static void RetrieveDaysInBattle()
        {
            var battles = _context.Battles
                .Select(b => new
                {
                    b.Name,
                    Days = _context.DaysInBattle(b.StartDate, b.EndDate)
                })
                .ToList();
        }

        private static int DateDiffDaysPlusOne(DateTime start, DateTime end)
        {
            return (int)end.Subtract(start).TotalDays + 1;
        }

        private static void RetrieveDaysInBattleWithoutDbFunction()
        {
            var battles = _context.Battles
                .Select(b => new
                {
                    b.Name,
                    Days = DateDiffDaysPlusOne(b.StartDate, b.EndDate)
                })
                .ToList();
        }

        private static void RetrieveYearUsingDbBuiltInFunction()
        {
            var battles = _context.Battles
                 .Select(b => new
                 {
                     b.Name,
                     b.StartDate.Year
                 })
                 .ToList();
        }
        #endregion

        #region --- Working with Database Views ---
        private static void GetStats()
        {
            var stats = _context.SamuraiStats.ToList();
        }

        private static void Filter()
        {
            var stats = _context.SamuraiStats
                .Where(s => s.SamuraiId == 2)
                .ToList();
        }

        private static void Project()
        {
            var stats = _context.SamuraiStats
                .Select(s => new
                {
                    s.Name,
                    s.NumberOfBattles
                })
                .ToList();
        }
        #endregion

        #endregion
    }
}