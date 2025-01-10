using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using test2.Data;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;

    private readonly UserManager<IdentityUser> _manager;
    private readonly ApplicationDbContext _context;


    public ChatHub (UserManager<IdentityUser>? manager,ApplicationDbContext context,ILogger<ChatHub> logger)
    {
        _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendMessage(string message)
    {
        var user = Context.User;
       
        string username = user != null ? _manager.GetUserName(user) ?? "Unknown User" : "Unknown User";

    
        await Clients.All.SendAsync("ReceiveMessage", username, message);
        string? userid = user!= null? _manager.GetUserId(user) ?? "ANONIMO": "ANONIMO";

        await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC addMensaje {userid},{message}");

    }
    public override async Task OnConnectedAsync()
    {
        var user = Context.User;
        string username = user != null ? _manager.GetUserName(user) ?? "Unknown User" : "Unknown User";

        await Clients.All.SendAsync("ReceiveMessage", "Aviso", $"{username} has joined the chat!");
        await base.OnConnectedAsync();
    }
}
