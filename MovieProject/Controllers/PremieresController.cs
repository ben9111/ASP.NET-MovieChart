using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieProject.DAL;
using MovieProject.Models;

using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;

namespace MovieProject.Controllers
{
    //Premiere controller
    public class PremieresController : Controller
    {
        //the db including the tables:Actors,Actor,trailers,Premieres that connecct to out db in app_data
        private MovieContext db = new MovieContext();

        // GET:return Premieres or filtered Premieres
        public ActionResult Index(string searchString)
        {
            //return all the rows in the table
            var name = from m in db.Premiere select m;

            //if the search string is not empty-we want to filter the Premieres
            if (!String.IsNullOrEmpty(searchString))
            {
                //select the rows that the Premiere's name contains the searchstring
                name = name.Where(s => s.PremiereName.Contains(searchString));
            }

            //return all the relevent rows
            return View(name);
        }

        // GET: return details of a specific Premiere
        //? means nullable
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premiere Premiere = db.Premiere.Find(id);
            if (Premiere == null)
            {
                return HttpNotFound();
            }
            return View(Premiere);
        }

        // GET: Premieres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Premieres/Create
        [HttpPost]
        //against attacks
        [ValidateAntiForgeryToken]
        //bind-wait for the paramters
        public ActionResult Create([Bind(Include = "ID,PremiereName,PremiereDate,locationName,locationX,locationY,participants")] Premiere Premiere)
        {
            //check all the paramters according to 
            if (ModelState.IsValid)
            {
                db.Premiere.Add(Premiere);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                TempData["alertMessage"] = "<script>alert('Error in input');</script>";

            return RedirectToAction("Index");
        }

        // GET: Premieres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premiere Premiere = db.Premiere.Find(id);
            if (Premiere == null)
            {
                return HttpNotFound();
            }
            return View(Premiere);
        }

        // POST: Premieres/Edit/5
        [HttpPost]
        //againts from attacks
        [ValidateAntiForgeryToken]
        //bind-wait for parameters
        public ActionResult Edit([Bind(Include = "ID,PremiereName,PremiereDate,locationName,locationX,locationY,participants")] Premiere Premiere)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Premiere).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                TempData["alertMessage"] = "<script>alert('Error in input');</script>";

            return RedirectToAction("Index");
        }

        // GET: Premieres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premiere Premiere = db.Premiere.Find(id);
            if (Premiere == null)
            {
                return HttpNotFound();
            }
            return View(Premiere);
        }

        // POST: Premieres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Premiere Premiere = db.Premiere.Find(id);
            db.Premiere.Remove(Premiere);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //destructor
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult getLocs()
        {
            // for the MAP 
            //put in the array the location  x ,y and name 
            Places[] js = new Places[db.Premiere.Count()];
            int i = 0;
            foreach (var item in db.Premiere)
            {
                Places p=new Places();
                p.placeName = item.locationName;
                p.placeX = item.locationX;
                p.placeY = item.locationY;
                js[i] = p;
                i++;
            }
            //convert to Json
            return Json(js, JsonRequestBehavior.AllowGet);
        }

    }
    public class Places
    {
        // use for the Json
        public string placeName { get; set; }
        public float placeX { get; set; }
        public float placeY { get; set; }
    }
}