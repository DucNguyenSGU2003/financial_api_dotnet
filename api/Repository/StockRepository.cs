using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    
    {
        private readonly ApplicationDBContext _context;
        public StockRepository (ApplicationDBContext Context)
        {
            this._context = Context;
        }
        public  Task<List<Stock>> GetAllAsync()
        {
            return this._context.Stocks.ToListAsync();
        }
    }
}
