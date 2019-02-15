package org.yijing;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;

public class QuestionActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener {

	private Spinner m_spiType;
	private EditText m_edtQuestion;
	private static String m_strQuestion;
	private static boolean m_bFirst = true;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_question);

		m_spiType = (Spinner) findViewById(R.id.spiType);
		ArrayAdapter<CharSequence> adapter1 = ArrayAdapter.createFromResource(this,
				R.array.type_array, android.R.layout.simple_spinner_item);
		adapter1.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		m_spiType.setAdapter(adapter1);
		m_spiType.setOnItemSelectedListener(this);

		m_edtQuestion = (EditText) findViewById(R.id.edtQuestion);

		findViewById(R.id.btnOK).setOnClickListener(new Button.OnClickListener() {
			public void onClick(View v) {
				YijingActivity.StartActivity(R.id.action_diagram,0);
			}
		});

		findViewById(R.id.btnCancel).setOnClickListener(new Button.OnClickListener() {
			public void onClick(View v) {
				m_spiType.setSelection(0);
				m_strQuestion = "";
				m_edtQuestion.setText("");
			}
		});
	}

	@Override
	protected void onStart() {
		super.onStart();
		m_edtQuestion.setText(m_strQuestion);
	}

	@Override
	protected void onStop() {
		super.onStop();
		m_strQuestion = m_edtQuestion.getText().toString();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.diagram, menu);
		menu.findItem(R.id.action_question).setEnabled(false);
		if(m_bFirst) {
			menu.findItem(R.id.action_text).setEnabled(false);
			menu.findItem(R.id.action_mindwave).setEnabled(false);
			menu.findItem(R.id.action_settings).setEnabled(false);
		}
		m_bFirst = false;
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		YijingActivity.startMenuActivity(item);
		return super.onOptionsItemSelected(item);
	}

	public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {
		if (parent.getId() == R.id.spiDiagramLsb)
			;
	}

	public void onNothingSelected(AdapterView<?> parent) {}
}
