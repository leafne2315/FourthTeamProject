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
    public class EmployeesManageController : Controller
    {
        private readonly PetHeavenDbContext _db;


        public EmployeesManageController(PetHeavenDbContext context)
        {
            this._db = context;
        }


        //[Authorize(Roles = "admin")]

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

            return View("~/Areas/Admin/Views/EmployeesManage/OrderDetail.cshtml", productOrderDetails);
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

            return View("~/Areas/Admin/Views/EmployeesManage/SalonOrderDetail.cshtml", SalonOrderDetails);
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

            return View("~/Areas/Admin/Views/EmployeesManage/HotelOrderDetail.cshtml", HotelOrderDetails);
        }


        public async Task<IActionResult> EmployeeLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}

