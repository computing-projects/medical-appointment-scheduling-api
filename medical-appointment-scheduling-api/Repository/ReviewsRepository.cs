using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using medical_appointment_scheduling_api.Data;
using medical_appointment_scheduling_api.Models;

namespace medical_appointment_scheduling_api.Repositories
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly AppDbContext _db;

        public ReviewsRepository(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Reviews>> GetAllAsync()
        {
            return await _db.Reviews.ToListAsync();
        }

        public async Task<Reviews> GetByIdAsync(int id)
        {
            return await _db.Reviews.FindAsync(id);
        }

        public async Task<bool> EvaluateAsync(Reviews review)
        {
            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
