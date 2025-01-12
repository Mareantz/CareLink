using Application.AIMLforAlzheimer;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLAlzheimerRiskPredictionController : ControllerBase
    {
        private readonly AlzheimerRiskPredictionModel alzheimerRiskPredictionModel;

        public AIMLAlzheimerRiskPredictionController()
        {
            alzheimerRiskPredictionModel = new AlzheimerRiskPredictionModel();
            var sampleData = AlzheimerDataGenerator.GetAlzheimerData();
            alzheimerRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] AlzheimerData alzheimer)
        {
            var predictedRisk = alzheimerRiskPredictionModel.Predict(alzheimer);
            return Ok(predictedRisk);
        }
    }
}
