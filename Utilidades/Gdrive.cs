using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Web.Utilidades
{
    public class Gdrive
    {
        private const string PathToCredentials = @"C:\credentials.json";

        public async static void Coonnect()
        {

            try
            {
                UserCredential credential;
                await using (var stream = new FileStream(PathToCredentials, FileMode.Open, FileAccess.Read))
                {
                    // Requesting Authentication or loading previously stored authentication for userName
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.Load(stream).Secrets,
                            new[] { DriveService.ScopeConstants.DriveReadonly },
                            "userName",
                            CancellationToken.None)
                        .Result;
                }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
                
                var request = service.Files.List();
                var results = await request.ExecuteAsync();

                foreach (var driveFile in results.Files)
                {
                    Debug.Print($"{driveFile.Name}  {driveFile.MimeType}  {driveFile.Id}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
        
    }
}
