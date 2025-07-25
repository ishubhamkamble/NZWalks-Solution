﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NzWalksAPI.Models.DTO;
using NzWalksAPI.Repositories;

namespace NzWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //POST : /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            { 
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                //Add Roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User registered Successfully, Please do login.");
                    }
                } 
            }

            return BadRequest("Something went wrong...!!");
        }

        //POST : /api/auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByNameAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkpasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkpasswordResult)
                {
                    //Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //createToken
                        var newJwttoken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto()
                        {
                            NewJwtToken = newJwttoken,
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username OR Password incorrect");
        }
    }
}
