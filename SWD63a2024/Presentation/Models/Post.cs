namespace Presentation.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public Google.Cloud.Firestore.Timestamp DateCreated { get; set; }
        public Google.Cloud.Firestore.Timestamp DateUpdated { get; set; }

        public string BlogId { get; set; }

        public string PictureUri { get; set; }

    }
}
