using System.Diagnostics;
using BackendCine.Entities.Models;
using Company.TestProject1;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace BackendCine.Services;

public class RolService
{
    private readonly CineDbContext _context;
    public RolService(CineDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rol>> GetRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Rol> GetRolByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task<Rol> AddRolAsync(Rol rol)
    {
        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();
        return rol;
    }

    public async Task<Rol> UpdateRolAsync(int id, Rol rol)
    {
        var rolToUpdate = await _context.Roles.FindAsync(id);
        if (rolToUpdate == null)
        {
            return null;
        }
        rolToUpdate.NombreRol = rol.NombreRol;
        await _context.SaveChangesAsync();
        return rolToUpdate;
    }








    /*     public int numero(int numeroParam)
        {
            return numeroParam * 5;
        }

        public void numero2(int numeroParam)
        {
            Console.WriteLine(numeroParam * 5);
        }

        public void resultados()
        {
            int p = numero(6);
            numero2(p);
        } */

}
