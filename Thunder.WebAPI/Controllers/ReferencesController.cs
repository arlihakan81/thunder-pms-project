using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thunder.Application.Dto.References;
using Thunder.Domain.Entities;
using Thunder.Infrastructure.Repositories.References;
using Thunder.Persistence.AutoMapper;

namespace Thunder.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReferencesController(IReferenceRepository referenceRepository, IMapper mapper) : ControllerBase
	{
		private readonly IReferenceRepository referenceRepository = referenceRepository;
		private readonly IMapper mapper = mapper;

		[HttpGet]
		public ActionResult<List<ResultReferenceDto>> GetAllReferences()
		{
			var references = referenceRepository.GetAll();
			if (references == null || references.Count == 0)
			{
				return NotFound("No references found");
			}

			return Ok(mapper.Map<Reference, ResultReferenceDto>(references));
		}

		[HttpGet("{id}")]
		public ActionResult<ResultReferenceDto> GetReferenceById(Guid id)
		{
			var reference = referenceRepository.GetById(id);
			if (reference == null)
			{
				return NotFound($"No reference found with Id {id}");
			}

			return Ok(mapper.Map<Reference, ResultReferenceDto>(reference));
		}


		[HttpGet("user={userId}")]
		public ActionResult<ResultReferenceDto> GetReferenceByUser(Guid userId)
		{
			var reference = referenceRepository.GetReferenceByUser(userId);
			if (reference == null)
			{
				return NotFound($"No reference found with user Id {userId}");
			}

			return Ok(mapper.Map<Reference, ResultReferenceDto>(reference));
		}

		[HttpPost]
		public async Task<ActionResult> CreateReferenceAsync([FromBody] CreateReferenceDto createReferenceDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Reference data is null");
			}

			if(referenceRepository.EmailAddressCannotDuplicateWhenInserted(createReferenceDto.EmailAddress))
			{
				ModelState.AddModelError(createReferenceDto.EmailAddress, "Email address already exists");
				return BadRequest($"Reference email address {createReferenceDto.EmailAddress} already exists");
			}

			var reference = mapper.Map<CreateReferenceDto, Reference>(createReferenceDto);
			reference.CreatedAt = DateTime.Now;
			await referenceRepository.AddAsync(reference);

			return Ok("New reference saved");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateReferenceAsync(Guid id, [FromBody] UpdateReferenceDto updateReferenceDto)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest("Reference data is null");
            }

            if (referenceRepository.EmailAddressCannotDuplicateWhenUpdated(id, updateReferenceDto.EmailAddress))
            {
                ModelState.AddModelError(updateReferenceDto.EmailAddress, "Email address already exists");
                return BadRequest($"Reference email address {updateReferenceDto.EmailAddress} already exists");
            }

            var reference = referenceRepository.GetById(id);
			if (reference == null)
			{
				return NotFound($"No reference found with Id {id}");
			}
			reference.Name = updateReferenceDto.Name;
			reference.EmailAddress = updateReferenceDto.EmailAddress;
			reference.UpdatedAt = DateTime.Now;
			await referenceRepository.UpdateAsync(reference);
			return Ok("Reference updated successfully");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteReferenceAsync(Guid id)
		{
			var reference = referenceRepository.GetById(id);
			if (reference == null)
			{
				return NotFound($"No reference found with Id {id}");
			}

			await referenceRepository.DeleteAsync(id);
			return Ok("Reference deleted successfully");

		}

	}
}
