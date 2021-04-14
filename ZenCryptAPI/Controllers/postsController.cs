using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Services;
using Domain.Services.Forum;


namespace ZenCryptAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class postsController : ControllerBase
    {
        private readonly IPostService _postService; 
        private readonly IMapper _mapper;

        public postsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        // GET: api/<postsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<postsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<postsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<postsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<postsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
