using FluentValidation;
using gerenciadorLivraria.DTOs;
using gerenciadorLivraria.Services;
using Microsoft.AspNetCore.Mvc;


namespace gerenciadorLivraria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrariaController : ControllerBase
    {
        private readonly BookServices _service;

        public LivrariaController(BookServices service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>Created(CreateBookDto dto)
        {
            try
            {
                var book = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Created), new { id = book.Id }, book);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<BookResponseDto>>> GetAll()
        {
            var books = await _service.GetBooksAsync();
            return Ok(books);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseDto>> GetBook(Guid id)
        {
            var book = await _service.GetByIdAsync(id);

            if (book == null) 
                return NotFound();

            return Ok(book);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBookDto dto)
        {
            try
            {
                var book = await _service.UpdateAsync(id, dto);

                if (book is null)
                    return NotFound();

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            
            var book = await _service.DeleteAsync(id);

            if (book is null)
                return NotFound();

            return NoContent();
            
        }
    }
}
