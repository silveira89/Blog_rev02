﻿using Blog_rev02.Data;
using Blog_rev02.Extensions;
using Blog_rev02.Models;
using Blog_rev02.Services;
using Blog_rev02.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog_rev02.Controllers {

    [ApiController]
    public class AccountController : ControllerBase {
        private readonly TokenService _tokenService;
        public AccountController(TokenService tokenService) {
            _tokenService = tokenService;
        }

        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] BlogDataContext context, [FromServices] EmailService emailService) {
            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }
            var user = new User {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };
            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);
            try {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                emailService.Send(
                    user.Name,
                    user.Email,
                    "Bem vindo ao blog!",
                    $"A sua senha é <strong>{password}</strong>");
                return Ok(new ResultViewModel<dynamic> (new {
                    user = user.Email, password
                }));
            } catch (DbUpdateException ex) {
                return StatusCode(400, new ResultViewModel<string>("Este e-mail já está cadastrado."));
            } catch (Exception ex) {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor."));
            }
        }

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] BlogDataContext context) {
            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }
            var user = await context.Users.AsNoTracking().Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null) {
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));
            }
            if (!PasswordHasher.Verify(user.PasswordHash, model.Password)) {
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));
            }
            try {
                var token = _tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            } catch {
                return StatusCode(500, new ResultViewModel<string>("Falha interna no servicor."));
            }
        }
    }
}
