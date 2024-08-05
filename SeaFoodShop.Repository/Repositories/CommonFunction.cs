using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Common
{
    public class CommonFunction
    {
        public async Task<UploadImageDriveModel?> UploadImg(IFormFile file, string idFolder)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string credentialsPath = Path.Combine(currentDirectory, "credentials.json");
                string folderId = idFolder;
                var FileId = await UploadFileToGoogledrive(credentialsPath, folderId, file);
                UploadImageDriveModel uploadImageDriveModel = new UploadImageDriveModel();
                uploadImageDriveModel.Id = FileId;
                return uploadImageDriveModel;
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is FileNotFoundException)
                {
                    Console.WriteLine("File not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        public async Task<string?> DeleteImg(string idImg)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string credentialsPath = Path.Combine(currentDirectory, "credentials.json");
                string folderId = idImg;
                return await DeleteFileFromGoogleDrive(credentialsPath, idImg);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string?> DeleteFileFromGoogleDrive(string credentialsPath, string idImg)
        {
            GoogleCredential googleCredential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(stream)
                .CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = googleCredential,
                    ApplicationName = "SeaFood"
                });

                FilesResource.DeleteRequest request = service.Files.Delete(idImg);
                await request.ExecuteAsync();
                return "Delete successfully !";
            }
        }
        public async Task<string> UploadFileToGoogledrive(string credentialsPath, string folderId, IFormFile file)
        {
            var FileId = "";

            GoogleCredential googleCredential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                googleCredential = GoogleCredential.FromStream(stream)
                .CreateScoped(new[] { DriveService.ScopeConstants.DriveFile });
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = googleCredential,
                    ApplicationName = "SeaFood"
                });

                var fileMetaData = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = file.FileName,
                    Parents = new List<string> { folderId }
                };

                FilesResource.CreateMediaUpload request;
                using (var fileStream = file.OpenReadStream())
                {
                    request = service.Files.Create(fileMetaData, fileStream, file.ContentType);
                    request.Fields = "id";
                    await request.UploadAsync();
                }

                var fileRespon = request.ResponseBody;
                FileId = fileRespon.Id;
            }
            return FileId;
        }

        public async Task<UploadImageDriveModel?> UpdateImg(string idImg, IFormFile newFile, string idFolder)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string credentialsPath = Path.Combine(currentDirectory, "credentials.json");

                // Xóa ảnh cũ
                await DeleteFileFromGoogleDrive(credentialsPath, idImg);

                // Tải ảnh mới lên
                var newFileId = await UploadFileToGoogledrive(credentialsPath, idFolder, newFile);

                UploadImageDriveModel uploadImageDriveModel = new UploadImageDriveModel();
                uploadImageDriveModel.Id = newFileId;
                return uploadImageDriveModel;
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is FileNotFoundException)
                {
                    Console.WriteLine("File not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }
    }
}
