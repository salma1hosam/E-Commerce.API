using DomainLayer.Models.OrderModule;

namespace Service.Specifications.OrderModuleSpecifications
{
	internal class OrderSpecification : BaseSpecifications<Order, Guid>
	{
		//Get All Orders By Email
		public OrderSpecification(string email) : base(O => O.UserEmail == email)
		{
			AddInclude(O => O.DeliveryMethod);
			AddInclude(O => O.Items);
			AddOrderByDescending(O => O.OrderDate);
		}

		//Get Order By Id
		public OrderSpecification(Guid id) : base(O => O.Id == id)
		{
			AddInclude(O => O.DeliveryMethod);
			AddInclude(O => O.Items);
		}
	}
}
