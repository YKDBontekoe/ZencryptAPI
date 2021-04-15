using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Frames;
using Domain.Services.Forum;
using Domain.Services.User;
using ZenCryptAPI.Models.Data.Comment;


namespace ZenCryptAPI.Controllers
{
    [Route("api/posts/{postId}/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentsController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(Guid postId) 
        {
            try
            {
                // Get comments from a post
                var foundComments = await _commentService.GetCommentFromPost(postId);

                // Map comments to model
                var commentModel = _mapper.Map<IEnumerable<MultiCommentModel>>(foundComments);

                // Wrap the commentModel object to an api frame
                var returnable = new MultiItemFrame<MultiCommentModel>()
                    { Message = $"Found posts", TotalResults = commentModel.Count(), Results = commentModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(Guid id)
        {
            try
            {
                // Get comment by id
                var foundComment = await _commentService.GetComment(id);

                // Map comment to model
                var commentModel = _mapper.Map<SingleCommentModel>(foundComment);

                // Wrap the commentModel object to an api frame
                var returnable = new SingleItemFrame<SingleCommentModel>()
                    { Message = $"Found posts", Result = commentModel };

                // Returns code 200 and the userModel
                return Ok(returnable);
            }
            catch (Exception e)
            {
                // Returns 404 with exception message
                return NotFound(new SingleItemFrame<object> { Message = e.Message });
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
