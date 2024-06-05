using Microsoft.AspNetCore.Components.Forms;
using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Ermittelt alle Datein und Ordner in einem Verzeichnis
        /// </summary>
        /// <param name="path">Username</param>
        /// <returns>Liste aller Ordner und Dateien</returns>
        Task<List<FileSlim>> GetdirsAndFiles(string path);
        /// <summary>
        /// Formatiert die Datei ins Passende Datei format
        /// </summary>
        /// <param name="fileSizeInBytes">Dateigröße in Bytes</param>
        /// <returns>formatierte Dateigröße als string</returns>
        string FileSizeFormater(double fileSizeInBytes);
        /// <summary>
        /// gibt den Dateipfad im string format zurück
        /// </summary>
        /// <param name="usrname">benutzername</param>
        /// <param name="fileName">dateiname</param>
        /// <returns>Dateipfad als string</returns>
        string DownloadPath(string usrname, string fileName);
        /// <summary>
        /// Lädt eine Datei hoch
        /// </summary>
        /// <param name="usrName">Benutzername</param>
        /// <param name="file">Datei die Hochgeladen werden soll</param>
        /// <returns></returns>
        Task Upload(string usrName, IBrowserFile file);
        /// <summary>
        /// Wandelt ein Bild in ein Base64 String um
        /// </summary>
        /// <param name="input">Hochgeladenes Profilbild</param>
        /// <returns>Bild als Base64 string</returns>
        Task<string> PicToBase64Async(IBrowserFile input);
        /// <summary>
        /// Löscht eine Datei
        /// </summary>
        /// <param name="usrName">Benutzername</param>
        /// <param name="fileName">Dateiname</param>
        /// <returns></returns>
        Task Delete(string usrName, string fileName);
        /// <summary>
        /// Erstellt ein ZipPfad zum Download
        /// </summary>
        /// <param name="usrname">Benutzername</param>
        /// <param name="dirName">Ordnername</param>
        /// <returns>Pfad zum Ordner</returns>
        string DownloadZipPath(string usrname, string dirName);

    }
}
