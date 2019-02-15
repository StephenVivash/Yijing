package org.yijing;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.List;

public class SettingsActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener {

	private Spinner m_spiDiagramLsb;

	private Spinner m_spiLineSequence;
	private Spinner m_spiLineRatio;
	private Spinner m_spiLineLabel;
	private Spinner m_spiLineText;

	private Spinner m_spiTrigramSequence;
	private Spinner m_spiTrigramRatio;
	private Spinner m_spiTrigramLabel;
	private Spinner m_spiTrigramText;

	private Spinner m_spiHexagramSequence;
	private Spinner m_spiHexagramRatio;
	private Spinner m_spiHexagramLabel;
	private Spinner m_spiHexagramText;

	@Override
	protected void onCreate(Bundle savedInstanceState) {

		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_settings);

		m_spiDiagramLsb = (Spinner) findViewById(R.id.spiDiagramLsb);
		m_spiDiagramLsb.setOnItemSelectedListener(this);

		m_spiLineSequence = (Spinner) findViewById(R.id.spiLineSequence);
		m_spiLineSequence.setOnItemSelectedListener(this);
		m_spiLineRatio = (Spinner) findViewById(R.id.spiLineRatio);
		m_spiLineRatio.setOnItemSelectedListener(this);
		m_spiLineLabel = (Spinner) findViewById(R.id.spiLineLabel);
		m_spiLineLabel.setOnItemSelectedListener(this);
		m_spiLineText = (Spinner) findViewById(R.id.spiLineText);
		m_spiLineText.setOnItemSelectedListener(this);

		m_spiTrigramSequence = (Spinner) findViewById(R.id.spiTrigramSequence);
		m_spiTrigramSequence.setOnItemSelectedListener(this);
		m_spiTrigramRatio = (Spinner) findViewById(R.id.spiTrigramRatio);
		m_spiTrigramRatio.setOnItemSelectedListener(this);
		m_spiTrigramLabel = (Spinner) findViewById(R.id.spiTrigramLabel);
		m_spiTrigramLabel.setOnItemSelectedListener(this);
		m_spiTrigramText = (Spinner) findViewById(R.id.spiTrigramText);
		m_spiTrigramText.setOnItemSelectedListener(this);

		m_spiHexagramSequence = (Spinner) findViewById(R.id.spiHexagramSequence);
		m_spiHexagramSequence.setOnItemSelectedListener(this);
		m_spiHexagramRatio = (Spinner) findViewById(R.id.spiHexagramRatio);
		m_spiHexagramRatio.setOnItemSelectedListener(this);
		m_spiHexagramLabel = (Spinner) findViewById(R.id.spiHexagramLabel);
		m_spiHexagramLabel.setOnItemSelectedListener(this);
		m_spiHexagramText = (Spinner) findViewById(R.id.spiHexagramText);
		m_spiHexagramText.setOnItemSelectedListener(this);

		List<String> lstSettings;
		int nLength;
		ArrayAdapter<String> dataAdapter;

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[4]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[4][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiDiagramLsb.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[5]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[5][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiLineSequence.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[6]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[6][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiLineRatio.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[7]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[7][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiLineLabel.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[8]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[8][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiLineText.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[9]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[9][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiTrigramSequence.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[10]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[10][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiTrigramRatio.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[11]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[11][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiTrigramLabel.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[12]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[12][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiTrigramText.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[13]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[13][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiHexagramSequence.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[14]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[14][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiHexagramRatio.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[15]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[15][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiHexagramLabel.setAdapter(dataAdapter);

		lstSettings = new ArrayList<String>();
		nLength = Array.getLength(Sequences.strDiagramSettings[16]);
		for (int i = 1; i < nLength; ++i)
			lstSettings.add(Sequences.strDiagramSettings[16][i]);
		dataAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, lstSettings);
		dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiHexagramText.setAdapter(dataAdapter);
	}

	@Override
	protected void onStart() {
		super.onStart();
		m_spiDiagramLsb.setSelection(DiagramCanvas.m_edlDiagramLsb.ordinal());

		m_spiLineSequence.setSelection(DiagramCanvas.m_nLineSequence);
		m_spiLineRatio.setSelection(DiagramCanvas.m_nLineRatio);
		m_spiLineLabel.setSelection(DiagramCanvas.m_nLineLabel);
		m_spiLineText.setSelection(DiagramCanvas.m_nLineText);

		m_spiTrigramSequence.setSelection(DiagramCanvas.m_nTrigramSequence);
		m_spiTrigramRatio.setSelection(DiagramCanvas.m_nTrigramRatio);
		m_spiTrigramLabel.setSelection(DiagramCanvas.m_nTrigramLabel);
		m_spiTrigramText.setSelection(DiagramCanvas.m_nTrigramText);

		m_spiHexagramSequence.setSelection(DiagramCanvas.m_nHexagramSequence);
		m_spiHexagramRatio.setSelection(DiagramCanvas.m_nHexagramRatio);
		m_spiHexagramLabel.setSelection(DiagramCanvas.m_nHexagramLabel);
		m_spiHexagramText.setSelection(DiagramCanvas.m_nHexagramText);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.diagram, menu);
		menu.findItem(R.id.action_settings).setEnabled(false);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		YijingActivity.startMenuActivity(item);
		return super.onOptionsItemSelected(item);
	}

	public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {
		//String s = parent.getItemAtPosition(pos).toString();
		if (parent.getId() == R.id.spiDiagramLsb)
			DiagramCanvas.SetDiagramLsb(pos);
		else
		if (parent.getId() == R.id.spiLineSequence)
			DiagramCanvas.SetLineSequence(pos);
		else
		if (parent.getId() == R.id.spiLineRatio)
			DiagramCanvas.SetLineRatio(pos);
		else
		if (parent.getId() == R.id.spiLineLabel)
			DiagramCanvas.SetLineLabel(pos);
		else
		if (parent.getId() == R.id.spiLineText)
			DiagramCanvas.SetLineText(pos);
		if (parent.getId() == R.id.spiTrigramSequence)
			DiagramCanvas.SetTrigramSequence(pos);
		else
		if (parent.getId() == R.id.spiTrigramRatio)
			DiagramCanvas.SetTrigramRatio(pos);
		else
		if (parent.getId() == R.id.spiTrigramLabel)
			DiagramCanvas.SetTrigramLabel(pos);
		else
		if (parent.getId() == R.id.spiTrigramText)
			DiagramCanvas.SetTrigramText(pos);
		if (parent.getId() == R.id.spiHexagramSequence)
			DiagramCanvas.SetHexagramSequence(pos);
		else
		if (parent.getId() == R.id.spiHexagramRatio)
			DiagramCanvas.SetHexagramRatio(pos);
		else
		if (parent.getId() == R.id.spiHexagramLabel)
			DiagramCanvas.SetHexagramLabel(pos);
		else
		if (parent.getId() == R.id.spiHexagramText)
			DiagramCanvas.SetHexagramText(pos);
	}

	public void onNothingSelected(AdapterView<?> parent) {}
}
