using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PortifolioTeste1.Models;
using PortifolioTeste1.Util;

namespace PortifolioTeste1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Processo : ControllerBase
    {
        /// <summary>
        /// Obter Todos os Processos
        /// </summary>
        /// <param name="UserId">ID do usuário</param>
        /// <param name="REQ_RT">Índice de responsável técnico ou requerente</param>
        /// <param name="CurrentItemmount">Qt. Itens</param>
        /// <returns>Listagem dos processos e todos os seus respectivos dados</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="500">Erro genérico do servidor</response>
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize]
        [Route("GetProcessos/{UserId}/{REQ_RT}/{CurrentItemmount}")]
        public IActionResult GetProcessos_One(string UserId, int REQ_RT, int CurrentItemmount)
        {
            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@USER_ID", UserId),
                new SqlParameter("@REQ_RT", REQ_RT),
                new SqlParameter("@CURRENT_AMMOUNT", CurrentItemmount)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "CONFIGURAR MENSAGEM DE RETORNO", ""));
            }

            string json = DbaJsonObject.ExecProcToJson($"SM_GetProcessosByUserId", lsql);
            //System.Exception: 'Houve um erro na Consulta: The ConnectionString property has not been initialized.'

            if (!string.IsNullOrWhiteSpace(json) && !json.Equals("[]"))
            {
                try
                {
                    //string jsonDef = JsonConvert.SerializeObject(json);

                    Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);



                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<ProcessoOne>>(json));
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
        }
    }
}
