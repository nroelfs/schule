using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Components
{

    partial class SinglePost
    {
        [Parameter]
        public Post Post { get; set; }
        [Parameter]
        public string LoggedInUID { get; set; }
        [Parameter]
        public string Class { get; set; } = string.Empty;
        string _profilePicture = string.Empty;
        string _username = string.Empty;
        string _shortusername = string.Empty;
        readonly string _heartFilledIcon = "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24\" viewBox=\"0 -960 960 960\" width=\"24\"><path d=\"m480-120-58-52q-101-91-167-157T150-447.5Q111-500 95.5-544T80-634q0-94 63-157t157-63q52 0 99 22t81 62q34-40 81-62t99-22q94 0 157 63t63 157q0 46-15.5 90T810-447.5Q771-395 705-329T538-172l-58 52Z\"/></svg>";
        readonly string _heartShapeIcon = "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24\" viewBox=\"0 -960 960 960\" width=\"24\"><path d=\"m480-120-58-52q-101-91-167-157T150-447.5Q111-500 95.5-544T80-634q0-94 63-157t157-63q52 0 99 22t81 62q34-40 81-62t99-22q94 0 157 63t63 157q0 46-15.5 90T810-447.5Q771-395 705-329T538-172l-58 52Zm0-108q96-86 158-147.5t98-107q36-45.5 50-81t14-70.5q0-60-40-100t-100-40q-47 0-87 26.5T518-680h-76q-15-41-55-67.5T300-774q-60 0-100 40t-40 100q0 35 14 70.5t50 81q36 45.5 98 107T480-228Zm0-273Z\"/></svg>";
        List<string>Pictures { get; set; } = new List<string>();
        [Inject] 
        ISettingsService _settingsService { get; set; }
        [Inject]
        IUserService _userService { get; set; }
        [Inject]
        IPostService _postService { get; set; }
        [Inject]
        NavigationManager _navigationManager { get; set; }
        HubConnection _hubConnection;
        string _comment = string.Empty;
        string _presetClass = "Card";
        private string _classes
        {
            get
            {
                return $"{_presetClass} {Class}";
            }
        }
        bool _expanded = false;

        protected override Task OnInitializedAsync()
        {
            _profilePicture = _settingsService.GetPicture(Post.CreatedBy);
            _username = _userService.GetUsernameById(Post.CreatedBy);
            
            if(string.IsNullOrEmpty(_profilePicture) || _profilePicture == "data:image/png;base64,")
            {
                _shortusername = _username.Substring(0, 1);
            }
            if (Post.Pictures == null)
            {
                Post.Pictures = new List<PostedPicture>();
            }
            if(Post.Likes == null)
            {
                Post.Likes = new List<User>();
            }
            if(Post.Comments == null)
            {
                Post.Comments = new List<Comment>();
            }
            if(Post.Pictures != null){
                foreach (PostedPicture pic in Post.Pictures)
                {
                    if (!String.IsNullOrEmpty(pic.PictureAsBase64))
                    {
                        Pictures.Add(String.Concat("data:image/" + pic.Type + ";base64,", pic.PictureAsBase64));
                    }
                }
            }
            _expanded = Post.Comments.Count <= 2;
            _hubConnection = new HubConnectionBuilder()
             .WithUrl(_navigationManager.ToAbsoluteUri("/serverappschulehub"))
             .WithAutomaticReconnect()
             .Build();
            _hubConnection.StartAsync();
            return base.OnInitializedAsync();
        }

        private async Task Like()
        {
            await _postService.LikePost(Post.Id, LoggedInUID);
            await _hubConnection.InvokeAsync("LikePost", Post.Id);
        }

        private async Task Delete()
        {
            await _postService.DeletePost(Post.Id);
            await _hubConnection.InvokeAsync("DeletePost", Post.Id);
        }
        private async Task AddComment()
        {
            Comment comment = new Comment();
            comment.Content = _comment;
            comment.CreatedAt = DateTime.Now;
            comment.CreatedBy = LoggedInUID;
            Post.Comments.Add(comment);
            await _postService.AddComment(Post);
            _comment = string.Empty;
            await _hubConnection.InvokeAsync("AddComment", Post.Id);
           
        }
        private void OnExpandCollapseClick()
        {
            _expanded = !_expanded;
        }
    }
}
