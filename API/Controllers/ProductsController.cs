using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {

  public class ProductsController : BaseAPIController {
    //private readonly StoreContext context;
    private readonly IGenericRepository<Product> _genericRepository;

    public ProductsController(IProductRepository productRepository, IGenericRepository<Product> genericRepository) {
      _genericRepository = genericRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams productSpecParams) {
      var spec = new ProductSpecification(productSpecParams);

      return await CreatePagedResult(_genericRepository, spec, productSpecParams.PageIndex, productSpecParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id) {
      var product = await _genericRepository.GetByIdAsync(id);
      if (product == null) return NotFound();

      return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product) {
      _genericRepository.Add(product);
      if (await _genericRepository.SaveAllAsync()) {
        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
      }
      return BadRequest("Product not created");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product) {
      if (id != product.Id || !ProductExists(product)) {
        return BadRequest("Cannot update this product");
      }

      _genericRepository.Update(product);
      if (!await _genericRepository.SaveAllAsync()) {
        return BadRequest("Product not updated");
      }
      return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id) {
      var product = await _genericRepository.GetByIdAsync(id);
      if (product == null) {
        return NotFound();
      }

      _genericRepository.Remove(product);
      if (!await _genericRepository.SaveAllAsync()) {
        return BadRequest("Product not deleted");
      }
      return NoContent();
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands() {
      var specification = new BrandSpecification();

      return Ok(await _genericRepository.ListAsync(specification));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes() {
      var specification = new TypeSpecification();

      return Ok(await _genericRepository.ListAsync(specification));
    }

    private bool ProductExists(Product product) {
      return _genericRepository.Exists(product.Id);
    }
  }
}
