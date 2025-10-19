using Microsoft.AspNetCore.Mvc;
using SalesTerritoryApi.Models.DTOs;
using SalesTerritoryApi.Models.ViewModels;
using SalesTerritoryApi.Services.Interfaces;

namespace SalesTerritoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TerritoriesController(ITerritoryService _territoryService, ILogger<TerritoriesController> _logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TerritoryViewModel>>> GetTerritories()
        {
            _logger.LogInformation("Getting all territories.");
            var territories = await _territoryService.GetAllAsync();
            return Ok(territories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TerritoryViewModel>> GetTerritory(int id)
        {
            _logger.LogInformation("Getting territory with ID: {Id}.", id);
            var territory = await _territoryService.GetByIdAsync(id);
            if (territory == null)
            {
                _logger.LogWarning("Territory with ID: {Id} not found.", id);
                return NotFound();
            }
            return Ok(territory);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TerritoryViewModel>> CreateTerritory(CreateTerritoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTerritory = await _territoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTerritory), new { id = createdTerritory.Id }, createdTerritory);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TerritoryViewModel>> UpdateTerritory(int id, UpdateTerritoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTerritory = await _territoryService.UpdateAsync(id, dto);
            if (updatedTerritory == null)
            {
                return NotFound();
            }

            return Ok(updatedTerritory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTerritory(int id)
        {
            var deleted = await _territoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
