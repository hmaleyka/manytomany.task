using Microsoft.EntityFrameworkCore;
using Pronia.Business.Services.Interfaces;
using Pronia.Core.Models;
using Pronia.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pronia.Business.Helpers;
using Microsoft.AspNetCore.Http;


namespace Pronia.Business.Services.Implementations
{
    public class SliderService : ISliderService
    {
        private readonly ISliderRepository _repo;
       // private readonly IWebHostEnvironment _env;

        public SliderService(ISliderRepository repo)
        {
            _repo = repo;
            //_env = env;
        }

        public async Task<Slider> Create(Slider slider)
        {
            if (slider == null) throw new Exception("Slider should noy be null");
            Slider sliders = new Slider()
            {
                Title=slider.Title,
                SubTitle=slider.SubTitle,
                Description=slider.Description,
                ImageFile=slider.ImageFile,


            };
            if (!slider.ImageFile.CheckType("image")) throw new Exception("you should apply the image");
            if (!slider.ImageFile.CheckLong(2097152)) throw new Exception("the image should not be large than 2 mb");
            
            await _repo.Create(slider);
            await _repo.SaveChangesAsync();
            return sliders;
        }

        public async Task <Slider> Delete(Slider slider)
        {
            if (slider == null) throw new Exception("Category should not be null");
            _repo.Delete(slider);

            await _repo.SaveChangesAsync();
            return slider;
        }

        public async Task<ICollection<Slider>> GetAllAsync()
        {
            var sliders = await _repo.GetAllAsync();
            return await sliders.ToListAsync();
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            if (id <= 0) throw new Exception("Id can't be less than zero");
            var slider = await _repo.GetByIdAsync(id);
            if (slider == null) throw new Exception("category should not be null");
            return slider;
        }

        public async Task<Slider> Update(Slider slider)
        {
            if (!slider.ImageFile.CheckType("image")) throw new Exception("you should apply the image");
            if (!slider.ImageFile.CheckLong(2097152)) throw new Exception("the image should not be large than 2 mb");
            if (slider.Id <= 0) throw new Exception("Id should not be less than zero");

            Slider sliders = await _repo.GetByIdAsync(slider.Id);
            if (slider == null) throw new Exception("Category should not be null");
            sliders.Title = slider.Title;
            sliders.SubTitle = slider.SubTitle;
            sliders.Description = slider.Description;
            sliders.ImageFile = slider.ImageFile;

            _repo.Update(slider);
            await _repo.SaveChangesAsync();
            return sliders;
        }
    }
}
