using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace starwars
{

    //MAKE SURE CLASSES MATCH UP THE DOWNLOADED STRING FROM API
    //USED JSON2CSHARP ONLINE TOOL FOR CLASS BUILD 

    public class People
    {
        public string name { get; set; }
        public string height { get; set; }
        public string mass { get; set; }
        public string hair_color { get; set; }
        public string skin_color { get; set; }
        public string eye_color { get; set; }
        public string birth_year { get; set; }
        public string gender { get; set; }
        public string homeworld { get; set; }
        public List<string> films { get; set; }
        public List<string> species { get; set; }
        public List<string> vehicles { get; set; }
        public List<string> starships { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }
        public string url { get; set; }
        public string API = "https://swapi.co/api/people";


    }

    public class PeopleResult
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<People> results { get; set; }

    }

    public class StarshipResult
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<StarShip> results { get; set; }

    }


    public class StarShip
    {
        public string name { get; set; }
        public string model { get; set; }
        public string manufacturer { get; set; }
        public string cost_in_credits { get; set; }
        public string length { get; set; }
        public string max_atmosphering_speed { get; set; }
        public string crew { get; set; }
        public string passengers { get; set; }
        public string cargo_capacity { get; set; }
        public string consumables { get; set; }
        public string hyperdrive_rating { get; set; }
        public string MGLT { get; set; }
        public string starship_class { get; set; }
        public List<object> pilots { get; set; }
        public List<string> films { get; set; }
        public DateTime created { get; set; }
        public DateTime edited { get; set; }
        public string url { get; set; }
    }


    // STRATEGY TO KNOW IF ITS EITHER A PERSON LIST OR STARSHIP LIST 
    // FROM HERE ITS POSSIBLE TO ADD MORE TO EACH STRATEGY, LIKE PARTICULAR METHODS SUCH AS SERACH OR CALCULATE SOMETHNG 
    public interface IStrategy
    {
        string GetTypeofCharacter();
    }

    public class SpaceShipStrategy : IStrategy
    {
        string IStrategy.GetTypeofCharacter()
        {

            //Request API FOR STARSHIPS 

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create("https://swapi.co/api/starships");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();



            StarshipResult objects = JsonConvert.DeserializeObject<StarshipResult>(result);
            string starships = string.Empty;
            foreach(StarShip resultsItem in objects.results)
            {
                starships += "\nName: " + resultsItem.name + "\n" +
                    " Cost in Credits: \n" + resultsItem.cost_in_credits.ToString() + "\n" +
                    "Crew: " + resultsItem.crew + "\n";
            }

            return starships;

        }
           
        }
    

    public class PersonStrategy : IStrategy
   {
        string IStrategy.GetTypeofCharacter()
        {
            //Request API FOR People

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create("https://swapi.co/api/people");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();

            PeopleResult objects = JsonConvert.DeserializeObject<PeopleResult>(result);
            string people = string.Empty;

            foreach (People resultsItem in objects.results)
            {
                people += "\nName: " + resultsItem.name + "\n" + " Height: " + resultsItem.height 
                    + "\n" +"BirthYear: "+ resultsItem.birth_year +"\n";

            }


            return people;

        }
    }


public class newCall
    {
        public string Name { get; set; }

        public IStrategy CurrentStrategy;

        public newCall(IStrategy NewStrategy)
        {
            CurrentStrategy = NewStrategy;
        }

        public string getCharacter()
        {
           return CurrentStrategy.GetTypeofCharacter();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            Console.Title = "Strategy Pattern";
            Console.WindowHeight = (int)(Console.LargestWindowHeight * 0.75);
            Console.BufferHeight = 5000;
            Console.WriteLine("\n Create and display Star Wars characters from SWAPI.co .\n");

            

            newCall objCharacter = new newCall(null);

            int menuChoice = 0;

            while (menuChoice != 3)
            {
                Console.WriteLine("\n Create and display Star Wars characters from Star Wars API (SWAPI.CO .\n");
                Console.WriteLine("1. Spaceships\n");
                Console.WriteLine("2. People\n");
                Console.WriteLine("3. Bail\n");


                //  objCharacter.Name= "Spaceship";

                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        Console.Clear();
                        objCharacter.CurrentStrategy = new SpaceShipStrategy();
                        Console.WriteLine(objCharacter.getCharacter());

                        break;

                    case 2:
                        Console.Clear();
                        objCharacter.CurrentStrategy = new PersonStrategy();
                        Console.WriteLine(objCharacter.getCharacter());
                        break;

                    case 3:
                        return;

                }

            }
            


            Console.ReadKey();
        }
    }
}
