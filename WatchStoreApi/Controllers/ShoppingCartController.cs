using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System.Security.Claims;
using WatchStore.Domain;
using WatchStore.Domain.Dto;
using WatchStore.Service;
using WatchStore.Service.Interface;

namespace WatchStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;


        public ShoppingCartController(IShoppingCartService shoppingCartService, IUserService userService, IJwtService jwtService)
        {
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet("shoppingCart")]
        public ShoppingCartDto shoppingCart()
        {
            try
            {
                string jwt = Request.Cookies["jwt"];
                Guid userId = _jwtService.GetUser(jwt);
                var user = _userService.GetUserById(userId);
                var shoppingCart = _shoppingCartService.getShoppingCartInfo(user);
                return shoppingCart;
            }
            catch (Exception)
            {
                return new ShoppingCartDto();
            }



        }

        [HttpGet("Order")]
        public Boolean Order()
        {
            string jwt = Request.Cookies["jwt"];
            Guid userId = _jwtService.GetUser(jwt);
            var user = _userService.GetUserById(userId);
            var result = this._shoppingCartService.order(user);

            return result;
        }


        [HttpPost("PayOrderReact")]
        public ActionResult Create()
        {
            var paymentIntentService = new PaymentIntentService();
            string jwt = Request.Cookies["jwt"];
            Guid userId = _jwtService.GetUser(jwt);
            var user = _userService.GetUserById(userId);

            var order = this._shoppingCartService.getShoppingCartInfo(user);
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice)),
                Currency = "eur",
                
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });
            return new  JsonResult(new { clientSecret = paymentIntent.ClientSecret });
            
        }
    }
}
