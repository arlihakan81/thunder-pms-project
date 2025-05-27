using Microsoft.AspNetCore.Mvc;
using Thunder.Application.Dto.Users;
using Thunder.Domain.Entities;
using Thunder.Infrastructure.Identity;
using Thunder.Infrastructure.Repositories.Users;
using Thunder.Persistence.AutoMapper;

namespace Thunder.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController(IUserRepository userRepository, IMapper mapper, PasswordHasher hasher) : ControllerBase
	{
		private readonly IUserRepository userRepository = userRepository;
		private readonly PasswordHasher hasher = hasher;
		private readonly IMapper mapper = mapper;

		[HttpGet]
		public ActionResult<List<ResultUserDto>> GetAllUsers()
		{
			var users = userRepository.GetAll();
			if (users == null || users.Count == 0)
			{
				return NotFound("No users found.");
			}

			return Ok(mapper.Map<User,ResultUserDto>(users));

		}

		[HttpGet("Reference={id}")]
		public ActionResult<ResultUserDto> GetUsersByReference(Guid id)
		{
			var user = userRepository.GetUsersByReference(id);
			if (user == null)
			{
				return NotFound($"User with Reference ID {id} not found.");
			}

			return Ok(mapper.Map<User, ResultUserDto>(user));
		}

		[HttpGet("Members/Project={id}")]
		public ActionResult<List<ResultUserDto>> GetMembersByProject(Guid id)
		{
			var users = userRepository.GetMembersByProject(id);
			if (users == null || users.Count == 0)
			{
				return NotFound("Members with Project ID no found");
			}

			return Ok(mapper.Map<User, ResultUserDto>(users));
		}

		[HttpGet("Members/Team={id}")]
		public ActionResult<List<ResultUserDto>> GetMembersByTeam(Guid id)
		{
			var users = userRepository.GetMembersByTeam(id);
			if (users == null || users.Count == 0)
			{
				return NotFound("Members with Team ID no found");
			}

			return Ok(mapper.Map<User, ResultUserDto>(users));
		}

		[HttpGet("Leads/Project={id}")]
		public ActionResult<List<ResultUserDto>> GetLeadsByProject(Guid id)
		{
			var users = userRepository.GetLeadsByProject(id);
			if (users == null || users.Count == 0)
			{
				return NotFound($"Leads with Project ID {id} not found.");
			}

			return Ok(mapper.Map<User, ResultUserDto>(users));
		}

		[HttpGet("Member/Task={id}")]
		public ActionResult<ResultUserDto> GetAssigneeByTask(Guid id)
		{
			var user = userRepository.GetAssigneeByTask(id);
			if (user == null)
			{
				return NotFound($"Member with Task ID {id} not found.");
			}

			return Ok(mapper.Map<User, ResultUserDto>(user));
		}

		[HttpGet("{id}")]
		public ActionResult<ResultUserDto> GetUserById(Guid id)
		{
			var user = userRepository.GetById(id);
			if (user == null)
			{
				return NotFound($"User with ID {id} not found.");
			}

			return Ok(mapper.Map<User, ResultUserDto>(user));
		}

		[HttpPost]
		public async Task<ActionResult> CreateUserAsync(CreateUserDto createUserDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid user data.");
			}

			if (userRepository.NameCannotDuplicateWhenInserted(createUserDto.Name))
			{
				ModelState.AddModelError(createUserDto.Name, "User name already exists");
				return BadRequest($"User with name {createUserDto.Name} already exists.");
			}

			if(userRepository.EmailCannotDuplicateWhenInserted(createUserDto.Email!) && !string.IsNullOrEmpty(createUserDto.Email))
			{
                ModelState.AddModelError(createUserDto.Email, "Email address already exists");
                return BadRequest($"User with email {createUserDto.Email} already exists.");
			}

			var user = mapper.Map<CreateUserDto, User>(createUserDto);
			user.PasswordHash = hasher.Hash(createUserDto.Password);
			user.CreatedAt = DateTime.Now;
			await userRepository.AddAsync(user);

			return Ok("New User saved");
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid user data.");
			}

			var existingUser = userRepository.GetById(id);
			if (existingUser == null)
			{
				return NotFound($"User with ID {id} not found.");
			}

			if (userRepository.NameCannotDuplicateWhenUpdated(id, updateUserDto.Name))
			{
				ModelState.AddModelError(updateUserDto.Name, "User name already exists");
				return BadRequest($"User with name {updateUserDto.Name} already exists.");
			}

			if (userRepository.EmailCannotDuplicateWhenUpdated(id, updateUserDto.Email!) && !string.IsNullOrEmpty(updateUserDto.Email))
			{
				ModelState.AddModelError(updateUserDto.Email, "Email address already exists");
				return BadRequest($"User with email {updateUserDto.Email} already exists.");
			}

			existingUser.Avatar = updateUserDto.Avatar;
			existingUser.TeamId = updateUserDto.TeamId;
			existingUser.Name = updateUserDto.Name;
			existingUser.Email = updateUserDto.Email;
			existingUser.Status = updateUserDto.Status;
			existingUser.Role = updateUserDto.Role;
			existingUser.UpdatedAt = DateTime.Now;
			await userRepository.UpdateAsync(existingUser);
			return Ok("User updated successfully.");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteUserAsync(Guid id)
		{
			var user = userRepository.GetById(id);
			if (user == null)
			{
				return NotFound($"User with ID {id} not found.");
			}

			await userRepository.DeleteAsync(id);
			return Ok("User deleted successfully.");
		}




	}
}
