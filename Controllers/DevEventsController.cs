using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortifolioTeste1.Models;
using PortifolioTeste1.Persistence;
using PortifolioTeste1.Util;

namespace PortifolioTeste1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;

        public bool login = false;

        public DevEventsController(DevEventsDbContext context)
        {
            _context = context;
        }

        /*    /// <summary>
        /// Obter Todos os Processos
        /// </summary>
        /// <returns>Listagem dos processos e todos os seus respectivos dados</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login (User user)
        {

            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@MatriculaAgente", user.matricula),
                new SqlParameter("@Senha", user.senha),
                new SqlParameter("@IMEI", user.imei)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "DADOS NÃO ENCONTRADOS", ""));
            }

            string json = DbaJsonObject.ExecProcToJson("STb_APP_LOGIN", lsql);

            if (!string.IsNullOrEmpty(json) && !json.Equals("[]"))
            {
                try
                {
                    Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);

                    login = true;
                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<User>>(json));
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Status("FALHA", "CONFIGURAR MENSAGEM DE RETORNO ->" + ex.Message, ""));
                }
            }
            else 
            {
                return NotFound(new Status("NÃO LOCALIZADO", "CONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            return Ok(status);
        }*/

        /// <summary>
        /// Obter todos os Eventos
        /// </summary>
        /// <returns>Coleção de Eventos</returns>
        /// <response code = "200">Sucesso</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Route("GetProcessos/{UserId}/{REQ_RT}/{CurrentItemmount}")]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList();

            return Ok(devEvents);
        }

        /// <summary>
        /// Obter um Evento
        /// </summary>
        /// <param name="id">Identificador do Evento</param>
        /// <returns>Dados de 1 Evento</returns>
        /// <response code = "200">Sucesso</response>
        /// <response code = "404">Não encontrado</response>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(de => de.Speakers)
                .SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }
            return Ok(devEvent);
        }

        /// <summary>
        /// Cadastrar um Evento
        /// </summary>
        /// <remarks>
        /// objeto JSON
        /// </remarks>
        /// <param name="devEvent">Dados do Evento</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code = "201">Sucesso</response>
        [HttpPost("Post/{devEventObj}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }

        /// <summary>
        /// Atualizar um Evento
        /// </summary>
        /// <remarks>
        /// objeto JSON
        /// </remarks>
        /// <param name="id">Identificador do Evento</param>
        /// <param name="input">Dados do Evento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado.</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("Update/{id}/{devEventObj}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvents.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletar um Evento
        /// </summary>
        /// <param name="id">Identificador de Evento</param>
        /// <returns>Nada</returns>
        /// <repsonse code="404">Não encontrado</repsonse>
        /// <repsonse code="204">Sucesso</repsonse>
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Delete();

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Cadastrar Palestrante
        /// </summary>
        /// <remarks>
        /// objeto JSON
        /// </remarks>
        /// <param name="id">Identificador do Evento</param>
        /// <param name="speaker">Dados do palestrante</param>
        /// <returns>Nada</returns>
        /// <repsonse code="404">Não encontrado</repsonse>
        /// <repsonse code="204">Sucesso</repsonse>
        [HttpPost("PostSpeaker/{id}/speakersObj")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            speaker.DevEventId = id;

            var devEvent = _context.DevEvents.Any(d => d.Id == id);

            if (!devEvent)
            {
                return NotFound();
            }

            _context.DevEventSpeakers.Add(speaker);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
