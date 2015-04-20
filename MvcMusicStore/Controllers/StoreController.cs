using MvcMusicStore.DataContexts;
using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        MusicDb storeDB = new MusicDb();

        // GET: Store
        public ActionResult Index()
        {
            List<Genre> genres = storeDB.Genres.ToList();
            return View(genres);
        }
        // GET: /Store/Browse?genre=bananas
        public ActionResult Browse(String genre)
        {
            // HtmlEncode is used to sanitize user input
            // This prevents javascript injection 
            //string str = HttpUtility.HtmlEncode("Store.Browse, Genre = " + genre);
            //var genreModel = new Genre { Name = genre };
            //return View(genreModel);

            // New way of doing things with DB
            var genreModel = storeDB.Genres.Include("Albums").Single(g => g.Name == genre);
            return View(genreModel);
        }
        // GET: /Store/Details/1
        public ActionResult Details(int id)
        {
            // Old way without db
            // var album = new Album { Title = "Album" + id };

            // New way with DB
            var album = storeDB.Albums.Find(id);
            return View(album);
        }
    }
}