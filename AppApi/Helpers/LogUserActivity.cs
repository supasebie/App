using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AppApi.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AppApi.Helpers
{
  public class LogUserActivity : IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var resultsContext = await next();
      
      var userId = int.Parse(resultsContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
      var repo = resultsContext.HttpContext.RequestServices.GetService<IFriendsRepository>();
      var user = await repo.GetUser(userId);
      user.LastActive = DateTime.Now;
      await repo.SaveAll();
    }
  }
}