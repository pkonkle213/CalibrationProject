using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalibrationApp.Controllers
{
    public class Common : ControllerBase
    {
        internal int GetCurrentUserID(ClaimsPrincipal user)
        {
            try
            {
                var idClaim = user.FindFirst("sub");
                string idString = idClaim.Value;
                return int.Parse(idString);
            }
            catch (NullReferenceException ex)
            {
                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
