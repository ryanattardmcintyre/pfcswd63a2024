using Google.Cloud.Firestore;

namespace Presentation.Models
{

    [FirestoreData]
    public class Blog
    {
        
        public string Id { get; set; }
        
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Author { get; set; }
        [FirestoreProperty]
        public Google.Cloud.Firestore.Timestamp DateCreated { get; set; }
        [FirestoreProperty]
        public Google.Cloud.Firestore.Timestamp DateUpdated { get; set; }
    
        
    }
}
