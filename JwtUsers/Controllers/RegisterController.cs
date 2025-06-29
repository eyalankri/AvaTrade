﻿using Common.Repositories;
using JwtUsers.Dtos;
using Members.Constants;
using Members.Entities;
using Members.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Members.Controllers;


[Route("[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly IRepository<User> _userRepository;
    private readonly MembersApiSettings _membersApiSettings;

    public RegisterController(IConfiguration config, IRepository<User> userRepository)
    {
        _userRepository = userRepository;
        _config = config;
        _membersApiSettings = _config.GetSection(nameof(MembersApiSettings)).Get<MembersApiSettings>()!;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {         
        try
        {
            var user = await _userRepository.GetAsync(u => u.Email == createUserDto.Email);           
            if (user != null)
            {
                return NotFound("Email is already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, _membersApiSettings.PasswordSalt);

        var newUser = new User
        {
            Email = createUserDto.Email,
            Password = passwordHash,
            Name = createUserDto.Name,
            Surname = createUserDto.Surname,
            Role = "Registered" 
        };

        await _userRepository.CreateAsync(newUser);

        return Ok(newUser.AsDto());
    }
}
