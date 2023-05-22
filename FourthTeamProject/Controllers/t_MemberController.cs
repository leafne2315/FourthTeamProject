using FourthTeamProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Text.Json;
using FourthTeamProject.Services;

namespace FourthTeamProject.Controllers
{
    public class t_MemberController : Controller
    {
        private readonly PetHeavenDbContext _db;
        private readonly EncryptService encrypt;

        public t_MemberController(PetHeavenDbContext context, EncryptService encrypt)
        {
            _db = context;
            this.encrypt = encrypt;
        }

        //public t_MemberController(PetHeavenDbContext context)
        //{
        //    this._db = context;
        //}
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = _db.TMember.FirstOrDefault(x => x.CMemberAccount == model.c_MemberAccount &&
             x.CMemberPassword == model.c_MemberPassword);

            if (user != null)
            {
                ViewBag.Error = "帳號已經存在!!";
                return View("Login");
            }

            _db.TMember.Add(new TMember()
            {
                CMemberAccount = model.c_MemberAccount,
                CMemberPassword = model.c_MemberPassword,
                CMemberAddress = model.c_MemberAddress,

                CMemberEmail = model.c_MemberEmail,
                CMemberGender = model.c_MemberGender,
                CMemberName = model.c_MemberName,
                CMemberPhone = model.c_MemberPhone,
                CMemberIsActive = model.c_MemberIsActive


            });
            _db.SaveChanges();
            //寄信
            var obj = new AesValidationDto(model.c_MemberAccount, DateTime.Now.AddDays(3));
            var jString = JsonSerializer.Serialize(obj);
            var code = encrypt.AesEncryptToBase64(jString);


            var mail = new MailMessage()
            {
                From = new MailAddress("mstyle912012@gmail.com"),
                Subject = "啟用網站驗證",        
                Body = (@$"請點這<a href='https://localhost:7089/t_Member/enable?code= {code}'>這裡</a>來啟用你的帳號"),
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };
            mail.To.Add(new MailAddress(model.c_MemberEmail));
  
            try
            {
                using (var sm = new SmtpClient("smtp.gmail.com", 587)) //465 ssl
                {
                    sm.EnableSsl = true;
                    sm.Credentials = new NetworkCredential("mystyle912012@gmail.com", "zkghpbiqhsviyrus");
                    sm.Send(mail);
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Error = "邮件发送失败：" + ex.Message;
                throw;

            }
            return View();
        }


        public async Task<IActionResult> Enable(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid code");
            }

            var str = encrypt.AesDecryptToString(code);

            if (string.IsNullOrEmpty(str))
            {
                return BadRequest($"{code}");
            }

            var obj = JsonSerializer.Deserialize<AesValidationDto>(str);
            if (DateTime.Now > obj.ExpiredDate)
            {
                return BadRequest("Code has expired");
            }

            var user = _db.TMember.FirstOrDefault(x => x.CMemberAccount == obj.CMemberAccount);
            if (user != null)
            {
                user.CMemberIsActive = true;
                _db.SaveChanges();
            }

            return Ok(new { code, str });
        }


        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel model)
        {
            var user = _db.TMember.FirstOrDefault(x => x.CMemberAccount == model.MemberAccount &&
             x.CMemberPassword == model.MemberPassword);

            if (user == null)
            {
                ViewBag.Error = "帳號密碼錯誤";
                return View("Login");

              
            }

            var claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Name, user.CMemberName),
            new Claim(ClaimTypes.Email, user.CMemberEmail),

             };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);  //夾帶一個cookie出去
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}