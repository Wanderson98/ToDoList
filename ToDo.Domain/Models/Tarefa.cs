using System;


namespace ToDo.Domain.Models
{
    public class Tarefa
    {

        public Tarefa(string titulo, string descricao)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("O título da tarefa não pode ser vazio.", nameof(titulo));

            this.Titulo = titulo;
            this.Descricao = descricao;
            this.DataCriacao = DateTime.Now;
            this.Concluido = false;
        }


        public int Id { get; private set; }
        
        public string Titulo { get; private set; }
        
        public string Descricao { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public bool Concluido { get; private set; }

        public void MarcarComoConcluida()
        {
            this.Concluido = true;
            this.DataConclusao = DateTime.Now;
        }

    }

    
}

