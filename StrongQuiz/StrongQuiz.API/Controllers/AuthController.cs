using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StrongQuiz.API.Models;
using StrongQuiz.Models.Models;

namespace StrongQuiz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(SignInManager<ApplicationUser> signInMgr)
        {
            this._signInManager = signInMgr;
        }

      

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login_DTO loginDTO)
        {
            var returnMessage = "";
            if (!ModelState.IsValid)
                return BadRequest("Onvolledige gegevens.");
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);

                if (result.Succeeded) { return Ok("Welkom " + loginDTO.UserName); }

                throw new Exception("User of paswoord niet gevonden.");
            }
            catch (Exception exc) { returnMessage = $"Foutief of ongeldig request: {exc.Message}"; ModelState.AddModelError("", returnMessage); }

            return BadRequest(returnMessage);
        }

    }
}
