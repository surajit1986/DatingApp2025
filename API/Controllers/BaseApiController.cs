using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    // This class can be used to add common functionality for all API controllers
    // For example, you can add logging, error handling, or authentication here
}