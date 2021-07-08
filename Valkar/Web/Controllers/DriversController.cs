namespace Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    [Authorize]
    public class DriversController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
