using Application.AIMLforStroke;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLStrokeRiskPredictionController : ControllerBase
    {
        private readonly StrokeRiskPredictionModel strokeRiskPredictionModel;

        public AIMLStrokeRiskPredictionController()
        {
            strokeRiskPredictionModel = new StrokeRiskPredictionModel();
            var sampleData = StrokeDataGenerator.GetStrokeData();
            strokeRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] StrokeData stroke)
        {
            // The user may provide risk, but it will be ignored during prediction.
            // The model predicts and returns the new risk value.
            var predictedRisk = strokeRiskPredictionModel.Predict(stroke);
            return Ok(predictedRisk);
        }
    }
}
