using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforParkinson
{
    public class ParkinsonRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public ParkinsonRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<ParkinsonData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(ParkinsonData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(ParkinsonData.weight), outputKind: DataKind.Single))

                .Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(ParkinsonData.gender)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("FamilyHistoryEncoded", nameof(ParkinsonData.family_history)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("PreviousHeadTraumaEncoded", nameof(ParkinsonData.previous_head_trauma)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("StrokeEncoded", nameof(ParkinsonData.stroke)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("BradykinesiaEncoded", nameof(ParkinsonData.bradykinesia)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("HandwritingChangesEncoded", nameof(ParkinsonData.handwriting_changes)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("SpeechChangesEncoded", nameof(ParkinsonData.speech_changes)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("DrugOrAlchoolUseEncoded", nameof(ParkinsonData.drug_or_alchool_use)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("SleepDisturbanceEncoded", nameof(ParkinsonData.sleep_disturbance)))


                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(ParkinsonData.age),
					nameof(ParkinsonData.weight),

                    "GenderEncoded", "FamilyHistoryEncoded", "PreviousHeadTraumaEncoded", "StrokeEncoded", "BradykinesiaEncoded", "HandwritingChangesEncoded", "SpeechChangesEncoded", "DrugOrAlchoolUseEncoded", "SleepDisturbanceEncoded")
					
				)
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(ParkinsonData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100));

			model = pipeline.Fit(dataView);
		}

		public float Predict(ParkinsonData parkinsonData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<ParkinsonData, ParkinsonDataPrediction>(model);
            var prediction=predictionEngine.Predict(parkinsonData);
            return prediction.risk;
        }

    }
}