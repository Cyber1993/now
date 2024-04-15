using Core.Interfaces;
using Core.Entities;
using AutoMapper;
using Core.Helpers;
using System.Net;
using Core.Models;
using Core.Resources;
using Core.Models.Category;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class CategoriesService(IRepository<Category> categoriesRepo,
                                   IRepository<ViewedProduct> viewedProductsRepo,
                                   IMapper mapper,
                                   IImagesService imageService) : ICategoriesService
    {
        public async Task<IEnumerable<GetCategoryDto>> GetAll()
        {
            var categories = await categoriesRepo.GetAllBySpec(new Categories.All());

            return mapper.Map<IEnumerable<GetCategoryDto>>(categories);
        }

        public async Task<GetCategorySelectionDto> GetByParentCategoryName(string parentName)
        {
            var parentCategory = await categoriesRepo
                                     .GetBySpec(new Categories.ByName(parentName)) ??
                                 throw new HttpException(ErrorMessages.CategoryByIdNotFound, HttpStatusCode.NotFound);

            var categories = await categoriesRepo
                .GetAllBySpec(new Categories.ByParentCategoryName(parentName));

            GetCategorySelectionDto categorySelectionDto = new()
            {
                Categories = (ICollection<GetCategoryDto>)mapper.Map<IEnumerable<GetCategoryDto>>(categories),
                ParentCategories = (ICollection<GetCategoryDto>)await GetParentCategories(parentCategory.Id),
            };

            return categorySelectionDto;
        }

        public async Task<GetCategorySelectionDto> GetByParentCategoryId(int parentId)
        {
            _ = await categoriesRepo
                                    .GetBySpec(new Categories.ById(parentId)) ??
                                throw new HttpException(ErrorMessages.CategoryByIdNotFound, HttpStatusCode.NotFound);

            var categories = await categoriesRepo
                .GetAllBySpec(new Categories.ByParentCategoryId(parentId));

            GetCategorySelectionDto categorySelectionDto = new()
            {
                Categories = (ICollection<GetCategoryDto>)mapper.Map<IEnumerable<GetCategoryDto>>(categories),
                ParentCategories = (ICollection<GetCategoryDto>)await GetParentCategories(parentId),
            };

            return categorySelectionDto;
        }

        private async Task<IEnumerable<GetCategoryDto>> GetParentCategories(int? categoryId)
        {
            var parentCategories = new List<GetCategoryDto>();

            while (categoryId != null)
            {
                var category = await categoriesRepo.GetByID(categoryId);

                if (category == null)
                {
                    break;
                }

                var categoryDto = mapper.Map<GetCategoryDto>(category);
                parentCategories.Add(categoryDto);

                categoryId = category.ParentCategoryId;
            }

            parentCategories.Reverse();
            return parentCategories;
        }

        public async Task<GetCategoryDto?> GetById(int id)
        {
            Category category = await categoriesRepo
                                    .GetBySpec(new Categories.ById(id)) ??
                                throw new HttpException(ErrorMessages.CategoryByIdNotFound, HttpStatusCode.NotFound);

            return mapper.Map<GetCategoryDto>(category);
        }

        public async Task Edit(int categoryId, EditCategoryDto editCategoryDto)
        {
            var existingCategory = await categoriesRepo
                                       .GetByID(categoryId) ??
                                   throw new HttpException(ErrorMessages.CategoryByIdNotFound, HttpStatusCode.NotFound);

            if (editCategoryDto.ParentCategoryId != null)
            {
                Category parentCategory =
                    await categoriesRepo.GetBySpec(new Categories.ById(editCategoryDto.ParentCategoryId))
                    ?? throw new HttpException(ErrorMessages.CategoryByIdNotFound, HttpStatusCode.NotFound);

                if (parentCategory.ParentCategoryId == categoryId)
                {
                    throw new HttpException(ErrorMessages.CategoryCyclicalReference, HttpStatusCode.NotFound);
                }
            }

            string image = existingCategory.Image;

            mapper.Map(editCategoryDto, existingCategory);
            existingCategory.Id = categoryId;

            if (editCategoryDto.Image != null)
            {
                imageService.DeleteImage(image);
                existingCategory.Image = imageService.SaveImage(editCategoryDto.Image);
            }
            else
            {
                existingCategory.Image = image;
            }

            await categoriesRepo.Update(existingCategory);
            await categoriesRepo.Save();
        }

        public async Task Create(CreateCategoryDto createCategoryDto)
        {
            var categoryEntity = mapper.Map<Category>(createCategoryDto);

            categoryEntity.Image = imageService.SaveImage(createCategoryDto.Image);

            await categoriesRepo.Insert(categoryEntity);
            await categoriesRepo.Save();
        }

        public async Task Delete(int id)
        {
            var category = await categoriesRepo
                .GetByID(id) ?? throw new HttpException(ErrorMessages.CategoryByIdNotFound, HttpStatusCode.NotFound);

            imageService.DeleteImage(category.Image);

            await categoriesRepo.Delete(id);
            await categoriesRepo.Save();
        }

        public async Task<IEnumerable<GetCategoryDto>> GetHead()
        {
            var categories = await categoriesRepo.GetAllBySpec(new Categories.ByParentCategoryId(null));
            return mapper.Map<IEnumerable<GetCategoryDto>>(categories);
        }


        public async Task<PaginatedList<GetCategoryDto>> GetByFilter(ItemsFilter itemsFilter)
        {
            var query = await categoriesRepo.GetAllBySpecQueryable(new Categories.All());

            if (!string.IsNullOrEmpty(itemsFilter.SearchTerm))
            {
                var searchTermLower = itemsFilter.SearchTerm.ToLower();

                query = query.Where(u => u.Name.Contains(searchTermLower));
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / itemsFilter.PageSize);

            var sortingMap = new Dictionary<string, Func<IQueryable<Category>, IOrderedQueryable<Category>>>
            {
                {
                    "Name",
                    q => itemsFilter.SortDirection == "asc"
                        ? q.OrderBy(category => category.Name)
                        : q.OrderByDescending(category => category.Name)
                },
                {
                    "Description",
                    q => itemsFilter.SortDirection == "asc"
                        ? q.OrderBy(category => category.Description)
                        : q.OrderByDescending(category => category.Description)
                },
            };

            if (sortingMap.TryGetValue(itemsFilter.SortBy, out var sortingFunction))
                query = sortingFunction(query);

            var skipAmount = (itemsFilter.PageNumber - 1) * itemsFilter.PageSize;
            query = query.Skip(skipAmount).Take(itemsFilter.PageSize);

            var categories = await query.ToListAsync();

            var categoriesMapped = new List<GetCategoryDto>();

            foreach (var category in categories)
            {
                var categoryDto = mapper.Map<GetCategoryDto>(category);
                categoriesMapped.Add(categoryDto);
            }

            PaginatedList<GetCategoryDto> categoryPaginatedList = new()
            {
                TotalPages = totalPages,
                Items = categoriesMapped,
            };

            return categoryPaginatedList;
        }

        public async Task<GetCategoryDto?> GetMostWatchedCategoryByUserId(string userId)
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

            var viewedProducts = await viewedProductsRepo
                .GetAllBySpec(new ViewedProducts.ByUserIdAndDate(userId, oneWeekAgo));

            var categoryCounts = viewedProducts
                .GroupBy(vp => vp.Product.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToList();

            int? mostWatchedCategoryId = categoryCounts
                .OrderByDescending(cc => cc.Count)
                .Select(cc => cc.CategoryId)
                .FirstOrDefault();

            var mostWatchedCategory = await categoriesRepo
                        .GetBySpec(new Categories.ById(mostWatchedCategoryId.Value));

            return mapper.Map<GetCategoryDto>(mostWatchedCategory);
        }
    }
}
