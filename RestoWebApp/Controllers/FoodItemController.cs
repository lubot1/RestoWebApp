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
    public class FoodItemController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static FoodItemController ()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44375/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: FoodItem
        public ActionResult Index()
        {
            return View();
        }
        // GET: FoodItem/List
        public ActionResult List()
        {
            string url = "fooditemdata/getfooditems";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<FoodItemDto> FoodItemList = httpResponse.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;
                return View(FoodItemList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: FoodItem/Details/5
        public ActionResult Details(int id)
        {
            string url = "fooditemdata/findfooditem/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                FoodItemDto SelectedFoodItem = httpResponse.Content.ReadAsAsync<FoodItemDto>().Result;
                return View(SelectedFoodItem);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: FoodItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodItem/Create
        [HttpPost]
        public ActionResult Create(FoodItem NewFoodItem)
        {
            string url = "fooditemdata/addfooditem";
            HttpContent content = new StringContent(jss.Serialize(NewFoodItem));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                int FoodItemID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = FoodItemID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: FoodItem/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "fooditemdata/findfooditem/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                FoodItemDto SelectedFoodItem = httpResponse.Content.ReadAsAsync<FoodItemDto>().Result;
                return View(SelectedFoodItem);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: FoodItem/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FoodItem SelectedFoodItem)
        {
            string url = "fooditemdata/updatefooditem/" + id;
            HttpContent content = new StringContent(jss.Serialize(SelectedFoodItem));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                int FoodItemID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = FoodItemID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: FoodItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FoodItem/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FoodItem SelectedFoodItem)
        {
            string url = "fooditemdata/deletefooditem/" + id;
            HttpContent content = new StringContent(jss.Serialize(SelectedFoodItem));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                int FoodItemID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        
        public ActionResult Error()
        {
            return View();
        }
    }
}
