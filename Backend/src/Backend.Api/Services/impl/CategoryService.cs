using Backend.Api.Models;
using Backend.Api.Repositories;

namespace Backend.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _repository.FindAsync(c => c.Deleted == false || c.Deleted == null);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null || category.Deleted == true)
                return null;

            return category;
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null || existingCategory.Deleted == true)
                return null;

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            _repository.Update(existingCategory);
            await _repository.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null || category.Deleted == true)
                return false;

            category.Deleted = true;
            _repository.Update(category);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
