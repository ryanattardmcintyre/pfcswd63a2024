using Google.Cloud.Firestore;
using Presentation.Models;

namespace Presentation.Repositories
{
    public class PostsRepository
    {
        FirestoreDb db;
        public PostsRepository(string project)
        {
            db = FirestoreDb.Create(project);


        }
        public async void Add(Post post)
        {
            post.Id = Guid.NewGuid().ToString();

            DocumentReference docRef = db.Collection($"blogs/{post.BlogId}/Posts").Document(post.Id);

            await docRef.SetAsync(post);
        }
    }
}
