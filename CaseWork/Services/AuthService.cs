﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using CaseWork.Data;
using CaseWork.Models;
using CaseWork.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CaseWork.Services;

public class AuthService : IAuthService
{
    private readonly CaseWorkContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(CaseWorkContext dbContext, IConfiguration configuration, IMapper mapper)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<bool> EmailExists(string email)
        => await _dbContext.Users.CountAsync(v => v.Email == email) != 0;

    public async Task<string> Login(UserLogin userLogin)
    {
        var candidate = await _dbContext.Users.FirstOrDefaultAsync(v => v.Email == userLogin.Email);
        if (candidate == null) throw new Exception("User not found!");
        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, candidate.Password))
            throw new Exception("Wrong data!");
        
        return GenerateToken(candidate);
    }

    public async Task<string> Create(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Config:Secret").Value!));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: cred);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}