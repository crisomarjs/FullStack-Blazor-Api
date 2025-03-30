using ApiBlog.Models;
using ApiBlog.Models.DTO;
using ApiBlog.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostsController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPosts()
        {
            var listaPost = _postRepository.GetPosts();
            var listaPostDTO = new List<PostDTO>();

            foreach (var lista in listaPost) 
            { 
                listaPostDTO.Add(_mapper.Map<PostDTO>(lista));
            }

            return Ok(listaPostDTO);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPost(int id)
        {
            var itemPost = _postRepository.GetPost(id);
            if (itemPost == null)
            {
                return NotFound();
            }

            var itemPostDTO = _mapper.Map<PostDTO>(itemPost);

            return Ok(itemPostDTO);
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PostCrearDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPost([FromBody] PostCrearDTO crearPostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearPostDTO == null)
            {
                return BadRequest(ModelState);
            }
                
            if(_postRepository.ExistePost(crearPostDTO.Titulo))
            {
                ModelState.AddModelError("", "El Post ya existe");
                return StatusCode(404, ModelState);
            }

            var post = _mapper.Map<Post>(crearPostDTO);
            if(!_postRepository.CrearPost(post))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {post.Titulo}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPost", new { id = post.Id }, post);
        }

        //[Authorize]
        [HttpPatch("{id:int}", Name = "ActualizarPatchPost")]
        [ProducesResponseType(201, Type = typeof(PostCrearDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPost(int id, [FromBody] PostActualizarDTO actualizarPostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actualizarPostDTO == null || id != actualizarPostDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var post = _mapper.Map<Post>(actualizarPostDTO);

            if (!_postRepository.ActualizarPost(post))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {post.Titulo}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //[Authorize]
        [HttpDelete("{id:int}", Name = "BorrarPost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BorrarPost(int id)
        {
            if(!_postRepository.ExistePost(id))
            {
                return NotFound();
            }

            var post = _postRepository.GetPost(id);
            if(!_postRepository.BorrarPost(post))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {post.Titulo}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
