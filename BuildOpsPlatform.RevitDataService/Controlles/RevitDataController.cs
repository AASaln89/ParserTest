using BuildOpsPlatform.RevitDataCommon.DTOs;
using BuildOpsPlatform.RevitDataService.Services;
using BuildOpsPlatform.ServicesCommon.DTOs.RevitData;
using Microsoft.AspNetCore.Mvc;

namespace BuildOpsPlatform.RevitDataService.Controlles
{
    [Route("api/revit")]
    [ApiController]
    public class RevitDataController : ControllerBase
    {
        private readonly IRevitDataService _revitDataService;

        public RevitDataController(IRevitDataService revitDataService)
        {
            _revitDataService = revitDataService;
        }

        [HttpGet("categpries")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var result = await _revitDataService.GetCategoriesAsync();
            return result;
        }

        [HttpGet("categpries/{projectId}")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategoriesByProjectId([FromRoute] Guid projectId)
        {
            var result = await _revitDataService.GetCategoriesByProjectIdAsync(projectId);
            return result;
        }

        [HttpPost("categpries/add")]
        public async Task<ActionResult<List<CategoryDto>>> AddCategoryToProject([FromBody] ProjectCategoryDto dto)
        {
            var result = await _revitDataService.AddCategoryToProjectAsync(dto);

            return result;
        }

        [HttpPost("categpries/delete")]
        public async Task<ActionResult<List<CategoryDto>>> DeleteCategoryFromProject([FromBody] ProjectCategoryDto dto)
        {
            var result = await _revitDataService.RemoveCategoryFromProjectAsync(dto);

            return result;
        }

        [HttpGet("categ")]
        public async Task<ActionResult> GetAnyCategory()
        {
            var result = await _revitDataService.GetAnyCategoryAsync();
            return result ? Ok() : BadRequest();
        }

        [HttpGet("param")]
        public async Task<ActionResult<bool>> GetAnySystemParameter()
        {
            var result = await _revitDataService.GetAnySystemParameterAsync();
            return result;
        }

        [HttpGet("documents/{projectId}")]
        public async Task<ActionResult<List<RvtDocumentDto>>> GetDocumentsByProjectId([FromRoute] Guid projectId)
        {
            var result = await _revitDataService.GetDocumentsByProjectIdAsync(projectId);
            return result;
        }

        [HttpPost("documents/add")]
        public async Task<ActionResult<List<RvtDocumentDto>>> AddDocumentToProject([FromBody] RvtDocumentDto dto)
        {
            var result = await _revitDataService.AddDocumentToProjectAsync(dto);

            return result;
        }

        [HttpPost("documents/delete")]
        public async Task<ActionResult<List<RvtDocumentDto>>> DeleteDocumentFromProject([FromBody] RvtDocumentDto dto)
        {
            var result = await _revitDataService.RemoveDocumentFromProjectAsync(dto);

            return result;
        }
    }
}
