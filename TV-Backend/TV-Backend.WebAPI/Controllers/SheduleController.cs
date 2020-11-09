using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TV_Backend.WebAPI.Controllers
{
    [Route("shedule")]
    public class SheduleController : Controller
    {
        private readonly IHttpClientFactory httpClient;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;

        public SheduleController(IHttpClientFactory httpClient,
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.env = env;
            this.configuration = configuration;
        }

        [HttpGet("bachelor")]
        public async Task<List<string>> GetBachelorShedule()
        {
            var client = httpClient.CreateClient();
            var responce = await client.GetAsync(new Uri(configuration.GetSection("URL")["Mirea.ru"]));
            var html = await responce.Content.ReadAsStringAsync();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var links = document.DocumentNode.SelectNodes("//a[@class='xls']");
            List<string> nodes = new List<string>();

            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.Contains("IIT") && !link.Attributes["href"].Value.Contains("mag"))
                {
                    var uriSegments = new Uri(link.Attributes["href"].Value).Segments;
                    var sheduleName = uriSegments[uriSegments.Length - 1];
                    var bytes = await client.GetByteArrayAsync(link.Attributes["href"].Value);
                    await System.IO.File.WriteAllBytesAsync(Path.Combine(env.ContentRootPath, $"Shedules/IIT/Bachelor/{sheduleName}"), bytes);

                    nodes.Add(link.Attributes["href"].Value);
                }
            }
            return nodes.Select(str => str).ToList();
        }

        [HttpGet("bachelor/exam")]
        public string GetBachelorExam()
        {
            return "";
        }

        [HttpGet("bachelor/offset")]
        public string GetBachelorOffset()
        {
            return "";
        }

        [HttpGet("master")]
        public async Task<List<string>> GetMasterShedule()
        {
            var client = httpClient.CreateClient();
            var responce = await client.GetAsync(new Uri(configuration.GetSection("URL")["Mirea.ru"]));
            var html = await responce.Content.ReadAsStringAsync();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var links = document.DocumentNode.SelectNodes("//a[@class='xls']")/*.Attributes["href"]*/;
            List<string> nodes = new List<string>();

            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.Contains("IIT") && link.Attributes["href"].Value.Contains("mag"))
                {
                    var uriSegments = new Uri(link.Attributes["href"].Value).Segments;
                    var sheduleName = uriSegments[uriSegments.Length - 1];
                    var bytes = await client.GetByteArrayAsync(link.Attributes["href"].Value);
                    await System.IO.File.WriteAllBytesAsync(Path.Combine(env.ContentRootPath, $"Shedules/IIT/Master/{sheduleName}"), bytes);

                    nodes.Add(link.Attributes["href"].Value);
                }
            }
            return nodes.Select(str => str).ToList();
        }

        [HttpGet("master/exam")]
        public string GetMasterExamShedule()
        {
            return "1234";
        }
    }
}