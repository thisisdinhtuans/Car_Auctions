using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
namespace IdentityService.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Index : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public Index(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public RegisterViewModel Input{ get; set; }

        [BindProperty]
        public bool RegisterSuccess { get; set; }

        public IActionResult OnGet(string returnUrl)
        {   
            Input=new RegisterViewModel
            {
                returnUrl=returnUrl,
            };

            return Pages();
        }

        public async Task<IActionResult> OnPost()
        {
            if(Input.Button!="register") return Redirect("~/");

            if(ModelState.IsValid)
            {
                var user=new ApplicationUser{
                    UserName=Input.Username,
                    Email=Input.Email,
                    EmailConfirmed=true
                };

                var result=await _userManager.CreateAsync(user, Input.Password);

                if(result.Succed)
                {
                    await _userManager.AddClaimsAsync(user, new Claims[]
                    {
                        new Claims(JwtClaimType.Name, Input.FullName)
                    });
                    RegisterSuccess=true;
                }
            }

            return Pages();
        }
    }
}
