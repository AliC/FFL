using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FFL.Data;

namespace FFL.UI.Web.Mvc.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _repository;

        public PlayerController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        // GET: Player
        public async Task<ActionResult> Index()
        {
            IEnumerable<Data.Player> players = await _repository.Get();
            IEnumerable<Models.Player> playerModels = Map(players);

            return View(playerModels);
        }

        private IEnumerable<Models.Player> Map(IEnumerable<Data.Player> players)
        {
            return players.Select(p => new Models.Player {Name = p.first_name + " " + p.second_name});
        }

        // GET: Player/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Player/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Player/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Player/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Player/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Player/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}