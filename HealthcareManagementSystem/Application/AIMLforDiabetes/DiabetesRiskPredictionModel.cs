using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforDiabetes
{
    public class DiabetesRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public DiabetesRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<DiabetesData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesData.hypertension), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesData.heart_disease), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesData.bmi), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesData.HbA1c_level), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesData.blood_glucose_level), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(DiabetesData.gender)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("SmokingHistoryEncoded", nameof(DiabetesData.smoking_history)))
                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(DiabetesData.age),
					nameof(DiabetesData.hypertension),
					nameof(DiabetesData.heart_disease),
					nameof(DiabetesData.bmi),
					nameof(DiabetesData.HbA1c_level),
					nameof(DiabetesData.blood_glucose_level),
					"GenderEncoded", "SmokingHistoryEncoded")
					
				)
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(DiabetesData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100));

			model = pipeline.Fit(dataView);
		}

		public float Predict(DiabetesData diabetesData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<DiabetesData, DiabetesDataPrediction>(model);
            var prediction=predictionEngine.Predict(diabetesData);
            return prediction.risk;
        }

    }
}