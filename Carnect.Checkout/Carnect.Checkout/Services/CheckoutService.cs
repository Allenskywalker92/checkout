using Carnect.Checkout.Models;

namespace Carnect.Checkout.Services
{
    public class CheckoutService
    {
        /// <summary>
        /// Calculate the price of bought products
        /// </summary>
        /// <param name="allProducts">Lst of all products</param>
        /// <param name="allSpecialOffers">List of special offers</param>
        /// <param name="boughtProducts">List of bought products</param>
        /// <returns>Return total price </returns>
        /// <exception cref="Exception">Throw error when one of any params is null and bought product is not exist</exception>
        public decimal Calculate(List<Product> allProducts, List<SpecialOffer> allSpecialOffers, List<Product> boughtProducts)
        {
            if (allProducts == null || allSpecialOffers == null || boughtProducts == null) throw new Exception("Bad data");
            Dictionary<string, int> groupBuyProducts = GroupBoughtProducts(boughtProducts);

            decimal total = 0;
            foreach (var item in groupBuyProducts)
            {
                var product = allProducts.SingleOrDefault(x => x.Name.Equals(item.Key));
                if (product == null) throw new Exception("Product is not exist");

                var specialOffer = allSpecialOffers.FirstOrDefault(x => x.ProductId.Equals(product.Id) && x.FromDate <= DateTime.Now.Date && x.ToDate >= DateTime.Now.Date);
                int quantity = item.Value;
                if (specialOffer != null)
                {
                    int numberOfSpecialPrice = quantity % specialOffer.NumberOfProduct;
                    total += numberOfSpecialPrice * specialOffer.SpecialPrice;
                    quantity -= numberOfSpecialPrice * specialOffer.NumberOfProduct;
                }

                total += quantity * product.Price;
            }

            return total;
        }

        private Dictionary<string, int> GroupBoughtProducts(List<Product> boughtProducts)
        {
            Dictionary<string, int> groupBuyProducts = new Dictionary<string, int>();
            foreach (var item in boughtProducts)
            {
                if (groupBuyProducts.ContainsKey(item.Name))
                {
                    groupBuyProducts[item.Name] = groupBuyProducts[item.Name] + 1;
                    continue;
                }

                groupBuyProducts.Add(item.Name, 1);
            }

            return groupBuyProducts;
        }
    }
}
