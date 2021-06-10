using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using MySql.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebsiteAlexCamiel2.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;

namespace WebsiteAlexCamiel2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //string connectionString = "Server=172.16.160.21;Port=3306;Database=110407;Uid=110407;Pwd=inf2021sql;";
        string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=110407;Uid=110407;Pwd=inf2021sql;";

        public IActionResult Index()
        {
            {  // alle namen ophalen
                var films = GetFilms();

                // stop de namen in de html
                return View(films);
            }
            {
                ViewData["user"] = HttpContext.Session.GetString("User");
                return View();
            }
        }

        [Route("login")]
        public IActionResult Loginpagina(string username, string password)
        {
            if (password == "geheim")
            {
                HttpContext.Session.SetString("User", username);
                return Redirect("/");
            }

            return View();
        }

        private object Names
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<string> GetNames()
        {
            // stel in waar de database gevonden kan worden

            // maak een lege lijst waar we de namen in gaan opslaan
            List<string> names = new List<string>();

            // verbinding maken met de database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                // verbinding openen
                conn.Open();

                // SQL query die we willen uitvoeren
                MySqlCommand cmd = new MySqlCommand("select * from film", conn);

                // resultaat van de query lezen
                using (var reader = cmd.ExecuteReader())
                {
                    // elke keer een regel (of eigenlijk: database rij) lezen
                    while (reader.Read())
                    {
                        // selecteer de kolommen die je wil lezen. In dit geval kiezen we de kolom "naam"
                        string Name = reader["Naam"].ToString();

                        // voeg de naam toe aan de lijst met namen
                        names.Add(Name);
                    }
                }
            }

            // return de lijst met namen
            return names;
        }
       
        [Route("contact")]
        public IActionResult Contact(Person person)
        {
            //als alles goed ingevuld is --> succes pagina
            if (ModelState.IsValid){
                // alle benodigde gegevens zijn aanwezig, we kunnen opslaan!
                SavePerson(person);

                return Redirect("/succes");
            }
            //als niet alles goed ingevuld is --> terug 
            return View(person);
        }

        private void SavePerson(Person person)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO ethapklant(voornaam, achternaam, email, omschrijving) VALUES(?voornaam, ?achternaam, ?email, ?omschrijving)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.Text).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.Text).Value = person.Achternaam;
                cmd.Parameters.Add("?email", MySqlDbType.Text).Value = person.Email;
                cmd.Parameters.Add("?omschrijving", MySqlDbType.Text).Value = person.Omschrijving;
                cmd.ExecuteNonQuery();
            }
        }


        [Route("succes")]
        public IActionResult Succes()
        {
            return View();
        }

        [Route("actie")]
        public IActionResult Actie()
        {
            return View();
        }
        [Route("romantiek")]
        public IActionResult Romantiek()
        {
            return View();
        }
        [Route("kinder")]
        public IActionResult Kinder()
        {
            return View();
        }
        [Route("comedy")]
        public IActionResult Comedy()
        {
            return View();
        }
        [Route("film/{id}")]
        public IActionResult Film(string id)
        {
            
            return View(GetFilm(id));
        }

        private List<Films> GetFilms()
        {
            List<Films> films = new List<Films>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from film", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Films p = new Films
                        {
                            id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Poster = reader["Poster"].ToString()
                        };
                        films.Add(p);
                    }
                }
            }
            return films;
        }

        private Films GetFilm(string id)
        {
            List<Films> films = new List<Films>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from film where id = {id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Films p = new Films
                        {
                            id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString()
                        };
                        films.Add(p);
                    }
                }
            }
            return films[0];
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

