using Application.AIMLforTuberculosis;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLTuberculosisRiskPredictionController : ControllerBase
    {
        private readonly TuberculosisRiskPredictionModel tuberculosisRiskPredictionModel;

        public AIMLTuberculosisRiskPredictionController()
        {
            tuberculosisRiskPredictionModel = new TuberculosisRiskPredictionModel();
            var sampleData = TuberculosisDataGenerator.GetTuberculosisData();
            tuberculosisRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] TuberculosisData tuberculosis)
        {
            // The user may provide risk, but it will be ignored during prediction.
            // The model predicts and returns the new risk value.
            var predictedRisk = tuberculosisRiskPredictionModel.Predict(tuberculosis);
            return Ok(predictedRisk);
        }
    }
}
