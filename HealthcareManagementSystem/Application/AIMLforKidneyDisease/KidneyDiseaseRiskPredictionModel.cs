using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforKidneyDisease
{
    public class KidneyDiseaseRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public KidneyDiseaseRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<KidneyDiseaseData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(KidneyDiseaseData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(KidneyDiseaseData.weight), outputKind: DataKind.Single))


				.Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(KidneyDiseaseData.gender)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("FamilyHistoryEncoded", nameof(KidneyDiseaseData.family_history)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("DiabetesEncoded", nameof(KidneyDiseaseData.diabetes)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("HypertensionEncoded", nameof(KidneyDiseaseData.hypertension)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("CardiovascularDiseaseEncoded", nameof(KidneyDiseaseData.cardiovascular_disease)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("ProteinuriaEncoded", nameof(KidneyDiseaseData.proteinuria)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("HematuriaEncoded", nameof(KidneyDiseaseData.hematuria)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("SmokingEncoded", nameof(KidneyDiseaseData.smoking)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("AlchoolUseEncoded", nameof(KidneyDiseaseData.alchool_use)))
                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(KidneyDiseaseData.age),
					nameof(KidneyDiseaseData.weight),
	
					"GenderEncoded", "FamilyHistoryEncoded", "DiabetesEncoded", "HypertensionEncoded", "CardiovascularDiseaseEncoded", "ProteinuriaEncoded", "HematuriaEncoded", "SmokingEncoded", "AlchoolUseEncoded")
					
				)
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(KidneyDiseaseData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100));

			model = pipeline.Fit(dataView);
		}

		public float Predict(KidneyDiseaseData kidneyDiseaseData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<KidneyDiseaseData, KidneyDiseaseDataPrediction>(model);
            var prediction=predictionEngine.Predict(kidneyDiseaseData);
            return prediction.risk;
        }

    }
}