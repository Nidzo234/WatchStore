using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchStore.Domain;
using WatchStore.Service;

namespace WatchStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IJwtService jwtService, IUserService userService)
        {
            _productService = productService;
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpGet("products")]
        public List<Product> Products()
        {
            return _productService.GetProducts();
        }


        [HttpGet("ProductDetails")]
        public Product ProductDetails(Guid id)
        {
            return _productService.getProductDetails(id);
        }

        [HttpPost("AddToCart")]
        public IActionResult AddProductToCard(AddToShoppingCardDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string jwt = Request.Cookies["jwt"];
                    Guid userId = _jwtService.GetUser(jwt);
                    var user = _userService.GetUserById(userId);
                    var result = _productService.addToShoppingCart(userId, model);
                    if (result)
                    {
                        return Ok();
                    }


                }
                catch (Exception)
                {
                    return Unauthorized();
                }

            }

            return NotFound();
        }
    }
}
