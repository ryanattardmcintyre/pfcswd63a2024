using Google.Cloud.Firestore;
using Presentation.Models;
using System.Configuration;

namespace Presentation.Repositories
{
    public class BlogsRepository
    {
        FirestoreDb db;
        public BlogsRepository(string project) {
            db = FirestoreDb.Create(project); 
        }
        public async void Add(Blog blog)
        {
            blog.Id = Guid.NewGuid().ToString();

            DocumentReference docRef = db.Collection("blogs").Document(blog.Id);
           
            await docRef.SetAsync(blog);
        }
    }
}
