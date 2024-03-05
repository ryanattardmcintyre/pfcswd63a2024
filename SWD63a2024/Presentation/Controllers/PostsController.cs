using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Repositories;

namespace Presentation.Controllers
{
    public class PostsController : Controller
    {

        private PostsRepository postsRepository;
        public PostsController(PostsRepository _postsRepository) { 
        postsRepository= _postsRepository;  
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(string blogId) { 
            ViewBag.BlogId = blogId;
            return View(); 
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Post post) {

            //security tip: check that the blogId received belongs to the logged in user

            post.DateCreated = Timestamp.FromDateTime(DateTime.UtcNow);
            post.DateUpdated = Timestamp.FromDateTime(DateTime.UtcNow);

            postsRepository.Add(post);

            ViewBag.BlogId = post.BlogId;

            return View(post); 
        
        }

        public async Task<IActionResult> Index(string id)
        {
            //id = blogId
            ViewBag.BlogId = id;
            var listOfPosts = await postsRepository.GetPosts(id);
            return View(listOfPosts);
        }

        public async Task<IActionResult> Read(string blogId, string postId)
        {
            var myPost = await postsRepository.GetPostForBlog(blogId, postId);
            return View(myPost);
        }

    }
}
