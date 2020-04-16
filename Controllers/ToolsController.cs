using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookkeeper.Models;
using Microsoft.AspNetCore.Identity;

namespace Bookkeeper.Controllers
{
    public class ToolsController : Controller
    {
        private readonly IToolsUtils toolsUtils;
        private readonly UserManager<IdentityUserExtended> userManager;
        public ToolsController(IToolsUtils _toolsUtils, UserManager<IdentityUserExtended> _userManager)
        {
            toolsUtils = _toolsUtils;
            userManager = _userManager;
        }

        [HttpGet]
        public IActionResult ImportExcelFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelFile(ImportExcel file)
        {
            if (!User.Identity.IsAuthenticated || file == null || !ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            IdentityUserExtended user = await userManager.GetUserAsync(User);
            bool success = await toolsUtils.ImportExcelToDatabase(file, user.UserInfoID);
            return View();
        }
    }
}