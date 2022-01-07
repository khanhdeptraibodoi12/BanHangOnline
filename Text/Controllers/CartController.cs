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
            Member member = (Member)Session["UserLogin"]; 
            List<Cart> listCart = getCart();
            Cart item = listCart.Find(model => model.IdItem == id);
            if (Session["UserLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
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
           
        }
        public int Quanlity() // Lấy tổng số sản phẩm giỏ hàng hiện tại
        {
            int amount = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                amount = listCart.Sum(model => model.Quantity);
            }
            return amount;
        }

        public double TotalPrice() //  Lấy tổng số tiền sản phẩm
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
            ViewBag.quanlityItem = Quanlity();
            ViewBag.totalPrice = TotalPrice();
            return View(listCart);
            
        }

        public ActionResult DeteteCart(int id)
        {
            List<Cart> listCart = getCart();
            Cart item = listCart.SingleOrDefault(model => model.IdItem == id);
            if (item != null)
            {
                listCart.Remove(item);
                return RedirectToAction("Cart", "Cart");
            }
            return RedirectToAction("Cart","Cart");
        }
        public ActionResult Checkout()
        {

            if (Session["UserLogin"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("ShopCatalog", "Product");
            }
            List<Cart> listcart = getCart();
            ViewBag.QuanlityItem = Quanlity();
            ViewBag.totalPrice = TotalPrice();
            return View(listcart);
        }
        [HttpPost]
        public ActionResult Checkout(FormCollection formCollection)
        {
            Invoince donhang = new Invoince();
            Member member = (Member)Session["UserLogin"];
            List<Cart> listcart = getCart();
            donhang.invoinceNo = createKey.CreateKey("HD");
            donhang.userName = member.userName;
            donhang.dateOrder = DateTime.Now;
            donhang.status = true;
            donhang.deliveryDate = null;
            donhang.deliveryStatus = null;
            donhang.totalMoney = (int)TotalPrice();
            db.Invoinces.Add(donhang);
            db.SaveChanges();
            foreach (var i in listcart)
            {
                InvoinceDetail ctdh = new InvoinceDetail();
                ctdh.invoinceNo = donhang.invoinceNo;
                ctdh.productId = i.IdItem;
                ctdh.quanlityProduct = i.Quantity;
                ctdh.unitPrice = i.unitPrice;
                ctdh.totalPrice = (int?)i.PriceTotal;
                ctdh.totalDiscount = i.Discount * i.Quantity;
                db.InvoinceDetails.Add(ctdh);
            }
            db.SaveChanges();
            Session.Remove("Cart");
            return RedirectToAction("xacnhandonhang","Cart");

        }
        public ActionResult xacnhandonhang()
        {

            return View();
        }
        
    }
}