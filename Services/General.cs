using System;
using BackendCine.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCine.Services;

public class General
{
    private readonly CineDbContext _context;
    public General(CineDbContext context)
    {
        _context = context;
    }
    

  
}
