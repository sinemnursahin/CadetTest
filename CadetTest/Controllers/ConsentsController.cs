using CadetTest.Entities;
using CadetTest.Models;
using CadetTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadetTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsentsController : ControllerBase
    {
        private readonly IDataService _dataService;

        public ConsentsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // Mevcut kayıtları belirtilen Id'den başlayarak okur.
        [HttpPost]
        public async Task<IActionResult> Post(ConsentRequest request)
        {
            var cevap = await _dataService.GetRangeByIdAsync(request.StartId, request.Count);
            return Ok(cevap);
        }

        // Yeni kayıt ekler.
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Consent consent)
        {
            var created = await _dataService.CreateAsync(consent);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Id ile tek bir kaydı getirir.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var consent = await _dataService.GetByIdAsync(id);
            if (consent == null) return NotFound("Kayıt bulunamadı.");
            return Ok(consent);
        }

        // Mevcut kaydı günceller.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Consent consent)
        {
            var updated = await _dataService.UpdateAsync(id, consent);
            if (updated == null) return NotFound("Güncellenecek kayıt bulunamadı.");
            return Ok(updated);
        }

        // Mevcut kaydı siler.
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _dataService.DeleteAsync(id);
            if (!deleted) return NotFound("Silinecek kayıt bulunamadı.");
            return NoContent();
        }
    }
}