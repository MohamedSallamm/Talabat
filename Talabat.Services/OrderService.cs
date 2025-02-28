using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork<BasketItem> _unitOfWork;


        //private readonly igenericrepository<product> _productrepo;
        //private readonly igenericrepository<deliverymethod> _deliverymethodrepo;
        //private readonly igenericrepository<order> _orderrepo;

        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork<BasketItem> unitOfWork
            //IGenericRepository<Product> ProductsRepo,
            //IGenericRepository<DeliveryMethod>deliveryMethodRepo,
            //IGenericRepository<Order> ordersRepo
            )
        {

            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            //_OrderRepo = ordersRepo;
            //_ProductRepo = ProductsRepo;
            //_DeliveryMethodRepo = deliveryMethodRepo;
        }

        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            // 1- Get Basket from Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2- Get Selected Items at Basket from Products Repo
            var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var Item in basket.Items)
                {
                    var Product = await _unitOfWork.repository<Product>().GetAsync(Item.Id);
                    var ProductItemOrderd = new ProductItemOrderd(Item.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrderd, Product.Price, Item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }

            // 3- Calculate SubTotal
            var SubTotal = OrderItems.Sum(OrderItems => OrderItems.Price * OrderItems.Quantity);
            // 4- Get Delivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            // 5- Create Order
            var Order = new Order(BuyerEmail, ShippingAddress, deliveryMethod, OrderItems, SubTotal);
            await _unitOfWork.repository<Order>().AddAsync(Order);

            // 6- Save To Database
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return Order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = _unitOfWork.repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await orderRepo.GetAllWithSpecAsync(spec);
            return (IReadOnlyList<Order>)orders;
        }

        public Task<Order?> GetOrderByIdForUserAsync(int OrderId, string buyerEmail)
        {
            var orderRepo = _unitOfWork.repository<Order>();
            var orderSpec = new OrderSpecifications(OrderId, buyerEmail);
            var order = orderRepo.GetByIdWithSpecAsync(orderSpec);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
           => (IReadOnlyList<DeliveryMethod>)await _unitOfWork.repository<DeliveryMethod>().GetAllAsync();
    }
}
