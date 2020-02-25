﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;

namespace WebBanSach_2_0.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {

    }
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(WebBanSach_2_0DbContext dbContext) : base(dbContext)
        {
        }
    }
}