using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NIHR.StudyManagement.Web.Models;


namespace NIHR.StudyManagement.Web.Controllers;

[Authorize]
public class GrisId01Controller : Controller
{

    public IActionResult Index()
    {
        ViewData["Title"] = "GrisId01";
        return View(new GrisId01Model());
    }

    [HttpPost]
    public IActionResult Index(GrisId01Model model)
    {
        ViewData["Title"] = "GrisId01";
        return View(model);
    }

}

