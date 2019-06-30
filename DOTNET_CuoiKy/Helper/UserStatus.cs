
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DOTNET_CuoiKy.Helper
{
    public class UserStatus
    {
        // get User status by taking User from controller base 
        // filter out by checkking client attributes then we get the right 
        // Authentication attributes for each user !!!
        public static bool getUserStatus(ControllerBase controller, string client)
        {
            if (client.Equals("Client"))
            {
                // Check Startup.cs for config to understands
                return (controller.User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
                    && controller.User.Identity.IsAuthenticated);
            }
            return (controller.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme
                && controller.User.Identity.IsAuthenticated);
        }
        public static bool getRazorPageUserStatus(RazorPageBase razor, string client)
        {
            if (client.Equals("Client"))
            {
                // Check Startup.cs for config to understands
                return (razor.User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
                    && razor.User.Identity.IsAuthenticated);
            }
            return (razor.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme
                && razor.User.Identity.IsAuthenticated);
        }
    }
}
