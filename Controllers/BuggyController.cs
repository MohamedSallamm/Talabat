using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;

namespace Talabat.API.Controllers
{
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        [HttpGet("NotFound")]

        public ActionResult GetNotFoundrequest()
        {
            var product = _dbcontext.Products.Find(100);
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpGet("ServerError")]
        public ActionResult GetServeError()
        {
            var product = _dbcontext.Products.Find(100);
            var productToReturn = product.ToString(); // Error
            return Ok(productToReturn); // will throw Exception (Null reference exception)
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("BadRequest/{id}")]
        public IActionResult GetBadRequest(int id)
        {
            return Ok();
        }


    }
}
