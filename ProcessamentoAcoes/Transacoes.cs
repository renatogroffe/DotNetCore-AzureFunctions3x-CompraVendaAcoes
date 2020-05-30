using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProcessamentoAcoes.Data;

namespace ProcessamentoAcoes
{
    public static class Transacoes
    {
        [FunctionName("Transacoes")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function Transacoes - HTTP GET");
            return new OkObjectResult(AcoesRepository.GetAll());
        }
    }
}