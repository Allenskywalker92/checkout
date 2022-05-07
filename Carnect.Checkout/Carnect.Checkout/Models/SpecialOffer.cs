using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carnect.Checkout.Models
{
    public class SpecialOffer
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int NumberOfProduct { get; set; }

        public decimal SpecialPrice { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
