using BlazorDownloadFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;
namespace ServerAppSchule.Pages
{
    partial class FileExplorer
    {
        [Inject]
        NavigationManager _navManager { get; set; }
        [Inject]
        IFileService _fileService { get; set; }
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        [Inject] 
        IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
        List<FileSlim> _files = new List<FileSlim>();
        IList<IBrowserFile> _filesToUpload = new List<IBrowserFile>();
        bool _loading = false;
        string _usr = string.Empty;
        int maxAllowedFiles = 15;
        protected override async Task OnInitializedAsync()
        {

            var currentauth = await _authenticationState;
            if (!currentauth.User.Identity.IsAuthenticated)
            {               
               _navManager.NavigateTo("/login", true);
            }
            _usr = currentauth.User.Identity.Name;
            _loading = true;
            _files = await _fileService.GetdirsAndFiles(_usr);
            if(_files == null)
            {
               await _jsRuntime.InvokeVoidAsync("alert", "Keine Dateien vorhanden");
               _navManager.NavigateTo("/", true);
            }
            _loading = false;
        }
        /// <summary>
        /// Lädt eine Datei runter
        /// </summary>
        /// <param name="filename">Name der Datei die Heruntergeaden werden soll</param>
        /// <returns>Datei download</returns>
        async Task DownloadFile(string filename)
        {
            string filePath = _fileService.DownloadPath(_usr, filename);
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            await _blazorDownloadFileService.DownloadFile(filename, fileBytes.ToList(), CancellationToken.None, "application/octet-stream");
        }
        /// <summary>
        /// Lädt ein Verzeichnis runter
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        async Task DownloadDirectory(string dirName)
        {
            string dirPath = _fileService.DownloadZipPath(_usr, dirName);
            byte[] dirBytes = await File.ReadAllBytesAsync(dirPath);
            await _blazorDownloadFileService.DownloadFile(dirName + ".zip", dirBytes.ToList(), CancellationToken.None, "application/octet-stream");
            await _fileService.Delete(_usr, dirName + ".zip");

        }
        /// <summary>
        /// Lädt eine oder mehrere Dateien hoch
        /// </summary>
        /// <param name="files">Dateien die heruntergeladen werden sollen</param>
        /// <returns></returns>
        private async Task UploadFilesAsync(IReadOnlyList<IBrowserFile> files)
        {
            try
            {
                
                foreach (var file in files)
                {
                    await _fileService.Upload(_usr, file);
                }
            }
            catch (Exception ex)
            {
                await _jsRuntime.InvokeVoidAsync("alert", ex.Message);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Formartiert die Datei Größe
        /// </summary>
        /// <param name="size">Datei größe als Zahl</param>
        /// <returns>Datei größe als string</returns>
        string getFormatedFileSize(double? size)
        {
            if (size == null)
            {
                return "";
            }
            else
            {
                return _fileService.FileSizeFormater((long)size);
            }
        }
        
        /// <summary>
        /// Löscht eine Datei
        /// </summary>
        /// <param name="filename">Dateiname</param>
        /// <returns></returns>
        async Task DeleteFile(string filename)
        {
            await _fileService.Delete(_usr, filename);
            _files = await _fileService.GetdirsAndFiles(_usr);
            StateHasChanged();
        }

        async Task GoToSubDirectory(string name)
        { 
            string extraDir = _usr + "/" + name;
            _files = await _fileService.GetdirsAndFiles(extraDir);
        }

    }
}
