
using BuildOpsPlatform.RevitDataCommon.DTOs;
using BuildOpsPlatform.RevitDataService.DbContexts;
using BuildOpsPlatform.RevitDataService.Models;
using BuildOpsPlatform.ServicesCommon.DTOs.RevitData;
using Microsoft.EntityFrameworkCore;

namespace BuildOpsPlatform.RevitDataService.Services
{
    public class RevitDataService : IRevitDataService
    {
        private readonly RevitDataDbContext _dbContext;

        public RevitDataService(RevitDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> GetAnyCategoryAsync()
        {
            return _dbContext.Categories.AnyAsync();
        }

        public Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = _dbContext.Categories;

            return categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    CategoryType = c.CategoryType.ToString(),
                    Name = c.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<CategoryDto>> GetCategoriesByProjectIdAsync(Guid projectId)
        {
            var categories = _dbContext.ProjectsCategories
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.Category);

            if(categories is null)
            {
                return new List<CategoryDto>();
            }

            return await categories
                .Select(pc => new CategoryDto
                {
                    CategoryId = pc.Category.CategoryId,
                    CategoryType = pc.Category.CategoryType.ToString(),
                    Name = pc.Category.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<CategoryDto>> AddCategoryToProjectAsync(ProjectCategoryDto dto)
        {
            var exists = await _dbContext.ProjectsCategories
                .AnyAsync(x => x.ProjectId == dto.ProjectId && x.CategoryId == dto.CategoryId);

            if (!exists)
            {
                var projectCategory = new ProjectsCategories
                {
                    ProjectId = dto.ProjectId,
                    CategoryId = dto.CategoryId
                };

                await _dbContext.ProjectsCategories.AddAsync(projectCategory);
                await _dbContext.SaveChangesAsync();

                return await GetCategoriesByProjectIdAsync(dto.ProjectId);
            }
            else
            {
                return await GetCategoriesByProjectIdAsync(dto.ProjectId);
            }
        }

        public async Task<List<CategoryDto>> RemoveCategoryFromProjectAsync(ProjectCategoryDto dto)
        {
            var projectCategory = await _dbContext.ProjectsCategories
                .FirstOrDefaultAsync(x => x.ProjectId == dto.ProjectId && x.CategoryId == dto.CategoryId);

            if (projectCategory is not null)
            {
                _dbContext.ProjectsCategories.Remove(projectCategory);
                await _dbContext.SaveChangesAsync();

                return await GetCategoriesByProjectIdAsync(dto.ProjectId);
            }
            else
            {
                return await GetCategoriesByProjectIdAsync(dto.ProjectId);
            }
        }

        public async Task<bool> GetAnySystemParameterAsync()
        {
            return await _dbContext.Parameters.AnyAsync();
        }

        public async Task<List<RvtDocumentDto>> GetDocumentsByProjectIdAsync(Guid projectId)
        {
            var documents = _dbContext.RvtDocuments
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.Author);

            if (documents is null)
            {
                return new List<RvtDocumentDto>();
            }

            return await documents
                .Select(pc => new RvtDocumentDto
                {
                    Id = pc.Id,
                    ProjectCode = pc.ProjectCode,
                    ProjectName = pc.ProjectName,
                    Stage = pc.Stage,
                    Building = pc.Building,
                    Discipline = pc.Discipline,
                    AppVersion = pc.AppVersion,
                    Description = pc.Description,
                    Author = pc.Author.Name,
                    ProjectId = projectId
                })
                .OrderBy(x => x.Discipline)
                .ThenBy(x => x.Building)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<RvtDocumentDto>> AddDocumentToProjectAsync(RvtDocumentDto dto)
        {
            var exists = await _dbContext.RvtDocuments
                .AnyAsync(x => x.Id == dto.Id);

            if (!exists)
            {
                var author = new Author
                {
                    Name = dto.Author
                };

                var document = new RvtDocument
                {
                    Id = dto.Id,
                    ProjectCode = dto.ProjectCode,
                    ProjectName = dto.ProjectName,
                    Stage = dto.Stage,
                    Building = dto.Building,
                    Discipline = dto.Discipline,
                    AppVersion = dto.AppVersion,
                    Description = dto.Description,
                    Author = author,
                    ProjectId = dto.ProjectId
                };

                try
                {
                await _dbContext.RvtDocuments.AddAsync(document);

                await _dbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {

                }

                return await GetDocumentsByProjectIdAsync(dto.ProjectId);
            }
            else
            {
                return await GetDocumentsByProjectIdAsync(dto.ProjectId);
            }
        }

        public async Task<List<RvtDocumentDto>> RemoveDocumentFromProjectAsync(RvtDocumentDto dto)
        {
            var document = await _dbContext.RvtDocuments
                 .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (document is not null)
            {
                _dbContext.RvtDocuments.Remove(document);
                await _dbContext.SaveChangesAsync();

                return await GetDocumentsByProjectIdAsync(dto.ProjectId);
            }
            else
            {
                return await GetDocumentsByProjectIdAsync(dto.ProjectId);
            }
        
        }
    }
}
