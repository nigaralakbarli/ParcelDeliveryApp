﻿using Microsoft.EntityFrameworkCore;
using OrderMicroservice.DbContext;
using OrderMicroservice.Repositories.Abstraction;
using Shared.Models;

namespace OrderMicroservice.Repositories.Concrete;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
}

