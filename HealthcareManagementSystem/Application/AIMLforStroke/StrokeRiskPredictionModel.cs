using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforStroke
{
    public class StrokeRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public StrokeRiskPredictionModel()
        {
            mLContext = new MLContext();

        }

        public void Train(List<StrokeData> trainingData)
        {
            var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(StrokeData.age), outputKind: DataKind.Single)
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(StrokeData.hypertension), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(StrokeData.heart_disease), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(StrokeData.avg_glucose_level), outputKind: DataKind.Single))

                .Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(StrokeData.gender)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("EverMarriedEncoded", nameof(StrokeData.ever_married)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("WorkTypeEncoded", nameof(StrokeData.work_type)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("ResidenceTypeEncoded", nameof(StrokeData.residence_type)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("SmokingStatusEncoded", nameof(StrokeData.smoking_status)))
                .Append(mLContext.Transforms.Concatenate("Features",
                    nameof(StrokeData.age),
                    nameof(StrokeData.hypertension),
                    nameof(StrokeData.heart_disease),
                    nameof(StrokeData.avg_glucose_level),

                    "GenderEncoded", "EverMarriedEncoded", "WorkTypeEncoded", "ResidenceTypeEncoded", "SmokingStatusEncoded")

                )
                .Append(mLContext.Regression.Trainers.Sdca(
                    labelColumnName: nameof(StrokeData.risk),
                    featureColumnName: "Features",
                    maximumNumberOfIterations: 100));

            model = pipeline.Fit(dataView);
        }

        public float Predict(StrokeData strokeData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<StrokeData, StrokeDataPrediction>(model);
            var prediction = predictionEngine.Predict(strokeData);
            return prediction.risk;
        }

    }
}