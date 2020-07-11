using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionSolution
{
    class Product
    {
        public string SKUID { get; set; }
        public int Price { get; set; }

    }
    class Promotion
    {

        public string productskku { get; set; }
        public int Price { get; set; }
        public bool isProductClub { get; set; }
        public int  ProductQuantity { get; set; }
    }
    class PromotionProcessing
    {
        List<Product> lstProduct = new List<Product>();
      static   List<Promotion> lstPromotion = new List<Promotion>();
        public PromotionProcessing()
        {
            lstProduct.Add(new Product() { SKUID = "A", Price = 50 });
            lstProduct.Add(new Product() { SKUID = "B", Price = 30 });
            lstProduct.Add(new Product() { SKUID = "C", Price = 20 });
            lstProduct.Add(new Product() { SKUID = "D", Price = 15 });

            lstPromotion.Add(new Promotion() { productskku = "A", Price = 130, ProductQuantity = 3 });
            lstPromotion.Add(new Promotion() { productskku = "B", Price = 45, ProductQuantity = 2 });
            lstPromotion.Add(new Promotion() { productskku = "C+D", Price = 30, isProductClub = true });
        }

        public void AddPromotion(Promotion pro)
        {
            lstPromotion.Add(pro);
        }
        public int GetFinalPriceAfterPromotion(List<ProductCart> lstcart)
        {
            int total = default(int);
             
            foreach ( var data  in lstPromotion)
            {
                if(!data.isProductClub)
                {  
                    var pro = lstcart.Where(x => x.Sku == data.productskku).FirstOrDefault();
                    var prodDetail = lstProduct.Where(x => x.SKUID == pro.Sku).FirstOrDefault();
                    int number = pro.NoOfProduct - data.ProductQuantity;
                    int reminder = pro.NoOfProduct % data.ProductQuantity;
                    if (reminder >= 0)
                    {
                        var promo = pro.NoOfProduct / data.ProductQuantity;
                        total = total + promo* data.Price + reminder * prodDetail.Price;
                        lstcart.Remove(pro);
                    }
                    
                    
                }
                else
                {
                    var skusplit = data.productskku.Split("+");
                    var pro1 = lstcart.Where(x => x.Sku == skusplit[0]).FirstOrDefault();
                    var pro2 = lstcart.Where(x => x.Sku == skusplit[1]).FirstOrDefault();
                    if (pro1 != null && pro2 != null)
                    {
                        total = total + data.Price;
                        lstcart.Remove(pro1);
                        lstcart.Remove(pro2);

                    }


                }

            }
            foreach (var d in lstcart)
            {
                var prodDetail = lstProduct.Where(x => x.SKUID == d.Sku).FirstOrDefault();

                total = total + d.NoOfProduct * prodDetail.Price;
            }

            return total;
        }
    }
    class ProductCart
    {
        public int NoOfProduct { get; set; }
        public string Sku { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<ProductCart> lstcart = new List<ProductCart>();

            Console.WriteLine("how many product in order");
            int qurderQuantity = 0;
            var conversionResult = int.TryParse(Console.ReadLine(), out qurderQuantity);
            if (!conversionResult)
            {
                Console.WriteLine("Enter quantity in number");
            }
            else
            {
                for (int i = 0; i < qurderQuantity; i++)
                {
                    var productDetail = Console.ReadLine().Split();
                    lstcart.Add(new ProductCart() { NoOfProduct = Convert.ToInt32(productDetail[0]), Sku = productDetail[1] });
                }
            }
            PromotionProcessing ob = new PromotionProcessing();
            Console.WriteLine(ob.GetFinalPriceAfterPromotion(lstcart));
            Console.ReadLine();

        }
    }
}
