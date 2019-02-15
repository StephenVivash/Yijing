package org.yijing;

import android.bluetooth.BluetoothAdapter;
import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.graphics.drawable.ShapeDrawable;
import android.graphics.drawable.shapes.OvalShape;
import android.graphics.drawable.shapes.RoundRectShape;
import android.util.AttributeSet;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Toast;

import com.neurosky.AlgoSdk.NskAlgoDataType;
import com.neurosky.AlgoSdk.NskAlgoSdk;
import com.neurosky.AlgoSdk.NskAlgoSignalQuality;
import com.neurosky.AlgoSdk.NskAlgoState;
import com.neurosky.AlgoSdk.NskAlgoType;
import com.neurosky.connection.ConnectionStates;
import com.neurosky.connection.DataType.MindDataType;
import com.neurosky.connection.TgStreamHandler;
import com.neurosky.connection.TgStreamReader;

import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

public class DiagramCanvas extends View {

	public static DiagramActivity m_da;
	public static DiagramCanvas m_dc;

	public static CHexagramValueSequencer m_hvsCurrent = null;
	public static CHexagramValueSequencer m_hvsCast;
	public static CHexagramValueSequencer m_hvsPrimary = null;
	public static CValueSequencer m_vsCurrent;

	public enum eDiagramMode {edmExplore, edmAnimate, edmTouchCast, edmMindCast, edmAutoCast}
	public enum eDiagramType {edtHexagram, edtTrigram, edtLine}
	public enum eDiagramColor {edcMono, edcDual, edcTrigram, edcRgb}
	public enum eDiagramSpeed {edsFast, edsMedium, edsSlow}
	public enum eDiagramLsb {edlTop, edlBottom}

	public static int[] m_nSpeeds = {3000, 600, 5};
	public static int m_nSpeed = 1;

	private eDiagramMode m_edmDiagramMode = eDiagramMode.edmExplore;
	private eDiagramType m_edtDiagramType = eDiagramType.edtHexagram;
	private eDiagramColor m_edcDiagramColor = eDiagramColor.edcTrigram;
	private eDiagramSpeed m_edsDiagramSpeed = eDiagramSpeed.edsMedium;
	public static eDiagramLsb m_edlDiagramLsb = eDiagramLsb.edlTop;

	public static int m_nLineSequence = 1;
	public static int m_nLineRatio = 1;
	public static int m_nLineLabel = 3;
	public static int m_nLineText = 0;

	public static int m_nTrigramSequence = 1;
	public static int m_nTrigramRatio = 0;
	public static int m_nTrigramLabel = 2;
	public static int m_nTrigramText = 0;

	public static int m_nHexagramSequence = 2;
	public static int m_nHexagramRatio = 0;
	public static int m_nHexagramLabel = 9;
	public static int m_nHexagramText = 6;

	private static int m_nCurrentLine = 0;
	private static int m_nCurrentTrigram = 0;

	public static boolean m_bTimerOn = false;
	public static boolean m_bForward = true;

	public static boolean m_bBtEnabled = false;
	public static boolean m_bAttention = false;
	public static boolean m_bMeditation = true;
	public static int m_nAttentionRange = 20;
	public static int m_nAttentionHigh = 60;
	public static int m_nAttentionLow = m_nAttentionHigh - m_nAttentionRange;
	public static int m_nMeditationRange = 20;
	public static int m_nMeditationHigh = 60;
	public static int m_nMeditationLow = m_nMeditationHigh - m_nMeditationRange;

	private static Timer m_timDiagram;
	private static MindCastThread m_thrMindCast;
	private static AutoCastThread m_thrAutoCast;
	private static boolean m_bModeCanceled = false;

	private static BluetoothAdapter m_btAdapter;
	public static TgStreamReader m_tgStreamReader;
	private static NskAlgoSdk m_nskAlgoSdk;

	private static int m_nPoorSignal = 0;
	public static int m_nRawDataIndex = 0;
	public static short[] m_nRawData = new short[512];
	private static short[] m_nAttention = {0};
	private static short[] m_nMeditation = {0};

	private Paint m_pntDiagram1;
	private Paint m_pntDiagram2;
	private Paint m_pntWhite;
	private Paint m_pntBlack;
	private Paint m_pntRed;

	private int m_nOffsetX = 100;
	private int m_nOffsetY = 100;
	private int m_nPadY = 20;
	private int m_nWidth = 480;
	private int m_nHeight = 180;
	private int m_nCenter = 480;
	private float[] m_fRadii = new float[]{0, 0, 0, 0, 0, 0, 0, 0};
	private static ShapeDrawable[][] m_sdLines = {{null, null, null}, {null, null, null},
		{null, null, null},{null, null, null}, {null, null, null}, {null, null, null}};

	//static {
	//	System.loadLibrary("NskAlgoAndroid");
	//}

	public DiagramCanvas(Context c, AttributeSet attrs) {
		super(c, attrs);
		m_dc = this;
		if (!isInEditMode())
			m_da = (DiagramActivity) c;
		CreatePaint();
		if (m_hvsCurrent == null) {
			m_hvsCurrent = new CHexagramValueSequencer(0);

			CLineValueSequencer.SetCurrentSequence(m_nLineSequence);
			CLineValueSequencer.SetCurrentRatio(m_nLineRatio);
			CLineValueSequencer.SetCurrentLabel(m_nLineLabel);

			CTrigramValueSequencer.SetCurrentSequence(m_nTrigramSequence);
			CTrigramValueSequencer.SetCurrentRatio(m_nTrigramRatio);
			CTrigramValueSequencer.SetCurrentLabel(m_nTrigramLabel);

			CHexagramValueSequencer.SetCurrentSequence(m_nHexagramSequence);
			CHexagramValueSequencer.SetCurrentRatio(m_nHexagramRatio);
			CHexagramValueSequencer.SetCurrentLabel(m_nHexagramLabel);

			SetDiagramLsb(0);
			m_hvsCurrent.First();
			m_vsCurrent = m_hvsCurrent;

		} else {
			if (m_edmDiagramMode == eDiagramMode.edmAnimate) {
				ModeChanged(eDiagramMode.edmExplore.ordinal());
				m_bModeCanceled = true;
				new EndAnimateThread().start();
			}
			if ((m_thrMindCast != null) && m_thrMindCast.isAlive()) {
				m_bModeCanceled = true;
				m_thrMindCast.interrupt();
			}
			if ((m_thrAutoCast != null) && m_thrAutoCast.isAlive()) {
				m_bModeCanceled = true;
				m_thrAutoCast.interrupt();
			}
		}

		if (!isInEditMode()) {
			m_nskAlgoSdk = new NskAlgoSdk();
			m_nskAlgoSdk.setOnStateChangeListener(new NskAlgoSdk.OnStateChangeListener() {
				@Override
				public void onStateChange(int state, int reason) {
					String stateStr = "";
					String reasonStr = "";
					for (NskAlgoState s : NskAlgoState.values())
						if (s.value == state)
							stateStr = s.toString();
					for (NskAlgoState r : NskAlgoState.values())
						if (r.value == reason)
							reasonStr = r.toString();
					//final String finalStateStr = stateStr + " | " + reasonStr;
					final int finalState = state;
					m_da.runOnUiThread(new Runnable() {
						@Override
						public void run() {
							//stateText.setText(finalStateStr);
							if (finalState == NskAlgoState.NSK_ALGO_STATE_RUNNING.value || finalState == NskAlgoState.NSK_ALGO_STATE_COLLECTING_BASELINE_DATA.value)
								;//bRunning = true;
							else if (finalState == NskAlgoState.NSK_ALGO_STATE_STOP.value) {
								//bRunning = false;
								//raw_data = null;
								//raw_data_index = 0;
								if (DiagramCanvas.m_tgStreamReader != null && DiagramCanvas.m_tgStreamReader.isBTConnected()) {
									DiagramCanvas.m_tgStreamReader.stop();
									DiagramCanvas.m_tgStreamReader.close();
								}
								//output_data_count = 0;
								//output_data = null;
								System.gc();
							} else if (finalState == NskAlgoState.NSK_ALGO_STATE_PAUSE.value)
								;//bRunning = false;
							else if (finalState == NskAlgoState.NSK_ALGO_STATE_ANALYSING_BULK_DATA.value)
								;//bRunning = true;
							else if (finalState == NskAlgoState.NSK_ALGO_STATE_INITED.value || finalState == NskAlgoState.NSK_ALGO_STATE_UNINTIED.value)
								;//bRunning = false;
						}
					});
				}
			});

			m_nskAlgoSdk.setOnBPAlgoIndexListener(new NskAlgoSdk.OnBPAlgoIndexListener() {
				@Override
				public void onBPAlgoIndex(float delta, float theta, float alpha, float beta, float gamma) {
					final float fDelta = delta, fTheta = theta, fAlpha = alpha, fBeta = beta, fGamma = gamma;
					m_da.runOnUiThread(new Runnable() {
						@Override
						public void run() {
							if (MindwaveActivity.xyDeltaSeries != null) {
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyDeltaSeries, fDelta);
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyThetaSeries, fTheta);
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyAlphaSeries, fAlpha);
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyBetaSeries, fBeta);
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyGammaSeries, fGamma);
							}
						}
					});
				}
			});

			m_nskAlgoSdk.setOnSignalQualityListener(new NskAlgoSdk.OnSignalQualityListener() {
				@Override
				public void onSignalQuality(final int level) {
					m_da.runOnUiThread(new Runnable() {
						@Override
						public void run() {
							String sqStr = NskAlgoSignalQuality.values()[level].toString();
						}
					});
				}
			});

			m_nskAlgoSdk.setOnAttAlgoIndexListener(new NskAlgoSdk.OnAttAlgoIndexListener() {
				@Override
				public void onAttAlgoIndex(int value) {
					final float fAttention = value;
					m_da.runOnUiThread(new Runnable() {
						@Override
						public void run() {
							if(m_bAttention)
								m_da.setTitle("Yijing - " + "Att: " + m_nAttention[0] + "% (" + m_nAttentionLow + "% - " + m_nAttentionHigh + "%)");
							if (MindwaveActivity.xyAttentionSeries != null)
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyAttentionSeries, fAttention);
						}
					});
				}
			});

			m_nskAlgoSdk.setOnMedAlgoIndexListener(new NskAlgoSdk.OnMedAlgoIndexListener() {
				@Override
				public void onMedAlgoIndex(int value) {
					final float fMeditation = value;
					m_da.runOnUiThread(new Runnable() {
						@Override
						public void run() {
							if(m_bMeditation)
								m_da.setTitle("Yijing - " + "Med: " + m_nMeditation[0] + "% (" + m_nMeditationLow + "% - " + m_nMeditationHigh + "%)");
							if (MindwaveActivity.xyMeditationSeries != null)
								MindwaveActivity.AddValueToPlot(MindwaveActivity.xyMeditationSeries, fMeditation);
						}
					});
				}
			});

			m_nskAlgoSdk.setOnEyeBlinkDetectionListener(new NskAlgoSdk.OnEyeBlinkDetectionListener() {
				@Override
				public void onEyeBlinkDetect(int strength) {
					m_da.runOnUiThread(new Runnable() {
						@Override
						public void run() {
							//blinkImage.setImageResource(R.mipmap.led_off);
						}
					});
				}
			});
		}
		if (m_bBtEnabled && !isInEditMode())
			StartMindwave();
	}

	@Override
	protected void onSizeChanged(int w, int h, int oldw, int oldh) {
		super.onSizeChanged(w, h, oldw, oldh);
		SetDiagramSize(w,h);
		CreateDiagrams();
	}

	@Override
	protected void onDraw(Canvas canvas) {
		super.onDraw(canvas);
		Paint pntColor;
		int nLine1 = 6;
		if (m_edcDiagramColor == eDiagramColor.edcMono)
			m_pntDiagram1.setColor(Color.DKGRAY);
		else
		if (m_edcDiagramColor == eDiagramColor.edcDual) {
			m_pntDiagram1.setColor(Color.LTGRAY);
			m_pntDiagram2.setColor(Color.DKGRAY);
		} else
		if (m_edcDiagramColor == eDiagramColor.edcRgb)
			m_pntDiagram1.setColor(RgbColor());
		else {
			m_pntDiagram1.setColor(TrigramColor(m_hvsCurrent.Trigram(0).Value()));
			m_pntDiagram2.setColor(TrigramColor(m_hvsCurrent.Trigram(1).Value()));
		}
		pntColor = m_pntDiagram1;
		for (int nLine = 0; nLine < 6; ++nLine) {
			--nLine1;

			boolean bSelected = ((m_edtDiagramType == eDiagramType.edtLine) && (m_nCurrentLine == nLine)) ||
					((m_edtDiagramType == eDiagramType.edtTrigram) && (m_nCurrentTrigram == (nLine / 3))) ||
					(m_edtDiagramType == eDiagramType.edtHexagram);

			boolean bYang = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3).Value() % 2 == 1;
			boolean bOld = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3).IsMoving();
			if ((m_edcDiagramColor == eDiagramColor.edcTrigram) && (nLine == 3))
				pntColor = m_pntDiagram2;
			else
			if (m_edcDiagramColor == eDiagramColor.edcDual)
				pntColor = bYang ? m_pntDiagram1 : m_pntDiagram2;
			DrawLine(canvas,nLine,m_nOffsetX,m_nOffsetY + (nLine1 * (m_nHeight + m_nPadY)),m_nWidth,m_nHeight,bYang,bOld,pntColor,bSelected);
		}
		canvas.drawText(m_hvsCurrent.SequenceStr() + ". " + m_hvsCurrent.Label() + " (" + m_hvsCurrent.ValueStr() + ")",
			m_nOffsetX,(m_nOffsetY * 4) + (6 * (m_nHeight + m_nPadY)),m_pntBlack);
	}

	@Override
	public boolean onTouchEvent(MotionEvent event) {
		switch (event.getAction()) {
			case MotionEvent.ACTION_DOWN:
				PointerPressed(event.getX(),event.getY());
				break;
			case MotionEvent.ACTION_MOVE:
				break;
			case MotionEvent.ACTION_UP:
				PointerReleased();
				break;
		}
		return true;
	}

	private void PointerPressed(float fX, float fY) {
		if ((m_edmDiagramMode != eDiagramMode.edmMindCast) && (m_edmDiagramMode != eDiagramMode.edmAutoCast)) {
			int nLine = 6;
			boolean bFound = false;
			for(int i = 0; i < 6; ++i) {
				Rect rec1 = m_sdLines[--nLine][0].getBounds();
				Rect rec2 = m_sdLines[nLine][2].getBounds();
				if((fY < rec1.bottom) && (fY > rec1.top) && (((fX < rec1.right) && (fX > rec1.left)) || ((fX < rec2.right) && (fX > rec2.left)))) {
					m_bForward = fX > m_nCenter;
					SetCurrentLine(nLine, false);
					bFound = true;
					break;
				}
			}
			if (!bFound)
				return;
			TypeChanged(m_edtDiagramType.ordinal());
			if (!m_bTimerOn)
			{
				m_bTimerOn = true;
				m_timDiagram = new Timer();
				m_timDiagram.schedule(new DiagramTimerTask(),0,m_nSpeeds[m_nSpeed]);
			}
		}
	}

	private void PointerReleased() {
		if (m_edmDiagramMode == eDiagramMode.edmExplore) {
			m_hvsPrimary = null;
			//SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
		}
		if (m_bTimerOn && (m_edmDiagramMode != eDiagramMode.edmAnimate) &&
			(m_edmDiagramMode != eDiagramMode.edmMindCast) && (m_edmDiagramMode != eDiagramMode.edmAutoCast)) {
			m_bTimerOn = false;
			m_timDiagram.cancel();
			if ((m_edmDiagramMode == eDiagramMode.edmTouchCast) && (m_nCurrentLine == 5))
				EndCast();
		}
	}
	
	public void ModeChanged(int nMode) {
		if (m_bModeCanceled)
			return;
		if ((nMode == eDiagramMode.edmMindCast.ordinal()) && !MindwaveConnected()) {
			SetMode(eDiagramMode.edmExplore.ordinal());
			ToastMsg("Mindwave not connected");
			return;
		}
		m_edmDiagramMode = eDiagramMode.values()[nMode];
		EnableControls(true);
		if (m_edmDiagramMode == eDiagramMode.edmAnimate) {
			m_timDiagram = new Timer();
			m_timDiagram.schedule(new DiagramTimerTask(),0,m_nSpeeds[m_nSpeed]);
			m_bTimerOn = true;
		}
		else {
			if (m_bTimerOn)
				m_timDiagram.cancel();
			m_bTimerOn = false;
			if ((m_edmDiagramMode == eDiagramMode.edmTouchCast) || (m_edmDiagramMode == eDiagramMode.edmMindCast) ||
				(m_edmDiagramMode == eDiagramMode.edmAutoCast)) {
				SetCurrentLine(0, true);
				SetDiagram(eDiagramType.edtLine.ordinal());
				SetSpeed(2);
				EnableControls(false);
			}
			if (m_edmDiagramMode == eDiagramMode.edmTouchCast)
				m_da.findViewById(R.id.spiMode).setEnabled(true);
			if (m_edmDiagramMode == eDiagramMode.edmMindCast) {
				m_thrMindCast = new MindCastThread();
				m_thrMindCast.start();
			}
			if (m_edmDiagramMode == eDiagramMode.edmAutoCast) {
				m_thrAutoCast = new AutoCastThread();
				m_thrAutoCast.start();
			}
		}
	}

	public void TypeChanged(int nDiagram) {
		m_edtDiagramType = eDiagramType.values()[nDiagram];
		if (m_edtDiagramType == eDiagramType.edtLine)
			m_vsCurrent = m_hvsCurrent.Trigram(m_nCurrentTrigram).Line(m_nCurrentLine % 3);
		else
		if (m_edtDiagramType == eDiagramType.edtTrigram)
			m_vsCurrent = m_hvsCurrent.Trigram(m_nCurrentTrigram);
		else
			m_vsCurrent = m_hvsCurrent;
		invalidate();
	}

	public void SpeedChanged(int nSpeed) {
		m_nSpeed = nSpeed;
		if (m_bTimerOn) {
			m_timDiagram.cancel();
			m_timDiagram = new Timer();
			m_timDiagram.schedule(new DiagramTimerTask(),0,m_nSpeeds[m_nSpeed]);
		}
	}

	public void ColorChanged(int nColor) {
		m_edcDiagramColor = eDiagramColor.values()[nColor];
		invalidate();
	}

	public void Transition() {
		m_hvsPrimary = null;
		//SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
		invalidate();
	}

	public void First() {
		m_vsCurrent.First();
		Transition();
	}

	public void Last() {
		m_vsCurrent.Last();
		Transition();
	}

	public void Inverse() {
		m_vsCurrent.Inverse();
		Transition();
	}

	public void Opposite() {
		m_vsCurrent.Opposite();
		Transition();
	}

	public void Transverse() {
		m_vsCurrent.Transverse();
		Transition();
	}

	public void Nuclear() {
		m_vsCurrent.Nuclear();
		Transition();
	}

	public void Move() {
		if (m_hvsCurrent.IsMoving()) {
			m_hvsPrimary = new CHexagramValueSequencer(m_hvsCurrent);
			m_vsCurrent.Move();
		}
		else
		if (m_hvsPrimary != null) {
			m_hvsCurrent = new CHexagramValueSequencer(m_hvsPrimary);
			m_vsCurrent = m_hvsCurrent;
			m_hvsPrimary = null;
		}
		//SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
		invalidate();
	}

	public void Home() {
		if (m_hvsCast != null) {
			m_hvsCurrent = new CHexagramValueSequencer(m_hvsCast);
			m_vsCurrent = m_hvsCurrent;
			//SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
			invalidate();
		}
	}

	public static void SetDiagramLsb(int nLsb) {
		m_edlDiagramLsb = eDiagramLsb.values()[nLsb];
		Sequences.SetLSB(m_edlDiagramLsb == eDiagramLsb.edlTop);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetLineSequence(int nSequence) {
		m_nLineSequence = nSequence;
		CLineValueSequencer.SetCurrentSequence(nSequence);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetLineRatio(int nRatio) {
		m_nLineRatio = nRatio;
		CLineValueSequencer.SetCurrentRatio(nRatio);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetLineLabel(int nLabel) {
		m_nLineLabel = nLabel;
		CLineValueSequencer.SetCurrentLabel(nLabel);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetLineText(int nText) {
		m_nLineText = nText;
	}

	public static void SetTrigramSequence(int nSequence) {
		m_nTrigramSequence = nSequence;
		CTrigramValueSequencer.SetCurrentSequence(nSequence);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetTrigramRatio(int nRatio) {
		m_nTrigramRatio = nRatio;
		CTrigramValueSequencer.SetCurrentRatio(nRatio);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetTrigramLabel(int nLabel) {
		m_nTrigramLabel = nLabel;
		CTrigramValueSequencer.SetCurrentLabel(nLabel);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetTrigramText(int nText) {
		m_nTrigramText = nText;
	}

	public static void SetHexagramSequence(int nSequence) {
		m_nHexagramSequence = nSequence;
		CHexagramValueSequencer.SetCurrentSequence(nSequence);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetHexagramRatio(int nRatio) {
		m_nHexagramRatio = nRatio;
		CHexagramValueSequencer.SetCurrentRatio(nRatio);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetHexagramLabel(int nLabel) {
		m_nHexagramLabel = nLabel;
		CHexagramValueSequencer.SetCurrentLabel(nLabel);
		m_hvsCurrent.Update();
		m_dc.invalidate();
	}

	public static void SetHexagramText(int nText) {
		m_nHexagramText = nText;
	}

	private void SetCurrentLine(int nLine, boolean bDiagram) {
		m_nCurrentLine = nLine;
		m_nCurrentTrigram = m_nCurrentLine / 3;
		if (bDiagram)
			m_vsCurrent = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3);
	}

	public static void SetHexagramValue(int nValue) {
		m_hvsCurrent.Value(nValue);
		m_hvsCurrent.Update();
		//Transition();
	}

	private void CreatePaint() {
		m_pntDiagram1 = new Paint();
		m_pntDiagram2 = new Paint();
		m_pntWhite = new Paint();
		m_pntWhite.setColor(Color.WHITE);
		m_pntBlack = new Paint();
		m_pntBlack.setColor(Color.BLACK);
		m_pntBlack.setTextAlign(Paint.Align.LEFT);
		m_pntRed = new Paint();
		m_pntRed.setColor(Color.RED);
		//m_pntDiagram1.setAntiAlias(true);
		//m_pntDiagram1.setStyle(Paint.Style.FILL_AND_STROKE);
		//m_pntDiagram1.setStrokeJoin(Paint.Join.ROUND);
		//m_pntDiagram1.setStrokeWidth(1);
		//m_pntDiagram1.setStrokeMiter();
		//sd.getPaint().setShader(mShader);
	}

	private void SetDiagramSize(int nWidth, int nHeight) {
		int nTextSize;
		if(nWidth > nHeight)
			nWidth /= 2;
		if((nWidth <= 500) || (nHeight <= 800)) {
			m_nPadY = 8;
			m_nWidth = 180;
			m_nHeight = 50;
			m_nOffsetX = (nWidth - ((m_nWidth * 2) + m_nHeight)) / 2;
			m_nOffsetY = 15;
			m_nCenter = m_nOffsetX + m_nWidth + (m_nHeight / 2);
			nTextSize = 30;
			m_fRadii = new float[] { 10, 10, 10, 10, 10, 10, 10, 10 };
		} else
		if((nWidth <= 800) || (nHeight <= 1000)) { //900
			m_nPadY = 10;
			m_nWidth = 240;
			m_nHeight = 90;
			m_nOffsetX = (nWidth - ((m_nWidth * 2) + m_nHeight)) / 2;
			m_nOffsetY = 20;
			m_nCenter = m_nOffsetX + m_nWidth + (m_nHeight / 2);
			nTextSize = 40;
			m_fRadii = new float[] { 20, 20, 20, 20, 20, 20, 20, 20 };
		} else
		if(nWidth <= 1100) { // 1200
			m_nPadY = 20;
			m_nWidth = 380;
			m_nHeight = 140;
			m_nOffsetX = (nWidth - ((m_nWidth * 2) + m_nHeight)) / 2;
			m_nOffsetY = 20;
			m_nCenter = m_nOffsetX + m_nWidth + (m_nHeight / 2);
			nTextSize = 60;
			m_fRadii = new float[] { 30, 30, 30, 30, 30, 30, 30, 30 };
		} else {
			m_nPadY = 20;
			m_nWidth = 480;
			m_nHeight = 160;
			m_nOffsetX = (nWidth - ((m_nWidth * 2) + m_nHeight)) / 2;
			m_nOffsetY = 20;
			m_nCenter = m_nOffsetX + m_nWidth + (m_nHeight / 2);
			nTextSize = 80;
			m_fRadii = new float[] { 40, 40, 40, 40, 40, 40, 40, 40 };
		}
		m_pntBlack.setTextSize(nTextSize);
	}

	private void CreateDiagrams() {
		for(int i = 0; i < 6; ++i){
			m_sdLines[i][0] = new ShapeDrawable(new RoundRectShape(m_fRadii, null, m_fRadii));
			m_sdLines[i][1] = new ShapeDrawable(new OvalShape());
			m_sdLines[i][2] = new ShapeDrawable(new RoundRectShape(m_fRadii, null, m_fRadii));
			m_sdLines[i][0].setBounds(0,0,0,0);
			m_sdLines[i][1].setBounds(0,0,0,0);
			m_sdLines[i][2].setBounds(0,0,0,0);
		}
	}

	private void DrawLine(Canvas canvas, int nLine, int nX, int nY, int nWidth, int nHeight, boolean bYang, boolean bOld, Paint pntColor, boolean bSelected) {
		if(bYang) {
			m_sdLines[nLine][0].setBounds(nX,nY,nX + (nWidth *2) + nHeight,nY + nHeight);
			m_sdLines[nLine][0].getPaint().set(pntColor);
			m_sdLines[nLine][0].draw(canvas);
			m_sdLines[nLine][0].getPaint().set(bSelected ? m_pntRed : m_pntBlack);
			m_sdLines[nLine][0].getPaint().setStyle(Paint.Style.STROKE);
			m_sdLines[nLine][0].getPaint().setStrokeWidth(2);
			m_sdLines[nLine][0].draw(canvas);

			m_sdLines[nLine][2].setBounds(0,0,0,0);
			m_sdLines[nLine][2].draw(canvas);
		}
		else {
			m_sdLines[nLine][0].setBounds(nX, nY, nX + nWidth, nY + nHeight);
			m_sdLines[nLine][0].getPaint().set(pntColor);
			m_sdLines[nLine][0].draw(canvas);
			m_sdLines[nLine][0].getPaint().set(bSelected ? m_pntRed : m_pntBlack);
			m_sdLines[nLine][0].getPaint().setStyle(Paint.Style.STROKE);
			m_sdLines[nLine][0].getPaint().setStrokeWidth(2);
			m_sdLines[nLine][0].draw(canvas);

			m_sdLines[nLine][2].setBounds(nX + nWidth + nHeight, nY, nX + (nWidth * 2) + nHeight, nY + nHeight);
			m_sdLines[nLine][2].getPaint().set(pntColor);
			m_sdLines[nLine][2].draw(canvas);
			m_sdLines[nLine][2].getPaint().set(bSelected ? m_pntRed : m_pntBlack);
			m_sdLines[nLine][2].getPaint().setStyle(Paint.Style.STROKE);
			m_sdLines[nLine][2].getPaint().setStrokeWidth(2);
			m_sdLines[nLine][2].draw(canvas);
		}
		if(bOld) {
			m_sdLines[nLine][1].setBounds(nX + nWidth,nY,nX + nWidth + nHeight,nY + nHeight);
			m_sdLines[nLine][1].getPaint().set(bYang ? m_pntWhite : pntColor);
			m_sdLines[nLine][1].draw(canvas);
			m_sdLines[nLine][1].getPaint().set(bSelected ? m_pntRed : m_pntBlack);
			m_sdLines[nLine][1].getPaint().setStyle(Paint.Style.STROKE);
			m_sdLines[nLine][1].getPaint().setStrokeWidth(2);
			m_sdLines[nLine][1].draw(canvas);
		}
		else {
			m_sdLines[nLine][1].setBounds(0,0,0,0);
			m_sdLines[nLine][1].draw(canvas);
		}
	}

	private int TrigramColor(int nValue) {
		switch (nValue) {
			case 0: // earth
				return Color.DKGRAY;
			case 1: // thunder
				return Color.GREEN;
			case 2: // water
				return Color.BLUE;
			case 3: // lake
				return Color.CYAN;
			case 4: // mountain
				return Color.MAGENTA;
			case 5: // fire
				return Color.RED;
			case 6: // wind
				return Color.YELLOW;
			case 7: // heaven
				break;
		}
		return Color.LTGRAY;
	}

	private int RgbColor() {
		boolean bLower, bUpper;
		int[] nRgb = { 0, 0, 0 };
		for (int i = 0; i < 3; ++i) {
			bUpper = m_hvsCurrent.Trigram(1).Line(i).Value() % 2 == 1;
			bLower = m_hvsCurrent.Trigram(0).Line(i).Value() % 2 == 1;
			nRgb[i] = bLower ? bUpper ? 0xFF : 0x55 : bUpper ? 0xAA : 0x00;
		}
		if((nRgb[0] == 0) && (nRgb[1] == 0) && (nRgb[2] == 0))
			return Color.DKGRAY;
		if((nRgb[0] == 255) && (nRgb[1] == 255) && (nRgb[2] == 255))
			return Color.LTGRAY;
		return Color.argb(0xFF, nRgb[0], nRgb[1], nRgb[2]);
	}

	public static String DescribeCast() {
		String strTemp = m_hvsCurrent.SequenceStr();
		for (int i = 0; i < 6; ++i)
			if (m_hvsCurrent.Trigram(i / 3).Line(i % 3).IsMoving())
				strTemp += "." + Integer.toString(i + 1);
		strTemp += " " + m_hvsCurrent.Label();
		if (m_hvsCurrent.IsMoving())
		{
			CHexagramValueSequencer hvsSecondary = new CHexagramValueSequencer(m_hvsCurrent);
			hvsSecondary.Move();
			strTemp += " > " + hvsSecondary.SequenceStr() + " " + hvsSecondary.Label();
		}
		return strTemp;
	}

	private void EndCast() {
		String strTemp = "#Yijing ☯ " + DescribeCast() + "\n\nAn5 v6.0.6 ";
		strTemp += Sequences.DiagramSetting(0,m_edmDiagramMode.ordinal()) + " " + Sequences.DiagramSetting(13,m_nHexagramSequence) + " " +
			Sequences.DiagramSetting(4,m_edlDiagramLsb.ordinal()) + " " + Sequences.DiagramSetting(6,m_nLineRatio) + " " +
			Sequences.DiagramSetting(15,m_nHexagramLabel) + "\n\nhttps://play.google.com/store/apps/details?id=org.yijing\n\n";
		Tweet(strTemp);

		m_hvsCast = new CHexagramValueSequencer(m_hvsCurrent);
		m_vsCurrent = m_hvsCurrent;

		SetMode(eDiagramMode.edmExplore.ordinal());
		SetDiagram(eDiagramType.edtHexagram.ordinal());
		SetSpeed(1);
		//SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
		EnableControls(true);

		YijingActivity.StartActivity(R.id.action_text,1000);
	}

	private void Tweet(String strDescription) {
	}

	private void EnableControls(boolean bEnable) {m_da.EnableControls(bEnable);}
	private void SetMode(int nMode) {m_da.SetMode(nMode);}
	private void SetDiagram(int nDiagram) {m_da.SetDiagram(nDiagram);}
	private void SetSpeed(int nSpeed) {m_nSpeed = nSpeed; m_da.SetSpeed(nSpeed);}
	private void SetColor(int nColor) {m_da.SetColor(nColor);}

	private static void ToastMsg(final String strMsg) {
		m_da.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				Toast.makeText(m_da, strMsg, Toast.LENGTH_LONG).show();
			}
		});
	}
	private static boolean MindwaveConnected() {return (m_nAttention[0] > 0) && (m_nMeditation[0] > 0);}

	public static void StartMindwave() {
		if (m_btAdapter == null) {
			try {
				m_btAdapter = BluetoothAdapter.getDefaultAdapter();
			} catch (Exception e) {}
			if ((m_btAdapter != null) && m_btAdapter.isEnabled()) {
				ToastMsg("Bluetooth enabled");
				m_tgStreamReader = new TgStreamReader(m_btAdapter, tgStreamHandler);
				m_tgStreamReader.connect();
				int nAlgoTypes = NskAlgoType.NSK_ALGO_TYPE_MED.value + NskAlgoType.NSK_ALGO_TYPE_ATT.value +
					NskAlgoType.NSK_ALGO_TYPE_BLINK.value + NskAlgoType.NSK_ALGO_TYPE_BP.value;
				if (NskAlgoSdk.NskAlgoInit(nAlgoTypes, m_da.getFilesDir().getAbsolutePath()) == 0)
					if (NskAlgoSdk.NskAlgoStart(false) == 0) {
						int x = 0;
					}
				//NskAlgoSdk.NskAlgoPause();
				//nskAlgoSdk.NskAlgoStop();
				//NskAlgoSdk.NskAlgoUninit();
			} else
				ToastMsg("Bluetooth disabled");
		}
	}

	public static void StopMindwave() {
		//m_nskAlgoSdk.NskAlgoStop();
		if (m_tgStreamReader != null && m_tgStreamReader.isBTConnected()) {
			m_tgStreamReader.stop();
			m_tgStreamReader.close();
		}
		m_btAdapter = null;
	}

	private static TgStreamHandler tgStreamHandler = new TgStreamHandler() {
		@Override
		public void onStatesChanged(int connectionStates) {
			switch (connectionStates) {
				case ConnectionStates.STATE_INIT:
					break;
				case ConnectionStates.STATE_CONNECTING:
					//ToastMsg("Mindwave connecting");
					break;
				case ConnectionStates.STATE_CONNECTED:
					m_tgStreamReader.start();
					ToastMsg("Mindwave connected");
					break;
				case ConnectionStates.STATE_WORKING:
					m_tgStreamReader.startRecordRawData();
					break;
				case ConnectionStates.STATE_STOPPED:
					//ToastMsg("Mindwave stopped");
					//m_tgStreamReader.stop();
					//m_tgStreamReader.close();
					break;
				case ConnectionStates.STATE_DISCONNECTED:
					m_nAttention[0] = 0;
					m_nMeditation[0] = 0;
					ToastMsg("Mindwave disconnected");
					break;
				case ConnectionStates.STATE_COMPLETE:
					//ToastMsg("Mindwave complete");
					break;
				case ConnectionStates.STATE_RECORDING_START:
					break;
				case ConnectionStates.STATE_RECORDING_END:
					break;
				case ConnectionStates.STATE_GET_DATA_TIME_OUT:
					if (m_tgStreamReader != null && m_tgStreamReader.isBTConnected()) {
						m_tgStreamReader.stop();
						m_tgStreamReader.close();
					}
					break;
				case ConnectionStates.STATE_FAILED:
					ToastMsg("Mindwave failed");
					break;
				case ConnectionStates.STATE_ERROR:
					ToastMsg("Mindwave error");
					break;
			}
		}

		@Override
		public void onRecordFail(int flag) {
			/*Log.e("","onRecordFail: " +flag);*/
		}

		@Override
		public void onChecksumFail(byte[] payload, int length, int checksum) {
			/*Log.e("","onChecksumFail: " +flag);*/
		}

		@Override
		public void onDataReceived(int datatype, int data, Object obj) {
			switch (datatype) {
				case MindDataType.CODE_ATTENTION:
					m_nAttention[0] = (short) data;
					NskAlgoSdk.NskAlgoDataStream(NskAlgoDataType.NSK_ALGO_DATA_TYPE_ATT.value, m_nAttention, 1);
					if (MindwaveConnected())
						m_nPoorSignal = 0;
					break;
				case MindDataType.CODE_MEDITATION:
					m_nMeditation[0] = (short) data;
					NskAlgoSdk.NskAlgoDataStream(NskAlgoDataType.NSK_ALGO_DATA_TYPE_MED.value, m_nMeditation, 1);
					if (MindwaveConnected())
						m_nPoorSignal = 0;
					break;
				case MindDataType.CODE_POOR_SIGNAL:
					short pqValue[] = {(short)data};
					NskAlgoSdk.NskAlgoDataStream(NskAlgoDataType.NSK_ALGO_DATA_TYPE_PQ.value, pqValue, 1);
					if(++m_nPoorSignal > 10) {
						ToastMsg("Mindwave poor signal");
						m_nPoorSignal = 0;
					}
					break;
				case MindDataType.CODE_RAW:
					m_nRawData[m_nRawDataIndex++] = (short)data;
					if (m_nRawDataIndex == 512) {
						NskAlgoSdk.NskAlgoDataStream(NskAlgoDataType.NSK_ALGO_DATA_TYPE_EEG.value, m_nRawData, m_nRawDataIndex);
						m_nRawDataIndex = 0;
					}
					break;
				default:
					break;
			}
		}
	};

	private class DiagramTimerTask extends TimerTask {
		@Override
		public void run() {
			boolean bRatio = (m_edmDiagramMode == DiagramCanvas.eDiagramMode.edmTouchCast) ||
				(m_edmDiagramMode == DiagramCanvas.eDiagramMode.edmMindCast) ||
				(m_edmDiagramMode == DiagramCanvas.eDiagramMode.edmAutoCast);
			if (m_bForward)
				m_vsCurrent.Next(bRatio);
			else
				m_vsCurrent.Previous(bRatio);
			postInvalidate();
		}
	}

	private class MindCastThread extends Thread implements Runnable {

		private boolean BelowHigh() {
			boolean bAttention = !m_bAttention || (m_nAttention[0] < m_nAttentionHigh);
			boolean bMeditation = !m_bMeditation || (m_nMeditation[0] < m_nMeditationHigh);
			return MindwaveConnected() && (bAttention && bMeditation);
		}

		private boolean AboveLow() {
			boolean bAttention = !m_bAttention || (m_nAttention[0] > m_nAttentionLow);
			boolean bMeditation = !m_bMeditation || (m_nMeditation[0] > m_nMeditationLow);
			return MindwaveConnected() && (bAttention && bMeditation);
		}

		private void LevelMsg(boolean bStart) {
			String strMsg = bStart ? "Start " : "Stop ";
			strMsg += m_bAttention ? "att: " + m_nAttention[0] + "% " : "";
			strMsg += m_bMeditation ? "med: " + m_nMeditation[0] + "% " : "";
			ToastMsg(strMsg);
		}

		@Override
		public void run() {
			Random r = new Random(System.currentTimeMillis());
			for (int i = 0; i < 6; ++i) {
				try {
					SetCurrentLine(i, true);
					while (BelowHigh()) {
						Thread.sleep(100 + r.nextInt(500));
						postInvalidate();
					}
					if (!MindwaveConnected())
						break;
					//LevelMsg(true);
					m_timDiagram = new Timer();
					m_timDiagram.schedule(new DiagramTimerTask(), 0, m_nSpeeds[m_nSpeed]);
					while (AboveLow()) {
						Thread.sleep(100 + r.nextInt(500));
						postInvalidate();
					}
					m_timDiagram.cancel();
					if (!MindwaveConnected())
						break;
					//LevelMsg(false);
					Thread.sleep(500);
				}
				catch(InterruptedException ie) {
					m_timDiagram.cancel();
					break;
				}
			}
			m_da.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					EndCast();
					m_bModeCanceled = false;
				}
			});
		}
	}

	private class AutoCastThread extends Thread implements Runnable {
		@Override
		public void run() {
			Random r = new Random(System.currentTimeMillis());
			for (int i = 0; i < 6; ++i) {
				SetCurrentLine(i, true);
				m_timDiagram = new Timer();
				m_timDiagram.schedule(new DiagramTimerTask(), 0, m_nSpeeds[m_nSpeed]);
				try {
					Thread.sleep((r.nextInt(5) + 1) * 1000 + r.nextInt(1000));
				}
				catch(InterruptedException ie) {
					m_timDiagram.cancel();
					break;
				}
				m_timDiagram.cancel();
			}
			m_da.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					EndCast();
					m_bModeCanceled = false;
				}
			});
		}
	}

	private class EndAnimateThread extends Thread implements Runnable {
		@Override
		public void run() {
			try {
				Thread.sleep(500);
			}
			catch(InterruptedException ie) {}
			m_da.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					m_bModeCanceled = false;
					SetMode(eDiagramMode.edmExplore.ordinal());
				}
			});
		}
	}
}
