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

namespace TV_Backend.WebAPI.Controllers
{
    [Route("shedule")]
    public class SheduleController : Controller
    {
        private readonly IHttpClientFactory httpClient;
        private readonly IWebHostEnvironment env;

        public SheduleController(IHttpClientFactory httpClient, IWebHostEnvironment env)
        {
            this.httpClient = httpClient;
            this.env = env;
        }

        [HttpGet("bachelor")]
        public async Task<List<string>> GetBachelorShedule()
        {
            var client = httpClient.CreateClient();
            var responce = await client.GetAsync(new Uri("https://www.mirea.ru/education/schedule-main/schedule/"));
            var html = await responce.Content.ReadAsStringAsync();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var links = document.DocumentNode.SelectNodes("//a[@class='xls']")/*.Attributes["href"]*/;
            List<string> nodes = new List<string>();
            int count = 1;
            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.Contains("IIT"))
                {

                    nodes.Add(link.Attributes["href"].Value);
                    var bytes = await client.GetByteArrayAsync(link.Attributes["href"].Value);
                    await System.IO.File.WriteAllBytesAsync(Path.Combine(env.ContentRootPath, $"xls/{count}.xlsx"), bytes);
                    count++;
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
        public string GetMasterShedule()
        {
            return "1234";
        }

        [HttpGet("master/exam")]
        public string GetMasterExamShedule()
        {
            return "1234";
        }

    }
}