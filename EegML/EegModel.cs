using System;

namespace EegML
{
	public class EegModel
	{
		//public static string MLNetModelPath = @"C:\\Src\\Yijing\\EegML\\EegML.mlnet";

		public EegModel(string strPath)
		{
			EegML.MLNetModelPath = strPath;
		}

		public float Predict(
			float fDelta_TP9,
			float fDelta_AF7,
			float fDelta_AF8,
			float fDelta_TP10,
			float fTheta_TP9,
			float fTheta_AF7,
			float fTheta_AF8,
			float fTheta_TP10,
			float fAlpha_TP9,
			float fAlpha_AF7,
			float fAlpha_AF8,
			float fAlpha_TP10,
			float fBeta_TP9,
			float fBeta_AF7,
			float fBeta_AF8,
			float fBeta_TP10,
			float fGamma_TP9,
			float fGamma_AF7,
			float fGamma_AF8,
			float fGamma_TP10)
		{
			var sampleData = new EegML.ModelInput()
			{
				//Delta_TP9 = fDelta_TP9,
				Delta_AF7 = fDelta_AF7,
				Delta_AF8 = fDelta_AF8,
				Delta_TP10 = fDelta_TP10,
				Theta_TP9 = fTheta_TP9,
				Theta_AF7 = fTheta_AF7,
				Theta_AF8 = fTheta_AF8,
				Theta_TP10 = fTheta_TP10,
				Alpha_TP9 = fAlpha_TP9,
				Alpha_AF7 = fAlpha_AF7,
				Alpha_AF8 = fAlpha_AF8,
				Alpha_TP10 = fAlpha_TP10,
				Beta_TP9 = fBeta_TP9,
				Beta_AF7 = fBeta_AF7,
				Beta_AF8 = fBeta_AF8,
				Beta_TP10 = fBeta_TP10,
				Gamma_TP9 = fGamma_TP9,
				Gamma_AF7 = fGamma_AF7,
				Gamma_AF8 = fGamma_AF8,
				Gamma_TP10 = fGamma_TP10,
			};
			var result = EegML.Predict(sampleData);
			return result.Score;
		}

		void test()
		{
			//Load sample data
			var sampleData = new EegML.ModelInput()
			{
				//Delta_TP9 = 0.645F,
				Delta_AF7 = 0.064F,
				Delta_AF8 = 0.819F,
				Delta_TP10 = 0.163F,
				Theta_TP9 = 0.201F,
				Theta_AF7 = 0.038F,
				Theta_AF8 = 0.515F,
				Theta_TP10 = 0.229F,
				Alpha_TP9 = 0.254F,
				Alpha_AF7 = -0.326F,
				Alpha_AF8 = 0.35F,
				Alpha_TP10 = 0.056F,
				Beta_TP9 = 0.268F,
				Beta_AF7 = -0.022F,
				Beta_AF8 = -0.076F,
				Beta_TP10 = 0.137F,
				Gamma_TP9 = -0.25F,
				Gamma_AF7 = -0.535F,
				Gamma_AF8 = -0.461F,
				Gamma_TP10 = -0.071F,
			};

			//Load model and predict output
			var result = EegML.Predict(sampleData);

		}
	}
}
