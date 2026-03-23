using ErefAIEnhancement.Exceptions;
using ErefAIEnhancement.DTOs.RoleDtos;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;

namespace ErefAIEnhancement.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<CreateRoleDto> _createRoleValidator;
        private readonly IValidator<UpdateRoleDto> _updateRoleValidator;

        public RoleService(IRoleRepository roleRepository, IValidator<CreateRoleDto> createRoleValidator, IValidator<UpdateRoleDto> updateRoleValidator)
        {
            _roleRepository = roleRepository;
            _createRoleValidator = createRoleValidator;
            _updateRoleValidator = updateRoleValidator;
        }

        public async Task<List<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(MapToDto).ToList();
        }

        public async Task<RoleResponseDto> GetByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new NotFoundException("Role not found");

            return MapToDto(role);
        }

        public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
        {
            var validationResult = await _createRoleValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingRole = await _roleRepository.GetByNameAsync(dto.RoleName);
            if (existingRole != null)
                throw new Exception("Role with this name already exists.");

            var role = new Role
            {
                Id = Guid.NewGuid(),
                RoleName = dto.RoleName
            };

            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChangesAsync();

            return MapToDto(role);
        }

        public async Task<RoleResponseDto> UpdateAsync(Guid id, UpdateRoleDto dto)
        {
            var validationResult = await _updateRoleValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new NotFoundException("Role not found");

            var existingRole = await _roleRepository.GetByNameAsync(dto.RoleName);
            if (existingRole != null && existingRole.Id != id)
                throw new BadRequestException("Another role with this name already exists.");

            role.RoleName = dto.RoleName;

            _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync();

            return MapToDto(role);
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new NotFoundException("Role not found");

            var hasUsers = await _roleRepository.HasUsersAsync(id);
            if (hasUsers)
                throw new BadRequestException("Role cannot be deleted because it is assigned to one or more users.");

            _roleRepository.Delete(role);
            await _roleRepository.SaveChangesAsync();
        }

        private static RoleResponseDto MapToDto(Role role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }
    }
}