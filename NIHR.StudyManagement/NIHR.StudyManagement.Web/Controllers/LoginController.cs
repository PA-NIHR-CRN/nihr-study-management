using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NIHR.StudyManagement.Web.Models;

namespace NIHR.StudyManagement.Web.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(LoginViewModel model)
    {
        return View(model);
    }

}
