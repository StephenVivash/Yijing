package org.yijing;

import android.graphics.Color;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.CompoundButton;
import android.widget.Spinner;
import android.widget.Switch;

import com.androidplot.xy.BoundaryMode;
import com.androidplot.xy.LineAndPointFormatter;
import com.androidplot.xy.SimpleXYSeries;
import com.androidplot.xy.XYPlot;

public class MindwaveActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener {

	private Switch m_swiEnable;
	private Switch m_swiEegAlgorithm;
	private Switch m_swiAttention;
	private Switch m_swiMeditation;

	private Spinner m_spiAttentionLow;
	private Spinner m_spiAttentionHigh;
	private Spinner m_spiMeditationLow;
	private Spinner m_spiMeditationHigh;

	private static boolean m_bRawSeries = false;
	private static int gcCount = 0;

	private static XYPlot pltEEG;
	private final static int X_RANGE = 50;

	public static SimpleXYSeries xyDeltaSeries = null;
	public static SimpleXYSeries xyThetaSeries = null;
	public static SimpleXYSeries xyAlphaSeries = null;
	public static SimpleXYSeries xyBetaSeries = null;
	public static SimpleXYSeries xyGammaSeries = null;

	public static SimpleXYSeries xyAttentionSeries = null;
	public static SimpleXYSeries xyMeditationSeries = null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_mindwave);

		m_swiEnable = (Switch) findViewById(R.id.swiEnable);
		m_swiEnable.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
			public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
				DiagramCanvas.m_bBtEnabled = isChecked;
				m_swiEegAlgorithm.setEnabled(isChecked);
				m_swiAttention.setEnabled(isChecked);
				m_spiAttentionLow.setEnabled(isChecked && DiagramCanvas.m_bAttention);
				m_spiAttentionHigh.setEnabled(isChecked && DiagramCanvas.m_bAttention);
				m_swiMeditation.setEnabled(isChecked);
				m_spiMeditationLow.setEnabled(isChecked && DiagramCanvas.m_bMeditation);
				m_spiMeditationHigh.setEnabled(isChecked && DiagramCanvas.m_bMeditation);
				if (isChecked)
					DiagramCanvas.StartMindwave();
				else
					DiagramCanvas.StopMindwave();
			}
		});

		pltEEG = (XYPlot) findViewById(R.id.myPlot);
		m_swiEegAlgorithm = (Switch) findViewById(R.id.swiEegAlgorithm);
		m_swiEegAlgorithm.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
				public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
					m_bRawSeries = !isChecked;
					createPlot();
				}
		});

		m_swiAttention = (Switch) findViewById(R.id.swiAttention);
		m_swiAttention.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
			public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
				DiagramCanvas.m_bAttention = isChecked;
				m_swiAttention.setChecked(isChecked);
				m_swiMeditation.setChecked(!isChecked);
				m_spiAttentionLow.setEnabled(isChecked);
				m_spiAttentionHigh.setEnabled(isChecked);
				m_spiMeditationLow.setEnabled(!isChecked);
				m_spiMeditationHigh.setEnabled(!isChecked);
			}
		});

		m_swiMeditation = (Switch) findViewById(R.id.swiMeditation);
		m_swiMeditation.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
			public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
				DiagramCanvas.m_bMeditation = isChecked;
				m_swiMeditation.setChecked(isChecked);
				m_swiAttention.setChecked(!isChecked);
				m_spiMeditationLow.setEnabled(isChecked);
				m_spiMeditationHigh.setEnabled(isChecked);
				m_spiAttentionLow.setEnabled(!isChecked);
				m_spiAttentionHigh.setEnabled(!isChecked);
			}
		});

		m_spiAttentionLow = (Spinner) findViewById(R.id.spiAttentionLow);
		m_spiAttentionLow.setOnItemSelectedListener(this);
		m_spiAttentionHigh = (Spinner) findViewById(R.id.spiAttentionHigh);
		m_spiAttentionHigh.setOnItemSelectedListener(this);

		m_spiMeditationLow = (Spinner) findViewById(R.id.spiMeditationLow);
		m_spiMeditationLow.setOnItemSelectedListener(this);
		m_spiMeditationHigh = (Spinner) findViewById(R.id.spiMeditationHigh);
		m_spiMeditationHigh.setOnItemSelectedListener(this);

		ArrayAdapter<String> adaLow = new ArrayAdapter<String>(this,
			android.R.layout.simple_spinner_dropdown_item, new String[]{"40%", "60%", "80%"});
		m_spiAttentionLow.setAdapter(adaLow);
		m_spiMeditationLow.setAdapter(adaLow);

		ArrayAdapter<String> adaHigh = new ArrayAdapter<String>(this,
			android.R.layout.simple_spinner_dropdown_item, new String[]{"60%", "80%", "100%"});
		m_spiAttentionHigh.setAdapter(adaHigh);
		m_spiMeditationHigh.setAdapter(adaHigh);

		createPlot();
	}

	@Override
	protected void onStart() {
		super.onStart();
		m_swiEnable.setChecked(DiagramCanvas.m_bBtEnabled);
		m_swiEegAlgorithm.setChecked(!m_bRawSeries);
		m_swiEegAlgorithm.setEnabled(DiagramCanvas.m_bBtEnabled);
		m_swiAttention.setChecked(DiagramCanvas.m_bAttention);
		m_swiMeditation.setChecked(DiagramCanvas.m_bMeditation);
		m_swiAttention.setEnabled(DiagramCanvas.m_bBtEnabled);
		m_spiAttentionHigh.setEnabled(DiagramCanvas.m_bBtEnabled && DiagramCanvas.m_bAttention);
		m_spiAttentionLow.setEnabled(DiagramCanvas.m_bBtEnabled && DiagramCanvas.m_bAttention);
		m_swiMeditation.setEnabled(DiagramCanvas.m_bBtEnabled);
		m_spiMeditationHigh.setEnabled(DiagramCanvas.m_bBtEnabled && DiagramCanvas.m_bMeditation);
		m_spiMeditationLow.setEnabled(DiagramCanvas.m_bBtEnabled && DiagramCanvas.m_bMeditation);

		m_spiAttentionLow.setSelection((DiagramCanvas.m_nAttentionLow - 40) / 20);
		m_spiAttentionHigh.setSelection((DiagramCanvas.m_nAttentionHigh - 60) / 20);
		m_spiMeditationLow.setSelection((DiagramCanvas.m_nMeditationLow - 40) / 20);
		m_spiMeditationHigh.setSelection((DiagramCanvas.m_nMeditationHigh - 60) / 20);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.diagram, menu);
		menu.findItem(R.id.action_mindwave).setEnabled(false);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		YijingActivity.startMenuActivity(item);
		return super.onOptionsItemSelected(item);
	}

	public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {
		if (parent.getId() == R.id.spiAttentionLow) {
			if(m_spiAttentionHigh.getSelectedItemPosition() <= pos)
				m_spiAttentionHigh.setSelection(pos);
			DiagramCanvas.m_nAttentionLow = 40 + (pos * 20);
		}
		else
		if (parent.getId() == R.id.spiAttentionHigh) {
			if(m_spiAttentionLow.getSelectedItemPosition() >= pos)
				m_spiAttentionLow.setSelection(pos);
			DiagramCanvas.m_nAttentionHigh = 60 + (pos * 20);
		}
		else
		if (parent.getId() == R.id.spiMeditationLow) {
			if(m_spiMeditationHigh.getSelectedItemPosition() <= pos)
				m_spiMeditationHigh.setSelection(pos);
			DiagramCanvas.m_nMeditationLow = 40 + (pos * 20);
		}
		else
		if (parent.getId() == R.id.spiMeditationHigh) {
			if(m_spiMeditationLow.getSelectedItemPosition() >= pos)
				m_spiMeditationLow.setSelection(pos);
			DiagramCanvas.m_nMeditationHigh = 60 + (pos * 20);
		}
	}

	public void onNothingSelected(AdapterView<?> parent) {}

	private void createPlot() {
		pltEEG.removeMarkers();
		clearAllSeries();
		if(m_bRawSeries) {
			xyDeltaSeries = createSeries("Delta");
			xyThetaSeries = createSeries("Theta");
			xyAlphaSeries = createSeries("Alpha");
			xyBetaSeries = createSeries("Beta");
			xyGammaSeries = createSeries("Gamma");
			setupPlot(-20, 20, "EEG Bandpower");
			addSeries(pltEEG, xyDeltaSeries, R.xml.line_point_formatter_with_plf1);
			addSeries(pltEEG, xyThetaSeries, R.xml.line_point_formatter_with_plf2);
			addSeries(pltEEG, xyAlphaSeries, R.xml.line_point_formatter_with_plf3);
			addSeries(pltEEG, xyBetaSeries, R.xml.line_point_formatter_with_plf4);
			addSeries(pltEEG, xyGammaSeries, R.xml.line_point_formatter_with_plf5);
		} else {
			xyAttentionSeries = createSeries("Attention");
			xyMeditationSeries = createSeries("Meditation");
			setupPlot(0, 100, "EEG Algorithm");
			addSeries(pltEEG, xyAttentionSeries, R.xml.line_point_formatter_with_plf1);
			addSeries(pltEEG, xyMeditationSeries, R.xml.line_point_formatter_with_plf2);
		}
		pltEEG.redraw();
	}

	private XYPlot setupPlot(Number rangeMin, Number rangeMax, String title) {
		pltEEG.setDomainLeftMax(0);
		pltEEG.setDomainRightMin(X_RANGE);
		pltEEG.setDomainRightMax(X_RANGE);
		if ((rangeMax.intValue() - rangeMin.intValue()) < 10)
			pltEEG.setRangeStepValue((rangeMax.intValue() - rangeMin.intValue() + 1));
		else
			pltEEG.setRangeStepValue(11);
		pltEEG.setRangeBoundaries(rangeMin.intValue(), rangeMax.intValue(), BoundaryMode.FIXED);
		pltEEG.getGraphWidget().getGridBackgroundPaint().setColor(Color.WHITE);
		pltEEG.setTicksPerDomainLabel(10);
		pltEEG.getGraphWidget().setDomainLabelOrientation(-45);
		pltEEG.setPlotPadding(0, 0, 0, 0);
		pltEEG.setTitle(title);
		//pltEEG.setVisibility(View.VISIBLE);
		return pltEEG;
	}

	private void clearAllSeries () {
		if (xyDeltaSeries != null) {
			pltEEG.removeSeries(xyDeltaSeries);
			xyDeltaSeries = null;
		}
		if (xyThetaSeries != null) {
			pltEEG.removeSeries(xyThetaSeries);
			xyThetaSeries = null;
		}
		if (xyAlphaSeries != null) {
			pltEEG.removeSeries(xyAlphaSeries);
			xyAlphaSeries = null;
		}
		if (xyBetaSeries != null) {
			pltEEG.removeSeries(xyBetaSeries);
			xyBetaSeries = null;
		}
		if (xyGammaSeries != null) {
			pltEEG.removeSeries(xyGammaSeries);
			xyGammaSeries = null;
		}
		if (xyAttentionSeries != null) {
			pltEEG.removeSeries(xyAttentionSeries);
			xyAttentionSeries = null;
		}
		if (xyMeditationSeries != null) {
			pltEEG.removeSeries(xyMeditationSeries);
			xyMeditationSeries = null;
		}
		//pltEEG.setVisibility(View.INVISIBLE);
		System.gc();
	}

	private static SimpleXYSeries createSeries(String seriesName) {
		SimpleXYSeries series = new SimpleXYSeries(null,SimpleXYSeries.ArrayFormat.Y_VALS_ONLY,seriesName);
		series.useImplicitXVals();
		return series;
	}

	private SimpleXYSeries addSeries(XYPlot plot, SimpleXYSeries series, int formatterId) {
		LineAndPointFormatter seriesFormat = new LineAndPointFormatter();
		seriesFormat.setPointLabelFormatter(null);
		seriesFormat.configure(getApplicationContext(), formatterId);
		seriesFormat.setVertexPaint(null);
		series.useImplicitXVals();
		plot.addSeries(series, seriesFormat);
		return series;
	}

	public static void AddValueToPlot (SimpleXYSeries series, float value) {
		if (series.size() >= X_RANGE)
			series.removeFirst();
		Number num = value;
		series.addLast(null, num);
		pltEEG.redraw();
		gcCount++;
		if (gcCount >= 20) {
			System.gc();
			gcCount = 0;
		}
	}
}
