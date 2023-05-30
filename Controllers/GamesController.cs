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
    public class GamesController : Controller
    {
        private NRLEntities db = new NRLEntities();

        // GET: Games
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.Team).Include(g => g.Team1);
            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            ViewBag.HomeTeamID = new SelectList(db.Teams, "TeamID", "TeamName");
            ViewBag.AwayTeamID = new SelectList(db.Teams, "TeamID", "TeamName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameID,GameDate,GameWeek,HomeTeamID,AwayTeamID,HomeTeamScore,AwayTeamScore")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeTeamID = new SelectList(db.Teams, "TeamID", "TeamName", game.HomeTeamID);
            ViewBag.AwayTeamID = new SelectList(db.Teams, "TeamID", "TeamName", game.AwayTeamID);
            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeTeamID = new SelectList(db.Teams, "TeamID", "TeamName", game.HomeTeamID);
            ViewBag.AwayTeamID = new SelectList(db.Teams, "TeamID", "TeamName", game.AwayTeamID);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameID,GameDate,GameWeek,HomeTeamID,AwayTeamID,HomeTeamScore,AwayTeamScore")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeTeamID = new SelectList(db.Teams, "TeamID", "TeamName", game.HomeTeamID);
            ViewBag.AwayTeamID = new SelectList(db.Teams, "TeamID", "TeamName", game.AwayTeamID);
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
