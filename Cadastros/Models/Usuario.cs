namespace Cadastros.Models
{
    public enum Permissao
    {
        Funcionario = 1,
        Lider = 2,
        Diretor = 3
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string Email { get; set; }
        public string Documento { get; set; }
        public List<string> Telefones { get; set; }
        public int? GerenteId { get; set; } 
        public Permissao Permissao { get; set; } = Permissao.Funcionario;
        public string Senha { get; set; }
        public string NomeGerente { get; set; } 
    }
}
