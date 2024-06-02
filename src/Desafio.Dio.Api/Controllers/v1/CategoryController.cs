using Desafio.Dio.Api.Shared;
using Desafio.Dio.Application.Interfaces;
using Desafio.Dio.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Dio.Api.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obter todas as categorias.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Categorias obtidas com sucesso</response>
        /// <response code="401">Erro caso usuário não esteja autorizado</response>
        /// <response code="500">Retorna erros caso ocorram</response>
        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            try
            {

                var categories = _service.GetAll();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Obter categoria por Id.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id da categoria</param>
        /// <returns></returns>
        /// <response code="200">Categoria obtida com sucesso</response>
        /// <response code="404">Categoria não encontrada</response>
        /// <response code="401">Erro caso usuário não esteja autorizado</response>
        /// <response code="500">Retorna erros caso ocorram</response>
        [HttpGet("{id}")]
        public ActionResult<Category> Get(int id)
        {
            try
            {
                var category = _service.GetById(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }

        /// <summary>
        /// Adicionar nova categoria.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="category">Dados da categoria</param>
        /// <returns></returns>
        /// <response code="201">Categoria adicionada com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="401">Erro caso usuário não esteja autorizado</response>
        /// <response code="500">Retorna erros caso ocorram</response>
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                _service.Add(category);
                return CreatedAtAction(nameof(Post), category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}