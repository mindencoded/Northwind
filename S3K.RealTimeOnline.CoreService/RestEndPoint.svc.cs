using System.Collections.Generic;
using S3K.RealTimeOnline.CoreService.Contracts;
using S3K.RealTimeOnline.WebRole;
using Unity;

namespace S3K.RealTimeOnline.CoreService
{
    public class RestEndPoint : IRestEndPoint
    {
        private IUnityContainer _container;
        public RestEndPoint(IUnityContainer container)
        {
            _container = container;
        }

        public List<Players> GetPlayersXml()
        {
            return GetPlayers();
        }

        public List<Players> GetPlayersJson()
        {
            return GetPlayers();
        }

        private List<Players> GetPlayers()
        {
            List<Players> players = new List<Players>
            {
                new Players
                {
                    Country = "India",
                    Name = "Sachin Tendulkar",
                    Sports = "Cricket",
                    ImageUrl = "sachin.jpg"
                },
                new Players
                {
                    Country = "India",
                    Name = "MS Dhoni",
                    Sports = "Cricket",
                    ImageUrl = "dhoni.jpg"
                },
                new Players
                {
                    Country = "Australia",
                    Name = "Rickey Ponting",
                    Sports = "Cricket",
                    ImageUrl = "rickey.jpg"
                },
                new Players
                {
                    Country = "India",
                    Name = "Sandeep Singh",
                    Sports = "Hockey",
                    ImageUrl = "sandeep.jpg"
                },
            };
            return players;
        }
    }
}