using Microsoft.Data.SqlClient;
using Nancy.Json;
using PortifolioTeste1.Connection;
using PortifolioTeste1.Models;
using System.Data;
using System.Reflection.Metadata;

namespace PortifolioTeste1.Util
{
    public class DbaJsonObject
    {

        private SqlConnection sqlConn;

        public static string ExecQueryToJson(string sql, List<SqlParameter> listOfParams)
        {
            using (var con = Conn.GetConnection())
            {
                SqlDataReader dr = null;
                var cmd = new SqlCommand(sql, con);
                string json;

                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    dr = cmd.ExecuteReader();

                    json = JsonObject.ToJson(dr);
                }
                catch (Exception ex)
                {
                    throw new Exception("Houve um erro na Consulta: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
                return json;
            }
        }

        public static string ExecProcToJson(string sql, List<SqlParameter> listOfParams)
        {
            using (var con = Conn.GetConnection())
            {
                SqlDataReader dr = null;
                var cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string json;

                if (listOfParams != null)
                {
                    try
                    {
                        listOfParams.ForEach((item) => { cmd.Parameters.Add(item); });

                        con.Open();
                        dr = cmd.ExecuteReader();

                        json = JsonObject.ToJson(dr);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Houve um erro na Consulta: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                    return json;
                }
                else
                {
                    try
                    {
                        con.Open();
                        dr = cmd.ExecuteReader();

                        json = JsonObject.ToJson(dr);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Houve um erro na Consulta: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                    return json;
                }
                
            }
        }

        public static string ExecProcToJson2(string sql, List<SqlParameter> listOfParams, string sql2)
        {
            //var con = Conn.GetConnection()
            using (var con = Conn.GetConnection())
            {
                SqlDataReader dr = null;
                var cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string json;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }

                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql2;

                try
                {
                    con.Open();

                    dr = cmd.ExecuteReader();

                    json = JsonObject.ToJson(dr);
                }
                catch (Exception e)
                {
                    throw new Exception("Houve um erro na Consulta: " + e.Message);
                }
                finally
                {
                    con.Close();
                }
                
                return json;
            }
        }

        public static object ExectableSQLiteToJson(string all)
        {
            using (var con = Conn.GetConnection())
            {
                List<Object> ClassificacaoVitimaList = new List<Object>();
                List<Object> TipoAcidenteList = new List<Object>();
                List<Object> SituacaoPavimentoList = new List<Object>();
                List<Object> SituacaoSemaforoList = new List<Object>();
                List<Object> SituacaoTempoList = new List<Object>();
                List<Object> SituacaoViaList = new List<Object>();
                List<Object> TipoCategoriaCNHList = new List<Object>();
                List<Object> TipoVeiculoList = new List<Object>();
                List<Object> PontoImpactoList = new List<Object>();
                List<Object> TipoPessoaList = new List<Object>();
                List<Object> TipoLesaoList = new List<Object>();
                List<Object> CondicaoVitimaList = new List<Object>();
                List<Object> TipoServico = new List<Object>();
                List<Object> TipoAcessorio = new List<Object>();
                List<Object> TipoChoque = new List<Object>();
                List<Object> PerguntasList = new List<Object>();
                List<Object> RespostasList = new List<Object>();
                List<Object> PosicaoVitimaList = new List<Object>();
                List<Object> HospitaisList = new List<Object>();
                List<Object> ListTipoVeiculoServico = new List<Object>();

                string query1 = @"select CLV_Id, CLV_NomeClassificacao from ClassificacaoVitima";
                string query2 = @"select PTI_Id, PTI_Descricao from PontoImpacto";
                string query3 = @"select STP_Id, STP_NomePavimento from SituacaoPavimento";
                string query4 = @"select STF_Id, STF_NomeSemaforo from SituacaoSemaforo";
                string query5 = @"select STE_Id, STE_NomeTempo from SituacaoTempo";
                string query6 = @"select STV_Id, STV_NomeVia from SituacaoVia";
                string query7 = @"select TPA_Id, TPA_NomeTipoAcidente, TPA_DescricaoTipoAcidente from TipoAcidente where TPA_Id not in (1, 6)";
                string query8 = @"SELECT TPC_Id, TPC_NomeTipoCategoria FROM TipoCategoriaCNH";
                string query9 = @"SELECT TPV_Id, TPV_Descricao, TPV_RAT_Id from TipoVeiculo order by TPV_Descricao asc";
                string query10 = @"select TPP_Id, TPP_Descricao from tipopessoa";
                string query11 = @"select TPL_Id, TPL_Descricao from tipoLesao";
                string query12 = @"select CDV_Id, CDV_Descricao from condicaoVitima";
                string query13 = @"SELECT TS_ID, TS_DESCRICAO INTO #TempServicos FROM TIPOSERVICO
                                        SELECT * FROM #TempServicos AS tmp
                                        ORDER BY tmp.TS_DESCRICAO
                                        DROP TABLE #TempServicos";
                string query14 = @"select TA_ID, TA_descricao, TA_TPV_Id from tipoAcessorio";
                string query15 = @"SELECT TPC_Id, TPC_Descricao FROM TipoChoque";
                string query16 = @"SELECT RFR_id, RFR_Descricao FROM RespostaFatoresRisco";
                string query17 = @"SELECT PR.PFR_ID,
                                        	   PR.PFR_DESCRICAO,
                                        	   PF.PPT_RAT_ID,
                                               PF.PPT_TPP_ID
                                        FROM PERGUNTACATEGORIAFATORRISCO AS PF, 
                                        	 PERGUNTASFATORESRISCO AS PR
                                        WHERE 
                                        	PR.PFR_ID = PF.PPT_PFR_ID";
                string query18 = @"select PSV_Id, PSV_Descricao from PosicaoVitima";
                string query19 = @"SELECT HOSP_ID, HOSP_NOME FROM HOSPITAIS";
                string query20 = @"select 
                                             vts.VTS_TPV_Id,
                                             ts.TS_DESCRICAO,
                                             vts.vts_ts_id
                                        from 
                                        	veiculotiposervico as vts,
                                        	tipoveiculo as tv,
                                        	tiposervico as ts
                                        where
                                        	tv.TPV_Id = vts.VTS_TPV_Id
                                        and ts.TS_ID = vts.VTS_TS_Id
                                            order by ts.TS_DESCRICAO asc";

                string json1, json2, json3, json4, json5, json6, json7, json8, json9, json10, json11, json12, json13, json14, json15, json16, json17, json18, json19, json20;

                JavaScriptSerializer js = new JavaScriptSerializer();
                SqlDataReader dr = null;
                string json;
                //List<CheckTabela> itens = (List<CheckTabela>)js.DeserializeObject(items, typeof(List<CheckTabela>));

                if (all.Equals("all"))
                {
                    var cmd = new SqlCommand(query1, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    dr = cmd.ExecuteReader();
                    //try { json1 = JsonObject.ToJson(dr); ClassificacaoVitimaList.Add(json1); } catch (Exception ex) { }
                    while (dr.Read()) 
                    {
                        ClassificacaoVitimaList.Add(new
                        {
                            CLV_Id = dr["CLV_Id"],
                            CLV_NomeClassificacao = dr["CLV_NomeClassificacao"]
                        });
                    }

                    dr.Dispose();
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"select PTI_Id, PTI_Descricao from PontoImpacto";

                    dr = cmd.ExecuteReader();
                    //try { json2 = JsonObject.ToJson(dr); PontoImpactoList.Add(json2); } catch (Exception ex) { }
                    while (dr.Read())
                    {
                        PontoImpactoList.Add(new
                        {
                            PTI_Id = dr["PTI_Id"],
                            PTI_Descricao = dr["PTI_Descricao"]
                        });
                    }

                    con.Close();

                    return new
                    {
                        cv = ClassificacaoVitimaList,
                        pi = PontoImpactoList
                    };
                }

                return null;
            }
        }

        //      APENAS A PRIMEIRA CONSULTA DA PROCEDURE É FEITA
        public static string ExecExemplo(string sql)
        {
            using (var con = Conn.GetConnection())
            {

                SqlDataReader dr = null;
                var cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string json;

                try
                {
                    con.Open();
                    dr = cmd.ExecuteReader();
                    json = JsonObject.ToJson(dr);
                }
                catch (Exception ex)
                {
                    throw new Exception("Houve um erro na Consulta: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
                return json;

            }
        }

        public SqlConnection openConnection()
        {

            try
            {
                string connectionString = WebApplication.CreateBuilder().Configuration.GetConnectionString("DevEventsCsHom");

                SqlConnection sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                this.sqlConn = sqlConn;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível acessar a base da dados do sistema.");
            }

            return sqlConn;
        }

        public SqlConnection getConnectionXd()
        {

            try
            {
                string connectionString = WebApplication.CreateBuilder().Configuration.GetConnectionString("DevEventsCsHom");

                SqlConnection sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                this.sqlConn = sqlConn;
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível acessar a base de dados.");
            }

            return sqlConn;
        }

        public void closeConnection()
        {
            if (this.sqlConn != null)
            {
                sqlConn.Close();
            }

        }

    }
}
