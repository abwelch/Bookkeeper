using System.Threading.Tasks;
using Bookkeeper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookkeeper.Controllers
{
    public class StatementController : Controller
    {
        private readonly UserManager<IdentityUserExtended> userManager;
        private readonly IStatementUtils statementUtils;
        public StatementController(UserManager<IdentityUserExtended> _userManager, IStatementUtils _statementUtils)
        {
            userManager = _userManager;
            statementUtils = _statementUtils;
        }

        public IActionResult Overview()
        {
            return View();
        }

        public async Task<IActionResult> BalanceSheet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(null);
            }
            IdentityUserExtended user = await userManager.GetUserAsync(User);
            BalanceSheetViewModel model = statementUtils.GenerateBalanceSheet(user.UserInfoID);
            return View(model);
        }
    }
}