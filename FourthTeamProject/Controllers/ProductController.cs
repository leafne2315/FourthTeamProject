using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using static FourthTeamProject.Models.ViewModel.CheckPageViewModel;

namespace FourthTeamProject.Controllers
{
	public class ProductController : Controller
	{
        private readonly PetHeavenDbContext _db;

	
		public IActionResult Index()
		{
			var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			if (email != null)
			{
				ViewBag.Email = email;
			}
			return View();
		}

		public IActionResult Order() 
		{ 
			return View();
		}

        //金流
		public IActionResult Check()
		{
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            // 產生測試資訊
            ViewData["MerchantID"] = Config.GetSection("MerchantID").Value;//商店代號
            ViewData["MerchantOrderNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");  //訂單編號
            ViewData["ExpireDate"] = DateTime.Now.AddDays(3).ToString("yyyyMMdd"); //繳費有效期限
            ViewData["ReturnURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}Product/OrderDone"; //支付完成返回商店網址
            //ViewData["CustomerURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}Home/CallbackCustomer"; //商店取號網址
            //ViewData["NotifyURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}Home/CallbackNotify"; //支付通知網址
            //ViewData["ClientBackURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}"; //返回商店網址
             
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			if (email != null)
			{
				ViewBag.Email = email;
				return View();
			}
			return Redirect("/Member/Login");

		}

        public IActionResult OrderDone()
        {
            return View();
        }



        public IActionResult ProductDetail()
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (Request.Query.TryGetValue("Id", out var productId))
            {
				ViewBag.Email = email;
                ViewBag.productId = productId;
                return View();
            }
            return RedirectToAction("Index");
        }


        //金流方法
        /// <summary>
        /// 傳送訂單至藍新金流
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public IActionResult SendToNewebPay(SendToNewebPayIn inModel)
        {
            SendToNewebPayOut outModel = new SendToNewebPayOut();

            // 藍新金流線上付款

            //交易欄位
            List<KeyValuePair<string, string>> TradeInfo = new List<KeyValuePair<string, string>>();
            // 商店代號
            TradeInfo.Add(new KeyValuePair<string, string>("MerchantID", inModel.MerchantID));
            // 回傳格式
            TradeInfo.Add(new KeyValuePair<string, string>("RespondType", "String"));
            // TimeStamp
            TradeInfo.Add(new KeyValuePair<string, string>("TimeStamp", DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString()));
            // 串接程式版本
            TradeInfo.Add(new KeyValuePair<string, string>("Version", "2.0"));
            // 商店訂單編號
            TradeInfo.Add(new KeyValuePair<string, string>("MerchantOrderNo", inModel.MerchantOrderNo));
            // 訂單金額
            TradeInfo.Add(new KeyValuePair<string, string>("Amt", inModel.Amt));
            // 商品資訊
            TradeInfo.Add(new KeyValuePair<string, string>("ItemDesc", inModel.ItemDesc));
            // 繳費有效期限(適用於非即時交易)
            TradeInfo.Add(new KeyValuePair<string, string>("ExpireDate", inModel.ExpireDate));
            // 支付完成返回商店網址
            TradeInfo.Add(new KeyValuePair<string, string>("ReturnURL", inModel.ReturnURL));
            // 支付通知網址
            TradeInfo.Add(new KeyValuePair<string, string>("NotifyURL", inModel.NotifyURL));
            // 商店取號網址
            TradeInfo.Add(new KeyValuePair<string, string>("CustomerURL", inModel.CustomerURL));
            // 支付取消返回商店網址
            TradeInfo.Add(new KeyValuePair<string, string>("ClientBackURL", inModel.ClientBackURL));
            // 付款人電子信箱
            TradeInfo.Add(new KeyValuePair<string, string>("Email", inModel.Email));
            // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
            TradeInfo.Add(new KeyValuePair<string, string>("EmailModify", "0"));

            //信用卡 付款
            if (inModel.ChannelID == "CREDIT")
            {
                TradeInfo.Add(new KeyValuePair<string, string>("CREDIT", "1"));
            }
            //ATM 付款
            if (inModel.ChannelID == "VACC")
            {
                TradeInfo.Add(new KeyValuePair<string, string>("VACC", "1"));
            }
            string TradeInfoParam = string.Join("&", TradeInfo.Select(x => $"{x.Key}={x.Value}"));

            // API 傳送欄位
            // 商店代號
            outModel.MerchantID = inModel.MerchantID;
            // 串接程式版本
            outModel.Version = "2.0";
            //交易資料 AES 加解密
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            string HashKey = Config.GetSection("HashKey").Value;//API 串接金鑰
            string HashIV = Config.GetSection("HashIV").Value;//API 串接密碼
            string TradeInfoEncrypt = EncryptAESHex(TradeInfoParam, HashKey, HashIV);
            outModel.TradeInfo = TradeInfoEncrypt;
            //交易資料 SHA256 加密
            outModel.TradeSha = EncryptSHA256($"HashKey={HashKey}&{TradeInfoEncrypt}&HashIV={HashIV}");

            return Json(outModel);
        }

        /// <summary>
        /// 加密後再轉 16 進制字串
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>加密後的字串</returns>
        public string EncryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                var encryptValue = EncryptAES(Encoding.UTF8.GetBytes(source), cryptoKey, cryptoIV);

                if (encryptValue != null)
                {
                    result = BitConverter.ToString(encryptValue)?.Replace("-", string.Empty)?.ToLower();
                }
            }

            return result;
        }

        /// <summary>
        /// 字串加密AES
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <param name="cryptoKey">加密金鑰</param>
        /// <param name="cryptoIV">cryptoIV</param>
        /// <returns>加密後字串</returns>
        public byte[] EncryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                aes.Key = dataKey;
                aes.IV = dataIV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(source, 0, source.Length);
                }
            }
        }

        /// <summary>
        /// 字串加密SHA256
        /// </summary>
        /// <param name="source">加密前字串</param>
        /// <returns>加密後字串</returns>
        public string EncryptSHA256(string source)
        {
            string result = string.Empty;

            using (System.Security.Cryptography.SHA256 algorithm = System.Security.Cryptography.SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(source));

                if (hash != null)
                {
                    result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
                }

            }
            return result;
        }






    }
}
