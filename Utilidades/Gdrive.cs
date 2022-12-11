﻿using Google.Apis.Auth.OAuth2;
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

        public async static Task<string> GuardarArchivo(string fileName, string contentType)
        {
            string uploadString = fileName;
            string CarpetaPrincipal = variables.CarpetaPrincipalGdrive;

            // Upload file Metadata
            Google.Apis.Drive.v3.Data.File fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                Parents = new List<string>() { CarpetaPrincipal }  // folder to upload the file to
            };

            MemoryStream fsSource = new MemoryStream(Encoding.UTF8.GetBytes(uploadString ?? ""));

            string uploadedFileId;

            // Create a new file, with metadata and stream.
            var request = Coonnect().Result.Files.Create(fileMetadata, fsSource, contentType);
            request.Fields = "*";
            var results = await request.UploadAsync(CancellationToken.None);

            if (results.Status == UploadStatus.Failed)
            {
                Debug.Print($"Error uploading file: {results.Exception.Message}");
                return "";
            }
            
            uploadedFileId = request.ResponseBody?.Id;

            return uploadedFileId;
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

    }
}
