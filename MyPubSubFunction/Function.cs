using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using System;
using System.Threading;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Components.Forms;
using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;

namespace MyPubSubFunction
{
    public class Function : ICloudEventFunction<MessagePublishedData>
    {
        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {
            var blogId = data.Message?.TextData;
          if(string.IsNullOrEmpty(blogId))
          {
                Console.WriteLine("blogid is empty");
          }
          else 
          {
                Console.WriteLine($"Blogid {blogId} received");
                  //we have blogid with data

                //putting everything inside a pdf.
                PdfDocument document = new PdfDocument();
                Console.WriteLine($"Pdfdocument created");
                // Add a new page to the document
                PdfPage page = document.AddPage();
                Console.WriteLine($"PdfPage created");
                var myPosts = await GetPosts(blogId); //will get a list of posts pertaining to a blog
                Console.WriteLine($"posts retrieved count: {myPosts.Count}");
                int yPosition = 10;

                GlobalFontSettings.FontResolver = new FileFontResolver();
                Console.WriteLine($"FontResolver created");
                XFont font = new XFont("Verdana", 12, XFontStyleEx.Regular);
                // Get an XGraphics object for drawing
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    foreach (var post in myPosts)
                    {
                        Console.WriteLine($"Creating post...{post.Title}");
                        // Draw the text on the page
                        gfx.DrawString(post.Title, font, XBrushes.Black, new XRect(10, yPosition, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        // Move to the next line (increase Y-coordinate position)
                        yPosition += font.Height;
                        gfx.DrawString(post.Content, font, XBrushes.Black, new XRect(10, yPosition, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        // Move to the next line (increase Y-coordinate position)
                        yPosition += font.Height;
                        gfx.DrawString("-----------------------------------------------", font, XBrushes.Black, new XRect(10, yPosition, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                        // Move to the next line (increase Y-coordinate position)
                        yPosition += (font.Height * 3);

                    }
                }

                Console.WriteLine($"Saving the file locally in the function...");
                string filenamePDF = blogId + ".pdf";
                document.Save(filenamePDF);
                Console.WriteLine($"File {filenamePDF} Saved...");
                //code to open back the file and upload it.

                MemoryStream msIn = new MemoryStream(System.IO.File.ReadAllBytes(filenamePDF));
                msIn.Position = 0;
                Console.WriteLine($"Read the file again to be uploaded...");
                var t = await UploadFile(msIn, filenamePDF);
                Console.WriteLine($"Pdf {filenamePDF} uploaded...");
                System.IO.File.Delete(filenamePDF);
                Console.WriteLine($"Pdf {filenamePDF} deleted from the function...");
               //no return type
          }
 
        }

         public async Task<List<Post>> GetPosts(string blogId)
        {
           
            var db = FirestoreDb.Create("swd63apfc2024");

            Query allPostsQuery = db.Collection($"blogs/{blogId}/Posts");
            QuerySnapshot allPostsQuerySnapshot = await allPostsQuery.GetSnapshotAsync();

            List<Post> posts = new List<Post>();

            foreach (DocumentSnapshot documentSnapshot in allPostsQuerySnapshot.Documents)
            {
                Post b = documentSnapshot.ConvertTo<Post>();
                b.Id = documentSnapshot.Id;
                b.BlogId = blogId;
                posts.Add(b);
            }
            return posts;
        }

       public async Task<Google.Apis.Storage.v1.Data.Object> UploadFile(MemoryStream ms, string newFilename)
        {
            var storage = StorageClient.Create();
            //   byte[] byteArray = Encoding.UTF8.GetBytes(contents);
            //  MemoryStream stream = new MemoryStream(byteArray);

           return await storage.UploadObjectAsync("swd63apfc2024ra_fg",
                newFilename, "application/octet-stream", ms);
        }

    }

   


}