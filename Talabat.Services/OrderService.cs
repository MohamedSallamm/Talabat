using Talabat.Core;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork<Order> _unitOfWork;
        private readonly IPaymentService _paymentService;


        //private readonly igenericrepository<product> _productrepo;
        //private readonly igenericrepository<deliverymethod> _deliverymethodrepo;
        //private readonly igenericrepository<order> _orderrepo;

        public OrderService(
            IBasketRepository basketRepo,
            IUnitOfWork<Order> unitOfWork,
            IPaymentService paymentService
            //IGenericRepository<Product> ProductsRepo,
            //IGenericRepository<DeliveryMethod>deliveryMethodRepo,
            //IGenericRepository<Order> ordersRepo
            )
        {

            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_OrderRepo = ordersRepo;
            //_ProductRepo = ProductsRepo;
            //_DeliveryMethodRepo = deliveryMethodRepo;
        }

        //public async Task<Order?> CreateOrderAsync(string BuyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        //{
        //    // 1- Get Basket from Baskets Repo
        //    var basket = await _basketRepo.GetBasketAsync(basketId);

        //    // 2- Get Selected Items at Basket from Products Repo
        //    var OrderItems = new List<OrderItem>();
        //    if (basket?.Items.Count > 0)
        //    {
        //        foreach (var Item in basket.Items)
        //        {
        //            var Product = await _unitOfWork.repository<Product>().GetByIdAsync(Item.Id);
        //            if (Product == null)
        //            {
        //                throw new NullReferenceException($"❌ المنتج غير موجود! تحقق من سلة المشتريات. BasketItem ID: {Item.Id}");
        //            }

        //            var ProductItemOrderd = new ProductItemOrderd(Item.Id, Product.Name, Product.PictureUrl);
        //            var OrderItem = new OrderItem(ProductItemOrderd, Product.Price, Item.Quantity);
        //            OrderItems.Add(OrderItem);
        //        }
        //    }

        //    // 3- Calculate SubTotal
        //    var SubTotal = OrderItems.Sum(OrderItems => OrderItems.Price * OrderItems.Quantity);

        //    // 4- Get Delivery Method From DeliveryMethods Repo

        //    var deliveryMethod = await _unitOfWork.repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

        //    var orderReop = _unitOfWork.repository<Order>();

        //    var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

        //    var existingOrder = await orderReop.GetEntityWithSpecAsync(orderSpecs);

        //    if (existingOrder != null)
        //    {
        //        orderReop.Delete(existingOrder);
        //        await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        //    }

        //    // 5- Create Order
        //    var Order = new Order(BuyerEmail, ShippingAddress, deliveryMethod, OrderItems, SubTotal, basket.PaymentIntentId);
        //    await orderReop.AddAsync(Order);

        //    // 6- Save To Database
        //    var result = await _unitOfWork.CompleteAsync();
        //    if (result <= 0) return null;
        //    return Order;
        //}



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
            var order = orderRepo.GetEntityWithSpecAsync(orderSpec);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
           => (IReadOnlyList<DeliveryMethod>)await _unitOfWork.repository<DeliveryMethod>().GetAllAsync();





        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            // 1- جلب السلة من الـ Repository
            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket == null || basket.Items == null || basket.Items.Count == 0)
            {
                throw new NullReferenceException("❌ السلة غير موجودة أو لا تحتوي على عناصر.");
            }

            // 2- إنشاء قائمة لعناصر الطلب
            var OrderItems = new List<OrderItem>();

            foreach (var Item in basket.Items)
            {
                // 3- التأكد من أن معرف المنتج صحيح
                if (Item.Id <= 0)
                {
                    throw new ArgumentException($"❌ معرف المنتج غير صالح: {Item.Id}");
                }

                // 4- البحث عن المنتج في قاعدة البيانات
                var Product = await _unitOfWork.repository<Product>().GetByIdAsync(Item.Id);
                if (Product == null)
                {
                    throw new NullReferenceException($"❌ المنتج غير موجود! تحقق من سلة المشتريات. BasketItem ID: {Item.Id}");
                }

                // 5- إنشاء عنصر الطلب
                var ProductItemOrdered = new ProductItemOrderd(Product.Id, Product.Name, Product.PictureUrl);
                var OrderItem = new OrderItem(ProductItemOrdered, Product.Price, Item.Quantity);
                OrderItems.Add(OrderItem);
            }

            // 6- جلب طريقة التوصيل
            var deliveryMethod = await _unitOfWork.repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod == null)
            {
                throw new NullReferenceException("❌ طريقة التوصيل غير موجودة.");
            }

            // 7- حساب المبلغ الإجمالي
            var subtotal = OrderItems.Sum(item => item.Price * item.Quantity);

            // 8- إنشاء الطلب
            var order = new Order(BuyerEmail, ShippingAddress, deliveryMethod, OrderItems, subtotal);


            // 9- حفظ الطلب في قاعدة البيانات
            _unitOfWork.repository<Order>().AddAsync(order);
            try
            {
                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0)
                {
                    throw new Exception("❌ فشل في إنشاء الطلب.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطأ أثناء حفظ الطلب: {ex.Message}");
                Console.WriteLine($"🔍 التفاصيل الداخلية: {ex.InnerException?.Message}");
                throw; // يُعيد رمي الخطأ ليتم عرضه في الاستجابة
            }

            // 10- حذف السلة بعد إنشاء الطلب
            await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }







    }





}
