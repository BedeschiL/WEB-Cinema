﻿using DTO_DataTransferObject;
using GUImovieWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieWEB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieWEB.Controllers
{
    public class ActorDetailController : Controller
    {
        private readonly ILogger<FilmController> _logger;

        public ActorDetailController(ILogger<FilmController> logger)
        {
            _logger = logger;
        }
        public IActionResult ActorDetail(string id)
        {
            try
            {
                ActeurUImodel acm = new ActeurUImodel();

                using (var client = new WebClient())
                {
                    //Get a string representation of the Json
                    string route = ConfigurationManager.AppSettings["apiActeur"] + id + ConfigurationManager.AppSettings["apiKey"];
                    String rawJson = client.DownloadString(route);
                    var data = (JObject)JsonConvert.DeserializeObject(rawJson);
                    acm.Name = data["name"].Value<string>();
                    acm.resume = data["biography"].Value<string>();
                    acm.birthplace = data["place_of_birth"].Value<string>();
                    acm.birthday = data["birthday"].Value<string>();
                    var tempToken = data["also_known_as"] as JArray;
                    acm.avatar = ConfigurationManager.AppSettings["apiActeurPic"] + data["profile_path"].Value<string>();
                }
                string query = "actorsId?id=" + int.Parse(id);
                using (var client = new WebClient())
                {
                    //Get a string representation of the Json
                    String finalQuery = (ConfigurationManager.AppSettings["basicUrl"] + query);
                    String rawJson = client.DownloadString(finalQuery);
                    List<FilmDTO> temDTO = JsonConvert.DeserializeObject<List<FilmDTO>>(rawJson);
                    acm.Films = temDTO;
                    Console.WriteLine("countFilm" + temDTO.Count);
                    Trace.WriteLine("countFilm" + temDTO.Count);
                    
                }
                return View(acm);
            }
            catch(WebException wex)
            {

            }
            return null;

        }  
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
