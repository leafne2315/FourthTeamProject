using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers
{
	public class ProductController : Controller
	{
        private readonly PetHeavenDbContext _db;

        public IActionResult Index()
		{
			return View();
		}

        //[HttpPost]
        //public IActionResult BatchDelete(int[] selectedIds)
        //{
        //    if (selectedIds == null || selectedIds.Length == 0)
        //    {
        //        // 如果没有选择任何商品，则返回错误消息
        //        return Json(new { success = false, message = "请选择至少一个商品来删除。" });
        //    }

        //    try
        //    {
        //        // 根据选中的商品Id数组进行批量删除
        //        var productsToDelete = _db.Product.Where(p => selectedIds.Contains(p.ProductId));
        //        _db.Product.RemoveRange(productsToDelete);
        //        _db.SaveChanges();

        //        // 返回成功消息
        //        return Json(new { success = true, message = "批量删除成功。" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // 返回错误消息
        //        return Json(new { success = false, message = "删除商品时出现错误：" + ex.Message });
        //    }
        //}

    }
}
