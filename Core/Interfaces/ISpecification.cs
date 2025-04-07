using System.Linq.Expressions;

namespace Core.Interfaces {
  public interface ISpecification<T> {
    Expression<Func<T, bool>>? Criteria { get; }
    
    // used object here as order by can be done on different property types
    Expression<Func<T, object>>? OrderBy { get;}
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDistinct { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
    IQueryable<T> ApplyCriteria(IQueryable<T> query);
  }

  public interface ISpecification<T, TResult> : ISpecification<T> {
    Expression<Func<T,TResult>>? Select { get; }
  }
}
