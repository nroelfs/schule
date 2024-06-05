using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Components
{
    partial class CreatePostDialog
    {
        [Parameter]
        public string Uid { get; set; }
        [CascadingParameter]
        MudDialogInstance _mudDialog { get; set; }
        [Inject]
        NavigationManager _navigationManager { get; set; }
        [Inject]
        IFileService _fileService { get; set; }
        [Inject]
        IPostService _postService { get; set; }
        [Inject]
        IJSRuntime _jsRuntime { get; set; }
        HubConnection? _hubConnection;

        Post _post = new Post();
        protected override void OnInitialized()
        {
            _post = new Post();
            _post.Pictures = new List<PostedPicture>();
            _post.Comments = new List<Comment>();
            _post.Likes = new List<User>();
            _post.CreatedBy = Uid;
            _hubConnection = new HubConnectionBuilder()
             .WithUrl(_navigationManager.ToAbsoluteUri("/serverappschulehub"))
             .WithAutomaticReconnect()
             .Build();
            _hubConnection.StartAsync();
            base.OnInitialized();
        }

        /// <summary>
        /// Bricht den Dialog ab
        /// </summary>
        private void Cancel() => _mudDialog.Cancel();

        /// <summary>
        /// erstellt einen neuen Post
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            _post = await _postService.CreatePost(_post);
            await _hubConnection.InvokeAsync("CreatePost", _post.Id);
            Cancel();
        }


        private async Task UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            foreach (IBrowserFile file in files)
            {
                if (file.Size > 999999)
                {
                    _jsRuntime.InvokeVoidAsync("alert", "Die Datei ${file.name} ist zu groß, Sie die Maximale größe darf  betragen");
                    return;
                }
                PostedPicture pic = new PostedPicture();
                pic.Type = file.Name.Split('.')[1];
                pic.Size = file.Size;
                pic.PictureAsBase64 = await _fileService.PicToBase64Async(file);
                if(_post.Pictures.Count < 4)
                {
                    _post.Pictures.Add(pic);
                }
                else
                {
                    _post.Pictures.RemoveAt(0);
                    _post.Pictures.Add(pic);
                }
            }
        }
    }
}
