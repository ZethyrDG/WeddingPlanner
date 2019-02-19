using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingContext dbContext;
        public HomeController(WeddingContext context)
        {
            dbContext = context;
        }
        
        [Route("")]
        [HttpGet]
        public IActionResult RegisterPage()
        {
            return View("Register");
        }
        [Route("")]
        [HttpPost]
        public IActionResult Register(User NewUser)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.user.Any(u => u.Email == NewUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Register");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                    dbContext.user.Add(NewUser);
                    dbContext.SaveChanges();
                    User ThisUser = dbContext.user.FirstOrDefault(u => u.Email == NewUser.Email);
                    HttpContext.Session.SetInt32("UserId",ThisUser.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Register");
            }
        }
        [Route("login")]
        [HttpGet]
        public IActionResult LoginPage()
        {
            return View("Login");
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(User CheckUser)
        {
            var ThisUser = dbContext.user.FirstOrDefault(u => u.Email == CheckUser.Email);
            if(ThisUser == null)
            {
                ModelState.AddModelError("Email", "That email address does not exist.");
                return View("Login");
            }
            if(CheckUser.Password == null)
            {
                return View("Login");
            }
            var Hasher = new PasswordHasher<User>();
            var Result = Hasher.VerifyHashedPassword(CheckUser, ThisUser.Password, CheckUser.Password);
            if(Result == 0){
                ModelState.AddModelError("Password", "Invalid Password.");
                return View("Login");
            }
            HttpContext.Session.SetInt32("UserId",ThisUser.UserId);
            return RedirectToAction("Dashboard");
        }
        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }
        [Route("dashboard")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("LoginPage");
            }
            List<Wedding> AllWeddings = dbContext.wedding
                .Include(w => w.GuestList)
                .ThenInclude(g => g.User)
                .ToList();
            foreach(var wedding in AllWeddings)
            {
                if(wedding.Date < DateTime.Now)
                {
                    dbContext.wedding.Remove(wedding);
                    dbContext.SaveChanges();
                }
                User User1 = dbContext.user.FirstOrDefault(u => u.UserId == wedding.Wedder1Id);
                User User2 = dbContext.user.FirstOrDefault(u => u.UserId == wedding.Wedder2Id);
                wedding.Wedder1 = User1;
                wedding.Wedder2 = User2;
            }
            int? ThisUserId = HttpContext.Session.GetInt32("UserId");
            User ThisUser = dbContext.user.FirstOrDefault(u => u.UserId == ThisUserId);
            ViewBag.ThisUser = ThisUser;
            ViewBag.AllWeddings = AllWeddings;
            return View("Dashboard");
        }
        [Route("newwedding")]
        [HttpGet]
        public IActionResult NewWedding()
        {
            return View("NewWedding");
        }
        [Route("newwedding")]
        [HttpPost]
        public IActionResult AddWedding(Wedding NewWedding)
        {
            User User1 = dbContext.user.FirstOrDefault(u => u.FirstName == NewWedding.Wedder1Name);
            User User2 = dbContext.user.FirstOrDefault(u => u.FirstName == NewWedding.Wedder2Name);
            NewWedding.Wedder1 = User1;
            NewWedding.Wedder2 = User2;
            dbContext.wedding.Add(NewWedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [Route("wedding/{id}")]
        [HttpGet]
        public IActionResult OneWedding(int id)
        {
            Wedding ThisWedding = dbContext.wedding
                .Include(w => w.GuestList)
                .ThenInclude(g => g.User)
                .FirstOrDefault(w => w.WeddingId == id);
            User User1 = dbContext.user.FirstOrDefault(u => u.UserId == ThisWedding.Wedder1Id);
            User User2 = dbContext.user.FirstOrDefault(u => u.UserId == ThisWedding.Wedder2Id);
            ThisWedding.Wedder1 = User1;
            ThisWedding.Wedder2 = User2;
            ViewBag.ThisWedding = ThisWedding;
            return View("OneWedding");
        }
        [Route("rsvp/{id}")]
        [HttpGet]
        public IActionResult RSVP(int id)
        {
            int? ThisUserId = HttpContext.Session.GetInt32("UserId");
            User ThisUser = dbContext.user.FirstOrDefault(u => u.UserId == ThisUserId);
            Wedding ThisWedding = dbContext.wedding.FirstOrDefault(w => w.WeddingId == id);
            Guests NewGuest = new Guests();
            NewGuest.UserId = ThisUser.UserId;
            NewGuest.WeddingId = ThisWedding.WeddingId;
            dbContext.guests.Add(NewGuest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [Route("unrsvp/{id}")]
        [HttpGet]
        public IActionResult UnRSVP(int id)
        {
            int? ThisUserId = HttpContext.Session.GetInt32("UserId");
            User ThisUser = dbContext.user.FirstOrDefault(u => u.UserId == ThisUserId);
            Guests ThisRSVP = dbContext.guests.FirstOrDefault(g => g.UserId == ThisUserId && g.WeddingId == id);
            dbContext.guests.Remove(ThisRSVP);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [Route("delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Wedding ThisWedding = dbContext.wedding.FirstOrDefault(w => w.WeddingId == id);
            dbContext.wedding.Remove(ThisWedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
