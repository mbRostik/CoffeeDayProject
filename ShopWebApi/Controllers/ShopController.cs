using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebApi.DAL;
using System.Security.Claims;

namespace ShopWebApi.Controllers
{
    [ApiController]
    [Route("shop")]
    public class ShopController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public ShopController(ShopDbContext context)
        {
            _context = context;
        }

        public class AddToBagRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }

        }

        [HttpGet("bag")]
        [Authorize]
        public async Task<IActionResult> GetUserBag()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var bag = await _context.UserBags
                .Include(b => b.BagProducts)
                    .ThenInclude(bp => bp.Product)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (bag == null || bag.BagProducts == null || !bag.BagProducts.Any())
                return Ok(new List<object>());

            var result = bag.BagProducts.Select(bp => new
            {
                productId = bp.ProductId,
                name = bp.Product.Name,
                description = bp.Product.Description,
                price = bp.Product.Price,
                quantity = bp.Quantity,
                photo = $"data:image/png;base64,{Convert.ToBase64String(bp.Product.Photo)}"
            });

            return Ok(result);
        }

        [HttpDelete("bag/remove")]
        public async Task<IActionResult> RemoveFromBag([FromBody] RemoveBagItemRequest request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userBag = await _context.UserBags
                .Include(b => b.BagProducts)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (userBag == null)
                return NotFound("Bag not found");

            var bagProduct = userBag.BagProducts
                .FirstOrDefault(bp => bp.ProductId == request.ProductId);

            if (bagProduct == null)
                return NotFound("Product not in bag");

            _context.UserBagProducts.Remove(bagProduct);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public class RemoveBagItemRequest
        {
            public int ProductId { get; set; }
        }

        public class OrderRequestDto
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
        }
        [HttpPost("order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDto orderRequest)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _context.Users
                .Include(u => u.Bag)
                .ThenInclude(b => b.BagProducts)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Bag == null || user.Bag.BagProducts.Count == 0)
            {
                return BadRequest("No items in bag to order.");
            }

            var order = new UserOrder
            {
                UserId = user.Id,
                Name = orderRequest.Name,
                Phone = orderRequest.Phone,
                Address = orderRequest.Address,
                Date = DateTime.UtcNow,
                OrderProducts = user.Bag.BagProducts.Select(bp => new UserOrderProduct
                {
                    ProductId = bp.ProductId,
                    Quantity = bp.Quantity
                }).ToList()
            };

            _context.UserOrders.Add(order);

            // Clear the bag
            _context.UserBagProducts.RemoveRange(user.Bag.BagProducts);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Order placed successfully." });
        }

        [HttpPost("bag/add")]
        public async Task<IActionResult> AddToBag([FromBody] AddToBagRequest request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated.");

            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            var bag = await _context.UserBags
                .Include(b => b.BagProducts)
                .FirstOrDefaultAsync(b => b.UserId == userId);

            if (bag == null)
            {
                bag = new UserBag { UserId = userId };
                _context.UserBags.Add(bag);
                await _context.SaveChangesAsync(); // Save to generate UserBag.Id
            }

            var existing = await _context.UserBagProducts
                .FirstOrDefaultAsync(bp => bp.UserBagId == bag.Id && bp.ProductId == request.ProductId);

            if (existing != null)
            {
                existing.Quantity += request.Quantity;
            }
            else
            {
                _context.UserBagProducts.Add(new UserBagProduct
                {
                    UserBagId = bag.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        // GET: https://localhost:7071/shop/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    Photo = $"data:image/png;base64,{Convert.ToBase64String(c.Photo)}"
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: https://localhost:7230/shop/products?categoryId=1
        [HttpGet("products")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] int categoryId)
        {
            var products = await _context.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Include(pc => pc.Product)
                .Select(pc => new
                {
                    pc.Product.Id,
                    pc.Product.Name,
                    pc.Product.Description,
                    pc.Product.Price,
                    Photo = $"data:image/png;base64,{Convert.ToBase64String(pc.Product.Photo)}"
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}
