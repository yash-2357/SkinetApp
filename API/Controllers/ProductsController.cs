﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase {
    //private readonly StoreContext context;
    private readonly IProductRepository _productRepository;
    private readonly IGenericRepository<Product> _genericRepository;

    public ProductsController(IProductRepository productRepository, IGenericRepository<Product> genericRepository) {
      _productRepository = productRepository;
      _genericRepository = genericRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,string? type, string? sort) {
      return Ok(await _productRepository.GetProductsAsync(brand,type,sort));
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
        return CreatedAtAction("GetProduct",new { id = product.Id},product);
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
      return Ok(await _productRepository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes() {
      return Ok(await _productRepository.GetTypesAsync());
    } 

    private bool ProductExists(Product product) {
      return _genericRepository.Exists(product.Id);
    }
  }
}
