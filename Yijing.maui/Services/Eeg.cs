using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Newtonsoft.Json.Linq;

using CortexAccess;
using EegML;

using Yijing.Views;

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
		for (int j = 0; j < m_nAverageMax; ++j)
			m_alAverage.Add(0.0f);
		m_isInitialised = false;
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
	protected static DiagramView m_vwDiagram;

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

	public string m_strMuseData = ",800.0,810.0,820.0,830.0,840.0,58.0,8.0,0.1,0.0,0.9,5.2,-3.7,1.8,1,1.0,1.0,1.0,1.0,85.0";
	public string m_strMuseHeader = "TimeStamp,Delta_TP9,Delta_AF7,Delta_AF8,Delta_TP10,Theta_TP9,Theta_AF7,Theta_AF8,Theta_TP10,Alpha_TP9,Alpha_AF7,Alpha_AF8,Alpha_TP10,Beta_TP9,Beta_AF7,Beta_AF8,Beta_TP10,Gamma_TP9,Gamma_AF7,Gamma_AF8,Gamma_TP10,RAW_TP9,RAW_AF7,RAW_AF8,RAW_TP10,AUX_RIGHT,Mellow,Concentration,Accelerometer_X,Accelerometer_Y,Accelerometer_Z,Gyro_X,Gyro_Y,Gyro_Z,HeadBandOn,HSI_TP9,HSI_AF7,HSI_AF8,HSI_TP10,Battery,Elements\n";

	public static string m_strPrediction;

	private EegModel m_mlEEG = null;

	public static void SetEegView(EegView eeg) 
	{ 
		m_vwEeg = eeg; 
	}

	public static void SetDiagramView(DiagramView diagram) 
	{ 
		m_vwDiagram = diagram; 
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
		m_vwEeg.EnableEegControls(false, bLive);
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
		for (int i = 0; i < m_nChannelMax; ++i)
			m_eegChannel[i] = new EegChannel(m_vwEeg.m_nEegMode == (int)eEegMode.eSummary ? 400 : 200);
		m_vwEeg.InitialseTriggers();
		m_vwEeg.InitialiseChart();
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
				f *= AppPreferences.MuseScale;

				// Clean bad Muse data

				if (((i >= 0) && (i <= 4)) && (f > 0.3f * AppPreferences.MuseScale)) // Delta
					f = 0.3f * AppPreferences.MuseScale;

				if ((i >= 5) && (i <= 9) && (f > 0.3f * AppPreferences.MuseScale)) // Theta
					f = 0.3f * AppPreferences.MuseScale;

				if (((i >= 10) && (i <= 14)) && (f > 0.8f * AppPreferences.MuseScale)) // Alpha 
					f = 0.8f * AppPreferences.MuseScale;

				if (f < -0.6f * AppPreferences.MuseScale)
					f = -0.6f * AppPreferences.MuseScale;
				else
				if (f > 3.0f * AppPreferences.MuseScale)
					f = 3.0f * AppPreferences.MuseScale;
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
				{
					f /= 10.0f;
					if (f > 0.8f)
						f = 0.8f;
				}
			}

			//m_eegChannel[i].InitialseChannel(f);
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

		if (AppPreferences.TriggerSounding && ((AppPreferences.EegGoal == (int)eGoal.eMeditation)  ||
			// or MindCast is finished
			((AppPreferences.EegGoal == (int)eGoal.eYijingCast) && (AppPreferences.DiagramMode == (int) eDiagramMode.eExplore))))
		{
			DateTime Timestamp = DateTime.Now;
			TimeSpan TimeDiff = Timestamp - _dtSoundingUpdate;
			if (TimeDiff.TotalSeconds >= 10)
			{
				_dtSoundingUpdate = Timestamp;

				// single shot - hit target

				DiagramView.SoundTrigger(m_eegChannel[AppPreferences.TriggerIndex].m_fCurrentValue * AppPreferences.AudioScale, 
					m_eegChannel[AppPreferences.TriggerIndex].m_fHigh * AppPreferences.AudioScale);
			}
		}

		if ((AppPreferences.Timer != (int)eTimer.eNone)) // m_bLive
		{
			DateTime Timestamp1 = DateTime.Now;
			TimeSpan TimeDiff1 = Timestamp1 - _dtTimerUpdate;
			if (TimeDiff1.TotalMinutes >= MinuteTimer())
			{
				_dtTimerUpdate = Timestamp1;
				m_vwEeg.PlayTimer();
			}
		}

		float prediction = 0.0f;
		if (!bSummary && (m_mlEEG != null) && (AppPreferences.AiModel != (int)eAiModel.eNone))
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
#if DEBUG
			if (!bSummary && (m_nReplaySpeed == 1) && (AppPreferences.AiAnalysis == (int)eAiAnalysis.eOpenAI))
#else
			if (!bSummary && (m_bLive || (m_nReplaySpeed == 1)) && (AppPreferences.AiAnalysis == (int)eAiAnalysis.eOpenAI))
#endif
			{
				DateTime Timestamp = DateTime.Now;
				TimeSpan TimeDiff = Timestamp - _dtAiUpdate;
				if (TimeDiff.TotalSeconds >= 60)
				{
					_dtAiUpdate = Timestamp;
					TimeDiff = Timestamp - m_dtEegStart;
					String str = $"Minute={TimeDiff.TotalMinutes,1:#0}, Prediction={ prediction * 100, 1:#0.000}, " +
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

					m_vwEeg.AiData(str);
				}
			}
			else
				_dtAiUpdate = DateTime.Now;
			/*
			if ((AppPreferences.Timer != (int)eTimer.eNone)) // m_bLive
			{
				DateTime Timestamp1 = DateTime.Now;
				TimeSpan TimeDiff1 = Timestamp1 - _dtTimerUpdate;
				if (TimeDiff1.TotalMinutes >= MinuteTimer())
				{
					_dtTimerUpdate = Timestamp1;
					m_vwEeg.PlayTimer();
				}
			}
			*/
			m_vwEeg.UpdateData();
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
		if (m_socMuse == null)
		{
			Initialise(true);
			OpenFileStreams(false, true);
			m_epMuse = new IPEndPoint(IPAddress.Any, 5000);
			m_socMuse = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			m_socMuse.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
			m_socMuse.Bind(m_epMuse);
			m_tskMuse = new Task(ReceiveMuseData);
			m_tskMuse.Start();

			string s = m_socMuse.ToString();
			if ((s = Dns.GetHostName()) != null)
			{
				IPAddress[] ipa = Dns.GetHostEntry(s).AddressList;
				if ((ipa != null) && (ipa.Length > 0))
					if ((s = ipa[ipa.Length - 1].ToString()) != null)
						m_vwEeg.SetAppTitle("Listening on " + s + ":5000" + " - Yijing");
			}
		}
		return base.Connect();
	}

	public override void Disconnect() 
	{
		if (m_socMuse != null)
		{
			m_vwEeg.EnableEegControls(true, true);
			m_socMuse.Close();
			m_socMuse = null;
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
			DateTime FirstTimestamp = DateTime.Now;
			DateTime LastTimestamp = DateTime.Now;
			TimeSpan TimeDiff;
			m_vwEeg.SetAppTitle(Path.GetFileName(file) + " - Yijing");

			if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
				DiagramView.SetDiagramMode(eDiagramMode.eMindCast);

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
						m_vwEeg.UpdateTime(Timestamp);
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
			m_vwEeg.UpdateTime(FirstTimestamp, LastTimestamp);
		}
		m_bConnected = false;
		m_vwEeg.EnableEegControls(true, true);
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
			m_vwEeg.SetAppTitle(Path.GetFileName(file) + " - Yijing");

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
				m_vwEeg.UpdateTime(FirstTimestamp, LastTimestamp);
			}
		}
	}

	private void ReceiveMuseData()
	{
		//int[] ms = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		byte[] buffer = new byte[1024];
		int nSize = 0;
		int count = 0;
		bool bTitle = false;

		AppSettings._lastEegDataTime = DateTime.Now;

		if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
			DiagramView.SetDiagramMode(eDiagramMode.eMindCast);

		while (true)
		{
			IPPacketInformation pi;
			SocketFlags sf = SocketFlags.None;
			try { nSize = m_socMuse.ReceiveMessageFrom(buffer, 0, 1024, ref sf, ref m_epMuse, out pi); }
			catch {
				m_vwEeg.UpdateTime(m_dtEegStart, DateTime.Now);
				return; 
			}

			if (!bTitle)
			{
				bTitle = true;
				string ip = pi.Address.ToString();
				m_vwEeg.SetAppTitle("Received from " + ip + " - Yijing");
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
					m_vwEeg.UpdateTime(DateTime.Now);
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

	private void SocketTest()
	{
		EndPoint ep = new IPEndPoint(IPAddress.Any, 5000);
		Socket s1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		s1.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
		s1.Bind(ep);
		s1.Listen();
		Socket s2 = s1.Accept();
		byte[] buffer1 = new byte[s2.ReceiveBufferSize];
		while (s2.Poll(1000, SelectMode.SelectRead))
		{
			int size = s2.Receive(buffer1);
			//byte[] buffer2 = new byte[size] = buffer1;
			string s = Encoding.ASCII.GetString(buffer1); // UTF8
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
			m_vwEeg.EnableEegControls(true, true);
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
			m_vwEeg.SetAppTitle(Path.GetFileName(file) + " - Yijing");

			if (AppPreferences.EegGoal == (int)eGoal.eYijingCast)
				DiagramView.SetDiagramMode(eDiagramMode.eMindCast);

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
						m_vwEeg.UpdateTime(Timestamp);
						await Task.Delay((int)TimeDiff.TotalMilliseconds / m_nReplaySpeed);
					}
				}
			}
		}
		m_bConnected = false;
		m_vwEeg.EnableEegControls(true, true);
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
			m_vwEeg.SetAppTitle(Path.GetFileName(file) + " - Yijing");
			string s1 = sr.ReadLine();
			if ((s1 = sr.ReadLine()) != null) {
				string[] s2 = s1.Split(",", 2);
				try { FirstTimestamp = LastTimestamp = DateTime.Parse(s2[0]); } catch { }
				while (((s1 = sr.ReadLine()) != null) && !m_bCancelReplay) 
				{
					s2 = s1.Split(",");
					if (s2[1].Length > 0) {
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
				m_vwEeg.UpdateTime(FirstTimestamp, LastTimestamp);
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
			DiagramView.SetDiagramMode(eDiagramMode.eMindCast);

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
		m_vwEeg.UpdateTime(DateTime.Now);
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
		data1.Add(data[0]);					// Timestamp

		data1.Add(data[8]);					// T7/betaL -> Delta_TP9
		data1.Add(data[3]);					// AF3/betaL -> Delta_AF7
		if (emotiv) data1.Add(data[13]);	// Pz/betaL -> 
		data1.Add(data[23]);				// AF4/betaL -> Delta_AF8
		data1.Add(data[18]);				// T8/betaL -> Delta_TP10

		data1.Add(data[6]);					// T7/theta -> Theta_TP9
		data1.Add(data[1]);					// AF3/theta -> Theta_AF7
		if (emotiv) data1.Add(data[11]);	// Pz/theta -> 
		data1.Add(data[21]);				// AF4/theta -> Theta_AF8
		data1.Add(data[16]);				// T8/theta -> Theta_TP10

		data1.Add(data[7]);					// T7/alpha -> Alpha_TP9
		data1.Add(data[2]);					// AF3/alpha -> Alpha_AF7
		if (emotiv) data1.Add(data[12]);	// Pz/alpha -> 
		data1.Add(data[22]);				// AF4/alpha -> Alpha_AF8
		data1.Add(data[17]);				// T8/alpha -> Alpha_TP10

		data1.Add(data[9]);					// T7/betaH -> Beta_TP9
		data1.Add(data[4]);					// AF3/betaH -> Beta_AF7
		if (emotiv) data1.Add(data[14]);	// Pz/betaH -> 
		data1.Add(data[24]);				// AF4/betaH -> Beta_AF8
		data1.Add(data[19]);				// T8/betaH -> Beta_TP10

		data1.Add(data[10]);				// T7/gamma -> Gamma_TP9
		data1.Add(data[5]);					// AF3/gamma -> Gamma_AF7
		if (emotiv) data1.Add(data[15]);	// Pz/gamma -> 
		data1.Add(data[25]);				// AF4/gamma -> Gamma_AF8
		data1.Add(data[20]);				// T8/gamma -> Gamma_TP10

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

