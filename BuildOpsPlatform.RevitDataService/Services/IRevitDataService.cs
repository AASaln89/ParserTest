using BuildOpsPlatform.RevitDataCommon.DTOs;
using BuildOpsPlatform.RevitDataService.Models;
using BuildOpsPlatform.ServicesCommon.DTOs.RevitData;
using Microsoft.AspNetCore.Mvc;

namespace BuildOpsPlatform.RevitDataService.Services
{
    public interface IRevitDataService
    {
        Task<bool> GetAnyCategoryAsync();

        Task<bool> GetAnySystemParameterAsync();

        Task<List<CategoryDto>> GetCategoriesAsync();

        Task<List<CategoryDto>> GetCategoriesByProjectIdAsync(Guid projectId);

        Task<List<CategoryDto>> AddCategoryToProjectAsync(ProjectCategoryDto dto);

        Task<List<CategoryDto>> RemoveCategoryFromProjectAsync(ProjectCategoryDto dto);

        Task<List<RvtDocumentDto>> GetDocumentsByProjectIdAsync(Guid projectId);

        Task<List<RvtDocumentDto>> AddDocumentToProjectAsync(RvtDocumentDto dto);

        Task<List<RvtDocumentDto>> RemoveDocumentFromProjectAsync(RvtDocumentDto dto);
    }
}
