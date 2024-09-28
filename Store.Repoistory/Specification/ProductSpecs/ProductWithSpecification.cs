using Store.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Specification.ProductSpecs
{
    public class ProductWithSpecification : BaseSpecification<Product>
    {
        public ProductWithSpecification(ProductSpecification specs) : 
            base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) && 
                             (!specs.TypeId.HasValue || product.TypeId == specs.TypeId.Value) && 
                              (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower().Contains(specs.Search)))
            
        {
            AddIncludes(x => x.Brand);
            AddIncludes(x => x.Type);
            AddOrderBy(x => x.Name);
            AddPaginated(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);

            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch(specs.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
          
        }

        public ProductWithSpecification(int? Id) : 
            base(product => product.Id == Id)
        {
            AddIncludes(x => x.Brand);
            AddIncludes(x => x.Type);
        }
    }
}
