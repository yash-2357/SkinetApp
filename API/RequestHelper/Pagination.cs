﻿namespace API.RequestHelper {
  public class Pagination<T> {
    public int PageIndex {  get; set; }
    public int PageSize { get; set; } 
    public int Count { get; set; }  
    public IReadOnlyList<T> Data {  get; set; }

    public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data) {
      PageSize = pageSize;
      PageIndex = pageIndex;  
      Count = count;
      Data = data;
    }
  }
}
