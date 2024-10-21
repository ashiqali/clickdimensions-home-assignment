using AutoMapper;
using SocialSyncPortal.BLL.Utilities.CustomExceptions;
using SocialSyncPortal.DAL.Entities;
using SocialSyncPortal.DAL.Repositories.IRepositories;
using SocialSyncPortal.DTO.DTOs.User;
using Microsoft.Extensions.Logging;
using SocialSyncPortal.BLL.Services.IServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<UserDTO>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var usersToReturn = await _userRepository.GetListAsync(cancellationToken: cancellationToken);
        _logger.LogInformation("List of {Count} users has been returned", usersToReturn.Count);

        return _mapper.Map<List<UserDTO>>(usersToReturn);
    }

    public async Task<UserDTO> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("User with userId = {UserId} was requested", userId);
        var userToReturn = await _userRepository.GetAsync(x => x.Id == userId, cancellationToken);

        if (userToReturn is null)
        {
            _logger.LogError("User with userId = {UserId} was not found", userId);
            throw new UserNotFoundException();
        }

        return _mapper.Map<UserDTO>(userToReturn);
    }
    public async Task<UserDTO> AddUserAsync(UserToAddDTO userToAddDTO)
    {

        var existingUser = await _userRepository.GetAsync(c => c.Username == userToAddDTO.Username);
        if (existingUser != null)
        {
            _logger.LogError("User {Username} already exists", userToAddDTO.Username);
            throw new DuplicateUserException();
        }
        userToAddDTO.Username = userToAddDTO.Username.ToLower();

        var addedUser = await _userRepository.AddAsync(_mapper.Map<User>(userToAddDTO));
        return _mapper.Map<UserDTO>(addedUser);
    }

    public async Task<UserDTO> UpdateUserAsync(UserToUpdateDTO userToUpdateDTO)
    {
        userToUpdateDTO.Username = userToUpdateDTO.Username.ToLower();
        var user = await _userRepository.GetAsync(x => x.Id == userToUpdateDTO.Id);        

        if (user is null)
        {
            _logger.LogError("User with userId = {UserId} was not found", userToUpdateDTO.Id);
            throw new UserNotFoundException();
        }
        else
        {
            user.ModifiedDate = DateTime.UtcNow;
        }

        var userToUpdate = _mapper.Map<User>(userToUpdateDTO);

        _logger.LogInformation("User with these properties: {@UserToUpdate} has been updated", userToUpdateDTO);

        return _mapper.Map<UserDTO>(await _userRepository.UpdateUserAsync(userToUpdate));
    }

    public async Task DeleteUserAsync(int userId)
    {
        var userToDelete = await _userRepository.GetAsync(x => x.Id == userId);

        if (userToDelete is null)
        {
            _logger.LogError("User with userId = {UserId} was not found", userId);
            throw new UserNotFoundException();
        }

        await _userRepository.DeleteAsync(userToDelete);
    }
}
