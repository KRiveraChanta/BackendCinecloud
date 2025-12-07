using BackendCine.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendCine.Services
{
    public class GeneroXVideoService
    {
        private readonly CineDbContext _context;

        public GeneroXVideoService(CineDbContext context)
        {
            _context = context;
        }

        public async Task<List<GeneroXVideo>> GetGenerosXVideosAsync()
        {
            return await _context.GenerosXVideos.Where(c => c.swt).ToListAsync();
        }

        public async Task<GeneroXVideo> GetGeneroXVideoByIdAsync(int id)
        {
            return await _context.GenerosXVideos.FindAsync(id);
        }

        public async Task AddGeneroXVideoAsync(GeneroXVideo generoXVideo)
        {
            _context.GenerosXVideos.Add(generoXVideo);
            await _context.SaveChangesAsync();
        }

        public async Task<GeneroXVideo> UpdateGeneroXVideoAsync(GeneroXVideo generoXVideo)
        {
            var existingGeneroXVideo = await GetGeneroXVideoByIdAsync(generoXVideo.Id);
            if (existingGeneroXVideo == null)
            {
                throw new KeyNotFoundException($"GeneroXVideo with ID {generoXVideo.Id} not found.");
            }
            existingGeneroXVideo.IdVideo = generoXVideo.IdVideo;
            existingGeneroXVideo.IdGenero = generoXVideo.IdGenero;
            existingGeneroXVideo.swt = generoXVideo.swt;
            existingGeneroXVideo.usuario_modificacion = generoXVideo.usuario_modificacion;
            existingGeneroXVideo.fecha_modificacion = DateTime.Now;
            _context.GenerosXVideos.Update(existingGeneroXVideo);
            await _context.SaveChangesAsync();
            return existingGeneroXVideo;
        }

        public async Task<GeneroXVideo> DeleteGeneroXVideoAsync(int id)
        {
            var generoXVideo = await GetGeneroXVideoByIdAsync(id);
            if (generoXVideo != null)
            {
                generoXVideo.swt = false;
                _context.GenerosXVideos.Update(generoXVideo);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"GeneroXVideo with ID {id} not found.");
            }
            return generoXVideo;
        }
    }
}