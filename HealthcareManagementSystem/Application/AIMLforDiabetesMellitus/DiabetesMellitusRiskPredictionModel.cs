using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforDiabetesMellitus
{
    public class DiabetesMellitusRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public DiabetesMellitusRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<DiabetesMellitusData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.age), outputKind: DataKind.Single)
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.weight), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.blood_presure), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.HbA1c), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.BMI), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.FBG), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(DiabetesMellitusData.OGTT), outputKind: DataKind.Single))

                .Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(DiabetesMellitusData.gender)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("GeneticPredispositionEncoded", nameof(DiabetesMellitusData.genetic_predisposition)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("UnhealthyDietEncoded", nameof(DiabetesMellitusData.unhealthy_diet)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("InsulinResistanceEncoded", nameof(DiabetesMellitusData.insulin_resistance)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("PhysicalActivityEncoded", nameof(DiabetesMellitusData.physical_activity)))
                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(DiabetesMellitusData.age),
					nameof(DiabetesMellitusData.weight),
					nameof(DiabetesMellitusData.blood_presure),
					nameof(DiabetesMellitusData.HbA1c),
					nameof(DiabetesMellitusData.BMI),
                    nameof(DiabetesMellitusData.FBG),
                    nameof(DiabetesMellitusData.OGTT),

                    "GenderEncoded", "GeneticPredispositionEncoded", "UnhealthyDietEncoded", "InsulinResistanceEncoded", "PhysicalActivityEncoded")
					
				)
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(DiabetesMellitusData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 100));

			model = pipeline.Fit(dataView);
		}

		public float Predict(DiabetesMellitusData diabetesMellitusData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<DiabetesMellitusData, DiabetesMellitusDataPrediction>(model);
            var prediction=predictionEngine.Predict(diabetesMellitusData);
            return prediction.risk;
        }

    }
}