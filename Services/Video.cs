using Microsoft.EntityFrameworkCore;

namespace BackendCine.Entities.Models
{
    public class VideoService
    {
        private readonly CineDbContext _context;

        public VideoService(CineDbContext context)
        {
            _context = context;
        }

        public async Task<List<Video>> GetVideosAsync()
        {
            return await _context.Videos.Where(p => p.swt).ToListAsync();
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            return await _context.Videos.FindAsync(id);
        }

        public async Task AddVideoAsync(Video video)
        {
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();
        }

        public async Task<Video> UpdateVideoAsync(Video video)
        {
            var existingVideo = await GetVideoByIdAsync(video.Id);
            if (existingVideo == null)
            {
                throw new KeyNotFoundException($"Video with ID {video.Id} not found.");
            }
            // Update properties as needed
            existingVideo.Titulo = video.Titulo;
            existingVideo.Sinopsis = video.Sinopsis;
            existingVideo.Duracion = video.Duracion;
            existingVideo.Img = video.Img;
            existingVideo.swt = video.swt;
            existingVideo.usuario_modificacion = video.usuario_modificacion;
            existingVideo.fecha_modificacion = DateTime.Now;
            _context.Videos.Update(existingVideo);
            await _context.SaveChangesAsync();
            return existingVideo;
        }

        public async Task<Video> DeleteVideoAsync(int id)
        {
            var video = await GetVideoByIdAsync(id);
            if (video != null)
            {
                video.swt = false;
                _context.Videos.Update(video);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Video with ID {id} not found.");
            }
            return video;
        }
    }
}