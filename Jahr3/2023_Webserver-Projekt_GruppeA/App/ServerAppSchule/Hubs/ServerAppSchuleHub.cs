using Microsoft.AspNetCore.SignalR;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Hubs
{
    public class ServerAppSchuleHub : Hub
    {

        public ServerAppSchuleHub()
        {

        }
        public async Task CreatePost(int postId)
        {
            await Clients.All.SendAsync("PostCreated", postId);
        }
        public async Task LikePost(int postId)
        {
            await Clients.All.SendAsync("PostLiked", postId);
        }
        public async Task DeletePost(int postId)
        {
            await Clients.All.SendAsync("PostDeleted", postId);
        }
        public async Task AddComment(int postId)
        {
            await Clients.All.SendAsync("CommentAdded", postId);
        }
        public async Task ThemeChangeAfterFirstSignIn(bool change, string uid)
        {
            await Clients.All.SendAsync("ThemeHasToChange", change, uid );
        }
        public async Task ThemeChanged(bool change, string uid)
        {
            await Clients.All.SendAsync("ThemeChanged", change, uid);
        }


    }
}
