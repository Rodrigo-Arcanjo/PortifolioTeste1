namespace PortifolioTeste1.Models
{
    public record User
        (
            string MatriculaAgente,
            string senha,
            string nome,
            string imei,
            string status,
            string [] Roles
        );
}
