using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Nancy.Json;
using Newtonsoft.Json;
using PortifolioTeste1.Models;
using PortifolioTeste1.Util;

namespace PortifolioTeste1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Ocorrencia : ControllerBase
    {

        /// <summary>
        /// Obter Todos os Processos
        /// </summary>
        /// <param name="parameter">Matrícula do Agente</param>
        /// <returns>Listagem dos processos e todos os seus respectivos dados</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        //[AcceptVerbs("GET", "POST")]
        [HttpGet("ListarOcorrencia/{parameter}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult listarOcorrencia(string parameter)
        {
            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@ACI_MatriculaAgente", parameter)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "DADOS NÃO ENCONTRADOS", ""));
            }

            string json = DbaJsonObject.ExecProcToJson("STb_Acidente_app_Localizar", lsql);

            if (!string.IsNullOrEmpty(json) && !json.Equals("[]"))
            {
                try
                {
                    Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);

                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<Acidente>>(json));
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
