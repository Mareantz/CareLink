using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIML
{
    public class UserRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public UserRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<UserData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(UserData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(UserData.weight), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(UserData.bloodPressure), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(UserData.cholesterolLevel), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(UserData.physicalActivityLevel), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(UserData.smokingStatus), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(UserData.stressLevel), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(UserData.gender)))
				.Append(mLContext.Transforms.Concatenate("Features",
					nameof(UserData.age),
					nameof(UserData.weight),
					nameof(UserData.bloodPressure),
					nameof(UserData.cholesterolLevel),
					nameof(UserData.physicalActivityLevel),
					nameof(UserData.smokingStatus),
					nameof(UserData.stressLevel),
					"GenderEncoded"))
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(UserData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100));

			model = pipeline.Fit(dataView);
		}

		public float Predict(UserData userData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<UserData, UserDataPrediction>(model);
            var prediction=predictionEngine.Predict(userData);
            return prediction.risk;
        }

    }
}