using QOC.Application.DTOs;
using QOC.Application.DTOs.Slider;

namespace QOC.Application.Interfaces
{
    public interface ISliderService
    {

        Task<IEnumerable<SliderDto>> GetAllSlidersAsync();

        Task<SliderDto> GetSliderByIdAsync(int id);
        Task<SliderDto> CreateSliderAsync(SliderRequestDto dto);
        Task<SliderDto> UpdateSliderAsync(int id, SliderRequestDto dto);

        Task DeleteSliderAsync(int id);
    }
}

