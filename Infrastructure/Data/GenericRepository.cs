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
  }
}
