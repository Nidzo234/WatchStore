using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchStore.Domain;
using WatchStore.Service;
using WatchStore.Service.Interface;

namespace WatchStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        
        public OrderController(IUserService userService, IJwtService jwtService, IOrderService orderService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _orderService = orderService;
        }

        [HttpPost("myOrders")]
        public ActionResult<List<Order>> getOrders()
        {

            try
            {
                string jwt = Request.Cookies["jwt"];
                Guid userId = _jwtService.GetUser(jwt);
                var user = _userService.GetUserById(userId);
                var result = _orderService.getAllOrdersFromUser(user);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound(result);


            }
            catch (Exception)
            {
                return Unauthorized();
            }
            
        }


        [HttpGet("AllOrders")]
        public ActionResult<List<Order>> AllOrders()
        {
            return _orderService.getAllOrders();
        }

        }
}
