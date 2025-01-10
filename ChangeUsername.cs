using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using test2.Data;

namespace test2.Pages;

[Authorize]
public class CUModel : PageModel
{
    private readonly ILogger<CUModel> _logger;
    private readonly UserManager<IdentityUser> _manager;

    private readonly ApplicationDbContext _context;


    public CUModel(UserManager<IdentityUser> manager, ILogger<CUModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _manager = manager;
        _context = context;
    }
    [BindProperty]
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;

    public async Task OnPost()
    {   
        string? userid = _manager.GetUserId(User);
        Console.WriteLine("viva el chamuco");
        Console.WriteLine(_manager.GetUserId(User));

        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC updateUsername {userid},{Username}");
    }
}
