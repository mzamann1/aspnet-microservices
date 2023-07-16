using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZstdSharp.Unsafe;

namespace Catalog.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        #region GET METHODS


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }





        [HttpGet]
        [Route("{id:length(24)}", Name = "GetProductById")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Nullable), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var products = await _productRepository.GetProductByIdAsync(id);

            if (products == null)
            {
                _logger.LogError($"Product with id: {id} not found");
                return NotFound();
            }

            return Ok(products);
        }




        [HttpGet]
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Nullable), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _productRepository.GetProductByCategoryAsync(category);
            return Ok(products);
        }

        #endregion

        #region POST METHODS

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {

            }
            await _productRepository.CreateProductAsync(product);
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        #endregion

        #region PUT METHODS

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await _productRepository.UpdateProductAsync(product));
        }

        #endregion

        #region  DELETE METHODS


        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {

            return Ok(await _productRepository.DeleteProductByIdAsync(id));
        }

        #endregion


    }
}
