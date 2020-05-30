using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProcessamentoAcoes.Models;
using ProcessamentoAcoes.Validators;
using ProcessamentoAcoes.Data;

namespace ProcessamentoAcoes
{
    public static class VendaAcoesRabbitMQTrigger
    {
        [FunctionName("VendaAcoesRabbitMQTrigger")]
        public static void Run(
            [RabbitMQTrigger("venda-acoes-queue", ConnectionStringSetting = "RabbitMQConnection")]string inputMessage,
            ILogger log)
        {
           log.LogInformation($"VendaAcoesRabbitMQTrigger - Dados: {inputMessage}");

            Transacao transacao = null;
            try
            {
                transacao = JsonSerializer.Deserialize<Transacao>(inputMessage,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                log.LogInformation($"VendaAcoesRabbitMQTrigger - Erro durante a deserialização");
            }
            
            if (transacao == null)
                return;

            var validationResult = new TransacoesValidator().Validate(transacao);
            if (validationResult.IsValid)
            {
                log.LogInformation($"VendaAcoesRabbitMQTrigger - Dados pós formatação: {JsonSerializer.Serialize(transacao)}");
                AcoesRepository.Save<VendaAcaoDocument>(transacao);
                log.LogInformation("VendaAcoesRabbitMQTrigger - Transação registrada com sucesso!");
            }
            else
            {
                log.LogInformation("VendaAcoesRabbitMQTrigger - Dados inválidos para a Transação");
            }
        }
    }
}