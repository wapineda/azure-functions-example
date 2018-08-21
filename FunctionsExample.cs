
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;

namespace AzureFunctions
{
    public static class FunctionsExample
    {
        [FunctionName("FunctionsExample")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string date = req.Query["date"];            

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            date = date ?? data?.date;            

            int year = 0;
            int month = 0;
            int day = 0;
            DateTime dateTime;
            try
            {
                int.TryParse(date.Substring(0, 2), out day);
                int.TryParse(date.Substring(2, 2), out month);
                int.TryParse(date.Substring(4, 4), out year);
                dateTime = new DateTime(year, month, day);
            }
            catch
            {
                dateTime = DateTime.Now;
            }

            int age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now.Month < dateTime.Month || (DateTime.Now.Month == dateTime.Month && DateTime.Now.Day < dateTime.Day))
                age--;
            
            string resultado = String.Format("Hello {0}", name);
            if (age > 0)
                resultado += String.Format(", you have {0} years old", age);
            return name != null
                ? (ActionResult)new OkObjectResult(resultado)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
