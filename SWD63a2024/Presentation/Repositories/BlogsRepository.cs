using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
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

        public async Task<List<Blog>> GetBlogs()
        {
            Query allBlogsQuery = db.Collection("blogs");
            QuerySnapshot allBlogsQuerySnapshot = await allBlogsQuery.GetSnapshotAsync();

            List<Blog> blogs = new List<Blog>();

            foreach (DocumentSnapshot documentSnapshot in allBlogsQuerySnapshot.Documents)
            {
                Blog b = documentSnapshot.ConvertTo<Blog>();
                b.Id = documentSnapshot.Id;

                blogs.Add(b);
            }

             

            return blogs;
        }

        public async Task<List<Blog>> GetBlogs(string keyword)
        {
            Query allBlogsQuery = db.Collection("blogs").WhereEqualTo("Name", keyword);
            QuerySnapshot allBlogsQuerySnapshot = await allBlogsQuery.GetSnapshotAsync();

            List<Blog> blogs = new List<Blog>();

            foreach (DocumentSnapshot documentSnapshot in allBlogsQuerySnapshot.Documents)
            {
                Blog b = documentSnapshot.ConvertTo<Blog>();
                b.Id = documentSnapshot.Id;

                blogs.Add(b);
            }
            return blogs;


        }
    }
}
