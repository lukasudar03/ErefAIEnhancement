using ErefAIEnhancement.DTOs.StudentDto;
using ErefAIEnhancement.DTOs.StudentSubjectDtos;
using ErefAIEnhancement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ErefAIEnhancement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            return Ok(student);
        }

        [HttpGet("{id:guid}/subjects")]
        public async Task<IActionResult> GetSubjects(Guid id)
        {
            var subjects = await _studentService.GetSubjectsAsync(id);
            return Ok(subjects);
        }

        [HttpPut("{id:guid}/subjects")]
        public async Task<IActionResult> UpdateSubjects(Guid id, [FromBody] UpdateStudentSubjectsDto dto)
        {
            var updatedSubjects = await _studentService.UpdateSubjectsAsync(id, dto);
            return Ok(updatedSubjects);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            var createdStudent = await _studentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentDto dto)
        {
            var updatedStudent = await _studentService.UpdateAsync(id, dto);
            return Ok(updatedStudent);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _studentService.DeleteAsync(id);
            return NoContent();
        }
    }
}