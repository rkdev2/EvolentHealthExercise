using AutoMapper;
using EvolentHealthExercise.Core.Interfaces;
using EvolentHealthExercise.Core.Models;
using EvolentHealthExercise.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvolentHealthExercise.WebApi.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactController(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        [HttpGet("getcontacts")]
        [ProducesResponseType(typeof(BaseResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactRepository.GetContacts();
            return Ok(new BaseResponseModel { Result = _mapper.Map<List<Contact>, List<ContactModel>>(contacts) });
        }

        [HttpGet("getcontact")]
        [ProducesResponseType(typeof(BaseResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContact(int id)
        {
            var contact = await _contactRepository.GetContact(id);
            return Ok(new BaseResponseModel { Result = _mapper.Map<Contact, ContactModel>(contact) });
        }

        [HttpPost("savecontact")]
        [ProducesResponseType(typeof(BaseResponseModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> SaveContact(ContactModel model)
        {
            var contact = _mapper.Map<ContactModel, Contact>(model);
            await _contactRepository.InsertContact(contact);

            return Ok(new BaseResponseModel { Message = "Contact saved successfully" });
        }

        [HttpPost("updatecontact")]
        [ProducesResponseType(typeof(BaseResponseModel), StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateContact(ContactModel model)
        {
            var contact = _mapper.Map<ContactModel, Contact>(model);
            await _contactRepository.UpdateContact(contact);

            return Ok(new BaseResponseModel { Message = "Contact updated successfully" });
        }

        [HttpGet("deletecontact")]
        [ProducesResponseType(typeof(BaseResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _contactRepository.DeleteContact(id);
            return Ok(new BaseResponseModel { Message = "Contact deleted successfully" });
        }
    }
}