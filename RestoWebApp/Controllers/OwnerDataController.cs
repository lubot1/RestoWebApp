using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestoWebApp.Models;

namespace RestoWebApp.Controllers
{
    public class OwnerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OwnerData/GetOwners
        [ResponseType(typeof(OwnerDto))]
        [HttpGet]
        public IEnumerable<OwnerDto> GetOwners()
        {
            List<Owner> Owners = db.Owners.ToList();
            List<OwnerDto> OwnerDtos = new List<OwnerDto> { };

            foreach(var Owner in Owners)
            {
                OwnerDto NewOwner = new OwnerDto
                {
                    OwnerID = Owner.OwnerID,
                    OwnerFirstName = Owner.OwnerFirstName,
                    OwnerLastName = Owner.OwnerLastName

                };
                OwnerDtos.Add(NewOwner);
            }
            return OwnerDtos;
        }

        // GET: api/OwnerData/GetOwner/5
        [ResponseType(typeof(OwnerDto))]
        [HttpGet]
        public IHttpActionResult GetOwner(int id)
        {
            Owner Owner = db.Owners.Find(id);
            OwnerDto SelectedOwner = new OwnerDto
            {
                OwnerID = Owner.OwnerID,
                OwnerFirstName = Owner.OwnerFirstName,
                OwnerLastName = Owner.OwnerLastName,
                OwnerEmail = Owner.OwnerEmail,
                OwnerAddress = Owner.OwnerAddress,
                OwnerPhone = Owner.OwnerPhone

            };

            if (Owner == null)
            {
                return NotFound();
            }

            return Ok(SelectedOwner);
        }

        // POST: api/Owners/UpdateOwner/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateOwner(int id, [FromBody]Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != owner.OwnerID)
            {
                return BadRequest();
            }

            db.Entry(owner).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Owners/AddOwner
        [ResponseType(typeof(Owner))]
        [HttpPost]
        public IHttpActionResult AddOwner([FromBody]Owner NewOwner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Owners.Add(NewOwner);
            db.SaveChanges();

            return Ok(NewOwner.OwnerID);
        }

        // Post: api/Owners/DeleteOwner/5
        [ResponseType(typeof(Owner))]
        [HttpPost]
        public IHttpActionResult DeleteOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            db.Owners.Remove(owner);
            db.SaveChanges();

            return Ok(owner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OwnerExists(int id)
        {
            return db.Owners.Count(e => e.OwnerID == id) > 0;
        }
    }
}