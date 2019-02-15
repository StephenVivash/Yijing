package org.yijing;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;

public class YijingActivity extends AppCompatActivity {

	private static YijingActivity m_ya;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		m_ya = this;
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_yijing);

		StartActivity(R.id.action_question,2000);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.diagram, menu);
		menu.findItem(R.id.action_text).setEnabled(false);
		menu.findItem(R.id.action_mindwave).setEnabled(false);
		menu.findItem(R.id.action_settings).setEnabled(false);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		startMenuActivity(item);
		return super.onOptionsItemSelected(item);
	}

	public static void startMenuActivity(MenuItem item) {
		startMenuActivity(item.getItemId());
	}

	public static void startMenuActivity(int Id) {
		switch(Id) {
			case R.id.action_question: {
				m_ya.startActivity(new Intent("org.yijing.QuestionActivity"));
				break;
			}
			case R.id.action_diagram: {
				m_ya.startActivity(new Intent("org.yijing.DiagramActivity"));
				break;
			}
			case R.id.action_text: {
				m_ya.startActivity(new Intent("org.yijing.TextActivity"));
				break;
			}
			case R.id.action_settings: {
				m_ya.startActivity(new Intent("org.yijing.SettingsActivity"));
				break;
			}
			case R.id.action_mindwave: {
				m_ya.startActivity(new Intent("org.yijing.MindwaveActivity"));
				break;
			}
		}
	}

	public static void StartActivity(int nId, int nDelay) {
		m_ya.StartActivity(nId,nDelay,0);
	}

	private void StartActivity(int nId, int nDelay, int nShithead) {
		new StartActivityThread(nId,nDelay).start();
	}

	private class StartActivityThread extends Thread implements Runnable {

		int m_nId;
		int m_nDelay;

		StartActivityThread(int nId, int nDelay) {
			m_nId = nId;
			m_nDelay = nDelay;
		}

		@Override
		public void run() {
			try {
				Thread.sleep(m_nDelay);
			}
			catch(InterruptedException ie) {}
			m_ya.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					YijingActivity.startMenuActivity(m_nId);
				}
			});
		}
	}
}
