using System.Net;
using ApiBlog.Models.DTO;
using ApiBlog.Repository;
using ApiBlog.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        protected RespuestasAPI _respuestasAPI;
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            this._respuestasAPI = new RespuestasAPI();
        }

        [AllowAnonymous]
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDTO usuarioRegistroDTO)
        {
            bool validarNombreUnico = _usuarioRepository.IsUniqueUser(usuarioRegistroDTO.NombreUsuario);
            if(!validarNombreUnico)
            {
                _respuestasAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestasAPI.IsSuccess = false;
                _respuestasAPI.ErrorMessage.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestasAPI);
            }

            var usuario = await _usuarioRepository.Registro(usuarioRegistroDTO);
            if(usuario == null)
            {
                _respuestasAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestasAPI.IsSuccess = false;
                _respuestasAPI.ErrorMessage.Add("Error al registrar el usuario");
                return StatusCode(500, _respuestasAPI);
            }

            _respuestasAPI.statusCode = HttpStatusCode.OK;
            _respuestasAPI.IsSuccess = true;
            return Ok(_respuestasAPI);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO usuarioLoginDTO)
        {
            

            var res = await _usuarioRepository.Login(usuarioLoginDTO);
            if (res.Usuario == null || string.IsNullOrEmpty(res.Token))
            {
                _respuestasAPI.statusCode = HttpStatusCode.BadRequest;
                _respuestasAPI.IsSuccess = false;
                _respuestasAPI.ErrorMessage.Add("El nombre de usuario o password son incorrectos");
                return StatusCode(500, _respuestasAPI);
            }

            _respuestasAPI.statusCode = HttpStatusCode.OK;
            _respuestasAPI.IsSuccess = true;
            _respuestasAPI.Result = res;
            return Ok(_respuestasAPI);
        }

        //[Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usuarioRepository.GetUsuarios();
            var listaUsuariosDTO = new List<UsuarioDTO>();
            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDTO.Add(_mapper.Map<UsuarioDTO>(lista));
            }
            return Ok(listaUsuariosDTO);
        }

        //[Authorize]
        [HttpGet("{id:int}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsuario(int id)
        {
            var usuario = _usuarioRepository.GetUsuario(id);
            if (usuario == null)
            {
                return NotFound();
            }
            var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);
            return Ok(usuarioDTO);
        }


    }
}
