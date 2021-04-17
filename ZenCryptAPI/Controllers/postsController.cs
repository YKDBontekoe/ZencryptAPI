using AutoMapper;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Exceptions;
using Domain.Services.Forum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.SQL.Forums;
using Domain.Frames.Endpoint;
using ZenCryptAPI.Models.Data.Post;


namespace ZenCryptAPI.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        //------------------------------ POST --------------------------------
        // GET: api/<PostsController>
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Get posts
                var foundPosts = await _postService.GetPosts();

                // Map posts
                var postModel = _mapper.Map<IEnumerable<MultiPostModel>>(foundPosts);

                // Wrap the userModel object to an api frame
                var returnable = new MultiItemFrame<MultiPostModel>()
                { Message = $"Found posts", TotalResults = postModel.Count(), Results = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                // Get post
                var foundPost = await _postService.GetPost(id);

                // Map post
                var postModel = _mapper.Map<SinglePostModel>(foundPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                { Message = $"Found post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // POST api/<PostsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePostDTO createPost)
        {
            try
            {
                // Map post
                var post = _mapper.Map<Post>(createPost);

                // Create post
                var createdPost = await _postService.CreatePost(post, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(createdPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                { Message = $"Found post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // POST api/<PostsController>/like 
        [HttpPost("{id}/like")]
        public async Task<IActionResult> PostLike(Guid id) 
        {
            try
            {
                // Create post
                var likedPost = await _postService.UserLikePost(id, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(likedPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                    { Message = $"Liked post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // POST api/<PostsController>/{id}/dislike 
        [HttpPost("{id}/dislike")]
        public async Task<IActionResult> PostDislike(Guid id) 
        {
            try
            {
                // Create post
                var likedPost = await _postService.UserDislikePost(id, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(likedPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                    { Message = $"Disliked post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // POST api/<PostsController>/{id}/dislike 
        [HttpPost("{id}/view")]
        public async Task<IActionResult> PostView(Guid id) 
        {
            try
            {
                // Create post
                var likedPost = await _postService.UserDislikePost(id, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(likedPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                    { Message = $"Disliked post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePostDTO updatePost)
        {
            try
            {
                // Map post
                var post = _mapper.Map<Post>(updatePost);

                // Create post
                var createdPost = await _postService.UpdatePost(id, post, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(createdPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                { Message = $"Updated post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object>
                {
                    Message = e.Message
                });
            }
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid postId) 
        {
            try
            {
                // Create post 
                var deletedPost = await _postService.DeletePost(postId, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(deletedPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                    { Message = $"Deleted post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object>
                {
                    Message = e.Message
                });
            }
        }

        // DELETE api/<PostsController>/like 
        [HttpDelete("{id}/like")]
        public async Task<IActionResult> DeleteLike(Guid id)  
        {
            try
            {
                // Create post
                var likedPost = await _postService.UndoUserLikePost(id, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(likedPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                    { Message = $"un- Liked post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // DELETE api/<PostsController>/{id}/dislike 
        [HttpDelete("{id}/dislike")]
        public async Task<IActionResult> DeleteDislike(Guid id) 
        {
            try
            {
                // Create post
                var likedPost = await _postService.UndoUserDislikePost(id, GetBearerToken());

                // Map to SinglePostModel
                var postModel = _mapper.Map<SinglePostModel>(likedPost);

                // Wrap the userModel object to an api frame
                var returnable = new SingleItemFrame<SinglePostModel>()
                    { Message = $"un- Disliked post", Result = postModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        private string GetBearerToken()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var bearerToken);
                return bearerToken.ToString().Split(" ")[1];
            }
            catch
            {
                throw new InvalidTokenException();
            }
        }
    }
}
