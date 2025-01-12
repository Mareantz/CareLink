using Application.AIMLforParkinson;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLParkinsonRiskPredictionController : ControllerBase
    {
        private readonly ParkinsonRiskPredictionModel parkinsonRiskPredictionModel;

        public AIMLParkinsonRiskPredictionController()
        {
            parkinsonRiskPredictionModel = new ParkinsonRiskPredictionModel();
            var sampleData = ParkinsonDataGenerator.GetParkinsonData();
            parkinsonRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] ParkinsonData parkinson)
        {
            // The user may provide risk, but it will be ignored during prediction.
            // The model predicts and returns the new risk value.
            var predictedRisk = parkinsonRiskPredictionModel.Predict(parkinson);
            return Ok(predictedRisk);
        }
    }
}
