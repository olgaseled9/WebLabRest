using Microsoft.AspNetCore.Mvc;
using WebLabRest.Models;

namespace WebLabRest.Controllers;

public class CartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session.Get<Cart>("cart");
        return View(cart);
    }
}