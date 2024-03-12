using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using System.Security.AccessControl;
using System.Text;

namespace Presentation.Repositories
{
    public class BucketRepository
    {

        string _projectId;
        string _bucketId;
        public BucketRepository(string projectId, string bucketId) { 
        _projectId= projectId;
        _bucketId= bucketId;
        }

        public async Task<Google.Apis.Storage.v1.Data.Object> UploadFile(MemoryStream ms, string newFilename)
        {
            var storage = StorageClient.Create();
            //   byte[] byteArray = Encoding.UTF8.GetBytes(contents);
            //  MemoryStream stream = new MemoryStream(byteArray);

           return await storage.UploadObjectAsync(_bucketId,
                newFilename, "application/octet-stream", ms);
        }

        public void GrantPermissionToFile(string filename, string recipient)
        {
            var storage = StorageClient.Create();
            var storageObject = storage.GetObject(_bucketId, filename, new GetObjectOptions
            {
                Projection = Projection.Full
            });

            storageObject.Acl.Add(new ObjectAccessControl
            {
                Bucket = _bucketId,
                Entity = $"user-{recipient}",
                Role = "READER",
            });
            var updatedObject = storage.UpdateObject(storageObject);
        }


    }
}
