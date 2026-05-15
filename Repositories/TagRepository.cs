using EchoBlog.Data;
using Microsoft.EntityFrameworkCore;
using Models.Domain;

namespace EchoBlog.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _blogDbContext.Tags.AddAsync(tag);
            await _blogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery, string? sortBy, string? sortDirection, int pageSize, int pageNumber)
        {
            var query = _blogDbContext.Tags.AsQueryable();

            //filtering
            if(string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
            }

            //sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if(string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                if(string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }

            /// Pagination
            // Skip 0 Take 5 -> Page 1 of 5 results
            // Skip 5 Take next 5 -> Page 2 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if(existingTag != null)
            {
                _blogDbContext.Tags.Remove(existingTag);
                await _blogDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == tag.Id);
            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _blogDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<int> CountAsync()
        {
            return await _blogDbContext.Tags.CountAsync();
        }
    }
}