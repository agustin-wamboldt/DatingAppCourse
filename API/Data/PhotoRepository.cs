using API.Entities;
using API.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PhotoRepository(DataContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<Photo> GetPhotoById(int photoId)
        {
            return await _context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(p => p.Id == photoId);
        }

        public async Task<IEnumerable<PhotoForApprovalDTO>> GetUnapprovedPhotos() // TODO: Could benefit from pagination.
        {
            return await _context.Photos
                .IgnoreQueryFilters()
                .Where(p => p.IsApproved == false)
                .ProjectTo<PhotoForApprovalDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo); // Cascade delete will remove this from User's photos
        }
    }
}
