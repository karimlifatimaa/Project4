using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class TeamController : Controller
    {
        private readonly ITeamServices _services;

        public TeamController(ITeamServices services)
        {
            _services = services;
        }

        public IActionResult Index()
        {
            var teams = _services.GetAllTeams();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(team == null)
            {
                return NotFound();
            }
            try
            {
                _services.AddTeam(team);
            }
            catch (EntityNullException ex)
            {

                ModelState.AddModelError("",ex.Message);
                return View();
            }
            catch(FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileSizeErrorException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var team=_services.GetTeam(x=>x.Id == id);
            if(team == null)
            {
                return NotFound();
            }
            try
            {
                _services.RemoveTeam(id);
            }
            catch (EntityNullException ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var team=_services.GetTeam(x=> x.Id == id); 
            if(team == null)
            {
                return NotFound();
            }
            return View(team);
        }
        [HttpPost]
        public IActionResult Update(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(team == null)
            {
                return NotFound();
            }
            try
            {
                _services.UpdateTeam(team.Id, team);
            }
            catch (EntityNullException ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (FileSizeErrorException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
