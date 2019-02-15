package org.yijing;

import java.lang.reflect.Array;
import java.lang.reflect.Method;
import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

import org.eclipse.swt.SWT;
import swing2swt.layout.BorderLayout;
import org.eclipse.swt.browser.Browser;
import org.eclipse.swt.browser.LocationAdapter;
import org.eclipse.swt.browser.LocationEvent;
import org.eclipse.swt.custom.TableEditor;

import org.eclipse.swt.events.PaintListener;
import org.eclipse.swt.events.PaintEvent;
import org.eclipse.swt.events.ControlEvent;
import org.eclipse.swt.events.ControlListener;
import org.eclipse.swt.events.FocusEvent;
import org.eclipse.swt.events.FocusListener;
import org.eclipse.swt.events.MouseAdapter;
import org.eclipse.swt.events.MouseEvent;

import org.eclipse.swt.graphics.Rectangle;
import org.eclipse.swt.graphics.Point;
import org.eclipse.swt.graphics.RGBA;
import org.eclipse.swt.graphics.Color;
import org.eclipse.swt.graphics.Font;

import org.eclipse.swt.widgets.Combo;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Display;
import org.eclipse.swt.widgets.Event;
import org.eclipse.swt.widgets.Listener;
import org.eclipse.swt.widgets.Shell;
import org.eclipse.swt.widgets.Menu;
import org.eclipse.swt.widgets.MenuItem;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Canvas;
import org.eclipse.swt.widgets.ToolBar;
import org.eclipse.swt.widgets.ToolItem;
import org.eclipse.swt.widgets.Table;
import org.eclipse.swt.widgets.TableColumn;
import org.eclipse.swt.widgets.TableItem;
import org.eclipse.swt.widgets.TabFolder;
import org.eclipse.swt.widgets.TabItem;

/*
import com.neurosky.AlgoSdk.NskAlgoDataType;
import com.neurosky.AlgoSdk.NskAlgoSdk;
import com.neurosky.AlgoSdk.NskAlgoSignalQuality;
import com.neurosky.AlgoSdk.NskAlgoState;
import com.neurosky.AlgoSdk.NskAlgoType;
import com.neurosky.connection.ConnectionStates;
import com.neurosky.connection.DataType.MindDataType;
import com.neurosky.connection.TgStreamHandler;
import com.neurosky.connection.TgStreamReader;
*/

public class Yijing {

	protected Shell shell;
	protected Display display;
	protected Table m_tabSettings;	

	protected DiagramCanvas m_canDiagram1;
	protected DiagramCanvas m_canDiagram2;
	protected DiagramCanvas m_canCurrent;

	protected TabFolder m_tabText;
	protected TabItem m_tbtmText1;
	protected Browser m_broText;
	
	protected boolean m_bColumn1;
	protected boolean m_bEnableMode = true;
	protected boolean m_bEnableType = true;
	protected boolean m_bEnableSpeed = true;
	
	protected String m_strCurrentText = "";
	
	public int[] m_nSpeeds = {2000, 600, 5};
	
	public enum eDiagramMode {edmExplore, edmAnimate, edmTouchCast, edmMindCast, edmAutoCast}
	public enum eDiagramType {edtHexagram, edtTrigram, edtLine}
	public enum eDiagramColor {edcMono, edcDual, edcTrigram, edcRgb}
	public enum eDiagramSpeed {edsFast, edsMedium, edsSlow}
	public enum eDiagramLsb {edlTop, edlBottom}
	
	public Yijing() {
	}
	
	public static void main(String[] args) {
		try {
			//UIManager.setLookAndFeel("javax.swing.plaf.metal.MetalLookAndFeel");
			//UIManager.put("swing.boldMetal", false);
			Yijing window = new Yijing();
			window.open();
		} catch (Exception e) {e.printStackTrace();}
	}

	public void open() {
		display = Display.getDefault();
		createContents();
		shell.open();
		shell.layout();
		m_canCurrent.UpdateSettings();
		m_canCurrent.SetHexagramText(m_canCurrent.GetHexagramText());
		while (!shell.isDisposed()) {
			if (!display.readAndDispatch()) {
				display.sleep();
			}
		}
	}

	protected void createContents() {
		shell = new Shell();
		
		BorderLayout bl = new BorderLayout(0, 0);
		shell.setLayout(bl);
		
		//shell.getLayoutData()
		//bl.setPreferredSize();
		
		shell.setMinimumSize(new Point(1275, 800));
		shell.setSize(1275,800);
		shell.setLocation(new Point(300,100));
		shell.setText("Yijing - The classic of change");

		////////////////////////////////////////////////////////////////////
		// Menu
		Menu menu = new Menu(shell, SWT.BAR);
		shell.setMenuBar(menu);
		
		MenuItem mntmFile = new MenuItem(menu, SWT.CASCADE);
		mntmFile.setText("File");
		
		Menu menu_1 = new Menu(mntmFile);
		mntmFile.setMenu(menu_1);
		
		MenuItem mntmExit = new MenuItem(menu_1, SWT.NONE);
		mntmExit.setText("Exit");
		
		////////////////////////////////////////////////////////////////////
		// Toolbar
		ToolBar toolBar = new ToolBar(shell, SWT.FLAT | SWT.RIGHT);
		
		toolBar.setLayoutData(BorderLayout.NORTH);
		//toolBar.setLayoutData(new GridData (SWT.FILL, SWT.TOP, false, false));
		
		ToolItem tltmTool = new ToolItem(toolBar, SWT.NONE);
		tltmTool.setText("Tool");
		
		ToolItem tltmX = new ToolItem(toolBar, SWT.SEPARATOR);
		tltmX.setText("x");
		
		////////////////////////////////////////////////////////////////////
		// Table
		m_tabSettings = new Table (shell, SWT.FULL_SELECTION | SWT.VIRTUAL); // SWT.BORDER | 
		m_tabSettings.setLayoutData(BorderLayout.WEST);
		m_tabSettings.setTouchEnabled(true);
		m_tabSettings.setHeaderVisible(true);
		m_tabSettings.setLinesVisible(true);
	
		TableColumn tblclmnParameter = new TableColumn(m_tabSettings, SWT.NONE);
		tblclmnParameter.setWidth(120);
		//tblclmnNewColumn.setResizable(true);
		tblclmnParameter.setText("Parameter");
		
		TableColumn tblclmnValue = new TableColumn(m_tabSettings, SWT.NONE);
		tblclmnValue.setWidth(100);
		//tblclmnNewColumn.setResizable(true);
		tblclmnValue.setText("Value");

		////////////////////////////////////////////////////////////////////
		// Text
		m_tabText = new TabFolder(shell, SWT.NONE); 
		m_tabText.setLayoutData(BorderLayout.EAST);
		m_tabText.setSize(700, 100);
		//tabText.setPreferredSize();
		
		m_tbtmText1 = new TabItem(m_tabText, SWT.NONE); 
		m_tbtmText1.setText("Text 1");
		
		NewBrowser();
		
		TabItem tbtmText2 = new TabItem(m_tabText, SWT.NONE);
		tbtmText2.setText("Text 2");
		
		////////////////////////////////////////////////////////////////////
		// Diagram
		TabFolder tabDiagram = new TabFolder(shell, SWT.NONE);
		tabDiagram.setLayoutData(BorderLayout.CENTER);
		//tabDiagram.setSize(200, 100);
		//tabDiagram.setPreferredSize();
		
		TabItem tbtmDiagram1 = new TabItem(tabDiagram, SWT.NONE);
		//tbtmDiagram1.setPreferredSize();
		tbtmDiagram1.setText("Diagram 1");
		m_canDiagram1 = new DiagramCanvas(tabDiagram, SWT.DOUBLE_BUFFERED | SWT.SMOOTH);
		//m_canDiagram1.setSize(200, 100);
		tbtmDiagram1.setControl(m_canDiagram1);

		TabItem tbtmDiagram2 = new TabItem(tabDiagram, SWT.NONE);
		tbtmDiagram2.setText("Diagram 2");
		m_canDiagram2 = new DiagramCanvas(tabDiagram, SWT.DOUBLE_BUFFERED | SWT.SMOOTH);
		tbtmDiagram2.setControl(m_canDiagram2);
		
		////////////////////////////////////////////////////////////////////
		// Log
		TabFolder tabLog = new TabFolder(shell, SWT.NONE);
		tabLog.setLayoutData(BorderLayout.SOUTH);
		
		TabItem tbtmLog1 = new TabItem(tabLog, SWT.NONE);
		tbtmLog1.setText("Log 1");
		
		TabItem tbtmLog2 = new TabItem(tabLog, SWT.NONE);
		tbtmLog2.setText("Log 2");
		
		m_tabSettings.setItemCount(17);
		
		////////////////////////////////////////////////////////////////////
		// Listeners
		m_tabSettings.addListener (SWT.SetData, e -> {
			TableItem item = (TableItem) e.item;
			item.setText (Sequences.strDiagramSettings[m_tabSettings.indexOf(item)]);
		}); 
		
        m_bColumn1 = false;
		m_tabSettings.addListener(SWT.MouseDown, e -> {
            Point pt = new Point(e.x, e.y);
            TableItem item = m_tabSettings.getItem(pt);
            if (item != null)
                if (item.getBounds(1).contains(pt))
                    m_bColumn1 = true;
	    });
		
		m_tabSettings.addListener(SWT.Selection, e -> { 

            if (!m_bColumn1)
            	return;
            
			TableItem item = (TableItem) e.item;
			int nIndex = m_tabSettings.indexOf(item);
			if (!m_bEnableMode && (nIndex == 0))
				return;
			if (!m_bEnableType && (nIndex == 1))
				return;
			if (!m_bEnableSpeed && (nIndex == 3))
				return;
    		
    		Combo combo = new Combo (m_tabSettings, SWT.READ_ONLY);
			int length = Sequences.strDiagramSettings[nIndex].length;
			for(int i = 1; i < length; ++i)
				combo.add(Sequences.strDiagramSettings[nIndex][i]);
			
			//combo.setItems(Sequences.strDiagramSettings[nIndex][1]);
			//combo.select(0);

			TableEditor editor = new TableEditor(m_tabSettings);
			editor.grabHorizontal = true;
			editor.setEditor(combo, item, 1);
			combo.setText(item.getText(1));
			combo.setListVisible(true);
			combo.setFocus();
			
			combo.addListener(SWT.Selection, e1 -> {
				item.setText(1, combo.getText());
				String[] strTemp = Sequences.strDiagramSettings[m_tabSettings.getSelectionIndex()][0].split(" ");
				if (strTemp[0].equals("Diagram"))
					strTemp[0] = strTemp[1] + "Changed";
				else
					strTemp[0] = "Set" + strTemp[0] + strTemp[1];
				try {
				    Class<?> c = Class.forName("org.yijing.Yijing$DiagramCanvas");
				    Class<?>[] argTypes = new Class[] {int.class};
				    Method method = c.getDeclaredMethod(strTemp[0], argTypes);
				    method.invoke(m_canCurrent, (Object) ((Combo) e1.widget).getSelectionIndex());

				} catch (Exception ex) {ex.printStackTrace();}
				combo.dispose();
			});
			
		    combo.addListener(SWT.KeyUp, e1 -> {
	            if(e1.character == SWT.CR)
					combo.dispose();
	            if(e1.character == SWT.ESC)
					combo.dispose();
	            if(e1.keyCode == SWT.ARROW_UP)
					combo.dispose();
	            if(e1.keyCode == SWT.ARROW_DOWN)
					combo.dispose();
		    });
			
/*			
		    combo.addListener(SWT.DefaultSelection, e1 -> {
				combo.dispose();
		    });
			combo.addSelectionListener(new SelectionListener() {
				@Override
				public void widgetSelected(SelectionEvent e) {
					combo.dispose();
				}
				@Override
				public void widgetDefaultSelected(SelectionEvent e) {
					combo.dispose();
				}
			});
			combo.addSelectionListener(new SelectionAdapter() {
				int x = 0;
			});
			combo.addTraverseListener(e -> {
				if (e.detail == SWT.TRAVERSE_RETURN) {
					e.doit = false;
					e.detail = SWT.TRAVERSE_NONE;
				}
			});
			combo.addListener (SWT.DefaultSelection, e1 -> {
				combo.dispose();
			});
*/			
		});
		
		m_canCurrent = m_canDiagram1;
		tabDiagram.addListener(SWT.Selection, new Listener() {
			@Override
			public void handleEvent(Event e) {
				if (((TabItem) e.item).getText() == "Diagram 1")
					m_canCurrent = m_canDiagram1;
				else
					m_canCurrent = m_canDiagram2;
				m_canCurrent.UpdateSettings();
			}
		});
	}

	public void NewBrowser() {
		m_broText = new Browser(m_tabText, SWT.NONE);
		m_tbtmText1.setControl(m_broText);
		m_broText.addLocationListener(new LocationAdapter()
		{
			@Override
			public void changing(LocationEvent e)
			{
				if (!m_strCurrentText.equals("Wilhelm") || e.location.equals("about:blank"))
					e.doit = true;
				else 
				if (e.location.indexOf("Hexagram") > 0) {
					String[] strTemp = e.location.split("Hexagram");
					m_canCurrent.SetHexagramValue(new Integer(strTemp[1]));
					e.doit = false;
				}
				else 
				if (e.location.indexOf("Trigram") > 0) {
					String[] strTemp = e.location.split("Trigram");
					
					//Integer nValue = new Integer(strTemp[1]);
					//m_canCurrent.SetHexagramValue(nValue + (nValue * 8));
					
					CTrigramValueSequencer tvs = new CTrigramValueSequencer(0);
					tvs.Value(new Integer(strTemp[1]));
					m_broText.setUrl("https://en.wikipedia.org/wiki/" + tvs.Label());
					e.doit = false;
				}
				else
					e.doit = true;
			}
		});		
	}
	
	public void SetTextUrl(final CHexagramValueSequencer hvsPrimary, int nHexagramLabel) {
		String strText = m_tabSettings.getItem(16).getText(1);
		String strUrl = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Yijing</title></head><body>" +
			"<h1>" + Sequences.strSymbols[hvsPrimary.Value()] + "  " +
/*				
			"Hatcher: " + Sequences.strHexagramLabels[DiagramSetting(15,"Hatcher")][hvsPrimary.Value()] + "</br>" +
			"Heyboer: " + Sequences.strHexagramLabels[DiagramSetting(15,"Heyboer")][hvsPrimary.Value()] + "</br>" +
			"Karcher: " + Sequences.strHexagramLabels[DiagramSetting(15,"Karcher")][hvsPrimary.Value()] + "</br>" +
			"Legge: " + Sequences.strHexagramLabels[DiagramSetting(15,"Legge")][hvsPrimary.Value()] + "</br>" +
			"Machovec: " +Sequences.strHexagramLabels[DiagramSetting(15,"Machovec")][hvsPrimary.Value()] + "</br>" +
			"Marshall: " + Sequences.strHexagramLabels[DiagramSetting(15,"Marshall")][hvsPrimary.Value()] + "</br>" +
			"Rutt: " + Sequences.strHexagramLabels[DiagramSetting(15,"Rutt")][hvsPrimary.Value()] + "</br>" +
			"Wilhelm: " + Sequences.strHexagramLabels[DiagramSetting(15,"Wilhelm")][hvsPrimary.Value()] + "</br>" +
*/
			m_canCurrent.DescribeCast() + "</h1>\n";
		
		if (strText.equals("Andrade")) {
			strUrl += "<center><img src=\"" + Andrade.strText[hvsPrimary.Value()] + "\" alt=\"" + 
			hvsPrimary.Label() + "\"></center>" + 
			"</body></html>";
			m_broText.setText(strUrl);
		}
		else
		if (strText.equals("Chinese")) {
			strUrl += Chinese.strText[hvsPrimary.Value()] +
			"</body></html>";
			m_broText.setText(strUrl);
		}
		else
		if (strText.equals("Legge")) {
			strUrl += Legge.strText[hvsPrimary.Value()];
			for(int nLine = 0; nLine < 6; ++nLine) {
				strUrl += "<h2>Line " + Integer.toString(nLine + 1) + "</h2>" +
				Legge.strLine[nLine][hvsPrimary.Value()];
			}
			strUrl += "</body></html>";
			m_broText.setText(strUrl);
		} 
		else
		if (strText.equals("Hatcher")) {
			strUrl = "C:/Src/Yijing/Text/Hatcher/Yijing-One.pdf#page=" + Sequences.nHatcherPage[hvsPrimary.Sequence()];
			m_broText.dispose();
			NewBrowser();
			m_broText.setUrl(strUrl);
		}
		else
		if (strText.equals("Heyboer")) {
			strUrl = Heyboer.strText[hvsPrimary.Value()];
			m_broText.setUrl(strUrl); 
		}
		else
		if (strText.equals("Wilhelm")) {
			String strTemp = Wilhelm.strText[hvsPrimary.Value()] + 
			"<h2>The Image</h2>" +
			Wilhelm.strImage[hvsPrimary.Value()] +
			"<h2>The Judgement</h2>" +
			Wilhelm.strJudgement[hvsPrimary.Value()]; 
			for(int nLine = 0; nLine < 6; ++nLine) {
				strTemp += "<h2>Line " + Integer.toString(nLine + 1) + "</h2>" +
				Wilhelm.strLine[nLine][hvsPrimary.Value()];
			}
			strTemp += "</body></html>";
			for (int i = 0; i < 64; ++i)
				if (i != hvsPrimary.Value()) {
					String strHref = "<a href=\"Hexagram" + i + "\">" + Sequences.strHexagramLabels[9][i] + "</a>";
					strTemp = strTemp.replaceAll("\\b(?i)" + Sequences.strHexagramLabels[9][i] + 
						"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b", strHref + "$1$2$3$4$5$6$7$8$9$10");
				}
			for (int i = 0; i < 8; ++i) {
				String strHref = "<a href=\"Trigram" + i + "\">" + Sequences.strTrigramLabels[2][i] + "</a>";
				strTemp = strTemp.replaceAll("\\b(?i)" + Sequences.strTrigramLabels[2][i] + 
					"(s)?(t)?(y)?(ty)?(ing)?(ed)?(ous)?(ment)?(ate)?(in)?\\b", strHref + "$1$2$3$4$5$6$7$8$9$10");
			}
			
			m_broText.setText(strUrl + strTemp);
		}
		else
		if (strText.equals("YellowBridge")) {
			strUrl = YellowBridge.strText[hvsPrimary.Value()];
			m_broText.setUrl(strUrl); 
		}
		else
		if (strText.equals("Regis")) {
			strUrl = Regis.strText[hvsPrimary.Value()];
			m_broText.setUrl(strUrl); 
		}
		m_strCurrentText = strText;
		
		//m_broText.setBackground(display.getSystemColor(SWT.COLOR_BLUE));
		//strUrl = m_broText.getUrl(); 
		//m_broText.traverse(SWT.TRAVERSE_PAGE_NEXT);
		// C:/Src/aframe-boilerplate-master/index.html 
		// C:/Src/IChing.308/HomePage/index.html 
	}
	
	public void EnableTable(boolean bEnable, boolean bModeOnly) {
		m_bEnableMode = bEnable;
		if (!bModeOnly) {
			m_bEnableType = bEnable;
			m_bEnableSpeed = bEnable;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private class DiagramCanvas extends Canvas {
		
		private DiagramCanvas m_canLocal;
		private CHexagramValueSequencer m_hvsCurrent;
		private CHexagramValueSequencer m_hvsCast;
		private CHexagramValueSequencer m_hvsPrimary = null;
		private CValueSequencer m_vsCurrent;
		
		private Button m_btnFirst;
		private Button m_btnLast;
		private Button m_btnInverse;
		private Button m_btnOpposite;
		private Button m_btnTransverse;
		private Button m_btnNuclear;
		private Button m_btnMove;
		private Button m_btnHome;
		
		private eDiagramMode m_edmDiagramMode = eDiagramMode.edmExplore;
		private eDiagramType m_edtDiagramType = eDiagramType.edtHexagram;
		private eDiagramColor m_edcDiagramColor = eDiagramColor.edcTrigram;
		private eDiagramSpeed m_edsDiagramSpeed = eDiagramSpeed.edsMedium;
		private eDiagramLsb m_edlDiagramLsb = eDiagramLsb.edlTop;

		private int m_nLineSequence = 1;
		private int m_nLineRatio = 1;
		private int m_nLineLabel = 3;
		private int m_nLineText = 0;	

		private int m_nTrigramSequence = 1;
		private int m_nTrigramRatio = 0;
		private int m_nTrigramLabel = 2;
		private int m_nTrigramText = 0;	

		private int m_nHexagramSequence = 2;
		private int m_nHexagramRatio = 0;
		private int m_nHexagramLabel = 9;
		private int m_nHexagramText = 6;

		private int m_nCurrentLine = 0;
		private int m_nCurrentTrigram = 0;

		private boolean m_bTimerOn = false;
		private boolean m_bForward = true;

		private Timer m_timDiagram;
		private AutoCastThread m_thrAutoCast;
		
		private Rectangle[] m_recLines = {new Rectangle(0,0,0,0),new Rectangle(0,0,0,0),new Rectangle(0,0,0,0),
			new Rectangle(0,0,0,0),new Rectangle(0,0,0,0),new Rectangle(0,0,0,0)};
		
		private Color m_colDiagram1;
		private Color m_colDiagram2;
		
		private Font m_fonHexagram = new Font(display,"Times Roman",14,SWT.NORMAL);	
		//private Font m_fonButton = new Font(display,"Wingdings",10,SWT.NORMAL); // Segoe MDL2	
		
		public CValueSequencer GetCurrentDiagram() {return m_vsCurrent;}

		public boolean GetForward() {return m_bForward;}
		//public int GetCurrentLine() {return m_nCurrentLine;}
		
		public eDiagramMode GetDiagramMode() {return m_edmDiagramMode;}
		public eDiagramType GetDiagramType() {return m_edtDiagramType;}
		public eDiagramColor GetDiagramColor() {return m_edcDiagramColor;}
		public eDiagramSpeed GetDiagramSpeed() {return m_edsDiagramSpeed;}
		public eDiagramLsb GetDiagramLsb() {return m_edlDiagramLsb;}

		public int GetLineSequence() {return m_nLineSequence;}
		public int GetLineRatio() {return m_nLineRatio;}
		public int GetLineLabel() {return m_nLineLabel;}
		public int GetLineText() {return m_nLineText;}

		public int GetTrigramSequence() {return m_nTrigramSequence;}
		public int GetTrigramRatio() {return m_nTrigramRatio;}
		public int GetTrigramLabel() {return m_nTrigramLabel;}
		public int GetTrigramText() {return m_nTrigramText;}

		public int GetHexagramSequence() {return m_nHexagramSequence;}
		public int GetHexagramRatio() {return m_nHexagramRatio;}
		public int GetHexagramLabel() {return m_nHexagramLabel;}
		public int GetHexagramText() {return m_nHexagramText;}
		
		public DiagramCanvas(Composite parent, int style) {
			super (parent, style);
			
			m_canLocal = this;
			
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
			//CHexagramValueSequencer.SetCurrentText(m_nHexagramText);
			
			//SetHexagramText(m_nHexagramText);
			
			LSBChanged(m_edlDiagramLsb.ordinal());
			
			int nLine1 = 6;
			for (int nLine = 0; nLine < 6; ++nLine) {
				--nLine1;
				m_recLines[nLine] = new Rectangle(10, 50 + (nLine1 * 55), 290, 50);
			}
			
			m_btnFirst = new Button(this, SWT.NONE);
			m_btnFirst.setText("F"); // &#xE100; \uf021 ç¬¬ä¸€ æŒ�ç»­ 
			m_btnFirst.setBounds(20, 10, 30, 30);
			//m_btnFirst.setFont(m_fonButton);
			m_btnFirst.addListener(SWT.MouseDown, e -> {
				SetFirst();
			});

			m_btnLast = new Button(this, SWT.NONE);
			m_btnLast.setText("L");
			m_btnLast.setBounds(55, 10, 30, 30);
			m_btnLast.addListener(SWT.MouseDown, e -> {
				SetLast();
			});
			
			m_btnInverse = new Button(this, SWT.NONE);
			m_btnInverse.setText("I");
			m_btnInverse.setBounds(90, 10, 30, 30);
			m_btnInverse.addListener(SWT.MouseDown, e -> {
				SetInverse();
			});

			m_btnOpposite = new Button(this, SWT.NONE);
			m_btnOpposite.setText("O");
			m_btnOpposite.setBounds(125, 10, 30, 30);
			m_btnOpposite.addListener(SWT.MouseDown, e -> {
				SetOpposite();
			});
			
			m_btnTransverse = new Button(this, SWT.NONE);
			m_btnTransverse.setText("T");
			m_btnTransverse.setBounds(160, 10, 30, 30);
			m_btnTransverse.addListener(SWT.MouseDown, e -> {
				SetTransverse();
			});
			
			m_btnNuclear = new Button(this, SWT.NONE);
			m_btnNuclear.setText("N");
			m_btnNuclear.setBounds(195, 10, 30, 30);
			m_btnNuclear.addListener(SWT.MouseDown, e -> {
				SetNuclear();
			});
			
			m_btnMove = new Button(this, SWT.NONE);
			m_btnMove.setText("M");
			m_btnMove.setBounds(230, 10, 30, 30);
			m_btnMove.addListener(SWT.MouseDown, e -> {
				SetMove();
			});

			m_btnHome = new Button(this, SWT.NONE);
			m_btnHome.setText("H");
			m_btnHome.setBounds(265, 10, 30, 30);
			m_btnHome.addListener(SWT.MouseDown, e -> {
				SetHome();
			});

			m_hvsCurrent.First();
			m_vsCurrent = m_hvsCurrent;
			
			addControlListener(new ControlListener() {
				@Override
				public void controlMoved(ControlEvent e) {
				}
				@Override
				public void controlResized(ControlEvent e) {
					//setBounds(10, 30, 310, 370);
					//setSize(200, 100);
				}
			});
			
			addFocusListener(new FocusListener() {
				@Override
				public void focusGained(FocusEvent e) {
					//m_canCurrent = m_canLocal;
				}
				@Override
				public void focusLost(FocusEvent e) {
				}
			});
			
			addMouseListener(new MouseAdapter() {
				@Override
				public void mouseDown(MouseEvent e) {
					if ((m_edmDiagramMode != eDiagramMode.edmMindCast) && (m_edmDiagramMode != eDiagramMode.edmAutoCast))
					{
						int nLine = 6;
						boolean bFound = false;
						Point p = new Point(e.x,e.y);
						m_bForward = e.x > m_recLines[0].width / 2;
						for(int i = 0; i < 6; ++i) {
							if(m_recLines[--nLine].contains(p)) {
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
							m_timDiagram.schedule(new DiagramTimerTask(m_canLocal),0,m_nSpeeds[m_edsDiagramSpeed.ordinal()]);
						}
					}
				}
				
				@Override
				public void mouseUp(MouseEvent e) {
					if (m_edmDiagramMode == eDiagramMode.edmExplore) {
						m_hvsPrimary = null;
						SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
					}
					if (m_bTimerOn && (m_edmDiagramMode != eDiagramMode.edmAnimate) &&
							(m_edmDiagramMode != eDiagramMode.edmMindCast) && (m_edmDiagramMode != eDiagramMode.edmAutoCast)) {
							m_bTimerOn = false;
							m_timDiagram.cancel();
							if ((m_edmDiagramMode == eDiagramMode.edmTouchCast) && (m_nCurrentLine == 5))
								EndCast();
							if (m_edtDiagramType == eDiagramType.edtLine)
								;//SecondaryHexagram
						}
				}
			});
			
			addPaintListener(new PaintListener() {
				public void paintControl(PaintEvent e) {
					Color colColor;
				    //Rectangle rect = ((Canvas) e.widget).getBounds();
					
					if (m_edcDiagramColor == eDiagramColor.edcMono)
						m_colDiagram1 = e.display.getSystemColor(SWT.COLOR_DARK_GRAY);
					else
					if (m_edcDiagramColor == eDiagramColor.edcDual) {
						m_colDiagram1 = e.display.getSystemColor(SWT.COLOR_GRAY);
						m_colDiagram2 = e.display.getSystemColor(SWT.COLOR_DARK_GRAY);
					} else
					if (m_edcDiagramColor == eDiagramColor.edcRgb)
						m_colDiagram1 = new Color(display,RgbColor());
					else {
						m_colDiagram1 = e.display.getSystemColor(TrigramColor(m_hvsCurrent.Trigram(0).Value()));
						m_colDiagram2 = e.display.getSystemColor(TrigramColor(m_hvsCurrent.Trigram(1).Value()));
					}
					colColor = m_colDiagram1;
			
					e.gc.setLineWidth(1);
					for (int nLine = 0; nLine < 6; ++nLine) {
						boolean bYang = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3).Value() % 2 == 1;
						boolean bOld = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3).IsMoving();

				        if ((m_edcDiagramColor == eDiagramColor.edcTrigram) && (nLine == 3))
							colColor = m_colDiagram2;
						else
						if (m_edcDiagramColor == eDiagramColor.edcDual)
							colColor = bYang ? m_colDiagram1 : m_colDiagram2;
				        
				        e.gc.setForeground(colColor);
				        e.gc.setBackground(colColor);

						if(bYang)
							e.gc.fillRoundRectangle(10, m_recLines[nLine].y, 290, 50, 20, 20);
						else {
					        e.gc.fillRoundRectangle(10, m_recLines[nLine].y, 120, 50, 20, 20);
					        e.gc.fillRoundRectangle(180, m_recLines[nLine].y, 120, 50, 20, 20);
						}
						if(bOld) {
							if(bYang) {
								e.gc.setForeground(e.display.getSystemColor(SWT.COLOR_WIDGET_BACKGROUND));
								e.gc.setBackground(e.display.getSystemColor(SWT.COLOR_WIDGET_BACKGROUND));
							}
							e.gc.fillOval(130, m_recLines[nLine].y, 50, 50);
						}
					}

			        e.gc.setForeground(e.display.getSystemColor(SWT.COLOR_BLACK));
			        e.gc.setBackground(e.display.getSystemColor(SWT.COLOR_WHITE)); // COLOR_WIDGET_BACKGROUND
			        e.gc.setFont(m_fonHexagram);
			        
			        e.gc.drawText(m_hvsCurrent.SequenceStr() + ". " + m_hvsCurrent.Label() + " (" + m_hvsCurrent.ValueStr() + ")",
		        		10,m_recLines[0].y + 60); 
		        		
			        e.gc.drawText(Integer.toString(m_hvsCurrent.Trigram(1).Sequence() + 1) + ". " +
			        	m_hvsCurrent.Trigram(1).Label() +
		        		" (" + Integer.toString(Sequences.nTrigramSequences[0][m_hvsCurrent.Trigram(1).Value()]) + ")",
		        		10,m_recLines[0].y + 90); 
			        
			        e.gc.drawText(Integer.toString(m_hvsCurrent.Trigram(0).Sequence() + 1) + ". " + 
			        	m_hvsCurrent.Trigram(0).Label() +
		        		" (" + Integer.toString(Sequences.nTrigramSequences[0][m_hvsCurrent.Trigram(0).Value()]) + ")",
		        		10,m_recLines[0].y + 120); 
				}
			});
		}
		
		public void UpdateSettings() {
			m_tabSettings.getItem(0).setText(1, Sequences.DiagramSetting(0,GetDiagramMode().ordinal()));
			m_tabSettings.getItem(1).setText(1, Sequences.DiagramSetting(1,GetDiagramType().ordinal()));
			m_tabSettings.getItem(2).setText(1, Sequences.DiagramSetting(2,GetDiagramColor().ordinal()));
			m_tabSettings.getItem(3).setText(1, Sequences.DiagramSetting(3,GetDiagramSpeed().ordinal()));
			m_tabSettings.getItem(4).setText(1, Sequences.DiagramSetting(4,GetDiagramLsb().ordinal()));
	
			m_tabSettings.getItem(5).setText(1, Sequences.DiagramSetting(5,GetLineSequence()));
			m_tabSettings.getItem(6).setText(1, Sequences.DiagramSetting(6,GetLineRatio()));
			m_tabSettings.getItem(7).setText(1, Sequences.DiagramSetting(7,GetLineLabel()));
			m_tabSettings.getItem(8).setText(1, Sequences.DiagramSetting(8,GetLineText()));
			
			m_tabSettings.getItem(9).setText(1, Sequences.DiagramSetting(9,GetTrigramSequence()));
			m_tabSettings.getItem(10).setText(1, Sequences.DiagramSetting(10,GetTrigramRatio()));
			m_tabSettings.getItem(11).setText(1, Sequences.DiagramSetting(11,GetTrigramLabel()));
			m_tabSettings.getItem(12).setText(1, Sequences.DiagramSetting(12,GetTrigramText()));
	
			m_tabSettings.getItem(13).setText(1, Sequences.DiagramSetting(13,GetHexagramSequence()));
			m_tabSettings.getItem(14).setText(1, Sequences.DiagramSetting(14,GetHexagramRatio()));
			m_tabSettings.getItem(15).setText(1, Sequences.DiagramSetting(15,GetHexagramLabel()));
			m_tabSettings.getItem(16).setText(1, Sequences.DiagramSetting(16,GetHexagramText()));
		}
		
		@SuppressWarnings("unused")
		public void ModeChanged(int nMode) {
			m_edmDiagramMode = eDiagramMode.values()[nMode];
			EnableControls(true);
			if (m_edmDiagramMode == eDiagramMode.edmAnimate) {
				m_timDiagram = new Timer();
				m_timDiagram.schedule(new DiagramTimerTask(this),0,m_nSpeeds[m_edsDiagramSpeed.ordinal()]);
				m_bTimerOn = true;
			}
			else {
				if (m_bTimerOn)
					m_timDiagram.cancel();
				m_bTimerOn = false;
				if ((m_edmDiagramMode == eDiagramMode.edmTouchCast) || (m_edmDiagramMode == eDiagramMode.edmMindCast) ||
					(m_edmDiagramMode == eDiagramMode.edmAutoCast)) {
					SetCurrentLine(0, true);
					SetType(eDiagramType.edtLine.ordinal());
					SetSpeed(2);
					m_canCurrent.UpdateSettings();
					EnableControls(false);
				}
				if (m_edmDiagramMode == eDiagramMode.edmTouchCast)
					EnableTable(true,true);
				if (m_edmDiagramMode == eDiagramMode.edmMindCast) {
					;//m_thrMindCast = new MindCastThread();
					//m_thrMindCast.start();
				}
				if (m_edmDiagramMode == eDiagramMode.edmAutoCast) {
					m_thrAutoCast = new AutoCastThread(this);
					m_thrAutoCast.start();
				}
			}
			redraw();
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
			redraw();
		}

		@SuppressWarnings("unused")
		public void SpeedChanged(int nSpeed) {
			m_edsDiagramSpeed = eDiagramSpeed.values()[nSpeed];
			if (m_bTimerOn) {
				m_timDiagram.cancel();
				m_timDiagram = new Timer();
				m_timDiagram.schedule(new DiagramTimerTask(this),0,m_nSpeeds[nSpeed]);
			}
			redraw();
		}

		@SuppressWarnings("unused")
		public void ColorChanged(int nColor) {
			m_edcDiagramColor = eDiagramColor.values()[nColor];
			redraw();
		}

		public void LSBChanged(int nLsb) {
			m_edlDiagramLsb = eDiagramLsb.values()[nLsb];
			Sequences.SetLSB(m_edlDiagramLsb == eDiagramLsb.edlTop);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetLineSequence(int nSequence) {
			m_nLineSequence = nSequence;
			CLineValueSequencer.SetCurrentSequence(nSequence);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetLineRatio(int nRatio) {
			m_nLineRatio = nRatio;
			CLineValueSequencer.SetCurrentRatio(nRatio);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetLineLabel(int nLabel) {
			CLineValueSequencer.SetCurrentLabel(nLabel);
			m_nLineLabel = nLabel;
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetLineText(int nText) {
			m_nLineText = nText;
		}

		@SuppressWarnings("unused")
		public void SetTrigramSequence(int nSequence) {
			m_nTrigramSequence = nSequence;
			CTrigramValueSequencer.SetCurrentSequence(nSequence);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetTrigramRatio(int nRatio) {
			m_nTrigramRatio = nRatio;
			CTrigramValueSequencer.SetCurrentRatio(nRatio);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetTrigramLabel(int nLabel) {
			m_nTrigramLabel = nLabel;
			CTrigramValueSequencer.SetCurrentLabel(nLabel);
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetTrigramText(int nText) {
			m_nTrigramText = nText;
		}

		@SuppressWarnings("unused")
		public void SetHexagramSequence(int nSequence) {
			m_nHexagramSequence = nSequence;
			CHexagramValueSequencer.SetCurrentSequence(nSequence);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetHexagramRatio(int nRatio) {
			m_nHexagramRatio = nRatio;
			CHexagramValueSequencer.SetCurrentRatio(nRatio);
			m_hvsCurrent.Update();
			redraw();
		}

		@SuppressWarnings("unused")
		public void SetHexagramLabel(int nLabel) {
			m_nHexagramLabel = nLabel;
			CHexagramValueSequencer.SetCurrentLabel(nLabel);
			SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
			redraw();
		}

		//@SuppressWarnings("unused")
		public void SetHexagramText(int nText) {
			m_nHexagramText = nText;
			SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
		}
		
		public void SetHexagramValue(int nValue) {
			m_hvsCurrent.Value(nValue);
			m_hvsCurrent.Update();
			Transition();
		}

		private void SetMode(int nMode) {
			m_edmDiagramMode = eDiagramMode.values()[nMode]; 
		}
		
		private void SetType(int nType) {
			m_edtDiagramType = eDiagramType.values()[nType]; 
		}
		
		private void SetSpeed(int nSpeed) {
			m_edsDiagramSpeed = eDiagramSpeed.values()[nSpeed];
		}

		private void Transition() {
			m_hvsPrimary = null;
			SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
			redraw();
		}

		public void SetFirst() {
			m_vsCurrent.First();
			Transition();
		}

		public void SetPrevious() {
			m_vsCurrent.Previous(false);
			Transition();
		}

		public void SetNext() {
			m_vsCurrent.Next(false);
			Transition();
		}

		public void SetLast() {
			m_vsCurrent.Last();
			Transition();
		}

		public void SetInverse() {
			m_vsCurrent.Inverse();
			Transition();
		}

		public void SetOpposite() {
			m_vsCurrent.Opposite();
			Transition();
		}

		public void SetTransverse() {
			m_vsCurrent.Transverse();
			Transition();
		}

		public void SetNuclear() {
			m_vsCurrent.Nuclear();
			Transition();
		}

		private void SetMove() {
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
			SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
			redraw();
		}

		public void SetHome() {
			if (m_hvsCast != null) {
				m_hvsCurrent = new CHexagramValueSequencer(m_hvsCast);
				m_vsCurrent = m_hvsCurrent;
				SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
				redraw();
			}
		}
		
		private void SetCurrentLine(int nLine, boolean bDiagram) {
			m_nCurrentLine = nLine;
			m_nCurrentTrigram = m_nCurrentLine / 3;
			if (bDiagram)
				m_vsCurrent = m_hvsCurrent.Trigram(nLine / 3).Line(nLine % 3);
		}
		
		private void EnableControls(boolean bEnable) {
			EnableTable(bEnable,false);
			m_btnFirst.setEnabled(bEnable);
			m_btnLast.setEnabled(bEnable);
			m_btnInverse.setEnabled(bEnable);
			m_btnOpposite.setEnabled(bEnable);
			m_btnTransverse.setEnabled(bEnable);
			m_btnNuclear.setEnabled(bEnable);
			m_btnMove.setEnabled(bEnable);
			m_btnHome.setEnabled(bEnable);
		}
		
		private int TrigramColor(int nValue) {
			switch (nValue) {
				case 0: // earth
					return SWT.COLOR_DARK_GRAY;
				case 1: // thunder
					return SWT.COLOR_GREEN;
				case 2: // water
					return SWT.COLOR_BLUE;
				case 3: // lake
					return SWT.COLOR_CYAN;
				case 4: // mountain
					return SWT.COLOR_MAGENTA;
				case 5: // fire
					return SWT.COLOR_RED;
				case 6: // wind
					return SWT.COLOR_YELLOW;
				case 7: // heaven
					break;
			}
			return SWT.COLOR_GRAY;
		}

		private RGBA RgbColor() {
			boolean bLower, bUpper;
			int[] nRgb = { 0, 0, 0 };
			for (int i = 0; i < 3; ++i) {
				bUpper = m_hvsCurrent.Trigram(1).Line(i).Value() % 2 == 1;
				bLower = m_hvsCurrent.Trigram(0).Line(i).Value() % 2 == 1;
				nRgb[i] = bLower ? bUpper ? 0xFF : 0x55 : bUpper ? 0xAA : 0x00;
			}
			if((nRgb[0] == 0) && (nRgb[1] == 0) && (nRgb[2] == 0))
				return display.getSystemColor(SWT.COLOR_DARK_GRAY).getRGBA();
			if((nRgb[0] == 255) && (nRgb[1] == 255) && (nRgb[2] == 255))
				return display.getSystemColor(SWT.COLOR_GRAY).getRGBA();
			return new RGBA(nRgb[0], nRgb[1], nRgb[2], 0xFF);
		}

		public String DescribeCast()
		{
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
			String strTemp = "#Yijing ☯️  " + DescribeCast() + "\n\nSWT v7.0.0  ";
			strTemp += Sequences.DiagramSetting(0,m_edmDiagramMode.ordinal()) + " "  + Sequences.DiagramSetting(13,m_nHexagramSequence) + " " + 
				Sequences.DiagramSetting(4,m_edlDiagramLsb.ordinal()) + " " + Sequences.DiagramSetting(6,m_nLineRatio) + " " + 
				Sequences.DiagramSetting(15,m_nHexagramLabel) + "\n\nhttps://play.google.com/store/apps/details?id=org.yijing\n\n";
			System.out.println(strTemp);

			m_hvsCast = new CHexagramValueSequencer(m_hvsCurrent);
			m_vsCurrent = m_hvsCurrent;
			
			SetMode(eDiagramMode.edmExplore.ordinal());
			SetType(eDiagramType.edtHexagram.ordinal());
			SetSpeed(1);
			m_canCurrent.UpdateSettings();
			SetTextUrl(m_hvsCurrent,m_nHexagramLabel);
			EnableControls(true);

			//await Task.Delay(1000);
			//MainPage.NavigateTo(typeof(TextPage));
		}
		
		
/*
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

		String[][] strText = new String[3][];
		String[][] strImage = new String[3][];
		String[][] strJudgement = new String[3][];
		String[][][] strLine = new String[7][3][];
		
		@SuppressWarnings("unused")
		private void ImportData1() {
			int nImage = -1;
			int nJudgment = -1;
			int nLine = -1;
			int nText = -1;
			
			String strTemp = "";
			strText = new String[1][64];
			for (int i = 0; i < 64; ++i) {
				m_hvsCurrent.Sequence(i);
				int nValue = m_hvsCurrent.Value();
				strText[0][nValue] = Sequences.strSymbols[i];
			}
			for (int i = 0; i < 64; ++i) {
				strTemp += "\"" + strText[0][i] + "\",";
			}
			int x = 0;
			if (true)
				return;
			
			try {
				try(BufferedReader br = new BufferedReader(
					new FileReader("C:/Src/Yijing/Yijing.ecl/Text/Access/Book1.txt"))) {
				    String line;
				    while ((line = br.readLine()) != null) {
				    	if (line.startsWith("Image"))
				    		strImage[++nImage] = line.split("\t");
				    	else
				    	if (line.startsWith("Judgm"))
				    		strJudgement[++nJudgment] = line.split("\t");
				    	else
				    	if (line.startsWith("Line")) {
				    		int i = new Integer(line.substring(4,5));
				    		strLine[i][++nLine] = line.split("\t");
				    		if (nLine == 2)
				    			nLine = -1;
				    	}
				    	else
				    	if (line.startsWith("Text"))
				    		strText[++nText] = line.split("\t");
				    }
				}
				WriteClass(0);
				WriteClass(2);
/ *				
				try(BufferedReader br = new BufferedReader(
					new FileReader("C:/Src/Yijing/Yijing.ecl/Text/Access/Labels.txt"))) {
					try(BufferedWriter bw = new BufferedWriter(
						new FileWriter("C:/Src/Yijing/Yijing.ecl/Yijing/src/org/yijing/Labels.txt"))) {
						String line;
						boolean bWrite = true;
					    while ((line = br.readLine()) != null) {
					    	line = line.trim();
					    	if ((line.length() > 0) &&
				    			!line.equals("<td>") && !line.equals("</td>") &&
				    			!line.equals("<tr>") && !line.equals("</tr>") &&
				    			!line.equals("<table>") && !line.equals("</table>") &&
				    			!line.equals("<tbody>") && !line.equals("</tbody>") &&
				    			!line.equals("<br>") && !line.equals("</br>") &&
				    			!line.startsWith("<div style=") &&
					    		!line.equals("<td style=\"vertical-align: top;\"><br>")) {

					    		if (line.equals("<meta content=\"text/html; charset=windows-1252\""))
					    			bWrite = false;
					    		if (line.startsWith("<td style=\"vertical-align: top;\">"))
					    			bWrite = true;
					    		
					    		if(bWrite) {
					    			if (line.startsWith("<td style=\"vertical-align: top;\">"))
					    				line = line.substring(33);
					    			if (line.startsWith("<td style=\" vertical-align: top;\">"))
					    				line = line.substring(34);
					    			bw.write(line);
					    			if (line.endsWith("<br>") || line.endsWith("</td>"))
					    				bw.write("\n");
					    			else
					    				bw.write(" ");
					    		}
					    	}
					    }
					}
				}
* /				
			}
			catch (Exception e) {}
		}
		
		@SuppressWarnings("unused")
		private void ImportData2() {
			int nSequence = -1;
			int nValue = 0;

			String strAndrade = "";
			String strHeyboer = "";
			String strYellowBridge = "";

			try {

				strText[0] = strAndrade.split("\t");
				WriteClass(0);
				strText[0] = strHeyboer.split("\t");
				WriteClass(0);
				strText[0] = strYellowBridge.split("\t");
				WriteClass(0);
				
				strText = new String[3][67];
				strText[0][0] = "Text";
				strText[0][1] = "100";
				strText[0][2] = "Chinese";
				
				try(BufferedReader br = new BufferedReader(
					new	FileReader("C:/Src/Yijing/Yijing.ecl/Yijing/src/org/yijing/Yijing.txt"))) {
				    String line;
				    while ((line = br.readLine()) != null) {
				    	if (line.startsWith("Hexagram")) {
							m_hvsCurrent.Sequence(++nSequence);
							nValue = m_hvsCurrent.Value();
				    		strText[0][3 + nValue] = "";
				    	}
				    	else
				    	if ((line = line.trim()).length() > 0)
				    		strText[0][3 + nValue] += line + "</br></br>";
				    }
				}
				WriteClass(0);
			}
			catch (Exception e) {}
		}
		
		@SuppressWarnings("unused")
		private void ImportData3() {
			String[] strAuthor = new String[87];
			String[][] strLabel = new String[87][64];

			boolean bAuthor = true;
			int nAuthor = -1;
			int nHexagram = -1;
			int nLine = 0;
			
			try {
				try(BufferedReader br = new BufferedReader(
					new	FileReader("C:/Src/Yijing/Yijing.ecl/Text/Access/Labels1.txt"))) {
					try(BufferedWriter bw = new BufferedWriter(
						new FileWriter("C:/Src/Yijing/Yijing.ecl/Yijing/src/org/yijing/Labels.java"))) {
						String line;
					    while ((line = br.readLine()) != null) {
					    	++nLine;
			    			if ((line = line.trim()).length() > 0) {
				    			if (line.equals("Agmuller")) {
				    				bAuthor = true;
				    				nAuthor = -1;
				    				++nHexagram;
				    			}
					    		if (bAuthor) {
					    			if (strAuthor[++nAuthor] !=  null)
					    				if (!strAuthor[nAuthor].equals(line))
					    					return;
					    			strAuthor[nAuthor] = line;
					    			bAuthor = false;
					    		}
					    		else {
					    			strLabel[nAuthor][nHexagram] = line;
					    			bAuthor = true;
					    		}
						    }
					    }
					
						bw.write("package org.yijing;\n\n");
						bw.write("public class Labels\n");
						bw.write("{\n\n");
						
						bw.write("\tpublic static String[] strAuthor = {");
						for (int i = 0; i < 87; ++i)
							bw.write("\"" + strAuthor[i] + "\",");
						bw.write("};\n\n");
						
						bw.write("\tpublic static String[][] strLabel = {\n");
						for (int i = 0; i < 87; ++i) {
							bw.write("\t\t{");
							for (int j = 0; j < 64; ++j) {
								m_hvsCurrent.Value(j);
								int nSequence = m_hvsCurrent.Sequence();
								bw.write("\"" + strLabel[i][nSequence] + "\",");
							}
							bw.write("},\n");
						}
						bw.write("\t};\n");
						bw.write("}\n");
					}
				}
			}
			catch (Exception e) {}
		}
		
		private void WriteClass(int nIndex) throws IOException {
			try(BufferedWriter bw = new BufferedWriter(
				new FileWriter("C:/Src/Yijing/Yijing.ecl/Yijing/src/org/yijing/" + strText[nIndex][2] + ".java"))) {
				bw.write("package org.yijing;\n\n");
				bw.write("public class " + strText[nIndex][2] + "\n");
				bw.write("{\n\n");

				bw.write("\tpublic static String[] strText = {\n");
				for (int i = 3; i < 67; ++i)
					bw.write("\t\t"	+ "\"" + strText[nIndex][i]	+ "\",\n");
				bw.write("\t};\n\n");
				
				if (strImage[0] != null) {
					bw.write("\tpublic static String[] strImage = {\n");
					for (int i = 3; i < 67; ++i)
						bw.write("\t\t"	+ "\"" + strImage[nIndex][i]	+ "\",\n");
					bw.write("\t};\n\n");
	
					bw.write("\tpublic static String[] strJudgement = {\n");
					for (int i = 3; i < 67; ++i)
						bw.write("\t\t"	+ "\"" + strJudgement[nIndex][i]	+ "\",\n");
					bw.write("\t};\n\n");
					
					bw.write("\tpublic static String[][] strLine = {{\n");
					for(int nLine = 0; nLine < 6; ++nLine) {
						for (int i = 3; i < 67; ++i)
							bw.write("\t\t"	+ "\"" + strLine[nLine][nIndex][i]	+ "\",\n");
						bw.write("\t\t},{\n");
					}
					bw.write("\t};\n");
				}
				
				bw.write("}\n");
			}			
		}
*/		
	}
	
	private class DiagramTimerTask extends TimerTask {
		
		private DiagramCanvas m_canvas;
		
		public DiagramTimerTask(DiagramCanvas canvas) {
			m_canvas = canvas;
		}
		
		public void run() {
			boolean bRatio = /*true ||*/ (m_canvas.GetDiagramMode() == eDiagramMode.edmTouchCast) ||
				(m_canvas.GetDiagramMode() == eDiagramMode.edmMindCast) ||
				(m_canvas.GetDiagramMode() == eDiagramMode.edmAutoCast);
			if (m_canvas.GetForward())
				m_canvas.GetCurrentDiagram().Next(bRatio);
			else
				m_canvas.GetCurrentDiagram().Previous(bRatio);
			
			display.asyncExec(new Runnable() {
				public void run() {
					m_canvas.redraw();
				}
			});			
		}
	}

	private class AutoCastThread extends Thread implements Runnable {
		
		private DiagramCanvas m_canvas;
		
		public AutoCastThread(DiagramCanvas canvas) {
			m_canvas = canvas;
		}

		public void run() {
			Random r = new Random(System.currentTimeMillis());
			for (int i = 0; i < 6; ++i) {
				m_canvas.SetCurrentLine(i, true);
				Timer timDiagram = new Timer();
				timDiagram.schedule(new DiagramTimerTask(m_canvas), 0, m_nSpeeds[m_canvas.GetDiagramSpeed().ordinal()]);
				try {
					Thread.sleep((r.nextInt(5) + 1) * 1000 + r.nextInt(1000));
				}
				//catch(InterruptedException ie) {
					//timDiagram.cancel();
					//break;
				//}
				catch(Exception e) {e.printStackTrace();}
				timDiagram.cancel();
			}
			display.asyncExec(new Runnable() {
				public void run() {
					m_canvas.EndCast();
				}
			});			
		}
	}
}
