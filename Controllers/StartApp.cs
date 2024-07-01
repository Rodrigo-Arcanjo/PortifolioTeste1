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
    public class StartApp : ControllerBase
    {
        /// <summary>
        /// Obter Todos os Processos
        /// </summary>
        /// <param name="matriculaAgente">Matrícula do Agente</param>
        /// <param name="password">Senha do Agente</param>
        /// <param name="imeiAparelho">IMEI do equipamento</param>
        /// <returns>Listagem dos processos e todos os seus respectivos dados</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        //[AcceptVerbs("GET", "POST")]
        [HttpGet("Login/{matriculaAgente}/{password}/{imeiAparelho}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login (string matriculaAgente, string password, string imeiAparelho)
        {
            //354547677885719

            //User user;
            Status status = null;
            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@MatriculaAgente", matriculaAgente),
                new SqlParameter("@Senha", password),
                new SqlParameter("@IMEI", imeiAparelho)
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
        }

        /// <summary>
        /// Atualizar controle de Versão
        /// </summary>
        /// <param name="imei">IMEI do Aparelho</param>
        /// <param name="versao">Versão do App</param>
        /// <returns>Data e Versão</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet("AppControle/{imei}/{versao}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AppControle (string imei, string versao)
        {
            Status status = null;
            string sqlCommandTwo = @"SELECT TOP 1 Versao, Url FROM AppVersao";

            List<SqlParameter> lsql = new List<SqlParameter>
            {
                new SqlParameter("@imei", imei),
                new SqlParameter("@versao", versao)
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "DADOS NÃO ENCONTRADOS", ""));
            }

            string json = DbaJsonObject.ExecProcToJson2("INSERT_APP_CONTROLE", lsql, sqlCommandTwo);

            if (!string.IsNullOrEmpty(json) && !json.Equals("[]"))
            {
                try
                {
                    Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);

                    //  ALTERAR O TIPO DA LISTA PARA O OBETO CORRETO !!!
                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<AppVersao>>(json));
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

        /// <summary>
        /// Histórico de Atualização de tabelas
        /// </summary>
        /// <returns>Nome, Data e Hora</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet("ListarHorario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ListarHorario()
        {
            Status status = null;
            List<SqlParameter> lsql = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "DADOS NÃO ENCONTRADOS", ""));
            }

            string json = DbaJsonObject.ExecProcToJson("STb_AtualizarTabelas_app_Localizar", lsql);

            if (!string.IsNullOrEmpty(json) && !json.Equals("[]"))
            {
                try
                {
                    Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);

                    status = new Status("OK", "", JsonConvert.DeserializeObject<List<CheckTabela>>(json));
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

        /// <summary>
        /// Obter Todos os Processos
        /// </summary>
        /// <param name="all">Tipo de busca</param>
        /// <returns>Listagem dos processos e todos os seus respectivos dados</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Falha de comunicação com o servidor</response>
        /// <response code="404">Não encontrado</response>
        /// <response code="500">Erro genérico do servidor</response>
        [HttpGet("TableSQLite/{all}/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult tableSQLite(string all)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            //List<CheckTabela> itens = 

            Status status = null;
            Status status2 = null;
            Status status3 = null;
            List<SqlParameter> lsql = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(new Status("FALHA", "DADOS NÃO ENCONTRADOS", ""));
            }

            var json = DbaJsonObject.ExectableSQLiteToJson(all);

            if (json != null)
            {
                try
                {
                    //Newtonsoft.Json.Linq.JArray objJson = Newtonsoft.Json.Linq.JArray.Parse(json);

                    status = new Status("OK", "", json);
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
