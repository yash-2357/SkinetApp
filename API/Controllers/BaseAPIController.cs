using API.RequestHelper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class BaseAPIController : ControllerBase{
    protected async Task<ActionResult<IReadOnlyList<T>>>  CreatePagedResult<T>(IGenericRepository<T> repo,ISpecification<T> spec,int pageIndex,int pageSize ) where T : BaseEntity {

      var items = await repo.ListAsync(spec);
      var count = await repo.CountAsync(spec);
      var pagination = new Pagination<T>(pageIndex,pageSize,count,items);

      return Ok(pagination);
    }
  }
}
