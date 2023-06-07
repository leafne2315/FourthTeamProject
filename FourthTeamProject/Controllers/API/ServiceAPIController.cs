using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext petHeavenDb;

        public ServiceAPIController(PetHeavenDbContext petHeavenDb)
        {
            this.petHeavenDb = petHeavenDb;
        }

        [HttpGet("Payment/{payId}")]
        public IActionResult GetPayment(int? payId) 
        {
            if(payId.HasValue)
            {
                var datas = petHeavenDb.Payment.Where(p=>p.PayId==payId)
                    .Select(p=>new PaymentViewModel() 
                    {
                        PayId = p.PayId,
                        PayName = p.PayName,
                    });
                if(datas==null)
                {
                    return NotFound();
                }
                return Ok(datas);
            }
            else
            {
                var datas = petHeavenDb.Payment.Select(p => new PaymentViewModel()
                {
                    PayId= p.PayId,
                    PayName = p.PayName,
                });
                return Ok(datas);
            }
            
        }

        [HttpGet("Invoice/{invoiceId}")]
        public IActionResult GetInvoice(int? invoiceId)
        {
            if (invoiceId.HasValue)
            {
                var datas = petHeavenDb.Invoice.Where(p => p.InvoiceId == invoiceId)
                    .Select(p => new InvoiceViewModel()
                    {
                       InvoiceId = p.InvoiceId,
                       InvoiceName = p.InvoiceName,
                    });
                if (datas == null)
                {
                    return NotFound();
                }
                return Ok(datas);
            }
            else
            {
                var datas = petHeavenDb.Invoice.Select(p => new InvoiceViewModel()
                {
                    InvoiceId = p.InvoiceId,
                    InvoiceName = p.InvoiceName,
                });
                return Ok(datas);
            }

        }
    }
}
