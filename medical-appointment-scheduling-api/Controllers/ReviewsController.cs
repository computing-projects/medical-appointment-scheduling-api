using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsRepository _repo;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewsRepository repo, ILogger<ReviewsController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            var result = await _repo.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Evaluate")]
        public async Task<IActionResult> EvaluateAsync([FromBody] Reviews review)
        {
            var result = await _repo.EvaluateAsync(review);
            return Ok(result);
        }
    }
}
