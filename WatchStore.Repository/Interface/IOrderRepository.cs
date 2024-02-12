using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStore.Repository
{
    public interface IOrderRepository
    {
        List<Order> getAllOrdersFromUser(Guid userId);
        List<Order> getAllOrders();
    }
}
