using DirectoryApi.Application.Dtos;
using DirectoryApi.Application.Interfaces;
using Directory = DirectoryApi.Domain.Entities.Directory;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoryController : ControllerBase
    {
        private readonly IDirectoryService _directoryService;

        public DirectoryController(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        [HttpGet]
        public async Task<List<Directory>> Get()
        {
            var result = await _directoryService.Get();
            return result;
        }


        [HttpPost]
        public async Task<DirectoryDto> Insert(DirectoryDto request)
        {

            var result = await _directoryService.Insert(request);
            return result;
        }

        [HttpPut]
        public async Task<DirectoryDto> Update(DirectoryDto request)
        {

            var result = await _directoryService.Update(request);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _directoryService.Delete(id);
            return NoContent();
        }
    }

}
