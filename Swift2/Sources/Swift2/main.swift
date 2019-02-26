
import Foundation

func Test() {
	
	let vs = CValueSequencer(nInnerSequencers: 0, nValues: 4, nValue: 0)
	vs.Value = 0
	print ("\nvs v:\(vs.First().Value), s:\(vs.Sequence)")
	for _ in 0...3 {
		print ("vs v:\(vs.Next().Value), s:\(vs.Sequence)")
	}
	
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
	let hvs2 = CHexagramValueSequencer(hvs: hvs1)
	print ("hvs1:\(hvs1.Value), hvs2:\(hvs2.Value)")
	
}

Test()
