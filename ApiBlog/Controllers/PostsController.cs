using ApiBlog.Modelos;
using ApiBlog.Modelos.Dtos;
using ApiBlog.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepositorio _postRepo;
        private readonly IMapper _mapper;

        public PostsController(IPostRepositorio postRepo, IMapper mapper)
        {
            _postRepo = postRepo;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPosts()
        {
            var listaPosts = _postRepo.GetPosts();

            var listaPostsDto = new List<PostDto>();

            foreach (var lista in listaPosts)
            {
                listaPostsDto.Add(_mapper.Map<PostDto>(lista));
            }

            return Ok(listaPostsDto);
        }

        [AllowAnonymous]
        [HttpGet("{postId:int}", Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPost(int postId)
        {
            var itemPost = _postRepo.GetPost(postId);

            if (itemPost == null)
            {
                return NotFound();
            }

            var itemPostDto = _mapper.Map<PostDto>(itemPost);
            return Ok(itemPostDto);
        }

        [HttpPost("upload-imagen")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImagen([FromForm] ImagenUploadDto model)
        {
            if (model.Imagen == null || model.Imagen.Length == 0)
            {
                return BadRequest("No se proporcionó ninguna imagen");
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "posts");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = Path.GetExtension(model.Imagen.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(uploadsFolder, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await model.Imagen.CopyToAsync(stream);
                }

                var rutaRelativa = $"/imagenes/posts/{nombreArchivo}";
                return Ok(new { ruta = rutaRelativa });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
            }
        }


        //[Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PostDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPost([FromBody] PostCrearDto crearPostDto)
        {
            if (!ModelState.IsValid || crearPostDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_postRepo.ExistePost(crearPostDto.Titulo))
            {
                ModelState.AddModelError("Titulo", "El post con ese título ya existe");
                return Conflict(ModelState);
            }
            var post = _mapper.Map<Post>(crearPostDto);
            post.FechaCreacion = DateTimeOffset.UtcNow;
            post.FechaActualizacion = DateTimeOffset.UtcNow;
            if (!_postRepo.CrearPost(post))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro '{post.Titulo}'");
                return StatusCode(500, ModelState);
            }
            var postCreadoDto = _mapper.Map<PostDto>(post);
            return CreatedAtRoute("GetPost", new { postId = post.Id }, postCreadoDto);
        }

        //[Authorize]
        [HttpPatch("{postId:int}", Name = "ActualizarPatchPost")]
        [ProducesResponseType(201, Type = typeof(PostCrearDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchPost(int postId, [FromBody] PostActualizarDto actualizarPostDto)
        {
            if (!ModelState.IsValid || actualizarPostDto == null || postId != actualizarPostDto.Id)
            {
                return BadRequest(ModelState);
            }
            var postDesdeDb = _postRepo.GetPost(postId);
            if (postDesdeDb == null)
            {
                return NotFound();
            }

            // 2. MAPEAR los cambios del DTO sobre la entidad que ya existe.
            //    AutoMapper actualizará solo las propiedades que coincidan.
            _mapper.Map(actualizarPostDto, postDesdeDb);

            // 3. ACTUALIZAR la fecha de modificación.
            postDesdeDb.FechaActualizacion = DateTimeOffset.UtcNow;

            // 4. GUARDAR la entidad completa y actualizada.
            if (!_postRepo.ActualizarPost(postDesdeDb))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro '{postDesdeDb.Titulo}'");
                return StatusCode(500, ModelState);
            }

            // 5. Retornar NoContent (204) como indica el estándar para actualizaciones exitosas.
            return NoContent();
        }
            //[Authorize]
            [HttpDelete("{postId:int}", Name = "BorrarPost")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status403Forbidden)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public IActionResult BorrarPost(int postId)
            {
                if (!_postRepo.ExistePost(postId))
                {
                    return NotFound();
                }

                var post = _postRepo.GetPost(postId);

                if (!_postRepo.BorrarPost(post))
                {
                    ModelState.AddModelError("", $"Algo salió mal borrando el registro{post.Titulo}");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }

        }
    }