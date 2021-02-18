using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RestoWebApp.Models;

namespace RestoWebApp.Controllers
{
    public class OwnerController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static OwnerController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44375/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }

        // GET: Owner/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Owner/List 
        public ActionResult List()
        {
            string url = "ownerdata/getowners";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<OwnerDto> OwnerList = httpResponse.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;
                return View(OwnerList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Owner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        public ActionResult Create(Owner OwnerInfo)
        {
            string url = "ownerdata/create";
            HttpContent content = new StringContent(jss.Serialize(OwnerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                int OwnerID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = OwnerID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Owner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Owner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Owner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Owner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
