using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforTuberculosis
{
    public class TuberculosisRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public TuberculosisRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

		public void Train(List<TuberculosisData> trainingData)
		{
			var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

			var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(TuberculosisData.age), outputKind: DataKind.Single)
				.Append(mLContext.Transforms.Conversion.ConvertType(nameof(TuberculosisData.weight), outputKind: DataKind.Single))
		
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(TuberculosisData.gender)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("PastTBHistoryEncoded", nameof(TuberculosisData.past_TB_history)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("CoughEncoded", nameof(TuberculosisData.cough)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("HemoptysisEncoded", nameof(TuberculosisData.hemoptysis)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("FeverEncoded", nameof(TuberculosisData.fever)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("NightSweatsEncoded", nameof(TuberculosisData.night_sweats)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("WeightLossEncoded", nameof(TuberculosisData.weight_loss)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("CloseContactWithTPPatientsEncoded", nameof(TuberculosisData.close_contact_with_TP_patients)))
				.Append(mLContext.Transforms.Categorical.OneHotEncoding("HIVEncoded", nameof(TuberculosisData.HIV)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("PositiveTSTEncoded", nameof(TuberculosisData.positive_TST)))
                .Append(mLContext.Transforms.Concatenate("Features",
					nameof(TuberculosisData.age),
					nameof(TuberculosisData.weight),
		
					"GenderEncoded", "PastTBHistoryEncoded", "CoughEncoded", "HemoptysisEncoded", "FeverEncoded", "NightSweatsEncoded", "WeightLossEncoded", "CloseContactWithTPPatientsEncoded", "HIVEncoded", "PositiveTSTEncoded")
					
				)
				.Append(mLContext.Regression.Trainers.Sdca(
					labelColumnName: nameof(TuberculosisData.risk),
					featureColumnName: "Features",
					maximumNumberOfIterations: 70));

			model = pipeline.Fit(dataView);
		}

		public float Predict(TuberculosisData tuberculosisData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<TuberculosisData, TuberculosisDataPrediction>(model);
            var prediction=predictionEngine.Predict(tuberculosisData);
            return prediction.risk;
        }

    }
}