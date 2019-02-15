package org.yijing;

import android.support.v7.app.AppCompatActivity;
import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;

public class DiagramActivity extends AppCompatActivity {
	public static DiagramActivity m_da;
	public static DiagramCanvas m_dc;
	public static DiagramFragment m_df;
	public static ControlFragment m_cf;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_diagram);

		if (savedInstanceState == null) {
			m_da = this;
			m_df = new DiagramFragment();
			getFragmentManager().beginTransaction().add(R.id.activity_diagram,m_df).commit();
		}
		if (savedInstanceState == null) {
			m_cf = new ControlFragment();
			getFragmentManager().beginTransaction().add(R.id.activity_diagram,m_cf).commit();
		}
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.diagram, menu);
		menu.findItem(R.id.action_diagram).setEnabled(false);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		YijingActivity.startMenuActivity(item);
		return super.onOptionsItemSelected(item);
	}

	public void EnableControls(boolean bEnable) {m_cf.EnableControls(bEnable);}
	public void SetMode(int nMode) {m_cf.SetMode(nMode);}
	public void SetDiagram(int nDiagram) {m_cf.SetDiagram(nDiagram);}
	public void SetColor(int nColor) {m_cf.SetColor(nColor);}
	public void SetSpeed(int nSpeed) {m_cf.SetSpeed(nSpeed);}

	public static class DiagramFragment extends Fragment {
		private static View m_vwRoot;

		public DiagramFragment() {}

		@Override
		public void onCreate(Bundle savedInstanceState) {
			super.onCreate(savedInstanceState);
		}

		@Override
		public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			m_vwRoot = inflater.inflate(R.layout.fragment_diagram, container, false);
			m_dc = (DiagramCanvas) m_vwRoot.findViewById(R.id.view_diagram);
			return m_vwRoot;
		}
	}

	public static class ControlFragment extends Fragment implements AdapterView.OnItemSelectedListener {
		private View m_vwRoot;
		private Spinner m_spiMode;
		private Spinner m_spiDiagram;
		private Spinner m_spiColor;
		private Spinner m_spiSpeed;

		@Override
		public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			m_vwRoot = inflater.inflate(R.layout.fragment_controls, container, false);

			m_spiMode = (Spinner) m_vwRoot.findViewById(R.id.spiMode);
			ArrayAdapter<CharSequence> adapter1 = ArrayAdapter.createFromResource(m_da,
				R.array.mode_array, android.R.layout.simple_spinner_item);
			adapter1.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
			m_spiMode.setAdapter(adapter1);
			m_spiMode.setOnItemSelectedListener(this);

			m_spiDiagram = (Spinner) m_vwRoot.findViewById(R.id.spiType);
			ArrayAdapter<CharSequence> adapter2 = ArrayAdapter.createFromResource(m_da,
				R.array.diagram_array, android.R.layout.simple_spinner_item);
			adapter2.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
			m_spiDiagram.setAdapter(adapter2);
			m_spiDiagram.setOnItemSelectedListener(this);

			m_spiColor = (Spinner) m_vwRoot.findViewById(R.id.spiColor);
			ArrayAdapter<CharSequence> adapter3 = ArrayAdapter.createFromResource(m_da,
				R.array.color_array, android.R.layout.simple_spinner_item);
			adapter3.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
			m_spiColor.setAdapter(adapter3);
			m_spiColor.setOnItemSelectedListener(this);

			m_spiSpeed = (Spinner) m_vwRoot.findViewById(R.id.spiSpeed);
			ArrayAdapter<CharSequence> adapter4 = ArrayAdapter.createFromResource(m_da,
				R.array.speed_array, android.R.layout.simple_spinner_item);
			adapter4.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
			m_spiSpeed.setAdapter(adapter4);
			m_spiSpeed.setOnItemSelectedListener(this);

			m_vwRoot.findViewById(R.id.btnFirst).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.First();}
			});
			m_vwRoot.findViewById(R.id.btnInverse).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.Inverse();}
			});
			m_vwRoot.findViewById(R.id.btnOpposite).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.Opposite();}
			});
			m_vwRoot.findViewById(R.id.btnTransverse).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.Transverse();}
			});
			m_vwRoot.findViewById(R.id.btnNuclear).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.Nuclear();}
			});
			m_vwRoot.findViewById(R.id.btnMove).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.Move();}
			});
			m_vwRoot.findViewById(R.id.btnHome).setOnClickListener(new Button.OnClickListener() {
				public void onClick(View v) {m_dc.Home();}
			});

			SetColor(2);
			SetSpeed(1);

			return m_vwRoot;
		}

		public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {
			if (parent.getId() == R.id.spiMode)
				m_dc.ModeChanged(pos);
			else
			if (parent.getId() == R.id.spiType)
				m_dc.TypeChanged(pos);
			else
			if (parent.getId() == R.id.spiColor)
				m_dc.ColorChanged(pos);
			else
			if (parent.getId() == R.id.spiSpeed)
				m_dc.SpeedChanged(pos);
		}
		
		public void onNothingSelected(AdapterView<?> parent) {}
		
		public void EnableControls(boolean bEnable) {
			m_spiMode.setEnabled(bEnable);
			m_spiDiagram.setEnabled(bEnable);
			m_spiSpeed.setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnFirst).setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnInverse).setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnOpposite).setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnTransverse).setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnNuclear).setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnMove).setEnabled(bEnable);
			m_vwRoot.findViewById(R.id.btnHome).setEnabled(bEnable);
		}
		
		public void SetMode(int nMode) {m_spiMode.setSelection(nMode);}
		public void SetDiagram(int nDiagram) {m_spiDiagram.setSelection(nDiagram);}
		public void SetColor(int nColor) {m_spiColor.setSelection(nColor);}
		public void SetSpeed(int nSpeed) {m_spiSpeed.setSelection(nSpeed);}
	}
}
