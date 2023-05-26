using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Models.ViewModel
{
	public class ProductViewModel
	{
		public int ProductId { get; set; }
		public int ProductTypeId { get; set; }
		public string ProductName { get; set; }
		public string ProductContent { get; set; }
		public int UnitPrice { get; set; }
		public int ProductCatagoryId { get; set; }

		public int Amount { get; set; }

		//public virtual ProductCatagory ProductCatagory { get; set; }
		//public virtual ProductType ProductType { get; set; }
		//public virtual ICollection<ProductImage> ProductImage { get; set; }

	}
}
