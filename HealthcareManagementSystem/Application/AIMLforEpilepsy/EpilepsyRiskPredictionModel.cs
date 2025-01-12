using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Application.AIMLforEpilepsy
{
    public class EpilepsyRiskPredictionModel
    {
        private readonly MLContext mLContext;
        private ITransformer model;
        public EpilepsyRiskPredictionModel()
        {
            mLContext=new MLContext();

        }

        public void Train(List<EpilepsyData> trainingData)
        {
            var dataView = mLContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mLContext.Transforms.Conversion.ConvertType(nameof(EpilepsyData.age), outputKind: DataKind.Single)
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(EpilepsyData.weight), outputKind: DataKind.Single))
                .Append(mLContext.Transforms.Conversion.ConvertType(nameof(EpilepsyData.seizure_frequency), outputKind: DataKind.Single))

                .Append(mLContext.Transforms.Categorical.OneHotEncoding("GenderEncoded", nameof(EpilepsyData.gender)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("FamilyHistoryEncoded", nameof(EpilepsyData.family_history)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("BrainInjuriesEncoded", nameof(EpilepsyData.brain_injuries)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("StrokeHistoryEncoded", nameof(EpilepsyData.stroke_history)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("SeizureTypeEncoded", nameof(EpilepsyData.seizure_type)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("DrugOrAlchoolUseEncoded", nameof(EpilepsyData.drug_or_alchool_use)))
                .Append(mLContext.Transforms.Categorical.OneHotEncoding("TumorsOrLesionsEncoded", nameof(EpilepsyData.tumors_or_lesions)))
                .Append(mLContext.Transforms.Concatenate("Features",
                    nameof(EpilepsyData.age),
                    nameof(EpilepsyData.weight),
                    nameof(EpilepsyData.seizure_frequency),

                    "GenderEncoded", "FamilyHistoryEncoded", "BrainInjuriesEncoded", "StrokeHistoryEncoded", "SeizureTypeEncoded", "DrugOrAlchoolUseEncoded", "TumorsOrLesionsEncoded")

                )
                .Append(mLContext.Regression.Trainers.Sdca(
                    labelColumnName: nameof(EpilepsyData.risk),
                    featureColumnName: "Features",
                    maximumNumberOfIterations: 100));

            model = pipeline.Fit(dataView);
        }

        public float Predict(EpilepsyData epilepsyData)
        {
            var predictionEngine = mLContext.Model.CreatePredictionEngine<EpilepsyData, EpilepsyDataPrediction>(model);
            var prediction=predictionEngine.Predict(epilepsyData);
            return prediction.risk;
        }

    }
}