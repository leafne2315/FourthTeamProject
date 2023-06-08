using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        //public async Task<IActionResult> Ordermanagement()
        //{
        //    //var petHeavenDbContext = _db.ProductOrder.Include(p => p.ProductOrderDetail).Include(p => p.OrderId);
        //    var petHeavenDbContext = _db.ProductOrder
        //                 .Include(p => p.Payment)
        //                 .Include(p => p.Invoice)
        //                 .Include(p => p.Member)
        //                 .Include(p => p.ProductOrderDetail);

        //    return View(await petHeavenDbContext.ToListAsync());
        //}

        public async Task<IActionResult> Ordermanagement(string search, int page = 1)
        {
            int pageSize = 10;
            IQueryable<ProductOrder> query = _db.ProductOrder
                .Include(p => p.Payment)
                .Include(p => p.Invoice)
                .Include(p => p.Member)
                .Include(p => p.ProductOrderDetail);

            if (!string.IsNullOrEmpty(search))
            {
                if (search.StartsWith("/"))
                {
                    string birthdate = search.Substring(1); // get rid of the leading '/'
                    query = query.Where(order => order.Member.MemberBirthday.ToString().Contains(birthdate));
                }
                else
                {
                    query = query.Where(order => order.MemberId.ToString().Contains(search)
                                                  || order.OrderMemberName.Contains(search));
                }
            }


            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var orders = await query.ToListAsync();

            var viewModel = new ProductOrderViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }






        public async Task<IActionResult> OrderDetail(int id)
        {
            var productOrderDetails = await _db.ProductOrderDetail.Where(detail => detail.OrderId == id).ToListAsync();

            if (productOrderDetails == null || productOrderDetails.Count == 0)
            {
                return NotFound();
            }

            return View("~/Areas/Admin/Views/Employees/Product/OrderDetail.cshtml", productOrderDetails);
        }

        public async Task<IActionResult> SalonOrdermanagement(string search, int page = 1)
        {
            int pageSize = 10;
            IQueryable<SalonOrder> query = _db.SalonOrder
                .Include(p => p.Payment)
                .Include(p => p.Invoice)
                .Include(p => p.Member)
                .Include(p => p.SalonOrderDetail);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(order => order.MemberId.ToString().Contains(search)
                                            || order.OrderMemberName.Contains(search));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var orders = await query.ToListAsync();

            var viewModel = new SalonOrderViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }



        public async Task<IActionResult> SalonOrderDetail(int id)
        {
            var SalonOrderDetails = await _db.SalonOrderDetail.Where(detail => detail.SalonOrderId == id).ToListAsync();

            if (SalonOrderDetails == null || SalonOrderDetails.Count == 0)
            {
                return NotFound();
            }

            return View("~/Areas/Admin/Views/Employees/PetSalon/SalonOrderDetail.cshtml", SalonOrderDetails);
        }
        public async Task<IActionResult> HotelOrdermanagement(string search, int page = 1)
        {
            int pageSize = 10;
            IQueryable<HotelOrder> query = _db.HotelOrder
                .Include(p => p.Payment)
                .Include(p => p.Invoice)
                .Include(p => p.Member)
                .Include(p => p.HotelOrderDetail);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(order => order.MemberId.ToString().Contains(search)
                                            || order.Member.MemberName.Contains(search));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var orders = await query.ToListAsync();

            var viewModel = new HotelOrderPageViewModel
            {
                Orders = orders,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }



        public async Task<IActionResult> HotelOrderDetail(int id)
        {
            var HotelOrderDetails = await _db.HotelOrderDetail.Where(detail => detail.HotelOrderId == id).ToListAsync();

            if (HotelOrderDetails == null || HotelOrderDetails.Count == 0)
            {
                return NotFound();
            }

            return View("~/Areas/Admin/Views/Employees/PetHotel/HotelOrderDetail.cshtml", HotelOrderDetails);
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
                _db.Product.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Productmanagement));
            }
            ViewData["ProductCatagoryId"] = new SelectList(_db.ProductCatagory, "ProductCatagoryId", "ProductCatagoryName", product.ProductCatagoryId);
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType, "ProductTypeId", "ProductTypeName", product.ProductTypeId);
            return View(product);
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
            return RedirectToAction("Productmanagement", "Employees");
        }
        public async Task<IActionResult> EmployeeLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}