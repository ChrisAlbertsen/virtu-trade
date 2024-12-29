
using Microsoft.AspNetCore.Mvc;

public interface ITradingPlatformController {
    Task<IActionResult> GetData();
} 