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
                listCart.RemoveAll(model => model.IdItem == id);
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
        public static string ConvertTimeTo24(string hour) 
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }
        public static string CreateKey(string tiento) // Tạo chuỗi mã hóa ngày - giờ cho mã hóa đơn
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]); 
            key = key + t;
            return key;
        }
        [HttpPost]
        public ActionResult Checkout(FormCollection formCollection)
        {
            Invoince donhang = new Invoince();
            Member member = (Member)Session["UserLogin"];
            List<Cart> listcart = getCart();
            donhang.invoinceNo = CreateKey("HD");
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