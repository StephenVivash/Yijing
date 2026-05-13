using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Newtonsoft.Json.Linq;

using CortexAccess;
using EegML;

using Yijing.Views;
using YijingData;

namespace Yijing.Services;

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

public class EegChannel
{
	public ArrayList m_alAverage = new();
	public float m_fCurrentValue = 0.0f;
	//public float m_fMinValue = 0.0f;
	//public float m_fMaxValue = 0.0f;

	public float m_fInitialLow = 0.0f;
	public float m_fInitialHigh = 0.0f;
	public float m_fLow = 0.0f;
	public float m_fHigh = 0.0f;
	public float m_fDifference = 0.0f;

	public bool m_isTrigger = false;

	public bool m_isInitialised = false;

	public static int m_nAverageMax = 100;

	public EegChannel(int nAverageMax)
	{
		m_nAverageMax = nAverageMax;
		m_alAverage.Clear();
		m_isInitialised = false;
		InitialseChannel(0);
	}

	public void InitialseChannel(float f)
	{
		m_alAverage.Clear();
		for (int j = 0; j < m_nAverageMax; ++j)
			m_alAverage.Add(f);
		m_isInitialised = true;
	}
}

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

public class Eeg
{
	protected static EegView m_vwEeg;

	public DateTime m_dtEegStart;
	public bool m_bConnected = false;
	public bool m_bBadData = false;
	public const int m_nChannelMax = 25;

	public int m_nSummaryCount = 0;
	public bool m_bCancelReplay = false;
	public int m_nReplaySpeed = 1;

	public bool m_bLive = false;

	DateTime _dtAiUpdate = DateTime.Now;
	DateTime _dtSoundingUpdate = DateTime.Now;
	DateTime _dtTimerUpdate = DateTime.Now;

	public EegChannel[] m_eegChannel = new EegChannel[m_nChannelMax];

	public FileStream m_fsEmotiv = null;
	public FileStream m_fsMuse = null;

	public string m_strMuseData = ",800.000,800.000,800.000,800.000,800.000,0.000000,0.000000,0.000000,0.000000,0.000000,0.000000,0,0,0,0.000,1,1.0,1.0,1.0,1.0,0.00,";
	public string m_strMuseHeader = "TimeStamp,Delta_TP9,Delta_AF7,Delta_AF8,Delta_TP10,Theta_TP9,Theta_AF7,Theta_AF8,Theta_TP10,Alpha_TP9,Alpha_AF7,Alpha_AF8,Alpha_TP10,Beta_TP9,Beta_AF7,Beta_AF8,Beta_TP10,Gamma_TP9,Gamma_AF7,Gamma_AF8,Gamma_TP10,RAW_TP9,RAW_AF7,RAW_AF8,RAW_TP10,AUX_RIGHT,Accelerometer_X,Accelerometer_Y,Accelerometer_Z,Gyro_X,Gyro_Y,Gyro_Z,PPG_Ambient,PPG_IR,PPG_Red,Heart_Rate,HeadBandOn,HSI_TP9,HSI_AF7,HSI_AF8,HSI_TP10,Battery,Elements\n";

	public static string m_strPrediction;

	private EegModel m_mlEEG = null;

	public static void SetEegView(EegView eeg)
	{
		m_vwEeg = eeg;
	}

	public bool BadData()
	{
		return false; //  m_bBadData;
	}

	public void Initialise(bool bLive)
	{
		m_bLive = bLive;
		m_dtEegStart = DateTime.Now;
		m_nSummaryCount = 0;

		UI.Call<EegView>(v => v.EnableEegControls(false, bLive));
		InitialiseChannels();

		_dtAiUpdate = DateTime.Now;
		_dtSoundingUpdate = DateTime.Now;
		_dtTimerUpdate = DateTime.Now.AddSeconds(5);

		string str = Path.Combine(AppSettings.DocumentHome(), "Models");
		if (!Directory.Exists(str))
			Directory.CreateDirectory(str);
		str = Path.Combine(str, "Stephen V.mlnet"); // "John D.mlnet"

		if (File.Exists(str))
			m_mlEEG = new(str);
		else
			m_mlEEG = null;
	}

	public virtual void InitialiseChannels()
	{
		int max = 0;
		UI.Try<EegView>(v => max = v.m_nEegMode == (int)eEegMode.eSummary ? 400 : 200);
		max = m_vwEeg.m_nEegMode == (int)eEegMode.eSummary ? 400 : 200;
		for (int i = 0; i < m_nChannelMax; ++i)
			m_eegChannel[i] = new EegChannel(max);
		UI.Call<EegView>(v => v.InitialseTriggers());
		UI.Call<EegView>(v => v.InitialiseChart());
	}

	public void SetTriggers(bool bHigh, bool bLow)
	{
		for (int i = 0; i < m_nChannelMax; ++i)
			if (m_eegChannel[i].m_isTrigger)
			{
				if (bLow)
					m_eegChannel[i].m_fLow = m_eegChannel[i].m_fInitialLow;
				if (bHigh)
					m_eegChannel[i].m_fHigh = m_eegChannel[i].m_fInitialHigh;
			}
	}

	public void DecreaseTriggers(float amount)
	{
		for (int i = 0; i < m_nChannelMax; ++i)
			if (m_eegChannel[i].m_isTrigger)
			{
				float d = m_eegChannel[i].m_fLow - amount;
				///////////////////if (d < 0.0f) d = 0.0f;
				m_eegChannel[i].m_fLow = d;
				d = m_eegChannel[i].m_fHigh - amount;
				///////////////////if (d < 0.5f) d = 0.5f;
				m_eegChannel[i].m_fHigh = d;
			}
	}

	public void IncreaseTriggers(float amount)
	{
		for (int i = 0; i < m_nChannelMax; ++i)
			if (m_eegChannel[i].m_isTrigger)
			{
				m_eegChannel[i].m_fLow = m_eegChannel[i].m_fLow + amount;
				m_eegChannel[i].m_fHigh = m_eegChannel[i].m_fHigh + amount;
			}
	}

	public void CalculateTriggers()
	{
		for (int i = 0; i < m_nChannelMax; ++i)
			if (m_eegChannel[i].m_isTrigger)
				if (m_eegChannel[i].m_fLow + m_eegChannel[i].m_fDifference > m_eegChannel[i].m_fHigh) // 1.0f
				{
					///////////////////m_eegChannel[i].m_fInitialHigh = m_eegChannel[i].m_fLow + m_eegChannel[i].m_fDifference; // 1.0f;
					///////////////////m_eegChannel[i].m_fInitialLow = m_eegChannel[i].m_fLow;
				}
	}

	public bool IsTriggerOn()
	{
		bool bAnd = true;
		bool b = bAnd;
		if (BadData())
			return false;
		for (int i = 0; i < m_nChannelMax; ++i)
			if (m_eegChannel[i].m_isTrigger)
				if (bAnd)
					b &= m_eegChannel[i].m_fCurrentValue >= m_eegChannel[i].m_fHigh;
				else
					b |= m_eegChannel[i].m_fCurrentValue >= m_eegChannel[i].m_fHigh;
		return b;
	}

	public bool IsTriggerOff()
	{
		bool bAnd = true;
		bool b = bAnd;
		if (BadData())
			return false;
		for (int i = 0; i < m_nChannelMax; ++i)
			if (m_eegChannel[i].m_isTrigger)
				if (bAnd)
					b &= m_eegChannel[i].m_fCurrentValue <= m_eegChannel[i].m_fLow;
				else
					b |= m_eegChannel[i].m_fCurrentValue <= m_eegChannel[i].m_fLow;
		return b;
	}

	protected void UpdateData(ArrayList data, bool bSummary)
	{
		//int nBadData = 0;
		for (int i = 0; i < Eeg.m_nChannelMax; i++)
		{
			float sum = 0;
			float f = (float)data[i + 1];

			if (AppPreferences.EegDevice == (int)eEegDevice.eMuse)
			{
				// Clean bad Muse data
				
				if (((i >= 0) && (i <= 4)) && (f > 0.3f)) // Delta
					f = 0.3f;
				else
				if ((i >= 5) && (i <= 9) && (f > 0.3f)) // Theta
					f = 0.3f;
				else
				if (((i >= 10) && (i <= 14)) && (f > 0.8f)) // Alpha 
					f = 0.8f;
				else
				if (f > 3.0f)
					f = 3.0f;

				if (f < -0.6f)
					f = -0.6f;
				
			}
			else
			{
				// Clean bad Emotiv data

				if (((i >= 0) && (i <= 4)) || ((i >= 15) && (i <= 24))) // BetaL BetaH Gamma
				{
					f /= 4.0f;
					if (f > 2.0f)
						f = 2.0f;
				}
				else
					if ((i >= 10) && (i <= 14)) // Alpha
					{
						f /= 10.0f;
						if (f > 1.0f)
							f = 1.0f;
					}
					else
					{
						f /= 10.0f;
						if (f > 0.6f)
							f = 0.6f;
					}
			}

			if (!m_eegChannel[i].m_isInitialised)
				m_eegChannel[i].InitialseChannel(f);

			m_eegChannel[i].m_alAverage.Insert(0, f);
			m_eegChannel[i].m_alAverage.RemoveAt(EegChannel.m_nAverageMax);
			for (int j = 0; j < EegChannel.m_nAverageMax; ++j)
				sum += (float)m_eegChannel[i].m_alAverage[j];
			float f1 = sum / EegChannel.m_nAverageMax;

			if (AppPreferences.RawData)
				m_eegChannel[i].m_fCurrentValue = f;// (float)data[i + 1] * museScale;
			else
				m_eegChannel[i].m_fCurrentValue = f1;

			//if (f < m_eegChannel[i].m_fMinValue)
			//	m_eegChannel[i].m_fMinValue = f;
			//if (f > m_eegChannel[i].m_fMaxValue)
			//	m_eegChannel[i].m_fMaxValue = f;

			//if (f >= 8.0)
			//	++nBadData;
		}
		//m_bBadData = nBadData > 3;

		if (AppPreferences.TriggerSounding && ((AppPreferences.EegGoal == (int)eGoal.eMeditation) ||
			// or MindCast is finished
			((AppPreferences.EegGoal == (int)eGoal.eYijingCast) && (AppPreferences.DiagramMode == (int)eDiagramMode.eExplore))))
		{
			DateTime Timestamp = DateTime.Now;
			TimeSpan TimeDiff = Timestamp - _dtSoundingUpdate;
			if (TimeDiff.TotalSeconds >= 10)
			{
				_dtSoundingUpdate = Timestamp;
				DiagramView.SoundTrigger(m_eegChannel[AppSettings.TriggerIndex].m_fCurrentValue,
					m_eegChannel[AppSettings.TriggerIndex].m_fHigh);
			}
		}

		if ((AppPreferences.Timer != (int)eTimer.eNone)) // m_bLive
		{
			DateTime Timestamp1 = DateTime.Now;
			TimeSpan TimeDiff1 = Timestamp1 - _dtTimerUpdate;
			if (TimeDiff1.TotalMinutes >= MinuteTimer())
			{
				_dtTimerUpdate = Timestamp1;
				UI.Call<EegView>(v => v.PlayTimer());
			}
		}

		float prediction = 0.0f;
		if (!bSummary && (m_mlEEG != null) && (AppPreferences.AiEegMlModel != (int)eAiEegMlModel.eNone))
		{
			prediction = m_mlEEG.Predict(
				m_eegChannel[0].m_fCurrentValue, m_eegChannel[1].m_fCurrentValue, m_eegChannel[3].m_fCurrentValue, m_eegChannel[4].m_fCurrentValue,
				m_eegChannel[5].m_fCurrentValue, m_eegChannel[6].m_fCurrentValue, m_eegChannel[8].m_fCurrentValue, m_eegChannel[9].m_fCurrentValue,
				m_eegChannel[10].m_fCurrentValue, m_eegChannel[11].m_fCurrentValue, m_eegChannel[13].m_fCurrentValue, m_eegChannel[14].m_fCurrentValue,
				m_eegChannel[15].m_fCurrentValue, m_eegChannel[16].m_fCurrentValue, m_eegChannel[18].m_fCurrentValue, m_eegChannel[19].m_fCurrentValue,
				m_eegChannel[20].m_fCurrentValue, m_eegChannel[21].m_fCurrentValue, m_eegChannel[23].m_fCurrentValue, m_eegChannel[24].m_fCurrentValue);
			m_strPrediction = $"({prediction * 100,1:#0}%)";
		}
		else
			m_strPrediction = "";

		if (!bSummary || (++m_nSummaryCount % 100 == 0))
		{
			if (!bSummary && (m_bLive || (m_nReplaySpeed == 1)) && !AiPreferences.IsNoneService(AppPreferences.AiEegService))
			{
				DateTime Timestamp = DateTime.Now;
				TimeSpan TimeDiff = Timestamp - _dtAiUpdate;
				if (TimeDiff.TotalSeconds >= 120)
				{
					_dtAiUpdate = Timestamp;
					TimeDiff = Timestamp - m_dtEegStart;
					String str = $"Minute={TimeDiff.TotalMinutes,1:#0}, Prediction={prediction * 100,1:#0.000}, " +
						$"Delta_TP9={m_eegChannel[0].m_fCurrentValue,1:#0.000}, " +
						$"Delta_AF7={m_eegChannel[1].m_fCurrentValue,1:#0.000}, " +
						$"Delta_AF8={m_eegChannel[3].m_fCurrentValue,1:#0.000}, " +
						$"Delta_TP10={m_eegChannel[4].m_fCurrentValue,1:#0.000}, " +
						$"Theta_TP9={m_eegChannel[5].m_fCurrentValue,1:#0.000}, " +
						$"Theta_AF7={m_eegChannel[6].m_fCurrentValue,1:#0.000}, " +
						$"Theta_AF8={m_eegChannel[8].m_fCurrentValue,1:#0.000}, " +
						$"Theta_TP10={m_eegChannel[9].m_fCurrentValue,1:#0.000}, " +
						$"Alpha_TP9={m_eegChannel[10].m_fCurrentValue,1:#0.000}, " +
						$"Alpha_AF7={m_eegChannel[11].m_fCurrentValue,1:#0.000}, " +
						$"Alpha_AF8={m_eegChannel[13].m_fCurrentValue,1:#0.000}, " +
						$"Alpha_TP10={m_eegChannel[14].m_fCurrentValue,1:#0.000}, " +
						$"Beta_TP9={m_eegChannel[15].m_fCurrentValue,1:#0.000}, " +
						$"Beta_AF7={m_eegChannel[16].m_fCurrentValue,1:#0.000}, " +
						$"Beta_AF8={m_eegChannel[18].m_fCurrentValue,1:#0.000}, " +
						$"Beta_TP10={m_eegChannel[19].m_fCurrentValue,1:#0.000}, " +
						$"Gamma_TP9={m_eegChannel[20].m_fCurrentValue,1:#0.000}, " +
						$"Gamma_AF7={m_eegChannel[21].m_fCurrentValue,1:#0.000}, " +
						$"Gamma_AF8={m_eegChannel[23].m_fCurrentValue,1:#0.000}, " +
						$"Gamma_TP10={m_eegChannel[24].m_fCurrentValue,1:#0.000}";
					UI.Call<EegView>(v => v.AiData(str));
				}
			}
			else
				_dtAiUpdate = DateTime.Now;
			UI.Call<EegView>(v => v.UpdateData());
		}
	}
	public int MinuteTimer()
	{
		switch (AppPreferences.Timer)
		{
			case (int)eTimer.eTen:
				return 10;
			case (int)eTimer.eFifteen:
				return 15;
			case (int)eTimer.eTwenty:
				return 20;
			case (int)eTimer.eThirty:
				return 30;
			case (int)eTimer.eSixty:
				return 60;
		}
		return 0;
	}

	public void OpenFileStreams(bool bEmotiv, bool bMuse)
	{
		DateTime start = DateTime.Now;
		if (bEmotiv)
		{
			string strEmotiv = $"{start.Year}-{start.Month,2:#00}-{start.Day,2:#00}-{start.Hour,2:#00}-{start.Minute,2:#00}-{start.Second,2:#00}";
			EegView._strSession = strEmotiv;
			strEmotiv += "-Emotiv.csv";
			strEmotiv = Path.Combine(AppSettings.EegDataHome(), strEmotiv);
			m_fsEmotiv = new FileStream(strEmotiv, FileMode.Create, FileAccess.Write);
		}
		if (bMuse)
		{
			string strMuse = $"{start.Year}-{start.Month,2:#00}-{start.Day,2:#00}-{start.Hour,2:#00}-{start.Minute,2:#00}-{start.Second,2:#00}";
			EegView._strSession = strMuse;
			strMuse += "-Muse.csv";
			strMuse = Path.Combine(AppSettings.EegDataHome(), strMuse);
			m_fsMuse = new FileStream(strMuse, FileMode.Create, FileAccess.Write);
			byte[] val = Encoding.UTF8.GetBytes(m_strMuseHeader);
			m_fsMuse.Write(val, 0, val.Length);
		}
	}

	public virtual bool Connect()
	{
		m_bConnected = true;
		return true;
	}

	public virtual void Disconnect()
	{
		m_bConnected = false;
		CloseStreams();
	}

	public virtual void Replay(string file)
	{
		m_bConnected = true;
	}

	public virtual void Summary(string file) { }

	public void WriteFile(FileStream stream, ArrayList data, bool header, bool emotiv)
	{
		byte[] val;
		if (stream != null)
		{
			/*
			if (m_strDebug.Length > 0)
			{
				val = Encoding.UTF8.GetBytes(data[0].ToString() + ",,");
				stream.Write(val, 0, val.Length);
				val = Encoding.UTF8.GetBytes(m_strDebug);
				stream.Write(val, 0, val.Length);
				m_strDebug = "";
			}
			*/
			for (int i = 0; i < data.Count; i++)
			{
				string str;
				if (!emotiv && ((i == 3) || (i == 8) || (i == 13) || (i == 18) || (i == 23)))
					continue;
				if (header || (i == 0))
					str = data[i].ToString();
				else
					str = $"{(float)data[i],1:#0.000}";
				val = Encoding.UTF8.GetBytes(str + (i == data.Count - 1 ? "" : ","));
				stream.Write(val, 0, val.Length);
			}
			if (!emotiv)
			{
				val = Encoding.UTF8.GetBytes(m_strMuseData);
				stream.Write(val, 0, val.Length);
			}
			val = Encoding.UTF8.GetBytes("\n");
			stream.Write(val, 0, val.Length);
		}
	}

	public void CloseStreams()
	{
		if (m_fsEmotiv != null)
		{
			long l = m_fsEmotiv.Length;
			m_fsEmotiv.Close();
			m_fsEmotiv.Dispose();
			if (l < 200_000)
				File.Delete(m_fsEmotiv.Name);
		}
		if (m_fsMuse != null)
		{
			long l = m_fsMuse.Length;
			m_fsMuse.Close();
			m_fsMuse.Dispose();
			if (l < 200_000)
				File.Delete(m_fsMuse.Name);
		}
		m_fsEmotiv = null;
		m_fsMuse = null;
	}
}

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

public class MuseEeg : Eeg
{
	private EndPoint m_epMuse = null;
	private Socket m_socMuse = null;
	private Task m_tskMuse = null;
	private CancellationTokenSource m_ctsMuseBt = null;
#if DEBUG
	private readonly object m_oMuseDebugLogLock = new();
	private string m_strMuseDebugLog = "";
#endif
	private ArrayList m_alMuseData = new();

	public override void InitialiseChannels()
	{
		base.InitialiseChannels();

		m_alMuseData.Clear();
		m_alMuseData.Add(0.0f);
		for (int i = 0; i < m_nChannelMax; ++i)
			m_alMuseData.Add(0.0f);
	}

	public override bool Connect()
	{
		// if (m_socMuse == null) // don't abandon MM OSC yet
		if ((m_tskMuse == null) || m_tskMuse.IsCompleted)
		{
			Initialise(true);
			OpenFileStreams(false, true);
			StartMuseDebugLog();
			m_ctsMuseBt?.Cancel();
			m_ctsMuseBt = new CancellationTokenSource();
			bool connected = base.Connect();

			if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
				UI.Call<DiagramView>(v => v.SetDiagramMode(eDiagramMode.eMindCast));

			CancellationToken token = m_ctsMuseBt.Token;
			m_tskMuse = ReceiveMuseDataAsync(token);
			UI.Call<EegView>(v => v.SetAppTitle("Starting Muse BT - Yijing"));
			return connected;
		}
		return base.Connect();
	}

	public override void Disconnect()
	{
		m_ctsMuseBt?.Cancel();
		if (m_socMuse != null)
		{
			UI.Call<EegView>(v => v.EnableEegControls(true, true));
			m_socMuse.Close();
			m_socMuse = null;
		}
		m_tskMuse = null;
		base.Disconnect();
	}

	public override async void Replay(string file)
	{
		base.Replay(file);
		Initialise(false);
		using (StreamReader sr = File.OpenText(file))
		{
			float f = 0;
			ArrayList data = new() { 0.1f, 0.2f };
			DateTime Timestamp = DateTime.Now;
			DateTime FirstTimestamp = DateTime.Now;
			DateTime LastTimestamp = DateTime.Now;
			TimeSpan TimeDiff;
			UI.Call<EegView>(v => v.SetAppTitle(Path.GetFileName(file) + " - Yijing"));

			if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
				UI.Call<DiagramView>(v => v.SetDiagramMode(eDiagramMode.eMindCast));

			string s1 = sr.ReadLine();
			if ((s1 = sr.ReadLine()) != null)
			{
				string[] s2 = s1.Split(",", 2);
				try { FirstTimestamp = LastTimestamp = DateTime.Parse(s2[0]); } catch { }
				while (((s1 = sr.ReadLine()) != null) && !m_bCancelReplay)
				{

					AppSettings._lastEegDataTime = DateTime.Now;

					s2 = s1.Split(",");
					if (s2[1].Length > 0)
					{
						data.Clear();
						data.Add(0.0f);
						for (int i = 1; i <= Eeg.m_nChannelMax - 5; i++)
						{
							try { f = float.Parse(s2[i]); } catch { f = 0; }
							data.Add(f);
							if ((i == 2) || (i == 6) || (i == 10) || (i == 14) || (i == 18))
								data.Add(0.0f);
						}
						try { Timestamp = DateTime.Parse(s2[0]); } catch { }
						TimeDiff = Timestamp - LastTimestamp;
						LastTimestamp = Timestamp;
						UpdateData(data, false);
						UI.Call<EegView>(v => v.UpdateTime(Timestamp));
						if (m_nReplaySpeed == 1)
							await Task.Delay((int)TimeDiff.TotalMilliseconds);
						else
#if DEBUG
							await Task.Delay((int)(TimeDiff.TotalMilliseconds / m_nReplaySpeed * 0.42));
#else
							await Task.Delay((int)(TimeDiff.TotalMilliseconds / m_nReplaySpeed * 0.32));
#endif
					}
				}
			}
			UI.Call<EegView>(v => v.UpdateTime(FirstTimestamp, LastTimestamp));
		}
		m_bConnected = false;
		UI.Call<EegView>(v => v.EnableEegControls(true, true));
	}

	public override void Summary(string file)
	{
		base.Summary(file);
		Initialise(false);
		using (StreamReader sr = File.OpenText(file))
		{
			float f = 0;
			ArrayList data = new() { 0.1f, 0.2f };
			DateTime FirstTimestamp = DateTime.Now;
			DateTime LastTimestamp = DateTime.Now;
			UI.Call<EegView>(v => v.SetAppTitle(Path.GetFileName(file) + " - Yijing"));

			string s1 = sr.ReadLine();
			if ((s1 = sr.ReadLine()) != null)
			{
				string[] s2 = s1.Split(",", 2);
				try { FirstTimestamp = LastTimestamp = DateTime.Parse(s2[0]); } catch { }
				while (((s1 = sr.ReadLine()) != null) && !m_bCancelReplay)
				{
					s2 = s1.Split(",");
					if (s2[1].Length > 0)
					{
						data.Clear();
						data.Add(0.0f);
						for (int i = 1; i <= Eeg.m_nChannelMax - 5; i++)
						{
							try { f = float.Parse(s2[i]); } catch { f = 0; }
							data.Add(f);
							if ((i == 2) || (i == 6) || (i == 10) || (i == 14) || (i == 18))
								data.Add(0.0f);
						}
						try { LastTimestamp = DateTime.Parse(s2[0]); } catch { }
						UpdateData(data, true);
					}
				}
				UI.Call<EegView>(v => v.UpdateTime(FirstTimestamp, LastTimestamp));
			}
		}
	}

	private Task ReceiveMuseDataAsync(CancellationToken token)
	{
		return ReceiveBTDataAsync(token);
		//ReceiveMMData();
	}

	private void StartMuseDebugLog()
	{
#if DEBUG
		lock (m_oMuseDebugLogLock)
		{
			try
			{
				CreateMuseDebugLog(AppSettings.ReverseDateString() + ".txt");
			}
			catch
			{
				m_strMuseDebugLog = "";
			}
		}

		LogMuseDebug(string.IsNullOrWhiteSpace(m_strMuseDebugLog)
			? "Full debug log unavailable."
			: "Full debug log: " + m_strMuseDebugLog);
#endif
	}

	private void LogMuseDebug(string message)
	{
#if DEBUG
		string entry = $"[{DateTimeOffset.Now:HH:mm:ss.fff}] {message}";
		lock (m_oMuseDebugLogLock)
		{
			if (string.IsNullOrWhiteSpace(m_strMuseDebugLog))
				return;

			try
			{
				AppendMuseDebugLog(entry);
			}
			catch
			{
				m_strMuseDebugLog = "";
			}
		}
#endif
	}

#if DEBUG
	private void CreateMuseDebugLog(string fileName)
	{
		string directory = AppSettings.LogHome();
		Directory.CreateDirectory(directory);
		m_strMuseDebugLog = Path.Combine(directory, fileName);
		File.WriteAllText(m_strMuseDebugLog, "");
	}

	private void AppendMuseDebugLog(string entry)
	{
		File.AppendAllText(m_strMuseDebugLog, entry + Environment.NewLine);
	}
#endif

	private async Task<bool> EnsureBluetoothPermissionsAsync()
	{
#if ANDROID
		var bluetoothStatus = await Permissions.RequestAsync<Permissions.Bluetooth>();
		if (bluetoothStatus != PermissionStatus.Granted)
			return false;
		if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.Version.Major < 12)
		{
			var locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
			if (locationStatus != PermissionStatus.Granted)
				return false;
		}
		return true;
#else
		return await Task.FromResult(true);
#endif
	}

	private async Task ReceiveBTDataAsync(CancellationToken token)
	{
		bool abs = true;
		// Experimental BT calibration: first two minutes are treated as eyes-open baseline.
		bool baselineNormalise = true;
		TimeSpan baselineDuration = TimeSpan.FromMinutes(2);

		async Task ReceiveAsync()
		{
			int count = 0;
			bool bTitle = false;
			bool errorReported = false;
			object dataLock = new();
			string lastDiagnostic = "";
			DateTime? baselineStart = null;
			bool baselineFrozen = false;
			float[] rawBandData = new float[m_nChannelMax];
			bool[] rawBandSeen = new bool[m_nChannelMax];
			double[] baselineSums = new double[m_nChannelMax];
			int[] baselineCounts = new int[m_nChannelMax];
			float[] baselineValues = new float[m_nChannelMax];
			const double rawOffsetMicrovolts = 800.0;
			const double accelerometerScale = 1.0 / 16384.0;
			const double gyroScale = 245.0 / 32768.0;
			double rawTp9 = rawOffsetMicrovolts;
			double rawAf7 = rawOffsetMicrovolts;
			double rawAf8 = rawOffsetMicrovolts;
			double rawTp10 = rawOffsetMicrovolts;
			double rawAux = rawOffsetMicrovolts;
			double accelerometerX = 0.0;
			double accelerometerY = 0.0;
			double accelerometerZ = 0.0;
			double gyroX = 0.0;
			double gyroY = 0.0;
			double gyroZ = 0.0;
			double ppgAmbient = 0.0;
			double ppgIr = 0.0;
			double ppgRed = 0.0;
			double heartRate = 0.0;
			int headBandOn = 1;
			double hsiTp9 = 1.0;
			double hsiAf7 = 1.0;
			double hsiAf8 = 1.0;
			double hsiTp10 = 1.0;
			double battery = 0.0;

			void SetRawBand(int dataIndex, double value)
			{
				int channelIndex = dataIndex - 1;
				rawBandData[channelIndex] = (float)value;
				rawBandSeen[channelIndex] = true;
			}

			static double AverageAxis(short[] samples, int axis)
			{
				double sum = 0.0;
				int count = 0;
				for (int i = axis; i < samples.Length; i += 3)
				{
					sum += samples[i];
					count++;
				}
				return count == 0 ? 0.0 : sum / count;
			}

			void UpdateMuseExtraData()
			{
				m_strMuseData = FormattableString.Invariant($",{rawTp9:F3},{rawAf7:F3},{rawAf8:F3},{rawTp10:F3},{rawAux:F3},{accelerometerX:F6},{accelerometerY:F6},{accelerometerZ:F6},{gyroX:F6},{gyroY:F6},{gyroZ:F6},{ppgAmbient:F0},{ppgIr:F0},{ppgRed:F0},{heartRate:F3},{headBandOn},{hsiTp9:F1},{hsiAf7:F1},{hsiAf8:F1},{hsiTp10:F1},{battery:F2},");
			}

			void PopulateMuseData(DateTime now)
			{
				baselineStart ??= now;

				if (baselineNormalise && !baselineFrozen && (now - baselineStart.Value) >= baselineDuration)
					baselineFrozen = true;

				for (int i = 0; i < m_nChannelMax; i++)
				{
					if (!rawBandSeen[i])
					{
						m_alMuseData[i + 1] = 0.0f;
						continue;
					}

					if (baselineNormalise)
					{
						if (!baselineFrozen)
						{
							baselineSums[i] += rawBandData[i];
							baselineCounts[i]++;
							baselineValues[i] = (float)(baselineSums[i] / baselineCounts[i]);
						}

						m_alMuseData[i + 1] = rawBandData[i] - baselineValues[i];
					}
					else
						m_alMuseData[i + 1] = rawBandData[i];
				}
			}

			if (!await EnsureBluetoothPermissionsAsync())
			{
				const string message = "Muse BT needs Bluetooth permission, and location permission on older Android versions, before it can scan.";
				UI.Call<EegView>(v => v.SetAppTitle("Muse BT permission denied - Yijing"));
				UI.Call<EegView>(v => v.ShowError("Muse BT", message));
				return;
			}

			AppSettings._lastEegDataTime = DateTime.Now;

			if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
				UI.Call<DiagramView>(v => v.SetDiagramMode(eDiagramMode.eMindCast));
			var client = new Muse.Core.MuseBtClient();
			client.DiagnosticMessage += (_, message) =>
			{
				lastDiagnostic = message;
				LogMuseDebug(message);
			};
			client.InfoMessage += (_, message) =>
			{
				LogMuseDebug(message);
				UI.Call<EegView>(v => v.SetAppTitle(message + " - Yijing"));
			};
			client.ConnectionStatusChanged += (_, status) =>
			{
				LogMuseDebug("Connection status: " + status);
				UI.Call<EegView>(v => v.SetAppTitle("Muse BT " + status + " - Yijing"));
			};
			client.NotificationReceived += (_, notification) =>
			{
				lock (dataLock)
				{
					if (notification.Kind == Muse.Core.MuseSensorKind.Eeg &&
						Muse.Core.MusePacketDecoder.TryDecodeEeg(notification.Data, out var eegPacket))
					{
						double raw = rawOffsetMicrovolts + eegPacket.Samples.Average();
						switch (notification.Name)
						{
							case "EEG TP9":
								rawTp9 = raw;
								break;
							case "EEG AF7":
								rawAf7 = raw;
								break;
							case "EEG AF8":
								rawAf8 = raw;
								break;
							case "EEG TP10":
								rawTp10 = raw;
								break;
							case "EEG AUX":
								rawAux = raw;
								break;
						}
						return;
					}

					if (notification.Kind == Muse.Core.MuseSensorKind.Imu &&
						Muse.Core.MusePacketDecoder.TryDecodeImu(notification.Data, out var imuPacket))
					{
						double x = AverageAxis(imuPacket.Samples, 0);
						double y = AverageAxis(imuPacket.Samples, 1);
						double z = AverageAxis(imuPacket.Samples, 2);
						if (notification.Name == "Accelerometer")
						{
							accelerometerX = x * accelerometerScale;
							accelerometerY = y * accelerometerScale;
							accelerometerZ = z * accelerometerScale;
						}
						else if (notification.Name == "Gyroscope")
						{
							gyroX = x * gyroScale;
							gyroY = y * gyroScale;
							gyroZ = z * gyroScale;
						}
						return;
					}

					if (notification.Kind == Muse.Core.MuseSensorKind.Telemetry && notification.Data.Length >= 4)
					{
						battery = Muse.Core.MusePacketDecoder.ReadUInt16BigEndian(notification.Data, 2) / 512.0;
						return;
					}

					if (notification.Kind == Muse.Core.MuseSensorKind.Raw &&
						Muse.Core.MusePacketDecoder.TryDecodePpg(notification.Data, out var ppgPacket))
					{
						double ppg = ppgPacket.Samples.Sum(value => (double)value);
						switch (notification.Name)
						{
							case "PPG ambient":
								ppgAmbient = ppg;
								break;
							case "PPG IR":
								ppgIr = ppg;
								break;
							case "PPG red":
								ppgRed = ppg;
								break;
						}
					}
				}
			};
			client.BandPowersCalculated += (_, reading) =>
			{
				int channelOffset = reading.SensorName switch
				{
					"EEG TP9" => 1,
					"EEG AF7" => 2,
					"EEG AF8" => 4,
					"EEG TP10" => 5,
					_ => 0
				};
				if (channelOffset == 0)
					return;

				lock (dataLock)
				{
					AppSettings._lastEegDataTime = DateTime.Now;

					SetRawBand(channelOffset, abs ? reading.Bands.DeltaAbsolute : reading.Bands.DeltaDb);
					SetRawBand(channelOffset + 5, abs ? reading.Bands.ThetaAbsolute : reading.Bands.ThetaDb);
					SetRawBand(channelOffset + 10, abs ? reading.Bands.AlphaAbsolute : reading.Bands.AlphaDb);
					SetRawBand(channelOffset + 15, abs ? reading.Bands.BetaAbsolute : reading.Bands.BetaDb);
					SetRawBand(channelOffset + 20, abs ? reading.Bands.GammaAbsolute : reading.Bands.GammaDb);

					if (++count % 4 == 0)
					{
						DateTime now = DateTime.Now;
						PopulateMuseData(now);
						string date = $"{now.Year,4:#0000}-{now.Month,2:#00}-{now.Day,2:#00} {now.Hour,2:#00}:{now.Minute,2:#00}:{now.Second,2:#00}.{now.Millisecond,3:#000}";
						m_alMuseData[0] = date;
						UpdateMuseExtraData();
						UpdateData(m_alMuseData, false);
						UI.Call<EegView>(v => v.UpdateTime(now));
						WriteFile(m_fsMuse, m_alMuseData, false, false);
					}
				}

				if (!bTitle)
				{
					bTitle = true;
					UI.Call<EegView>(v => v.SetAppTitle("Received from Muse BT - Yijing"));
				}
			};

			try
			{
				UI.Call<EegView>(v => v.SetAppTitle("Scanning for Muse BT - Yijing"));
				var discovered = await client.FindMuseAsync(TimeSpan.FromSeconds(30), token);
				if (discovered == null)
				{
					errorReported = true;
					UI.Call<EegView>(v => v.SetAppTitle("No Muse BT headset found - Yijing"));
					string message = "No Muse headset was found during the 30 second scan.";
					if (!string.IsNullOrWhiteSpace(lastDiagnostic))
						message += "\n\nLast BT step: " + lastDiagnostic;
#if DEBUG
					if (!string.IsNullOrWhiteSpace(m_strMuseDebugLog))
						message += "\n\nDebug log: " + m_strMuseDebugLog;
#endif
					UI.Call<EegView>(v => v.ShowError("Muse BT", message));
					return;
				}

				UI.Call<EegView>(v => v.SetAppTitle("Connecting to " + discovered.DisplayName + " - Yijing"));
				LogMuseDebug($"Found {discovered.DisplayName} at 0x{discovered.BluetoothAddress:X}.");
				await client.ConnectAndStreamAsync(discovered, token);
			}
			catch (OperationCanceledException) when (token.IsCancellationRequested)
			{
			}
			catch (OperationCanceledException ex)
			{
				errorReported = true;
				UI.Call<EegView>(v => v.ShowError("Muse BT", ex.Message));
			}
			catch (Exception ex)
			{
				errorReported = true;
				string message = string.IsNullOrWhiteSpace(lastDiagnostic)
					? ex.Message
					: ex.Message + "\n\nLast BT step: " + lastDiagnostic;
#if DEBUG
				if (!string.IsNullOrWhiteSpace(m_strMuseDebugLog))
					message += "\n\nDebug log: " + m_strMuseDebugLog;
#endif
				UI.Call<EegView>(v => v.ShowError("Muse BT", message));
			}
			finally
			{
				await client.DisposeAsync();
				if (!bTitle && !errorReported && m_bConnected)
					UI.Call<EegView>(v => v.ShowError("Muse BT", "Muse BT stopped before any EEG band data was received."));
				UI.Call<EegView>(v => v.UpdateTime(m_dtEegStart, DateTime.Now));
				m_tskMuse = null;
			}
		}

		await ReceiveAsync();
	}

	private void ReceiveMMData()
	{
		//int[] ms = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		byte[] buffer = new byte[1024];
		int nSize = 0;
		int count = 0;
		bool bTitle = false;

		AppSettings._lastEegDataTime = DateTime.Now;

		if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
			UI.Call<DiagramView>(v => v.SetDiagramMode(eDiagramMode.eMindCast));

		while (true)
		{
			IPPacketInformation pi;
			SocketFlags sf = SocketFlags.None;
			try { nSize = m_socMuse.ReceiveMessageFrom(buffer, 0, 1024, ref sf, ref m_epMuse, out pi); }
			catch
			{
				UI.Call<EegView>(v => v.UpdateTime(m_dtEegStart, DateTime.Now));
				return;
			}

			if (!bTitle)
			{
				bTitle = true;
				string ip = pi.Address.ToString();
				UI.Call<EegView>(v => v.SetAppTitle("Received from " + ip + " - Yijing"));
			}

			Span<byte> bytes = new Span<byte>(buffer, 0, nSize);
			string s = Encoding.ASCII.GetString(bytes); // UTF8
			if (s.Contains("muse/elements")) // muse/elements /eeg
			{

				AppSettings._lastEegDataTime = DateTime.Now;

				int index = 0;
				if (s.Contains("delta")) index = 0;
				else
					if (s.Contains("theta")) index = 5;
					else
						if (s.Contains("alpha")) index = 10;
						else
							if (s.Contains("beta")) index = 15;
							else
								if (s.Contains("gamma")) index = 20;
								else
									continue;

				Array.Reverse(buffer, 60, 4);
				Array.Reverse(buffer, 64, 4);
				Array.Reverse(buffer, 68, 4);
				Array.Reverse(buffer, 72, 4);

				float f1 = BitConverter.ToSingle(buffer, 60);
				float f2 = BitConverter.ToSingle(buffer, 64);
				float f3 = BitConverter.ToSingle(buffer, 68);
				float f4 = BitConverter.ToSingle(buffer, 72);

				m_alMuseData[index + 1] = f1;
				m_alMuseData[index + 2] = f2;
				m_alMuseData[index + 4] = f3;
				m_alMuseData[index + 5] = f4;
				/*
				index = DateTime.Now.Millisecond / 100;
				if ((index < 10) && (ms[index] > -1))
				{
					ms[index] = -1;
					if (index == 9)
						ms = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
				}
				*/
				if (++count % 7 == 0)
				{
					string date = $"{DateTime.Now.Year,4:#0000}-{DateTime.Now.Month,2:#00}-{DateTime.Now.Day,2:#00} {DateTime.Now.Hour,2:#00}:{DateTime.Now.Minute,2:#00}:{DateTime.Now.Second,2:#00}.{DateTime.Now.Millisecond,3:#000}";
					m_alMuseData[0] = date;
					UpdateData(m_alMuseData, false);
					UI.Call<EegView>(v => v.UpdateTime(DateTime.Now));
					WriteFile(m_fsMuse, m_alMuseData, false, false);
				}
				Thread.Sleep(10);
				/*
				if (s.Contains("muse/eeg"))
				{
					Array.Reverse(buffer1, 40, 4);
					Array.Reverse(buffer1, 44, 4);
					Array.Reverse(buffer1, 48, 4);
					Array.Reverse(buffer1, 52, 4);
					Array.Reverse(buffer1, 56, 4);

					float f1 = BitConverter.ToSingle(buffer1, 40);
					float f2 = BitConverter.ToSingle(buffer1, 44);
					float f3 = BitConverter.ToSingle(buffer1, 48);
					float f4 = BitConverter.ToSingle(buffer1, 52);
					float f5 = BitConverter.ToSingle(buffer1, 56);

					s1 = $" - {f1,1:#0.00000}, {f2,1:#0.00000}, {f3,1:#0.00000}, {f4,1:#0.00000}, {f5,1:#0.00000}";
				}
				*/
			}
		}
	}
}

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

public class EmotivEeg : Eeg
{
	private DataStreamExample m_dsEmotiv = null;

	public override void InitialiseChannels()
	{
		base.InitialiseChannels();
	}

	public override bool Connect()
	{
		if (m_dsEmotiv == null)
		{
			Initialise(true);
			OpenFileStreams(true, false);
			m_dsEmotiv = new DataStreamExample();
			m_dsEmotiv.AddStreams("pow");
			m_dsEmotiv.OnSubscribed += OnSubscribed;
			m_dsEmotiv.OnBandPowerDataReceived += OnBandPowerDataReceived;
			m_dsEmotiv.Start();
		}
		return base.Connect();
	}

	public override void Disconnect()
	{
		if (m_dsEmotiv != null)
		{
			UI.Call<EegView>(v => v.EnableEegControls(true, true));
			m_dsEmotiv.UnSubscribe();
			Thread.Sleep(2000);
			m_dsEmotiv.CloseSession();
			m_dsEmotiv = null;
		}
		base.Disconnect();
	}

	public override async void Replay(string file)
	{
		base.Replay(file);
		Initialise(false);
		using (StreamReader sr = File.OpenText(file))
		{
			float f = 0;
			ArrayList data = new() { 0.1f, 0.2f };
			DateTime Timestamp = DateTime.Now;
			DateTime LastTimestamp = DateTime.Now;
			TimeSpan TimeDiff;
			UI.Call<EegView>(v => v.SetAppTitle(Path.GetFileName(file) + " - Yijing"));

			if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
				UI.Call<DiagramView>(v => v.SetDiagramMode(eDiagramMode.eMindCast));

			string s1 = sr.ReadLine();
			if ((s1 = sr.ReadLine()) != null)
			{
				string[] s2 = s1.Split(",", 2);
				try { LastTimestamp = DateTime.Parse(s2[0]); } catch { }
				while (((s1 = sr.ReadLine()) != null) && !m_bCancelReplay)
				{
					s2 = s1.Split(",");
					if (s2[1].Length > 0)
					{
						data.Clear();
						data.Add(0.0f);
						for (int i = 1; i <= Eeg.m_nChannelMax; i++)
						{
							try { f = float.Parse(s2[i]); } catch { f = 0; }
							data.Add(f);
						}
						try { Timestamp = DateTime.Parse(s2[0]); } catch { }
						TimeDiff = Timestamp - LastTimestamp;
						LastTimestamp = Timestamp;
						UpdateData(data, false);
						UI.Call<EegView>(v => v.UpdateTime(Timestamp));
						await Task.Delay((int)TimeDiff.TotalMilliseconds / m_nReplaySpeed);
					}
				}
			}
		}
		m_bConnected = false;
		UI.Call<EegView>(v => v.EnableEegControls(true, true));
	}

	public override void Summary(string file)
	{
		base.Summary(file);
		Initialise(false);
		using (StreamReader sr = File.OpenText(file))
		{
			float f = 0;
			ArrayList data = new() { 0.1f, 0.2f };
			DateTime FirstTimestamp = DateTime.Now;
			DateTime LastTimestamp = DateTime.Now;
			UI.Call<EegView>(v => v.SetAppTitle(Path.GetFileName(file) + " - Yijing"));
			string s1 = sr.ReadLine();
			if ((s1 = sr.ReadLine()) != null)
			{
				string[] s2 = s1.Split(",", 2);
				try { FirstTimestamp = LastTimestamp = DateTime.Parse(s2[0]); } catch { }
				while (((s1 = sr.ReadLine()) != null) && !m_bCancelReplay)
				{
					s2 = s1.Split(",");
					if (s2[1].Length > 0)
					{
						data.Clear();
						data.Add(0.0f);
						for (int i = 1; i <= Eeg.m_nChannelMax; i++)
						{
							try { f = float.Parse(s2[i]); } catch { f = 0; }
							data.Add(f);
						}
						try { LastTimestamp = DateTime.Parse(s2[0]); } catch { }
						UpdateData(data, true);
					}
				}
				UI.Call<EegView>(v => v.UpdateTime(FirstTimestamp, LastTimestamp));
			}
		}
	}

	/*
	private async void ReadEmotiveExportFile()
	{
		var result = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = fpftCVS,
			PickerTitle = "Pick a .cvs file",
		});
		if ((result != null) && File.Exists(result.FullPath))
		{
			m_eeg.Initialise(false);
			using (StreamReader sr = File.OpenText(result.FullPath))
			{
				float f = 0;
				ArrayList data = new() { 0.1f, 0.2f };
				double Timestamp = 0;
				double LastTimestamp = 0;
				int TimeDiff;
				float SampleRateQuality;
				float Overall;
				string s1 = sr.ReadLine();
				while ((s1 = sr.ReadLine()) != null)
				{
					string[] s2 = s1.Split(",");
					if (s2[60].Length > 0) // 60 BandPower - 30 Performance
					{
						data.Clear();
						data.Add(0.0f);
						for (int i = 60; i < 85; i++) // 60 BandPower s2.Length
													  //for (int i = 30; i < 60; i += 5) // 30 Performance
						{
							try { f = float.Parse(s2[i]); } catch { f = 0; } //  * 10 30 Performance
																			 //if (i == 35) ++i; // 30 Performance
							data.Add(f);
						}

						//for(int j = 0; j < 19; j++)	data.Add(0.1); // 30 Performance

						try { Timestamp = Double.Parse(s2[0]); } catch { }
						try { if (s2[22].Length > 0) SampleRateQuality = float.Parse(s2[22]); } catch { }
						try { if (s2[23].Length > 0) Overall = float.Parse(s2[23]); } catch { }
						TimeDiff = (int)((Timestamp - LastTimestamp) * 1000) + 1;
						if (Math.Abs(TimeDiff) > 600)
							TimeDiff = 600;
						LastTimestamp = Timestamp;
						//ArrayList data1 = Insight2Muse(data,false,true); /////////////////////////////////////////
						UpdateData(data, DateTime.Now);
						await Task.Delay(TimeDiff / m_nReplaySpeed);
					}
				}
			}
		}
		EnableEegControls(true, true);
	}
	*/

	private void OnSubscribed(object sender, System.Collections.Generic.Dictionary<string, JArray> e)
	{

		if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
			UI.Call<DiagramView>(v => v.SetDiagramMode(eDiagramMode.eMindCast));

		foreach (string key in e.Keys)
		{
			if (key == "pow")
			{
				ArrayList data = e[key].ToObject<ArrayList>();
				data.Insert(0, "Timestamp");
				ArrayList data1 = Emotiv2Muse(data, true, true);
				WriteFile(m_fsEmotiv, data1, true, true);

				//byte[] val = Encoding.UTF8.GetBytes(m_strMuseHeader);
				//m_fsMuse.Write(val, 0, val.Length);
			}
		}
	}

	private void OnBandPowerDataReceived(object sender, ArrayList dataxxx)
	{
		ArrayList data = new();
		foreach (var d in dataxxx)
			data.Add((float)(double)d);

		ArrayList data1 = Emotiv2Muse(data, false, true);
		//ArrayList data2 = Emotiv2Muse(data, false, false);

		UpdateData(data1, false);
		UI.Call<EegView>(v => v.UpdateTime(DateTime.Now));
		WriteFile(m_fsEmotiv, data1, false, true);
		//WriteFile(m_fsMuse, data2, false, false);
	}

	private ArrayList Emotiv2Muse(ArrayList data, bool header, bool emotiv)
	{
		if (!header)
		{
			string str = $"{DateTime.Now.Year,4:#0000}-{DateTime.Now.Month,2:#00}-{DateTime.Now.Day,2:#00} {DateTime.Now.Hour,2:#00}:{DateTime.Now.Minute,2:#00}:{DateTime.Now.Second,2:#00}.{DateTime.Now.Millisecond,3:#000}";
			data[0] = str;
			for (int i = 1; i < data.Count; i++)
			{
				float f = (float)data[i];
				/*
				if (f >= 10.0f)
					f = 9.99f;
				if (!emotiv)
					if ((i == 2) || (i == 7) || (i == 12) || (i == 17) || (i == 22))
						data[i] = f / 15; // Alpha
					else
						data[i] = f / 3; // Beta Gamma
				*/
			}
		}

		//	 1 - "Delta_TP9", "Delta_AF7", "", "Delta_AF8", "Delta_TP10",
		//	 6 - "Theta_TP9", "Theta_AF7", "", "Theta_AF8", "Theta_TP10",
		//	11 - "Alpha_TP9", "Alpha_AF7", "", "Alpha_AF8", "Alpha_TP10",
		//	16 - "Beta_TP9" , "Beta_AF7" , "", "Beta_AF8" , "Beta_TP10",
		//	21 - "Gamma_TP9", "Gamma_AF7", "", "Gamma_AF8", "Gamma_TP10"

		//	 1 - "AF3/theta", "AF3/alpha", "AF3/betaL", "AF3/betaH", "AF3/gamma",
		//	 6 - "T7/theta", "T7/alpha", "T7/betaL", "T7/betaH", "T7/gamma",
		//	11 - "Pz/theta", "Pz/alpha", "Pz/betaL", "Pz/betaH", "Pz/gamma",
		//	16 - "T8/theta", "T8/alpha", "T8/betaL", "T8/betaH", "T8/gamma",
		//	21 - "AF4/theta", "AF4/alpha", "AF4/betaL", "AF4/betaH", "AF4/gamma"

		ArrayList data1 = new();
		data1.Add(data[0]);                 // Timestamp

		data1.Add(data[8]);                 // T7/betaL -> Delta_TP9
		data1.Add(data[3]);                 // AF3/betaL -> Delta_AF7
		if (emotiv) data1.Add(data[13]);    // Pz/betaL -> 
		data1.Add(data[23]);                // AF4/betaL -> Delta_AF8
		data1.Add(data[18]);                // T8/betaL -> Delta_TP10

		data1.Add(data[6]);                 // T7/theta -> Theta_TP9
		data1.Add(data[1]);                 // AF3/theta -> Theta_AF7
		if (emotiv) data1.Add(data[11]);    // Pz/theta -> 
		data1.Add(data[21]);                // AF4/theta -> Theta_AF8
		data1.Add(data[16]);                // T8/theta -> Theta_TP10

		data1.Add(data[7]);                 // T7/alpha -> Alpha_TP9
		data1.Add(data[2]);                 // AF3/alpha -> Alpha_AF7
		if (emotiv) data1.Add(data[12]);    // Pz/alpha -> 
		data1.Add(data[22]);                // AF4/alpha -> Alpha_AF8
		data1.Add(data[17]);                // T8/alpha -> Alpha_TP10

		data1.Add(data[9]);                 // T7/betaH -> Beta_TP9
		data1.Add(data[4]);                 // AF3/betaH -> Beta_AF7
		if (emotiv) data1.Add(data[14]);    // Pz/betaH -> 
		data1.Add(data[24]);                // AF4/betaH -> Beta_AF8
		data1.Add(data[19]);                // T8/betaH -> Beta_TP10

		data1.Add(data[10]);                // T7/gamma -> Gamma_TP9
		data1.Add(data[5]);                 // AF3/gamma -> Gamma_AF7
		if (emotiv) data1.Add(data[15]);    // Pz/gamma -> 
		data1.Add(data[25]);                // AF4/gamma -> Gamma_AF8
		data1.Add(data[20]);                // T8/gamma -> Gamma_TP10

		return data1;
	}
}

// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////
// //////////////////////////////////////////////////////////////////////////////////////////////////

//2022-12-22 17:13:55.633,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,/muse/event/connected Muse-A579
//2022-12-22 17:13:57.095,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,/muse/elements/blink

/*
	Muse = Emotiv

	0 - "Delta_TP9", "Delta_AF7", "", "Delta_AF8", "Delta_TP10",
	5 - "Theta_TP9", "Theta_AF7", "", "Theta_AF8", "Theta_TP10",
	10 - "Alpha_TP9", "Alpha_AF7", "", "Alpha_AF8", "Alpha_TP10",
	15 - "Beta_TP9" , "Beta_AF7" , "", "Beta_AF8" , "Beta_TP10",
	20 - "Gamma_TP9", "Gamma_AF7", "", "Gamma_AF8", "Gamma_TP10"

	private string[] m_strEmotivHeader = {
		"Timestamp",
		"AF3/theta", "AF3/alpha", "AF3/betaL", "AF3/betaH", "AF3/gamma",
		"T7/theta", "T7/alpha", "T7/betaL", "T7/betaH", "T7/gamma",
		"Pz/theta", "Pz/alpha", "Pz/betaL", "Pz/betaH", "Pz/gamma",
		"T8/theta", "T8/alpha", "T8/betaL", "T8/betaH", "T8/gamma",
		"AF4/theta", "AF4/alpha", "AF4/betaL", "AF4/betaH", "AF4/gamma"
	};

	private string[] m_strMuseHeader1 = {
		"Timestamp",
		"Delta_TP9", "Delta_AF7", "Delta_AF8", "Delta_TP10",
		"Theta_TP9", "Theta_AF7", "Theta_AF8", "Theta_TP10",
		"Alpha_TP9", "Alpha_AF7", "Alpha_AF8", "Alpha_TP10",
		"Beta_TP9", "Beta_AF7", "Beta_AF8", "Beta_TP10",
		"Gamma_TP9", "Gamma_AF7", "Gamma_AF8", "Gamma_TP10"
	};
*/
