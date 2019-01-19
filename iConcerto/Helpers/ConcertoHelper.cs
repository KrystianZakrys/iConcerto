using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace iConcerto.Helpers
{
    public static class ConcertoHelper
    {
        /// <summary>
        /// Uploads file to Azure Blob Storage
        /// </summary>
        /// <returns>File url</returns>
        public static String UploadFileToAzureStorage(HttpPostedFileBase fileStream)
        {
            try
            {
                string storageConnection = System.Configuration.ConfigurationManager.ConnectionStrings["AzureStorage"].ToString();
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("tar");
                if (cloudBlobContainer.CreateIfNotExists())
                {
                    cloudBlobContainer.SetPermissions(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileStream.FileName);
                cloudBlockBlob.UploadFromStream(fileStream.InputStream);
      
                return cloudBlockBlob.Uri.AbsoluteUri;
            }
            catch(Exception e)
            {
                return e.Message;
            }
         
        }
    }
}