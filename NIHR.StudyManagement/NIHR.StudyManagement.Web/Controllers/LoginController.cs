using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NIHR.StudyManagement.Web.Models;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Configuration;
using Hl7.FhirPath.Sprache;

namespace NIHR.StudyManagement.Web.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly AuthenticationSettings _options;

    public LoginController(ILogger<LoginController> logger,
        IOptions<AuthenticationSettings> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public IActionResult Index()
    {
        var loginViewModel = new LoginViewModel
        {
            NhsLoginUrl = "",
            IdgLoginUrl = _options.NihrIdgLoginUrl
        };

        return View(loginViewModel);
    }

}
