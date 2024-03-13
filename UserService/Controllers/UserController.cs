using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public UserController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet("products")]
    public async Task<ActionResult<List<ProductModel>>> GetAllProducts()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5012/Admin/products");
        var client = _clientFactory.CreateClient();
        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductModel>>(responseString);
            return Ok(products);
        }
        else
        {
            return StatusCode((int)response.StatusCode, "Error occurred while fetching products");
        }
    }
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Test endpoint hit successfully");
    }
}