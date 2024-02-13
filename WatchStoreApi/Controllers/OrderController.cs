using GemBox.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WatchStore.Domain;
using WatchStore.Domain.Dto;
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
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
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
                    List<OrderDto> orders = new List<OrderDto>();
                    foreach(var order in result)
                    {
                        var pio = order.ProductInOrders;
                        List<ProductInShoppingCartDto> products = new List<ProductInShoppingCartDto>();
                        double total = 0.0;
                        foreach(var product in pio)
                        {
                            total += product.Quantity * product.Product.ProductPrice;
                            ProductInShoppingCartDto dto = new ProductInShoppingCartDto
                            {
                                id = product.ProductId,
                                ProductDescription = product.Product.ProductDescription,
                                ProductName = product.Product.ProductName,
                                ProductPrice = product.Product.ProductPrice,
                                ProductImage = product.Product.ProductImage,
                                quantity = product.Quantity
                              
                            };
                            products.Add(dto);
                        }

                        orders.Add(new OrderDto
                        {
                            Id = order.Id,
                            products = products,
                            totalPrice = total
                        });
                    }
                    return Ok(orders);
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




        [HttpGet("getInvoice")]
        public IActionResult getInvoice(Guid id)
        {
            var result = _orderService.getOrderDetails(id);
            if (result != null)
            {
                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
                var document = DocumentModel.Load(templatePath);
                document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
                document.Content.Replace("{{CostumerEmail}}", "nikolajovanovski234@gmail.com");
                StringBuilder sb = new StringBuilder();
                var total = 0.0;

                foreach (var item in result.ProductInOrders)
                {
                    total += item.Quantity * item.Product.ProductPrice;
                    sb.AppendLine("Часовник " + item.Product.ProductName + "  количина:  " + item.Quantity + "  цена:  " + item.Product.ProductPrice+ " ден.");
                }
                document.Content.Replace("{{AllProducts}}", sb.ToString());
                document.Content.Replace("{{TotalPrice}}",total.ToString() + "денари");

                var stream = new MemoryStream();

                document.Save(stream, new PdfSaveOptions());
                return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");

            }
           
            
            return BadRequest();
        }


        [HttpGet("getOrderDetails")]
        public IActionResult getOrderDetails(Guid id)
        {
            var result = _orderService.getOrderDetails(id);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
            
        }
    }


    
}
