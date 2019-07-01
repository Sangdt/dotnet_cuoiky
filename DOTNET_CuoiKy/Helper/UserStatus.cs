
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
                // Check login action in controller for more detail      

                // Check if there is a user login and check if an admin or not
                return (controller.User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
                    && controller.User.Identity.IsAuthenticated);
            }
            return (controller.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme
                && controller.User.Identity.IsAuthenticated);

            // Đoạn code này không sai nhưng sợ loz Nhân đéo hiểu nên để đó :))))
            //return client.Equals("Client") ? (controller.User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
            //            && controller.User.Identity.IsAuthenticated) : (controller.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme
            //                                                            && controller.User.Identity.IsAuthenticated);
        }

        // Dùng trên file cshtml
        public static bool getRazorPageUserStatus(RazorPageBase razor, string client)
        {
            if (client.Equals("Client"))
            {
                // Check if there is a user login and check if an admin or not
                return (razor.User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
                     && razor.User.Identity.IsAuthenticated);
            }

            return (razor.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme
                && razor.User.Identity.IsAuthenticated);

            // Đoạn code này không sai nhưng sợ loz Nhân đéo hiểu nên để đó haha :))))
            //return client.Equals("Client") ? (razor.User.Identity.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme
            //            && razor.User.Identity.IsAuthenticated) : (razor.User.Identity.AuthenticationType != CookieAuthenticationDefaults.AuthenticationScheme
            //                                                            && razor.User.Identity.IsAuthenticated);
        }
    }
}
