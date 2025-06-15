namespace WebLabRest.Models;

public class Cart
{
    public Dictionary<int, CartItem> CartItems { get; set; } = new();
    
    public void AddToCart(Dish dish)
    {
        if (CartItems.ContainsKey(dish.Id))
            CartItems[dish.Id].Qty++;
        else
            CartItems.Add(dish.Id, new CartItem { Item = dish, Qty = 1 });
    }
    
    public void RemoveItems(int id) => CartItems.Remove(id);
    public void ClearAll() => CartItems.Clear();
    public int Count => CartItems.Sum(item => item.Value.Qty);
    public double TotalCalories => CartItems.Sum(item => item.Value.Item.Calories * item.Value.Qty);
}