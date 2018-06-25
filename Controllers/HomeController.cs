using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
namespace weddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        
        private LoginContext _context;
        
        public HomeController(LoginContext context){
            _context=context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpGet]
        [Route("redirectlogin")]
        public IActionResult redirectlogin()
        {
            return View("Index");
        }

        [HttpPost]
        [Route("validation")]
        public IActionResult Validation(User user,string Confirm)
        {
          
           if(ModelState.IsValid){
                if(user.Password==Confirm){

                ViewBag.error="";

                PasswordHasher<User> Hasher=new PasswordHasher<User>();
                user.Password=Hasher.HashPassword(user,user.Password);

                _context.Add(user);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FirstName);

                return RedirectToAction("Success");
                }
                else{
                    ViewBag.error="Password & confirm password don't match!";
                    return View("Index");
                }

           }
           else{
               return View("Index");
           }

        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            ViewData["Name"] = HttpContext.Session.GetString("UserName");

            int? ID=HttpContext.Session.GetInt32("UserId");

            ViewBag.id=ID;

            return View("Success");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string lemail,string lpassword)
        {
            
            User ReturnedValue = _context.users.SingleOrDefault(user => user.Email == lemail);
            if(ReturnedValue != null && lpassword != null)
            {
                var Hasher = new PasswordHasher<User>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(ReturnedValue, ReturnedValue.Password,lpassword))
                {
                    ViewBag.loginerror="";
                    HttpContext.Session.SetString("UserName", ReturnedValue.FirstName);
                    HttpContext.Session.SetInt32("UserId", ReturnedValue.Id);
                    return RedirectToAction("Success");
                    
                }
                else
                {
                    ViewBag.loginerror="Something went wrong...Try again";
                    return View("Index");
                }
            }
            else{
                ViewBag.loginerror="Both fields are required!";
                return View("Index");
            }
            
        }

        [HttpGet]
        [Route("logout")]

        public IActionResult logout(){
            System.Console.WriteLine("Hey there *********************************");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            List<Wedding> wedding=_context.weddings.Include(u=>u.myuser).Include(u=>u.guests).ToList();            

            ViewBag.Weddings=wedding; 
            ViewBag.id=(int)HttpContext.Session.GetInt32("UserId");
            foreach(var weddin in wedding)
            {
                System.Console.WriteLine("Guest counts {0}",weddin.guests.Count);
                if(weddin.guests.Exists(x=>x.UserId ==ViewBag.id))
                {
                    System.Console.WriteLine("Unrsvp&&&&&&&&&");
                }
                else{
                    System.Console.WriteLine("Rsvp###################");
                }
            }
            return View("Dashboard");
        }

        [HttpPost]
        [Route("weddingdetails")]
        public IActionResult weddingdetails(int wedding_id){

            int userId = (int)HttpContext.Session.GetInt32("UserId");
            Wedding weds=_context.weddings.SingleOrDefault(Wedding=>Wedding.Id==wedding_id);
            ViewBag.Weds=weds;
            List<Guest> AllGuest=_context.guest.Include(Guest=>Guest.myUser).Include(Guest=>Guest.weddings).Where(Guest=>Guest.WeddingId==wedding_id).ToList();
            List<string> guest=new List<string>();
            foreach(var gest in AllGuest)
            {
                guest.Add(gest.myUser.FirstName+gest.myUser.LastName);
            } 
            ViewBag.what=AllGuest;
            ViewBag.guest=guest ;    
            return View("Guestlist");
        }

        [HttpPost]
        [Route("rsvp")]
        public IActionResult rsvp(int wedding_id){
           
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            Guest newGuest = new Guest();
            newGuest.WeddingId = wedding_id;
            newGuest.UserId = userId;
            _context.Add(newGuest);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("unrsvp")]
        public IActionResult unrsvp(int wedding_id){

            int userId = (int)HttpContext.Session.GetInt32("UserId");
            Guest deleteGuest=_context.guest.Where(x=>x.WeddingId==wedding_id).SingleOrDefault(Guest=>Guest.UserId==userId);
            _context.guest.Remove(deleteGuest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult delete(int wedding_id){

            int? UserId = HttpContext.Session.GetInt32("userid");
            Wedding Yourwedding = _context.weddings.SingleOrDefault(x=>x.Id == wedding_id);
            _context.weddings.Remove(Yourwedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("AddWedding")]
        public IActionResult AddWedding(Wedding wedding)
        {
            wedding.UserId=(int)HttpContext.Session.GetInt32("UserId");
            _context.Add(wedding);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
