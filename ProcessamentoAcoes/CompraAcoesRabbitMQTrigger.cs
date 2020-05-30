using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProcessamentoAcoes.Models;
using ProcessamentoAcoes.Validators;
using ProcessamentoAcoes.Data;

namespace ProcessamentoAcoes
{
    public static class CompraAcoesRabbitMQTrigger
    {
        [FunctionName("CompraAcoesRabbitMQTrigger")]
        public static void Run(
            [RabbitMQTrigger("compra-acoes-queue", ConnectionStringSetting = "RabbitMQConnection")] string inputMessage,
            ILogger log)
        {
            log.LogInformation($"CompraAcoesRabbitMQTrigger - Dados: {inputMessage}");

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
                log.LogInformation($"CompraAcoesRabbitMQTrigger - Erro durante a deserialização");
            }
            
            if (transacao == null)
                return;

            var validationResult = new TransacoesValidator().Validate(transacao);
            if (validationResult.IsValid)
            {
                log.LogInformation($"CompraAcoesRabbitMQTrigger - Dados pós formatação: {JsonSerializer.Serialize(transacao)}");
                AcoesRepository.Save<CompraAcaoDocument>(transacao);
                log.LogInformation("CompraAcoesRabbitMQTrigger - Transação registrada com sucesso!");
            }
            else
            {
                log.LogInformation("CompraAcoesRabbitMQTrigger - Dados inválidos para a Transação");
            }
        }
    }
}