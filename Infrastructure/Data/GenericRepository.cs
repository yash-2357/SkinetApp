﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data {
  public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity {
    public void Add(T entity) {
      //Set method is used to find the type of T list
      context.Set<T>().Add(entity);
    }

    public void Remove(T entity) {
      context.Set<T>().Remove(entity);
    }

    public bool Exists(int id) {
      return context.Set<T>().Any(x => x.Id == id);
    }

    public async Task<T?> GetByIdAsync(int id) {
      return await context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync() {
      return await context.Set<T>().ToListAsync();
    }

    public async Task<bool> SaveAllAsync() {
      return await context.SaveChangesAsync() > 0;
    }

    public void Update(T entity) {
      context.Set<T>().Attach(entity);
      context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification) {
      return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> specification) {
      return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification) {
      return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) {
      return SpecificationEvaluator<T>.GetQuery<T,TResult>(context.Set<T>().AsQueryable(), specification);
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification) {
      return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification) {
      return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec) {
      var query = context.Set<T>().AsQueryable();
      query = spec.ApplyCriteria(query);

      return await query.CountAsync();
    }
  }
}
