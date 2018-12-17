using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PersonFullName BetterName { get; set; }
        public List<Quote> Quotes { get; set; }
        //public int BattleId { get; set; }
        public List<SamuraiBattle> SamuraiBattles { get; set; }
        public SecretIdentity SecretIdentity { get; set; }

        public Samurai()
        {
            Quotes = new List<Quote>();
            SamuraiBattles = new List<SamuraiBattle>();
        }
        
        ///example of forcing the SecretIdentity is required rule
        ///in your business logic since EF Core can't map or enforce that
        ///in this case the parameterless constructor would need to be private
        //public Samurai(string publicName, string secretName) : this()
        //{
        //    Name = publicName;
        //    SecretIdentity = new SecretIdentity { RealName = secretName };
        //}

        //For GetSamuraiWithBattlesV2()
        public List<Battle> Battles()
        {
            var battles = new List<Battle>();

            foreach (var join in SamuraiBattles)
            {
                battles.Add(join.Battle);
            }

            return battles;
        }
    }
}
