using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using dotnet_stock.Interfaces;
using dotnet_stock.Data;
using dotnet_stock.Entities;
using static dotnet_stock.Installers.JwtInstaller;

namespace dotnet8_hero.Interfaces
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext databaseContext;
        private readonly JwtSettings jwtSettings;
        public AccountService(DatabaseContext databaseContext, JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
            this.databaseContext = databaseContext;
        }

        public async Task Register(Account account)
        {
            var existingAccount = await databaseContext.Accounts.SingleOrDefaultAsync(a => a.Username == account.Username);
            if (existingAccount != null)
            {
                throw new Exception("Existing Account");
            }

            account.Password = CreatePasswordHash(account.Password);
            databaseContext.Accounts.Add(account);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<Account?> Login(string username,
                                          string password)
        {
            var account = await databaseContext.Accounts.Include(a => a.Role)
            .SingleOrDefaultAsync(a => a.Username == username);

            if (account != null && VerifyPassword(account.Password, password))
            {
                return account;
            }

            return null;
        }

        private string CreatePasswordHash(string password)
        {
            // hasd ให้เป็นเลขฐาน 16
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            // รับ hashed password มาถอดเช็คดู
            var parts = hashedPassword.Split('.', 2);
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var passwordHashed = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));

            return passwordHashed == hashed;
        }

        public string GenerateToken(Account account)
        {
            // paylaod ว่าส่งอะไรบ้าง
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim("role", account.Role.Name),
                new Claim("additional", "todo"),
                new Claim("level", "general"),
            };

            return BuildToken(claims);
        }

        public Account GetInfo(string accessToken)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var username = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            var role = token.Claims.First(claim => claim.Type == "role").Value;

            var account = new Account
            {
                Username = username,
                Role = new Role
                {
                    Name = role
                }
            };

            return account;
        }

        private string BuildToken(Claim[] claims)
        {
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.Expire));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            // claims = payload
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}