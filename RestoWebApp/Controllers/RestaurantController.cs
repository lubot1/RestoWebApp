using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RestoWebApp.Models;
using RestoWebApp.Models.ViewModels;

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

        // POST: Restaurant/Create
        [HttpPost]
        public ActionResult Create(Restaurant NewRestaurant)
        {
            string url = "restaurantdata/addrestaurant";
            HttpContent content = new StringContent(jss.Serialize(NewRestaurant));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url,content).Result;

            // Below is an idea to create a conenction in the ownersrestaurant bridging table
            //string url = "restaurantdata/associaterestaurantowner";
            //HttpContent content = new StringContent(jss.Serialize(NewRestaurant.));
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpResponseMessage httpResponse2 = client.PostAsync(url, content).Result;

            //if (httpResponse.IsSuccessStatusCode && httpResponse2.IsSuccessStatusCode)
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

        // GET: Restaurant/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Instantiate class to collect necessary edit info
            UpdateRestaurant ViewModel = new UpdateRestaurant();
            // Send a request to findrestaurant method to pull info
            string url = "restaurantdata/findrestaurant/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                RestaurantDto SelectedRestaurant = httpResponse.Content.ReadAsAsync<RestaurantDto>().Result;
                ViewModel.Restaurant = SelectedRestaurant;

                // Find owners of the restaurant and add to the view model
                //url = "restaurantdata/findrestaurantowners/" + id;
                //httpResponse = client.GetAsync(url).Result;
                //IEnumerable<OwnerDto> RestaurantOwners = httpResponse.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;
                //ViewModel.RestaurantOwners = RestaurantOwners;

                // Find a list of all owners for 
                url = "ownerdata/getowners/" + id;
                httpResponse = client.GetAsync(url).Result;
                IEnumerable<OwnerDto> OwnersList = httpResponse.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;
                ViewModel.OwnersList = OwnersList;

                return View(ViewModel);
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
            string url = "restaurantdata/deleterestaurant/" + id;
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
