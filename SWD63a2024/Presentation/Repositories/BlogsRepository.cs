using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Presentation.Models;
using System.Configuration;

namespace Presentation.Repositories
{
    public class BlogsRepository
    {
        FirestoreDb db;
        PostsRepository _postsRepository;
        public BlogsRepository(string project, PostsRepository postRepo) {
            db = FirestoreDb.Create(project); 

            _postsRepository = postRepo;
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


        public async Task<bool> DeleteBlog(string blogId)
        {
            var listOfPosts = await _postsRepository.GetPosts(blogId);

            if (listOfPosts.Count == 0)
            { 
                 DocumentReference blogRef = db.Collection("blogs").Document(blogId);
                    await blogRef.DeleteAsync();
                return true;
            }
            else
            {
                //throw exception
            }
            return false;


        }

        public async Task<Google.Cloud.Firestore.WriteResult> UpdateBlog(Blog b)
        {
            DocumentReference blogRef = db.Collection("blogs").Document(b.Id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Name", b.Name },
                { "DateUpdated", Timestamp.FromDateTime(DateTime.UtcNow) }
            };
            return await blogRef.UpdateAsync(updates);
        }
    }
}
