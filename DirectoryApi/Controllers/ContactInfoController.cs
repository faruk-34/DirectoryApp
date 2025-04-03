using DirectoryApi.Application.Interfaces;
using DirectoryApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Directory = DirectoryApi.Domain.Entities.Directory;


namespace DirectoryApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly IContactInfoService _contactInfoService;

        public ContactInfoController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }

        [HttpPost]
        public async Task<Directory> Insert(ContactInfo request)
        {

            var result = await _contactInfoService.Insert(request);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contactInfoService.Delete(id);
            return NoContent();
        }
    }
}

