using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entity
{
    public class Product:BaseEntity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public ProductBrand Brand { get; set; }

        public int BrandId { get; set; }

        public ProductType Type { get; set; }

        public int TypeId { get; set; }
    }
}
