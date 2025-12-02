namespace ToDo.Domain.Models
{
    public class Usuario
    {
        public int Id { get;  private set; }
        public string? Nome { get; private set; }
        public string? Email { get; private set; }
        public string? SenhaHash { get; private set; }
        public DateTime DataCriacao { get; private set; }


        public Usuario(string nome, string email, string senhaHash)
        {
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
            DataCriacao = DateTime.UtcNow;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }
        public void AtualizarEmail(string email)
        {
            Email = email;
        }
        public void AtualizarSenha(string senhaHash)
        {
            SenhaHash = senhaHash;
        }
    }
}