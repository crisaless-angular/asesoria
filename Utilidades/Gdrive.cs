using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.Business;
using Web.Business.Interfaces;
using Web.Data;

namespace Web.Utilidades
{
    public class Gdrive
    {
        private const string PathToCredentials = @"credentials.json";

        private async static Task<DriveService> Coonnect()
        {

            UserCredential credential;

            await using (var stream = new FileStream(PathToCredentials, FileMode.Open, FileAccess.Read))
            {
                // Requesting Authentication or loading previously stored authentication for userName
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { DriveService.ScopeConstants.DriveFile, DriveService.ScopeConstants.Drive },
                        "userName",
                        CancellationToken.None)
                    .Result;
            }

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

        }

        public async static void ListararchivosGdrive()
        {
            var request = Coonnect().Result.Files.List();
            var results = await request.ExecuteAsync();

            foreach (var driveFile in results.Files)
            {
                Debug.Print($"{driveFile.Name}  {driveFile.MimeType}  {driveFile.Id}");
            }
        }
        
        public static string CrearCarpeta(string NombreCarpeta)
        {
            string CarpetaPrincipal = variables.CarpetaPrincipalGdrive;
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = NombreCarpeta,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string>() { CarpetaPrincipal }
            };

            // Create a new folder on drive.
            var request = Coonnect().Result.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();

            return file.Id;
        }


        public static string UploadFile(Stream file, string fileName, string fileMime, string fileDescription)
        {
            Task<DriveService> service = Coonnect();
            
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = fileName;
            driveFile.Description = fileDescription;
            driveFile.MimeType = fileMime;
            driveFile.Parents = new string[] { variables.CarpetaPrincipalGdrive };
            
            var request = service.Result.Files.Create(driveFile, file, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                throw response.Exception;

            return request.ResponseBody.Id;
        }



    }
}
