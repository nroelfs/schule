using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface IPostService
    {
        /// <summary>
        /// Sucht Alle Posts eines Users
        /// </summary>
        /// <param name="uid">Id des Users</param>
        /// <returns>Eine Liste aller Posts die zu einem User gehören</returns>
        Task<List<Post>> GetUserPosts(string uid);
        /// <summary>
        /// Erstellt einen Post
        /// </summary>
        /// <param name="post">Daten des Post</param>
        /// <returns>Post der erstellt wurde</returns>
        Task<Post> CreatePost(Post post);
        /// <summary>
        /// Sucht alle Posts
        /// </summary>
        /// <returns>Liste von allen Posts</returns>
        Task<List<Post>> GetAllPosts();
        /// <summary>
        /// Sucht einen Post anhand der Id
        /// </summary>
        /// <param name="id">Id des Posts</param>
        /// <returns>gesuchter Post</returns>
        Task<Post> GetPostById(int id);
        /// <summary>
        /// Entfernt oder fügt einen Like zu einem Post hinzu
        /// </summary>
        /// <param name="postId">Id des Posts</param>
        /// <param name="userId">Id des Benutzers</param>
        /// <returns></returns>
        Task LikePost(int postId, string userId);
        /// <summary>
        /// Markiert einen Post als gelöscht
        /// </summary>
        /// <param name="postId">Id des Posts</param>
        /// <returns></returns>
        Task DeletePost(int postId);
        /// <summary>
        /// Fügt einen Kommentar zu einem Post hinzu
        /// </summary>
        /// <param name="post">Post mit Kommentar</param>
        /// <returns></returns>
        Task AddComment(Post post);
    }
}
