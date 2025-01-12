using Application.AIMLforKidneyDisease;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AIMLKidneyDiseaseRiskPredictionController : ControllerBase
    {
        private readonly KidneyDiseaseRiskPredictionModel kidneyDiseaseRiskPredictionModel;

        public AIMLKidneyDiseaseRiskPredictionController()
        {
            kidneyDiseaseRiskPredictionModel = new KidneyDiseaseRiskPredictionModel();
            var sampleData = KidneyDiseaseDataGenerator.GetKidneyDiseaseData();
            kidneyDiseaseRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk([FromBody] KidneyDiseaseData kidneyDisease)
        {
            // The user may provide risk, but it will be ignored during prediction.
            // The model predicts and returns the new risk value.
            var predictedRisk = kidneyDiseaseRiskPredictionModel.Predict(kidneyDisease);
            return Ok(predictedRisk);
        }
    }
}
