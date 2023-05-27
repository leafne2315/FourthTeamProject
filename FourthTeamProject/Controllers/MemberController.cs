using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Text.Json;
using FourthTeamProject.Services;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Principal;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.Models.PetHeavenModels;

namespace FourthTeamProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly PetHeavenDbContext _db;
        private readonly EncryptService encrypt;

        public MemberController(PetHeavenDbContext context, EncryptService encrypt)
        {
            _db = context;
            this.encrypt = encrypt;
        }

        //public MemberController(PetHeavenDbContext context)
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
            var user = _db.Member.FirstOrDefault(x => x.MemberAccount == model.c_MemberAccount &&
             x.MemberPassword == model.c_MemberPassword);

            if (user != null)
            {
                ViewBag.Error = "帳號已經存在!!";
                return View("Member/Login");
            }

            _db.Member.Add(new Member()
            {
                MemberAccount = model.c_MemberAccount,
                MemberPassword = model.c_MemberPassword,
                MemberAddress = model.c_MemberAddress,

                MemberEmail = model.c_MemberEmail,
                MemberGender = model.c_MemberGender,
                MemberName = model.c_MemberName,
                MemberPhone = model.c_MemberPhone,
                MemberIsActive = model.c_MemberIsActive


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
                Body = (@$"請點這<a href='https://localhost:7089/Member/enable?code= {code}'>這裡</a>來啟用你的帳號"),
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

            var user = _db.Member.FirstOrDefault(x => x.MemberAccount == obj.CMemberAccount);
            if (user != null)
            {
                user.MemberIsActive = true;
                _db.SaveChanges();
            }

            return Ok(new { code, str });
        }


        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel model)
        {
            var user = _db.Member.FirstOrDefault(x => x.MemberAccount == model.MemberAccount &&
             x.MemberPassword == model.MemberPassword);

            if (user == null)
            {
                ViewBag.Error = "帳號密碼錯誤";
                return View("Login");


            }

            var claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Name, user.MemberName),
            new Claim(ClaimTypes.Email, user.MemberEmail),

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
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Forgetpassword(ForgetpasswordViewModel model)
        {
            var user = _db.Member.FirstOrDefault(x => x.MemberEmail == model.MemberEmail);

            if (user == null)
            {
                ViewBag.Error = "帳號已經存在!!";
                return View("Login");
            }
            //寄信
            var obj = new AesValidationDto(model.MemberEmail, DateTime.Now.AddDays(3));
            var jString = JsonSerializer.Serialize(obj);
            var code = encrypt.AesEncryptToBase64(jString);


            var mail = new MailMessage()
            {
                From = new MailAddress("mstyle912012@gmail.com"),
                Subject = "忘記密碼",
                Body = (@$"請點這<a href='https://localhost:7089/Member/Reset?code= {code}'>這裡</a>來更新您的密碼"),
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            };
            mail.To.Add(new MailAddress(model.MemberEmail));

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
        public IActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Reset(ForgetpasswordViewModel model)
        {
            var user = _db.Member.FirstOrDefault(x => x.MemberEmail == model.MemberEmail);
            if (user == null)
            {
                
                return NotFound();
            }
            user.MemberPassword = model.NewPassword;
            try
            {
                _db.SaveChanges();
                return View("Login"); 
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "密碼更新失敗：" + ex.Message);
            }
            
        }
    }
}