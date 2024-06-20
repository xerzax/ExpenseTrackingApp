using Application.DTO;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository _genericRepository;

        public CategoryService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<CategoryResponseDTO> CreateCategory(CategoryRequestDTO categoryRequest)
        {
            var categories = new CategoryResponseDTO
            {
               Title = categoryRequest.Title,
               Type = categoryRequest.Type,
            };

            await _genericRepository.Insert(categories);
            return categories;
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategories()
        {
            var categories = await _genericRepository.Get<Category>();
            var results = categories.Select(x => new CategoryResponseDTO
            {
                CategoryId = x.CategoryId,
                Title = x.Title,
                Type = x.Type,
            });
            return results;
            
        }
    }
}
