using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using ServerAppSchule.Interfaces;
using System.Security.Claims;
using ServerAppSchule.Models;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using ServerAppSchule.Components;

namespace ServerAppSchule.Pages
{
    partial class Index
    {
        [Inject]
        NavigationManager _navManager { get; set; }
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        [Inject]
        IUserService _userService { get; set; }
        [Inject]
        IPostService _postService { get; set; }
        [Inject]
        IDialogService _dialogService { get; set; }
        string _uid { get; set; }
        string _usrname { get; set; }
        List<Post> _posts = new List<Post>();
        HubConnection _hubConnection;

        bool _firstSigInDone = false;
        protected override async Task OnInitializedAsync()
        {
            var currentauth = await _authenticationState;
            if (!currentauth.User.Identity.IsAuthenticated)
            {
                _navManager.NavigateTo("/login", true);
            }
            _uid = currentauth.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            _usrname = _userService.GetUsernameById(_uid);
            if (_hubConnection == null)
            {
                _posts = await _postService.GetAllPosts();
            }
            await _userService.UpdateLastLoginRefresh(currentauth.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _hubConnection = new HubConnectionBuilder()
             .WithUrl(_navManager.ToAbsoluteUri("/serverappschulehub"))
             .WithAutomaticReconnect()
             .Build();
            await _hubConnection.StartAsync();
            _firstSigInDone = await _userService.IsFirstSignInDone(_uid);
            if (!_firstSigInDone)
            {   
                await _hubConnection.InvokeAsync("ThemeChangeAfterFirstSignIn", !_firstSigInDone, _uid);
            }
            _hubConnection.On("PostCreated", async (int postId) =>
            {
                _posts.Add(await _postService.GetPostById(postId));
                _posts = _posts.OrderByDescending(p => p.CreatedAt).ToList();
                StateHasChanged();
            });
            _hubConnection.On("PostLiked", async (int postId) =>
            {
                Post post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    Post getPost = await _postService.GetPostById(postId);
                    _posts.Where(p => p.Id == postId).FirstOrDefault().Likes = getPost.Likes;
                    StateHasChanged();
                }
            });
            _hubConnection.On("PostDeleted", async (int postId) =>
            {
                Post post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    _posts.Remove(post);
                    StateHasChanged();
                }
            }); 
            _hubConnection.On("CommentAdded", async (int postId) =>
            {
                Post post = _posts.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    Post getPost = await _postService.GetPostById(postId);
                    _posts.Where(p => p.Id == postId).FirstOrDefault().Comments = getPost.Comments;
                    StateHasChanged();
                }
            });
        }

        void OpenDialog()
        {
            DialogParameters<CreatePostDialog> parameters = new DialogParameters<CreatePostDialog>();
            parameters.Add(x => x.Uid, _uid);
            _dialogService.Show<CreatePostDialog>("Beitrag Erstellen", parameters);

        }
    }
}
