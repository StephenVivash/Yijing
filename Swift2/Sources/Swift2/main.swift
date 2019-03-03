
import Foundation

func Test() {
	
	Sequences.Initialise()
	
	CLineValueSequencer.SetCurrentRatio(nRatio: 5); // 0 - 5 "Equal", "Coin", "Yarrow", "Marbles", "Yin", "Yang"
	CHexagramValueSequencer.SetCurrentSequence(nSequence: 0); // 0 - 3 "Numeric", "Fuxi", "Wen", "Mystery"

	var total = 0
	var coverage = 0
	let ha: CHexagramArray = CHexagramArray()
	ha.MultiCast(nCount: 2000);
	for h in ha.HexagramArray() {
		if h.m_nCount > 0 {
			print("\(String(format: "%4d", h.m_nCount)) \(h.m_strPrimary)");
			total += h.m_nCount
			coverage += 1
		}
	}
	print("\nTotal: \(total) Coverage: \(coverage)")
}

Test()
