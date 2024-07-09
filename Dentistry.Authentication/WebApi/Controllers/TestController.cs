using Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("open")]
    public IActionResult GetOpenInfo()
    {
        return Ok("Open endpoint");
    }

    [HttpGet("auth-only")]
    [Authorize]
    public IActionResult GetInfoForAuth()
    {
        return Ok("You are authorized.");
    }

    [HttpGet("doctor-only")]
    [Authorize(Policy = AuthenticationPolicies.DoctorOnly)]
    public IActionResult GetInfoForDoctor()
    {
        return Ok("You are a doctor.");
    }
    [HttpGet("patient-only")]
    [Authorize(Policy = AuthenticationPolicies.PatientOnly)]
    public IActionResult GetInfoForPatient()
    {
        return Ok("You are a patient.");
    }
    [HttpGet("admin-only")]
    [Authorize(Policy = AuthenticationPolicies.AdminOnly)]
    public IActionResult GetInfoForAdmin()
    {
        return Ok("You are an admin.");
    }
}