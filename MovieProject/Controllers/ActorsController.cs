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
using System.Drawing;


namespace MovieProject.Controllers
{
    public class ActorsController : Controller
    {
        private MovieContext db = new MovieContext(); //Create a new DB that connecct to out db in app_data
       

        // GET: Actors
        public ActionResult Index(string Category, string gender, string ActorName)
        {
            // make a list of the parameters from the DB
            var CategoryList = new List<string>();
            var CategoryQry = from d in db.Actor orderby d.catagory select d.catagory;
            CategoryList.AddRange(CategoryQry.Distinct());
            ViewBag.Category = new SelectList(CategoryList);

            var GenderList = new List<string>();
            var GenderQry = from g in db.Actor orderby g.gender select g.gender;
            GenderList.AddRange(GenderQry.Distinct());
            ViewBag.Gender = new SelectList(GenderList);


            var Actors = from s in db.Actor select s;
            // if not null make a filter
            if (!String.IsNullOrEmpty(ActorName))
            {
                Actors = Actors.Where(s => s.ActorName.Contains(ActorName));
            }
            if (!String.IsNullOrEmpty(gender))
            {
                Actors = Actors.Where(s => s.gender.Equals(gender)); //ActorName.Contains(ActorString));
            }
            if (!String.IsNullOrEmpty(Category))
            {
                Actors = Actors.Where(r => r.catagory == Category);
            }
            return View(Actors);
        }


        //GET
        public ActionResult EditMovie(int? MovieId, int? TrailerId)
        {
            // send the Movie of the id that send to a EDIT view

            if ((TrailerId == null) ||(MovieId==null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trailer Trailer = db.Trailer.Find(TrailerId);
            if (Trailer == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            foreach (var Movie in Trailer.Movies)
            {
               
                        if (Movie.ID == MovieId)
                            return View(Movie);
                   
            }


            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



        }

        // Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMovie([Bind(Include = "ID,TrailerID,MovieName,writer,stars,videoLink,brief")] Movie Movie)
        {
            //wait for date that the user enter and change the Movie
            if (ModelState.IsValid)
            {
                db.Entry(Movie).State = EntityState.Modified;
                db.SaveChanges();

            }
            else
                TempData["alertMessage"] = "<script>alert('Error in input');</script>";
         
            return RedirectToAction("Index");
        }



        //GET
        public ActionResult EditTrailer(int? TrailerId ,int ? ActorId)
        {
            // send the trailer of the id that send to a EDIT view
        
            if ((TrailerId == null) || (ActorId==null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor Actor = db.Actor.Find(ActorId);
            if(Actor==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
           foreach (var item in Actor.Trailers)
           {
               if (item.ID == TrailerId)
                   return View(item);
           }

            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrailer([Bind(Include = "ID,ActorID,TrailerName,DirectorName,TrailerImage,releaseDate")] Trailer Trailer)
        {
            //wait for date that the user enter and change the disc
            // chake if valid , if not pop a alert
            if (ModelState.IsValid)
            {
                db.Entry(Trailer).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            else 
                TempData["alertMessage"] = "<script>alert('Error in input');</script>";
            return RedirectToAction("Index");
        }

        // remove Trailer // GET
        public ActionResult DeleteTrailer(int? TrailerId, int? ActorId)  
        {
            // delete the trailer from trailer table and from Actor table
            Actor Actor = db.Actor.Find(ActorId);
            Trailer Trailer=null;
            foreach (var item in Actor.Trailers)
            {
                if (item.ID == TrailerId)
                    Trailer = item;
            }
            Actor.Trailers.Remove(Trailer);

            db.Trailer.Remove(Trailer);
            db.SaveChanges();
            


            if (TrailerId == null || ActorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("Details"+"/"+ActorId); //after we delete we return to the normal page with the Actor ID and other Details about him with other movies
          
        }


        //Add Trailer POST
        public ActionResult AddTrailer([Bind(Include = "ID,ActorID,TrailerName,DirectorName,TrailerImage,releaseDate")] Trailer Trailer)
        {

            //wait for date that the user enter and add a trailer
            if (String.IsNullOrEmpty(Trailer.TrailerName) ||  String.IsNullOrEmpty(Trailer.TrailerImage))
            {
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>"; //we put it in the view area in actors->index
                return RedirectToAction("Index"); //return back to index
            }
            if (ModelState.IsValid)
            {
                db.Trailer.Add(Trailer);
                db.SaveChanges();
                db.Actor.Find(Trailer.ActorID).Trailers.Add(Trailer);
                db.SaveChanges();

                return RedirectToAction("Details" + "/" + Trailer.ActorID.ToString());
            }
            else
            {
                TempData["alertMessage"] = "<script>alert('Error in input');</script>";
                return RedirectToAction("Details" + "/" + Trailer.ActorID.ToString());
            }
        }

        //Add Movie
        public ActionResult AddMovie([Bind(Include = "ID,TrailerID,MovieName,writer,stars,videoLink,brief")] Movie Movie)
        {
            //wait for date that the user enter and add a Movie
            if (String.IsNullOrEmpty(Movie.MovieName))
            {
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                int ActorId;
          
                db.Movie.Add(Movie);
                db.SaveChanges();
                db.Trailer.Find(Movie.TrailerID).Movies.Add(Movie);
                db.SaveChanges();
               ActorId= db.Trailer.Find(Movie.TrailerID).ActorID;
               return RedirectToAction("Details" + "/" + ActorId);
               //return RedirectToAction("Details" + "/" + ActorId);
                 
              // return RedirectToAction("Movies?TrailerId="+Movie.TrailerID.ToString()+"&ActorId="+ActorId);
            }
            else
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>";
            return RedirectToAction("Index");
        }

        // remove Movie
        public ActionResult DeleteMovie(int? MovieId, int? TrailerId)
        {
            //delete the Movie from the Movie table and the disc table

            Trailer Trailer = db.Trailer.Find(TrailerId);
           Movie Movie = null;
            foreach (var item in Trailer.Movies)
            {
                if (item.ID == MovieId)
                    Movie = item;
            }
            Trailer.Movies.Remove(Movie);

            db.Movie.Remove(Movie);
            db.SaveChanges();

            int ActorId = db.Trailer.Find(Movie.TrailerID).ActorID;

            if (TrailerId == null || MovieId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("Details" + "/" + ActorId);
        }


        public ActionResult TrailerActorSearch(string ActorName, string category, string relDate)
        {

            // search trailers by parameters
            var trailers = from d in db.Trailer select d;
            var Actors = from s in db.Actor select s;

            if (trailers == null)
            {
                return HttpNotFound();
            }
            if ((String.IsNullOrEmpty(category)) && (String.IsNullOrEmpty(ActorName)) && (String.IsNullOrEmpty(relDate)))
            {
                return View(trailers.ToList());
            }

            if (!String.IsNullOrEmpty(category))
            {
                Actors = Actors.Where(s => s.catagory.Contains(category));
            }
            if (!String.IsNullOrEmpty(ActorName))
            {
                Actors = Actors.Where(s => s.ActorName.Contains(ActorName));
            }
            if (!String.IsNullOrEmpty(relDate))
            {
                DateTime date = Convert.ToDateTime(relDate);
                trailers = from d in trailers where DateTime.Compare(d.releaseDate, date) >= 0 select d;
                //trailers = trailers.Where(s => s.releaseDate );
            }

            var query = from d in trailers //connect JOIN between trailers and actor by equals they id
                        join s in Actors on d.ActorID equals s.ID
                        select d;



            return View(query.ToList());
        }


        //a search Trailer page
        public ActionResult TrailerSearch(string TrailerName, string DirectorName, string relDate)
        {
            // search trailers by parameters
            var trailers = from d in db.Trailer select d;
            if (trailers == null)
            {
                return HttpNotFound();
            }
            if (!String.IsNullOrEmpty(TrailerName))
            {
                trailers = trailers.Where(s => s.TrailerName.Contains(TrailerName));
            }
            if (!String.IsNullOrEmpty(DirectorName))
            {
                trailers = trailers.Where(s => s.DirectorName.Contains(DirectorName));
            }
            if (!String.IsNullOrEmpty(relDate))
            {
                DateTime date = Convert.ToDateTime(relDate);
                trailers = from d in trailers where DateTime.Compare(d.releaseDate,date)>=0 select d;
            }
            return View(trailers);
        }
    

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor Actor = db.Actor.Find(id);
            if (Actor == null)
            {
                return HttpNotFound();
            }
            return View(Actor);
        }

        // GET: Actors/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ActorName,gender,ActorImage,catagory")] Actor Actor)
        {
            if (String.IsNullOrEmpty(Actor.ActorName) || String.IsNullOrEmpty(Actor.gender) || String.IsNullOrEmpty(Actor.ActorImage) || String.IsNullOrEmpty(Actor.catagory))
            {
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Actor.Add(Actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>";

            return RedirectToAction("Index");
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            //Premiere to the user the Edit view
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor Actor = db.Actor.Find(id);
            if (Actor == null)
            {
                return HttpNotFound();
            }
            return View(Actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ActorName,gender,ActorImage,catagory")] Actor Actor)
        {
            if (String.IsNullOrEmpty(Actor.ActorName) || String.IsNullOrEmpty(Actor.gender) || String.IsNullOrEmpty(Actor.ActorImage) || String.IsNullOrEmpty(Actor.catagory))
            {
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>";
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Entry(Actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                TempData["alertMessage"] = "<script>alert('Hey!you forgot to put some fileds,cheak again');</script>";
            return RedirectToAction("Index");
        }

        // GET: Actors/Delete/5
        // GOES TO HERE if on the delete confirm the user click yes.
        // have different name because they are getting the same parameters
        public ActionResult Delete(int? id)
        {
            //delete Actor
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor Actor = db.Actor.Find(id);
            if (Actor == null)
            {
                return HttpNotFound();
            }
            return View(Actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // delete confirme
            Actor Actor = db.Actor.Find(id);
            db.Actor.Remove(Actor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            // what happend when you click 'X'
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Movies(int? TrailerId, int? ActorId) //this for top 10 movies.. link with they trailer ID and actor id
        {                                                         //mekabel data TrailerId and ActorId raz vebodek Actorid em ho nimza function shel Shelta TrailerActorSearch and TrailerSearch
            // return all Movies
            Actor Actor = db.Actor.Find(ActorId);
            Trailer Trailer = null;
            
            foreach (var item in Actor.Trailers)
            {
                if (item.ID == TrailerId)
                    Trailer = item;
            }
        
            if (TrailerId == null || ActorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(Trailer);
        }
        


        public JsonResult getCategors() //this function is for the PieChart in the Actors Category, JAVASCRIPT
        {

           
            // for the PIE graph 
            //put in the array the categoty , and count using group by the num of Actors in each category
            var random = new Random();
            /*var categoryList = new List<string>();
            var categoryQry = from d in db.Actor orderby d.catagory select d.catagory;
            categoryList.AddRange(categoryQry.Distinct());
             */
            var categ_num = from s in db.Actor //takes from db the information and lines and then do group by by category
                            group s by s.catagory into x
                            select new
                            {
                                category = x.Key,
                                num = x.Count()
                            };
            NumInCatagory[] js = new NumInCatagory[categ_num.Count()]; //after he count the number he using the function down with an arry
            int i = 0;
            foreach (var item in categ_num) //for each num he open an object and start to lemala oto
            {
                NumInCatagory p = new NumInCatagory();
                bool flag = false;
                var color = "0";
                p.label = item.category.ToString();
                p.value = item.num;
                // cheack if the color is not the same 
                do
                {
                    flag = false;
                    color = String.Format("#{0:X6}", random.Next(0x1000000)); //this function after refresh the colors change of the pai graph
                    for (int j = 0; j < i; j++)
                    {
                        if (color.Equals(js[j].color))
                            flag = true;
                           
                    }
                }
                while (flag);
                p.color = color.ToString();
                js[i] = p;
                i++;
            }
            // convert to jason and allow to user "get"
            return Json(js, JsonRequestBehavior.AllowGet); //return the result to view actors (in the index)
        } 
    }

 public class NumInCatagory
    {
        // the class using the array in Json
        public string label { get; set; }
        public int value { get; set; }
     public string color{get;set;}
    }


}
