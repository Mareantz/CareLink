using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforAlzheimer
{
    public class AlzheimerRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public AlzheimerRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<AlzheimerData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(AlzheimerData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(AlzheimerData.weight), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(AlzheimerData.educational_level), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(AlzheimerData.depresion), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(AlzheimerData.amnesia), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(AlzheimerData.gender)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("FamilyHistoryEncoded", nameof(AlzheimerData.family_history)))
                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(AlzheimerData.age),
					nameof(AlzheimerData.weight),
					nameof(AlzheimerData.educational_level),
					nameof(AlzheimerData.depresion),
					nameof(AlzheimerData.amnesia),
					"GenderEncoded", "FamilyHistoryEncoded")
					
				)
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(AlzheimerData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100));

			model = pipeline.Fit(dataView);
		}

		public float Predict(AlzheimerData alzheimerData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<AlzheimerData, AlzheimerDataPrediction>(model);
            var prediction=predictionEngine.Predict(alzheimerData);
            return prediction.risk;
        }

    }
}