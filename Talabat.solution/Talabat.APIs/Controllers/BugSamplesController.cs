using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data.Contexts;

namespace Talabat.APIs.Controllers
{
    public class BugSamplesController : ApiBaseController
    {
        private readonly StoreContext _dbContext;

        public BugSamplesController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundError()
        {
            var product = _dbContext.Products.Find(100);
            if (product is null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product.ToString();

            return Ok(productToReturn);
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequestError()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequestError(int id)
        {
            return Ok();
        }
        [HttpGet("unauthorized")]
        public ActionResult GetUnAuthorizedError(int id)
        {
            return Unauthorized(new ApiResponse(401));
        }




    }
}
