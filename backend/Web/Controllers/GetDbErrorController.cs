using CleanArch.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class GetDbErrorController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet()]
    public IActionResult ForceDbError()
    {
        // deliberately query a non-existent table or column to trigger a database error
        var _ = db.Database.ExecuteSqlRaw("SELECT * FROM NonExistentTable");
        return Ok();
    }
}
