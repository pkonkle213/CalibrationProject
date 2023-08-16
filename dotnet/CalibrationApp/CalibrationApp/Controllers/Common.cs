using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalibrationApp.Controllers
{
    public class Common : ControllerBase
    {
        internal int GetCurrentUserID(ClaimsPrincipal user)
        {
                var idClaim = user.FindFirst("sub");
                string idString = idClaim.Value;
                return int.Parse(idString);
        }
    }
}
