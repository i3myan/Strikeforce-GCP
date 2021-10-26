using Microsoft.AspNetCore.Authentication;
using OCDETF.StrikeForce.Business.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Web.API.Security
{
    public class CustomRolesClaimsTransformation : IClaimsTransformation
    {
        private readonly IUserService _userService;

        public CustomRolesClaimsTransformation(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            // Support AD and local accounts
            var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier ||
                                                              c.Type == ClaimTypes.Name);
            if (nameId == null)
            {
                return principal;
            }
            this._userService.Get(nameId.Value);
            //var claim = new Claim(ClaimTypes.Role, "Administrator");
            //newIdentity.AddClaim(claim);

            // Get user from database
            //var user = await _userService.GetByUserName(nameId.Value);
            //if (user == null)
            //{
            //    return principal;
            //}

            // Add role claims to cloned identity
            //foreach (var role in user.Roles)
            //{
            //    var claim = new Claim(ClaimTypes.Role, "Administrator");
            //    newIdentity.AddClaim(claim);
            //}

            return clone;
        }
    }
}
