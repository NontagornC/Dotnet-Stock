using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet_stock.DTOs.Account;
using dotnet_stock.Entities;
using dotnet_stock.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
//using dotnet_stock.Models;

namespace dotnet_stock.Controllers
{
      [Route("[controller]")]
      [ApiController]
      public class AccountController : ControllerBase
      {
            public IAccountService AccountService { get; set; }
            public AccountController(IAccountService accountService)
            {
                  this.AccountService = accountService;
            }

            [HttpPost("[action]")]
            public async Task<ActionResult> Register(RegisterRequest request)
            // ไม่มี payload อะไร return กลับ
            {
                  // dto เพื่อให้ request map type ให้ตรงกับ Account
                  var account = request.Adapt<Account>();
                  await AccountService.Register(account);
                  return StatusCode((int)HttpStatusCode.Created);
            }

            [HttpPost("[action]")]
            public async Task<ActionResult> Login(LoginRequest loginRequest)
            {
                  var account = await AccountService.Login(loginRequest.Username, loginRequest.Password);
                  if (account == null)
                  {
                        return Unauthorized();
                  }

                  return Ok(new { token = AccountService.GenerateToken(account) });

            }
      }
}