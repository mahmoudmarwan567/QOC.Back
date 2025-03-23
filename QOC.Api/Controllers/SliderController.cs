using Microsoft.AspNetCore.Mvc;
using QOC.Application.DTOs;
using QOC.Application.Interfaces;

namespace QOC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SliderController : ControllerBase
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSliders()
        {
            var sliders = await _sliderService.GetAllSlidersAsync();
            return Ok(sliders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlider(int id)
        {
            var slider = await _sliderService.GetSliderByIdAsync(id);
            if (slider == null) return NotFound();
            return Ok(slider);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlider([FromBody] SliderDto dto)
        {
            var created = await _sliderService.CreateSliderAsync(dto);
            return CreatedAtAction(nameof(GetSlider), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlider(int id, [FromBody] SliderDto dto)
        {
            var updated = await _sliderService.UpdateSliderAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            await _sliderService.DeleteSliderAsync(id);
            return NoContent();
        }
    }
}
