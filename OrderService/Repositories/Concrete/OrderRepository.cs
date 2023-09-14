using Microsoft.EntityFrameworkCore;
using OrderMicroservice.DbContext;
using OrderMicroservice.Models;
using OrderMicroservice.Repositories.Abstraction;
using System;
using System.Linq.Expressions;

namespace OrderMicroservice.Repositories.Concrete;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{

}

