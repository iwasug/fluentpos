﻿using System.Threading.Tasks;
using FluentPOS.Modules.Identity.Core.Abstractions;
using FluentPOS.Shared.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluentPOS.Modules.Identity.Controllers
{
    internal class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [HttpGet("roles/{id}")]
        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            var userRoles = await _userService.GetRolesAsync(id);
            return Ok(userRoles);
        }
    }
}