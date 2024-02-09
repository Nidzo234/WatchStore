using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchStore.Domain
{
    public class ProductDto
    {
        public Guid id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public double ProductRating { get; set; }
    }
}
