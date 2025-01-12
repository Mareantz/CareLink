using Application.AIMLforDiabetesMellitus;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLDiabetesMellitusRiskPredictionController : ControllerBase
    {
        private readonly DiabetesMellitusRiskPredictionModel diabetesMellitusRiskPredictionModel;

        public AIMLDiabetesMellitusRiskPredictionController()
        {
            diabetesMellitusRiskPredictionModel = new DiabetesMellitusRiskPredictionModel();
            var sampleData = DiabetesMellitusDataGenerator.GetDiabetesMellitusData();
            diabetesMellitusRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] DiabetesMellitusData diabetesMellitus)
        {
            var predictedRisk = diabetesMellitusRiskPredictionModel.Predict(diabetesMellitus);
            return Ok(predictedRisk);
        }
    }
}
