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
            DetailsRestaurant ViewModel = new DetailsRestaurant();
            string url = "OwnerData/GetOwner/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                ViewModel.Owner = httpResponse.Content.ReadAsAsync<OwnerDto>().Result;

                url = "restaurantData/GetRestaurantsByOwner/" + id;
                httpResponse = client.GetAsync(url).Result;
                ViewModel.Restaurants = httpResponse.Content.ReadAsAsync<IEnumerable<RestaurantDto>>().Result;
                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
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
        public ActionResult Create(Owner NewOwner)
        {
            string url = "ownerdata/AddOwner";
            
            HttpContent content = new StringContent(jss.Serialize(NewOwner));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;
            Debug.WriteLine(jss.Serialize(NewOwner));
            if (httpResponse.IsSuccessStatusCode)
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
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string url = "OwnerData/GetOwner/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                OwnerDto SelectedOwner = httpResponse.Content.ReadAsAsync<OwnerDto>().Result;
                return View(SelectedOwner);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Owner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Owner SelectedOwner)
        {
            string url = "ownerdata/updateowner/" + id;
            HttpContent content = new StringContent(jss.Serialize(SelectedOwner));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, content).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Owner/DeleteConfirm/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "OwnerData/GetOwner/" + id;
            HttpResponseMessage httpResponse = client.GetAsync(url).Result;

            if(httpResponse.IsSuccessStatusCode)
            {
                OwnerDto SelectedOwner = httpResponse.Content.ReadAsAsync<OwnerDto>().Result;
                return View(SelectedOwner);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Owner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "OwnerData/deleteowner/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage httpResponse = client.PostAsync(url,content).Result;

            if(httpResponse.IsSuccessStatusCode)
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
