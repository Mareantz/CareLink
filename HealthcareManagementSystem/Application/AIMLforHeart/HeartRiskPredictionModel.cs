using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforHeart
{
    public class HeartRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public HeartRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<HeartData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.sex), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.cp), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.trestbps), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.chol), outputKind: DataKind.Single))
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.fbs), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.restecg), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.thalach), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.exang), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.oldpeak), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.slope), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.ca), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(HeartData.thal), outputKind: DataKind.Single))

                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(HeartData.age),
					nameof(HeartData.sex),
					nameof(HeartData.cp),
					nameof(HeartData.trestbps),
					nameof(HeartData.chol),
					nameof(HeartData.fbs),

                    nameof(HeartData.restecg),
                    nameof(HeartData.thalach),
                    nameof(HeartData.exang),
                    nameof(HeartData.oldpeak),
                    nameof(HeartData.slope),
                    nameof(HeartData.ca),
                    nameof(HeartData.thal)



                )
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(HeartData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100)));

			model = pipeline.Fit(dataView);
		}

		public float Predict(HeartData heartData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<HeartData, HeartDataPrediction>(model);
            var prediction=predictionEngine.Predict(heartData);
            return prediction.risk;
        }

    }
}