using Application.AIMLforDiabetes;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLDiabetesRiskPredictionController : ControllerBase
    {
        private readonly DiabetesRiskPredictionModel diabetesRiskPredictionModel;

        public AIMLDiabetesRiskPredictionController()
        {
            diabetesRiskPredictionModel = new DiabetesRiskPredictionModel();
            var sampleData = DiabetesDataGenerator.GetDiabetesData();
            diabetesRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] DiabetesData diabetes)
        {
            var predictedRisk = diabetesRiskPredictionModel.Predict(diabetes);
            return Ok(predictedRisk);
        }
    }
}
