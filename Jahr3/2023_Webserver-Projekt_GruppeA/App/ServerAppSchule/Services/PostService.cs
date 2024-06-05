using Microsoft.AspNetCore.Identity;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using ServerAppSchule.Data;
using ServerAppSchule.Factories;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Services
{
    public class PostService: IPostService
    {
        private readonly ApplicationDbContextFactory  _contextFactory;
        public PostService(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Post> CreatePost(Post post)
        {
            using var context = _contextFactory.CreateDbContext();
            post.CreatedAt = DateTime.Now;
            //List<PostedPicture> pictureList = post.Pictures.ToList();
            //post.Pictures = new();
            context.Posts.Add(post);
            await context.SaveChangesAsync();
            //foreach(var picture in pictureList)
            //{
            //    picture.PostId = post.Id;
            //    context.PostedPictures.Add(picture);
            //    await context.SaveChangesAsync();
            //}
            
            //post.Pictures = pictureList;
            return post;

            
        }

        public async Task<List<Post>> GetUserPosts(string uid)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                    .Include(p => p.Comments)
                    .Include(p => p.Pictures)
                    .Include(p => p.Likes)
                    .Where(p => p.CreatedBy == uid && !p.IsDeleted)
                    .OrderByDescending(p => p.CreatedAt)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync();
        }   
        public async Task<List<Post>> GetAllPosts()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                    .Include(p => p.Comments)
                    .Include(p => p.Pictures)
                    .Include(p => p.Likes)
                    .Where(p => !p.IsDeleted)
                    .OrderByDescending(p => p.CreatedAt)
                    .AsSplitQuery()
                    .AsNoTracking()
                    .ToListAsync();
        }
        public async Task<Post> GetPostById(int id)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            return await  context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Pictures)
                .Include(p => p.Likes)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted) ?? new();

        }
        public async Task LikePost(int postId, string userId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            Post? post = await context.Posts
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                return;
            }
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return;
            }
            if (post.Likes.Contains(user))
            {
                post.Likes.Remove(user);
            }
            else
            {
                post.Likes.Add(user);
            }
            await context.SaveChangesAsync();
        } 
        public async Task DeletePost(int postId)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            Post? post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postId && !p.IsDeleted);
            if (post == null)
            {
                return;
            }
            post.IsDeleted = true;
            await context.SaveChangesAsync();
        }
        public async Task AddComment(Post post)
        {
            using ApplicationDbContext context = _contextFactory.CreateDbContext();
            Post? dbPost = await context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == post.Id && !p.IsDeleted);
            if (dbPost == null)
            {
                return;
            }
            dbPost.Comments.Add(post.Comments.Last());
            await context.SaveChangesAsync();
        }

    }
}
