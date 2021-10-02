using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurniturePlus.Areas.Admin.Controllers
{
    //creating this base class to avoid using the Area and Authorixze attributes in all admin controllers.

    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public abstract class AdminController : Controller
    {
    }
}
