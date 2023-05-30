using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NRLAdmin.Models;

namespace NRLAdmin.Controllers
{
    public class PerformsController : Controller
    {
        private NRLEntities db = new NRLEntities();

        // GET: Performs
        public ActionResult Index()
        {
            var performs = db.Performs.Include(p => p.Team).OrderByDescending(p => p.Points);
            return View(performs.ToList());
        }

        public ActionResult Ladder(NRLAdmin.Models.Ladder modelLadder)
        {
            //var performs = db.Performs.Include(p => p.Team);

            //var performsLadder = db.Performs.GroupBy(p => p.TeamID).Select(g => new
            // {
            //     //PerformID = g.Max(pc => pc.PerformID).ToString(),
            //     TeamID = g.Max(pc => pc.TeamID).ToString(),
            //     Points = g.Sum(pc => pc.Points).ToString(),
            //     Wins = g.Sum(pc => (Int32)(pc.Wins)).ToString(),
            //     Draws = g.Sum(pc => (Int32)(pc.Draws)).ToString(),
            //     Losts = g.Sum(pc => (Int32)(pc.Losts)).ToString()
            // } ).AsEnumerable();

            //return View(performsLadder.ToList());

            modelLadder.GetLadderInfo();
            return View(modelLadder);
        }

        // GET: Performs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perform perform = db.Performs.Find(id);
            if (perform == null)
            {
                return HttpNotFound();
            }
            return View(perform);
        }

        // GET: Performs/Create
        public ActionResult Create()
        {
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName");
            return View();
        }

        // POST: Performs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PerformID,TeamID,Points,Wins,Draws,Losts")] Perform perform)
        {
            if (ModelState.IsValid)
            {
                db.Performs.Add(perform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", perform.TeamID);
            return View(perform);
        }

        // GET: Performs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perform perform = db.Performs.Find(id);
            if (perform == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", perform.TeamID);
            return View(perform);
        }

        // POST: Performs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PerformID,TeamID,Points,Wins,Draws,Losts")] Perform perform)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamID = new SelectList(db.Teams, "TeamID", "TeamName", perform.TeamID);
            return View(perform);
        }

        // GET: Performs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perform perform = db.Performs.Find(id);
            if (perform == null)
            {
                return HttpNotFound();
            }
            return View(perform);
        }

        // POST: Performs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Perform perform = db.Performs.Find(id);
            db.Performs.Remove(perform);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
