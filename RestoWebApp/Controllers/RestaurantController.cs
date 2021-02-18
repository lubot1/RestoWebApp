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
    public class RestaurantController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static RestaurantController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44375/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }

        // GET: Restaurant/Details/5
        public ActionResult Details(int id)
        {
            string url = "restaurantdata/findrestaurant/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                RestaurantDto SelectedRestaurant = httpResponse.Content.ReadAsAsync<RestaurantDto>().Result;
                return View(SelectedRestaurant);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Restaurant/List
        public ActionResult List()
        {
            string url = "restaurantdata/getrestaurants";
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<RestaurantDto> RestaurantList = httpResponse.Content.ReadAsAsync<IEnumerable<RestaurantDto>>().Result;
                return View(RestaurantList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Restaurant/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
        [HttpPost]
        public ActionResult Create(Restaurant NewRestaurant)
        {
            string url = "restaurantdata/addrestaurant";
            HttpContent content = new StringContent(jss.Serialize(NewRestaurant));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url,content).Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                int RestaurantID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = RestaurantID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Restaurant/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string url = "restaurantdata/findrestaurant/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                RestaurantDto SelectedRestaurant = httpResponse.Content.ReadAsAsync<RestaurantDto>().Result;
                return View(SelectedRestaurant);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Restaurant/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Restaurant SelectedRestaurant)
        {
            string url = "restaurantdata/updaterestaurant/" + id;
            HttpContent content = new StringContent(jss.Serialize(SelectedRestaurant));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                int RestaurantID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = RestaurantID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Restaurant/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "restaurantdata/findrestaurant/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                RestaurantDto SelectedRestaurant = httpResponse.Content.ReadAsAsync<RestaurantDto>().Result;
                return View(SelectedRestaurant);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Restaurant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "OwnerData/deleterestaurant/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
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
