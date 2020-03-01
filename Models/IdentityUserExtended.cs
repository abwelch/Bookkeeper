using Microsoft.AspNetCore.Identity;


namespace Bookkeeper.Models
{
    public class IdentityUserExtended : IdentityUser
    {
        public int UserInfoID { get; set; }
    }
}
