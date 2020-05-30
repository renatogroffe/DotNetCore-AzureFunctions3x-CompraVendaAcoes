namespace ProcessamentoAcoes.Models
{
    public abstract class TransacaoDocument
    {
        public string id { get; set; }
        public string Codigo { get; set; }
        public string Data { get; set; }
        public double Valor { get; set; }
    }

    public class CompraAcaoDocument : TransacaoDocument
    {
        public bool Compra => true;
    }

    public class VendaAcaoDocument : TransacaoDocument
    {
        public bool Venda => true;
    }
}