using System;
using System.Security.Cryptography;
using BackendCine.Entities;
using BackendCine.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCine.Services;

public class CarteleraService
{
    private readonly CineDbContext _context;
    public CarteleraService(CineDbContext context)
    {
        _context = context;
    }
    public async Task<List<PeliculaXSala>> GetPeliculaXSala(int? idPelicula)
    {
        using (var cn = new CineDbContext())
        {
            var query = from p in cn.Videos
                        where p.swt && (p.Id == idPelicula || idPelicula == null || idPelicula == 0)
                        orderby p.Titulo
                        select new PeliculaXSala
                        {
                            Id = p.Id,
                            Titulo = p.Titulo,
                            Sinopsis = p.Sinopsis,
                            Duracion = p.Duracion,
                            Img = p.Img,
                            Generos = cn.GenerosXVideos
                                .Where(gp => gp.IdVideo == p.Id && gp.swt && (p.Id == idPelicula || idPelicula == null || idPelicula == 0))
                                .Select(gp => gp.Genero)
                                .ToList(),

                        };
            return await query.ToListAsync();
        }
    }

    public async Task<List<PeliculaXSala>> GetPeliculaByParam(ListarPeliculasParam? param)
    {
        using (var cn = new CineDbContext())
        {
            var query = from p in cn.Videos
                        where p.swt &&
                            (
                                // Mostrar todo si todos los parámetros son null
                                (param == null || 
                                (param.titulo == null && param.idGenero == null ))

                                // Filtros individuales si algún parámetro tiene valor
                                || (!string.IsNullOrEmpty(param.titulo) &&
                                    EF.Functions.Like(p.Titulo, $"%{param.titulo}%"))
                                || (param.idGenero != null &&
                                    cn.GenerosXVideos.Any(gp =>
                                        gp.IdVideo == p.Id &&
                                        gp.swt &&
                                        gp.IdGenero == param.idGenero))
                            )
                        orderby p.Titulo
                        select new PeliculaXSala
                        {
                            Id       = p.Id,
                            Titulo   = p.Titulo,
                            Sinopsis = p.Sinopsis,
                            Duracion = p.Duracion,
                            Img      = p.Img,

                            Generos = cn.GenerosXVideos
                                .Where(gp => gp.IdVideo == p.Id &&
                                            gp.swt &&
                                            (param.idGenero == null || gp.IdGenero == param.idGenero))
                                .Select(gp => gp.Genero)
                                .ToList(),

                        };

            return await query.ToListAsync();
        }
    }
}

