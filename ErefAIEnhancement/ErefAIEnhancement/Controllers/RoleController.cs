using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.RoleDtos;
using ErefAIEnhancement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ErefAIEnhancement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            try
            {
                var createdRole = await _roleService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto)
        {
            try
            {
                var updatedRole = await _roleService.UpdateAsync(id, dto);
                if (updatedRole == null)
                    return NotFound();

                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _roleService.DeleteAsync(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}