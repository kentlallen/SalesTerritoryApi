using Microsoft.AspNetCore.Mvc;

namespace SalesTerritoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TerritoriesController(ITerritoryRepository _repository, ILogger<TerritoriesController> _logger) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SalesTerritory>>> GetTerritories()
        {
            var territories = await _repository.GetAllAsync();
            return Ok(territories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SalesTerritory>> GetTerritory(int id)
        {
            _logger.LogInformation("Getting territory with ID: {Id}", id);
            var territory = await _repository.GetByIdAsync(id);
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
        public async Task<ActionResult<SalesTerritory>> CreateTerritory(SalesTerritory territory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdTerritory = await _repository.CreateAsync(territory);
            return CreatedAtAction(nameof(GetTerritory), new { id = createdTerritory.Id }, createdTerritory);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTerritory(int id, SalesTerritory territory)
        {
            if (id != territory.Id)
            {
                return BadRequest("ID in URL must match ID in request body.");
            }

            var existingTerritory = await _repository.GetByIdAsync(id);
            if (existingTerritory == null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(territory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTerritory(int id)
        {
            var existingTerritory = await _repository.GetByIdAsync(id);
            if (existingTerritory == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
