using FourthTeamProject.Models;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FourthTeamProject.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly PetHeavenDbContext _db;
       

        public EmployeesController(PetHeavenDbContext context)
        {
            this._db = context;
        }


        public IActionResult EmployeeLogin()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public IActionResult EmployeeSystem()
        {

            return View();
        }




		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Productmanagement()
		{
            var petHeavenDbContext = _db.Product.Include(p => p.ProductCatagory).Include(p => p.ProductType);
            return View(await petHeavenDbContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Product == null)
            {
                return NotFound();
            }

            var product = await _db.Product
                .Include(p => p.ProductCatagory)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        



        public IActionResult Create()
        {
            ViewData["ProductCatagoryId"] = new SelectList(_db.ProductCatagory, "ProductCatagoryId", "ProductCatagoryName");
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType, "ProductTypeId", "ProductTypeName");
            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductTypeId,ProductName,ProductSpecification,ProductContent,UnitPrice,Stock,ProductStatus,ProductCatagoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Productmanagement));
            }
            ViewData["ProductCatagoryId"] = new SelectList(_db.ProductCatagory, "ProductCatagoryId", "ProductCatagoryName", product.ProductCatagoryId);
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
        }
 
        public async Task <IActionResult> PutOn([Bind("ProductId,ProductStatus")]Product product)
        {
            if (product == null || product.ProductId == 0)
            {
                return NotFound();
            }

            var productInDb = await _db.Product.FindAsync(product.ProductId);

            if (productInDb == null)
            {
                return NotFound();
            }

            if (productInDb.ProductStatus == false)
            {
                productInDb.ProductStatus = true;
                _db.Update(productInDb);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Productmanagement));
        }


        //public IActionResult PutDown()
        //{

        //}



        public IActionResult ShowPhoto()
        {
            // 假设图片在wwwroot/images目录下，文件名为myImage.jpg
            string filePath = Path.Combine( "images", "myImage.jpg");

            // 检查文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return ViewBag.Error;
            }

            // 读取文件内容
            var imageBytes = System.IO.File.ReadAllBytes(filePath);

            // 返回图片内容
            return File(imageBytes, "image/jpeg");
        }




        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Product == null)
            {
                return NotFound();
            }

            var product = await _db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCatagoryId"] = new SelectList(_db.ProductCatagory, "ProductCatagoryId", "ProductCatagoryName", product.ProductCatagoryId);
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductTypeId,ProductName,ProductSpecification,ProductContent,UnitPrice,Stock,ProductStatus,ProductCatagoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(product);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Productmanagement));
            }
            ViewData["ProductCatagoryId"] = new SelectList(_db.ProductCatagory, "ProductCatagoryId", "ProductCatagoryName", product.ProductCatagoryId);
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _db.Product == null)
            {
                return NotFound();
            }

            var product = await _db.Product
                .Include(p => p.ProductCatagory)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Product == null)
            {
                return Problem("Entity set 'PetHeavenDbContext.Product'  is null.");
            }
            var product = await _db.Product.FindAsync(id);
            if (product != null)
            {
                _db.Product.Remove(product);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Productmanagement));
        }

        private bool ProductExists(int id)
        {
            return (_db.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }


        [HttpPost]
        public async Task<IActionResult> EmployeeLogin(EmployeeLoginViewModel model)
        {

            var user = _db.Employees.FirstOrDefault(x => x.EmployeeEmail == model.EmployeeEmail &&
             x.EmployeePassword == model.EmployeePassword);

            if (user == null)
            {
                ViewBag.Error = "帳號密碼錯誤";
                return View("EmployeeLogin");
            }


            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Name, user.EmployeeName),
               new Claim(ClaimTypes.Role, user.EmployeeRole),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);  //夾帶一個cookie出去
            return RedirectToAction("EmployeeSystem", "Employees");
        }
        public async Task<IActionResult> EmployeeLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
