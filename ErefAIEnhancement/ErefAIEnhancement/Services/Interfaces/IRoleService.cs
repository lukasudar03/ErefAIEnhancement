using ErefAIEnhancement.DTOs;
using ErefAIEnhancement.DTOs.RoleDtos;

namespace ErefAIEnhancement.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleResponseDto>> GetAllAsync();
        Task<RoleResponseDto?> GetByIdAsync(Guid id);
        Task<RoleResponseDto> CreateAsync(CreateRoleDto dto);
        Task<RoleResponseDto?> UpdateAsync(Guid id, UpdateRoleDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}