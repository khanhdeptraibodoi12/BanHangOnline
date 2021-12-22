using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Text.Models;
using System.Web.Mvc;

namespace Text.Controllers
{
    public class CartController : Controller
    {

        menfashionEntities db = new menfashionEntities();
        public List<Cart> getCart() //tao danh sach cart va luu no trong session
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart == null)
            {
                // neu danh sach cart chua ton tai thi khoi tao
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }

        public ActionResult AddToCart(int id, string strURL) //  them san pham vao gio hang
        {
            List<Cart> listCart = getCart();
            Cart item = listCart.Find(model => model.IdItem == id);
            if (item == null)
            {
                item = new Cart(id);
                listCart.Add(item);
                return Redirect(strURL);
            }
            else
            {
                item.Quantity++;
                return Redirect(strURL);
            }
        }
        private int Quanlity() // Lấy tổng số sản phẩm giỏ hàng hiện tại
        {
            int amount = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                amount = listCart.Sum(model => model.Quantity);
            }
            return amount;
        }

        private double TotalPrice() //  Lấy tổng số tiền sản phẩm
        {
            double total = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                total = listCart.Sum(model => model.PriceTotal);
            }
            return total;
        }
        // GET: Cart
        public ActionResult Cart()
        {
            List<Cart> listCart = getCart();
            Session["Cart"] = listCart;
            if (listCart.Count == 0)
            {
                return RedirectToAction("NoICart", "Cart");
            }
            ViewBag.quanlityItem = Quanlity();
            ViewBag.totalPrice = TotalPrice();
            return View(listCart);
            
        }
    }
}