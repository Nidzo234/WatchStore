using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStore.Service.Interface
{
    public interface IOrderService
    {
        List<Order> getAllOrdersFromUser(User u);
        List<Order> getAllOrders();

    }
}
