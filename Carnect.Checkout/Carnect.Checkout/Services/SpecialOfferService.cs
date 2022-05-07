using Carnect.Checkout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carnect.Checkout.Services
{
    public class SpecialOfferService
    {
        /// <summary>
        /// Add new offer
        /// </summary>
        /// <param name="allSpecialOffers">List of all offers</param>
        /// <param name="specialOffer">New offer</param>
        /// <exception cref="Exception">Throw exeception when one of params is null</exception>
        public void AddSpecialOffer(List<SpecialOffer> allSpecialOffers, SpecialOffer specialOffer)
        {
            if (allSpecialOffers == null || specialOffer == null)
            {
                throw new Exception("Bad data");
            }

            ValidateOffer(allSpecialOffers, specialOffer);
            allSpecialOffers.Add(specialOffer);
        }

        /// <summary>
        /// Validate new offer
        /// </summary>
        /// <param name="allSpecialOffers">List of all offers</param>
        /// <param name="specialOffer">New offer</param>
        /// <exception cref="Exception">Throw exeception when new offer is not valid</exception>
        private void ValidateOffer(List<SpecialOffer> allSpecialOffers, SpecialOffer specialOffer)
        {
            if (specialOffer.FromDate > specialOffer.ToDate)
            {
                throw new Exception("From date cannot be greater than to date");
            }

            if (specialOffer.SpecialPrice < 0)
            {
                throw new Exception("Price cannot be less than 0");
            }

            bool isSpecialOfferExist = allSpecialOffers.Any(x => x.ProductId == specialOffer.ProductId &&
              ((specialOffer.FromDate >= x.FromDate && specialOffer.FromDate <= x.ToDate) || (specialOffer.ToDate >= x.FromDate && specialOffer.FromDate <= x.ToDate)));
            if (isSpecialOfferExist)
            {
                throw new Exception("Special offer is exist");
            }
        }
    }
}
