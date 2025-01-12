using Application.AIMLforEpilepsy;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLEpilepsyRiskPredictionController : ControllerBase
    {
        private readonly EpilepsyRiskPredictionModel epilepsyRiskPredictionModel;

        public AIMLEpilepsyRiskPredictionController()
        {
            epilepsyRiskPredictionModel = new EpilepsyRiskPredictionModel();
            var sampleData = EpilepsyDataGenerator.GetEpilepsyData();
            epilepsyRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] EpilepsyData epilepsy)
        {
            var predictedRisk = epilepsyRiskPredictionModel.Predict(epilepsy);
            return Ok(predictedRisk);
        }
    }
}
