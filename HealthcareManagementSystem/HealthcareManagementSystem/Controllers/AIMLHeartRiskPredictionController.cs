using Application.AIMLforHeart;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLHeartRiskPredictionController : ControllerBase
    {
        private readonly HeartRiskPredictionModel heartRiskPredictionModel;

        public AIMLHeartRiskPredictionController()
        {
            heartRiskPredictionModel = new HeartRiskPredictionModel();
            var sampleData = HeartDataGenerator.GetHeartData();
            heartRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] HeartData heart)
        {
            var predictedRisk = heartRiskPredictionModel.Predict(heart);
            return Ok(predictedRisk);
        }
    }
}
