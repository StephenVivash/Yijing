
import Foundation

func Test() {
	
	Sequences.Initialise()
	
	let lvs = CLineValueSequencer(nValue: 0)
	lvs.Value = 0
	print ("\nlvs v:\(lvs.First().Value), s:\(lvs.Sequence)")
	for _ in 0...3 {
		print ("lvs v:\(lvs.Next().Value), s:\(lvs.Sequence)")
	}

	let tvs = CTrigramValueSequencer(nValue: 0)
	tvs.Value = 0
	print ("\ntvs v:\(tvs.First().Value), s:\(tvs.Sequence)")
	for _ in 0...7 {
		print ("tvs v:\(tvs.Next().Value), s:\(tvs.Sequence)")
	}
	
	let hvs1 = CHexagramValueSequencer(nValue: 0)
	hvs1.Value = 0
	print ("\nhvs v:\(hvs1.First().Value), s:\(hvs1.Sequence)")
	for _ in 0...63 {
		print ("hvs v:\(hvs1.Next().Value), s:\(hvs1.Sequence)")
	}

	hvs1.Value = 42
	hvs1.Update()
	let hvs2 = CHexagramValueSequencer(hvs: hvs1)
	
	printHexagram(label: "\nhvs1", hvs: hvs1)
	printHexagram(label: "hvs2", hvs: hvs2)
	hvs2.Inverse()
	hvs2.Trigram(nIndex: 0).Line(nIndex: 1).Next()
	hvs2.Trigram(nIndex: 1).Line(nIndex: 1).Next()

	printHexagram(label: "hvs2", hvs: hvs2)
	hvs2.Previous().Transverse()
	printHexagram(label: "hvs2", hvs: hvs2)
	print ("")

	//var hvs3 = CHexagramValueSequencer(StrValue: "63")

	let ha: CHexagramArray = CHexagramArray()
	ha.MultiCast(nCount: 100);
	for h in ha.HexagramArray() {
		if h.Count > 0 {
			print("\(String(format: "%4d", h.Count)) \(h.m_strPrimary)"); // DescribeCast
		}
	}
}

func printHexagram(label: String, hvs: CHexagramValueSequencer) {
	print ("\(label):\(hvs.DescribeCast()) tvs0:\(hvs.Trigram(nIndex: 0).Value) tvs1:\(hvs.Trigram(nIndex: 1).Value)")
}

Test()
