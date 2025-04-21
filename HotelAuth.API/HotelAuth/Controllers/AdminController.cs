using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost]
        [Route("AuthWithCredentials")]
        public async Task<IActionResult> AuthWithCredentials([FromBody] AdminDTO adminDTO)
        {
            string token = await adminService.AuthWithCredentials(adminDTO);
            TokenDTO tokenDTO = new TokenDTO { TokenDTOString = token };
            return Ok(tokenDTO);
        }
        [HttpPost]
        [Route("RegisterNewAdmin")]
        public async Task<IActionResult> RegisterNewAdmin([FromBody] AdminDTO adminDTO)
        {
            return Ok(await adminService.RegisterNewAdmin(adminDTO));
        }
        [HttpPost]
        [Route("VerifyToken")]
        public async Task<IActionResult> VerifyToken([FromBody] TokenDTO tokenDTO)
        {
            return Ok(await adminService.VerifyToken(tokenDTO));
        }
      

    }
}
