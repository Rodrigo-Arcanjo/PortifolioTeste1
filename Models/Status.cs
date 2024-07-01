namespace PortifolioTeste1.Models
{
    public class Status
    {
        public Status(string statusXd, string mensagemXd, object dadosXd)
        {
            status = statusXd;
            mensagem = mensagemXd;
            dados = dadosXd;
        }

        /*public Status(string statusXd, string mensagemXd, List<object> dadosXd)
        {
            status = statusXd;
            mensagem = mensagemXd;
            dados = dadosXd;
        }*/

        public string status { get; set; }
        public string mensagem { get; set; }
        public object dados { get; set; }
        //public List<object> dadosList { get; set; }
    }
}
