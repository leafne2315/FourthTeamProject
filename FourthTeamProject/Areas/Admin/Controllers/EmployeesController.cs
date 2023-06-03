using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace FourthTeamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class EmployeesController : Controller
    {
        private readonly PetHeavenDbContext _db;


        public EmployeesController(PetHeavenDbContext context)
        {
            this._db = context;
        }


        //[Authorize(Roles = "admin")]
    
		public async Task<IActionResult> Ordermanagement()
		{
			//var petHeavenDbContext = _db.ProductOrder.Include(p => p.ProductOrderDetail).Include(p => p.OrderId);
			var petHeavenDbContext = _db.ProductOrder
				.Include(p => p.Payment)
				.Include(p => p.Invoice)
				.Include(p => p.Member)
				.Include(p => p.ProductOrderDetail);

			return View(await petHeavenDbContext.ToListAsync());
		}


		//[Authorize(Roles = "admin")]
		public IActionResult EmployeeSystem()
        {
            return View();
        }



  
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Productmanagement()
        {
            var petHeavenDbContext = _db.Product.Include(p => p.ProductCatagory).Include(p => p.ProductType);
            return View(await petHeavenDbContext.ToListAsync());
        }

        //[Authorize(Roles = "admin")]
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
                _db.Product.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Productmanagement));
            }
            ViewData["ProductCatagoryId"] = new SelectList(_db.ProductCatagory, "ProductCatagoryId", "ProductCatagoryName", product.ProductCatagoryId);
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
        }




    
    
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
