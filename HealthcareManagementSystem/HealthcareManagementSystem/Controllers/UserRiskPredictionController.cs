using Application.AIML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class UserRiskPredictionController: ControllerBase
    {
        private readonly UserRiskPredictionModel userRiskPredictionModel;

        public UserRiskPredictionController()
        {
            userRiskPredictionModel = new UserRiskPredictionModel();
            var sampleData = UserDataGenerator.GetUsers();
            userRiskPredictionModel.Train(sampleData);
        }

        [HttpPost("predict")]
        public ActionResult<float> PredictRisk(UserData user)
        {
            return userRiskPredictionModel.Predict(user);
        }
    }
}