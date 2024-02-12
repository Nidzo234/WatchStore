using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchStore.Domain;
using WatchStore.Service;

namespace WatchStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IProductService _productService;
        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("addProduct")]
        public IActionResult addNewProduct(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                Product p = new Product();
                p.ProductName = dto.ProductName;
                p.ProductImage = dto.ProductImage;
                p.ProductPrice = dto.ProductPrice;
                p.ProductDescription = dto.ProductDescription;
                p.ProductRating = dto.ProductRating;

                var createdProduct = this._productService.CreateProduct(p);

                if (createdProduct == true)
                {
                  
                    return Ok(createdProduct);
                }
                else
                {
                    return BadRequest("Failed to create product");
                }
            }
            return BadRequest("Invalid product data");
        }


        [HttpGet("getAllProducts")]
        public ActionResult<List<Product>> GetProducts()
        {
            
            List<Product> products = _productService.GetProducts();
            if (products != null && products.Count > 0)
            {
                return Ok(products); // 200 OK with the list of products
            }
            else
            {
                return NotFound("Products not found"); // 404 Not Found with a message
            }
        }


        [HttpGet("getProduct")]
        public ActionResult<Product> getProduct(Guid id)
        {
            Product p = _productService.getProductDetails(id);
            if (p != null)
            {
                return Ok(p);
            }
            else { return NotFound("Product doesnt exist"); }
        }


        [HttpPost("EditProduct")]
        public IActionResult Edit(Guid id, ProductDto product)
        {
            if (ModelState.IsValid)
            {
                if (_productService.getProductDetails(id) != null)
                {

                    _productService.updateProduct(id, product);
                    return Ok();
                
            }
        }
            
            return NotFound("The product is not edited");
        }

        [HttpDelete("deleteProduct")]
        public IActionResult deleteProduct(Guid id)
        {

            if (_productService.DeleteProduct(id))
            {
                return Ok("The product is successfully dfeleted");
            }
            else { return NotFound("Product doesnt exist"); }
        }



        
    }
}
