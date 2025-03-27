using Core.Entities;

namespace Core.Specifications {
  public class BrandSpecification : BaseSpecification<Product,string> {
    public BrandSpecification() {
      AddSelect(x => x.Brand);
      ApplyDistinct();
    }
  }
}
