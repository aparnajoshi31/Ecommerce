using Microsoft.AspNetCore.Mvc;
using ProductDetailService.Services;
using System;
using ProductDetailService.Models;

[ApiController]
[Route("[controller]")]
public class ProductDetailController : ControllerBase
{
    private readonly ProductDetailProcessingService _productDetailService;

    public ProductDetailController(ProductDetailProcessingService productDetailService)
    {
        _productDetailService = productDetailService;
    }

    [HttpGet("/productDetail/{id}")]
    public IActionResult GetProductDetails(int id)
    {
        try
        {
            var productDetails = _productDetailService.GetProductDetails(id);
            if (productDetails == null)
            {
                return NotFound();
            }

            return Ok(productDetails);
        }
        catch (Exception ex)
        {
            // Log the exception...
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("/productDetail")]
    public IActionResult GetAllProductDetails()
    {
        try
        {
            var allProductDetails = _productDetailService.GetAllProductDetails();
            if (allProductDetails == null)
            {
                return NotFound();
            }

            return Ok(allProductDetails);
        }
        catch (Exception ex)
        {
            // Log the exception...
            Console.WriteLine(ex.ToString());
            return StatusCode(500, "Internal server error");
        }
    }

    // test hit
    [HttpGet("/productDetail/test")]
    public IActionResult Test()
    {
        return Ok("ProductDetailService is up and running...");
    }
}