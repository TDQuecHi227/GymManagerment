using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagemement.Models
{
    public class CartManager
    {
        public static List<Cart> CartList { get; set; } = new List<Cart>();
        public static void AddToCart(Cart cart)
        {
            var existingCart = CartList.FirstOrDefault(c => c.ProductId == cart.ProductId);
            if (existingCart != null)
            {
                existingCart.Quantity += cart.Quantity;
            }
            else
            {
                CartList.Add(cart);
            }
        }
        public static void RemoveFromCart(int productId)
        {
            var cartItem = CartList.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                CartList.Remove(cartItem);
            }
        }
        public static void ClearCart()
        {
            CartList.Clear();
        }
    }
}
