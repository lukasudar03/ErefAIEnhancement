using ErefAIEnhancement.DTOs.SubjectDtos;
using ErefAIEnhancement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ErefAIEnhancement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            return Ok(subject);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var subjects = await _subjectService.GetAvailableAsync();
            return Ok(subjects);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubjectDto dto)
        {
            var createdSubject = await _subjectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdSubject.Id }, createdSubject);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubjectDto dto)
        {
            var updatedSubject = await _subjectService.UpdateAsync(id, dto);
            return Ok(updatedSubject);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _subjectService.DeleteAsync(id);
            return NoContent();
        }
    }
}