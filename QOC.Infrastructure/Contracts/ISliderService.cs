using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QOC.Application.DTOs;
using QOC.Domain.Entities;

namespace QOC.Application.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<Slider>> GetAllSlidersAsync();
        Task<Slider> GetSliderByIdAsync(int id);
        Task<Slider> CreateSliderAsync(SliderDto dto);
        Task<Slider> UpdateSliderAsync(int id, SliderDto dto);
        Task DeleteSliderAsync(int id);
    }
}

