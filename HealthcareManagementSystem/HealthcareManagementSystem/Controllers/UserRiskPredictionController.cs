using Application.AIML;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareManagementSystem.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class UserRiskPredictionController : ControllerBase
	{
		private readonly UserRiskPredictionModel userRiskPredictionModel;

		public UserRiskPredictionController()
		{
			userRiskPredictionModel = new UserRiskPredictionModel();
			var sampleData = UserDataGenerator.GetUsers();
			userRiskPredictionModel.Train(sampleData);
		}

		[HttpPost("predict")]
		public ActionResult<float> PredictRisk([FromBody] UserData user)
		{
			// The user may provide risk, but it will be ignored during prediction.
			// The model predicts and returns the new risk value.
			var predictedRisk = userRiskPredictionModel.Predict(user);
			return Ok(predictedRisk);
		}
	}
}
