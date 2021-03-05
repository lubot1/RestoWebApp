using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // GET: Restaurant/Menu/5
        public ActionResult Menu(int id)
        {
            string url = "fooditemdata/getfooditemsbyrestaurant/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<FoodItemDto> FoodItemList = httpResponse.Content.ReadAsAsync<IEnumerable<FoodItemDto>>().Result;

                url = "restaurantdata/findrestaurant/" + id;
                httpResponse = client.GetAsync(url).Result;
                RestaurantDto selectedRestaurant = httpResponse.Content.ReadAsAsync<RestaurantDto>().Result;

                ViewBag.RestaurantName = selectedRestaurant.RestaurantName;
                return View(FoodItemList);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Restaurant/List
        public ActionResult List()
        {
            // Set the scope of these two important parameters
            string url;
            HttpResponseMessage httpResponse;

            string id = Request.QueryString["RestaurantCategoryID"];
            // Checks if a get parameter exists and loads appropriate data
            if(id == null || id == "") {
                url = "restaurantdata/getrestaurants";
                httpResponse = client.GetAsync(url).Result;
            }
            else
            {
                url = "restaurantdata/GetRestaurantsByCategory/" + id;
                httpResponse = client.GetAsync(url).Result;
            }

            if (httpResponse.IsSuccessStatusCode)
            {
                IEnumerable<RestaurantDto> RestaurantList = httpResponse.Content.ReadAsAsync<IEnumerable<RestaurantDto>>().Result;

                ViewBag.RestaurantList = RestaurantList;

                url = "restaurantcategorydata/getrestaurantcategories";
                httpResponse = client.GetAsync(url).Result;
                ViewBag.RestaurantCategoryList = httpResponse.Content.ReadAsAsync<IEnumerable<RestaurantCategoryDto>>().Result;

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
                UpdateRestaurant ViewModel = new UpdateRestaurant();
                ViewModel.OwnersList = httpResponse.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;

                url = "restaurantcategorydata/getrestaurantcategories";
                httpResponse = client.GetAsync(url).Result;
                ViewModel.RestaurantCategoryList = httpResponse.Content.ReadAsAsync<IEnumerable<RestaurantCategoryDto>>().Result;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Restaurant/Create
        [HttpPost]
        public ActionResult Create(RestaurantDto NewRestaurantData)
        {
            Restaurant NewRestaurant = new Restaurant
            {
                RestaurantID = NewRestaurantData.RestaurantID,
                RestaurantAddress = NewRestaurantData.RestaurantAddress,
                RestaurantName = NewRestaurantData.RestaurantName,
                RestaurantPhone = NewRestaurantData.RestaurantPhone,
                RestaurantCategoryID = NewRestaurantData.RestaurantCategoryID
            };

            string url = "restaurantdata/addrestaurant";
            HttpContent content = new StringContent(jss.Serialize(NewRestaurant));
            Debug.WriteLine(jss.Serialize(NewRestaurantData));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url,content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                int RestaurantID = httpResponse.Content.ReadAsAsync<int>().Result;
                NewRestaurantData.RestaurantID = RestaurantID;

                url = "restaurantdata/associaterestaurantowner";
                content = new StringContent(jss.Serialize(NewRestaurantData));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpResponse = client.PostAsync(url, content).Result;
                //Debug.WriteLine(httpResponse);
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
            // Send a request to findrestaurant method to pull info
            string url = "restaurantdata/findrestaurant/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                UpdateRestaurant ViewModel = new UpdateRestaurant();

                ViewModel.Restaurant = httpResponse.Content.ReadAsAsync<RestaurantDto>().Result;

                url = "restaurantcategorydata/getrestaurantcategories";
                httpResponse = client.GetAsync(url).Result;
                ViewModel.RestaurantCategoryList = httpResponse.Content.ReadAsAsync<IEnumerable<RestaurantCategoryDto>>().Result;

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
            Debug.WriteLine(jss.Serialize(SelectedRestaurant));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                //int RestaurantID = httpResponse.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = id });
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
