using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using HRIS.Models;
using HRIS.Data;

namespace HRIS.Controllers
{
    public class AdminController : Controller
    {
        private AppDbContext db;
        private readonly IConfiguration configuration;
        private readonly ILogger<AdminController> logger;

        public AdminController(AppDbContext db, IConfiguration conf, ILogger<AdminController> logger)
        {
            configuration = conf;
            this.db = db;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            var lAdmin = validateLoggedInAdmin();
            if (lAdmin != null)
                return Ok(lAdmin);

            else
                return View();
        }

        public IActionResult ValidateSuperAdmin()
        {
            return Ok();
        }

        public IActionResult Register(HRAdmin admin)
        {
            Console.WriteLine("{0}  {1}", admin.Email, admin.Password);

            if (string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Password))
                return Ok("NEGATIVE");

            var existData = from a in db.HRAdmin select a;
            var existUsername = from n in existData select n.Email;
            if (existUsername.Contains(admin.Email))
            {
                return Ok(new
                {
                    ERROR = "Email Exist!"
                });
            }

            admin.Password = Hashing.HashPassword(admin.Password);
            admin.CreatedAt = DateTime.Now;
            admin.UpdatedAt = DateTime.Now;

            db.HRAdmin.Add(admin);
            db.SaveChanges();

            return Ok();
        }

        public IActionResult Login(HRAdmin admin)
        {
            logger.LogInformation("Login attemp from {0}", admin.Email);

            var user = AuthenticatedUser(admin);

            if (user == null)
            {
                ViewData["loginInfo"] = "Email/ Password salah :(";
                return View("Index");
            }

            var token = generateJwtToken(user);

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetInt32("id", user.Id);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }


        private string generateJwtToken(HRAdmin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                // issuer: Configuration["Jwt:Issuer"],
                // audience: Configuration["Jwt:Audience"],
                null,
                null,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        public HRAdmin AuthenticatedUser(HRAdmin user_input)
        {
            var user = from _user in db.HRAdmin where _user.Email == user_input.Email select _user;

            if (user.FirstOrDefault() != null)
            {
                if (Hashing.ValidatePassword(user_input.Password, user.First().Password))
                    return user.First();
            }

            return null;
        }

        private HRAdmin validateLoggedInAdmin()
        {
            if (HttpContext.Session.IsAvailable)
            {
                var adminID = HttpContext.Session.GetInt32("id");

                return db.HRAdmin.Find(adminID);
            }

            else
                return null;
        }
    }
}