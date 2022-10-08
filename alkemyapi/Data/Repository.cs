using alkemyapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alkemyapi.Data
{
    public class Repository<TDbContext> : IRepository where TDbContext : DbContext
    {
        private readonly IConfiguration _appsettings;
        private readonly AppDbContext _context;
        private TDbContext _dbContext;

        public Repository(TDbContext context, IConfiguration appsettings, AppDbContext appcontext)
        {
            this._dbContext = context;
            this._appsettings = appsettings;
            this._context = appcontext;
        }

        public async Task<int> CreateAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Add(entity);
            await this._dbContext.SaveChangesAsync();
            var IdProperty = entity.GetType().GetProperty("Id").GetValue(entity, null);
            return (int)IdProperty;
        }

        public async Task<List<T>> SelectAll<T>() where T : class
        {
            return await this._context.Set<T>().ToListAsync();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Update(entity);
            _ = await this._dbContext.SaveChangesAsync();
        }

        public async Task<T> SelectById<T>(int Id) where T : class
        {
            return await this._dbContext.Set<T>().FindAsync(Id);
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            this._dbContext.Set<T>().Update(entity);
            _ = await this._dbContext.SaveChangesAsync();
        }
               
    }
}
