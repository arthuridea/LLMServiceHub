using LLMServiceHub.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LLMServiceHub.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class SignOutModel : PageModel
    {
        /// <summary>
        /// Called when [get].
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync();

            return await SignOutExternal([AppConsts.MicrosoftAuthScheme, AppConsts.GitHubAuthScheme], returnUrl);            
        }

        private async Task<IActionResult> SignOutExternal(string[] schemes, string returnUrl = null)
        {
            foreach (var scheme in schemes)
            {
                var oauthResult = await HttpContext.AuthenticateAsync(scheme);
                if (oauthResult.Succeeded)
                {
                    return RedirectToAction("SignOut", "Account", new { Area = $"{scheme}Identity", scheme, returnUrl });
                }
            }
            return LocalRedirect(returnUrl);
        }
    }
}
