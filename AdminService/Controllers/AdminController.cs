using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AdminService.Models;
using RabbitMQ.Client;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private static List<ProductModel> Products = new List<ProductModel>();
        private readonly ProductUpdateService _productUpdateService;

        public AdminController(ProductUpdateService productUpdateService)
        {
            _productUpdateService = productUpdateService;
        }

        // POST /admin/products
        [HttpPost("products")]
        public ActionResult<ProductModel> AddProduct(ProductModel product)
        {
            try
            {
                // Check if product already exists
                if (Products.Any(p => p.Id == product.Id))
                {
                    return Conflict("Product with the same ID already exists.");
                }

                // Add product to the list
                Products.Add(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /admin/products/{id}
        [HttpDelete("products/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var product = Products.Find(p => p.Id == id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                Products.Remove(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("products")]
        public ActionResult<List<ProductModel>> GetAllProducts()
        {
            return Ok(Products);
        }
        // GET /admin/products/{id} - for CreatedAtAction

        [HttpGet("products/{id}")]
        public ActionResult<ProductModel> GetProduct(int id)
        {
            var product = Products.Find(p => p.Id == id);
            if (product == null)
            {
                product = new ProductModel
                {
                    Id = 0, // Default ID
                    Name = "Default Product",
                    Details = new List<ProductDetail>
            {
                new ProductDetail
                {
                    Id = 0, // Default ID
                    Size = "Default Size",
                    Design = "Default Design",
                    Price = 100.00M
                }
            }
                };
            }

            return product;
        }

        // PUT /admin/products/{id}
        [HttpPut("products/{id}")]
        public ActionResult UpdateProduct(int id, ProductModel updatedProduct)
        {
            try
            {
                var product = Products.Find(p => p.Id == id);
                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                // Update product details
                product.Name = updatedProduct.Name;
                product.Details = updatedProduct.Details;

                // Publish the product detail update to RabbitMQ
                _productUpdateService.PublishProductDetailUpdate(updatedProduct);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test endpoint hit successfully");
        }
    }
}