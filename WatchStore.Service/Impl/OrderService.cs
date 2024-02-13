using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Repository;
using WatchStore.Service.Interface;

namespace WatchStore.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepossitory;

        public OrderService(IOrderRepository orderRepossitory)
        {
            _orderRepossitory = orderRepossitory;
        }

        public List<Order> getAllOrders()
        {
            return _orderRepossitory.getAllOrders();
        }

        public List<Order> getAllOrdersFromUser(User u)
        {
            return _orderRepossitory.getAllOrdersFromUser(u.Id);

        }

        public Order getOrderDetails(Guid id)
        {
            return _orderRepossitory.getOrderDetails(id);
        }
    }
}
