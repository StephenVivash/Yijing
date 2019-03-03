
import Foundation

func Test() {
	
	Sequences.Initialise()
	
	CLineValueSequencer.SetCurrentRatio(nRatio: 5); // 0 - 5 "Equal", "Coin", "Yarrow", "Marbles", "Yin", "Yang"
	CHexagramValueSequencer.SetCurrentSequence(nSequence: 2); // 0 - 3 "Numeric", "Fuxi", "Wen", "Mystery"

	let ha: CHexagramArray = CHexagramArray()
	ha.MultiCast(nCount: 10000);
	for h in ha.HexagramArray() {
		if h.m_nCount > 0 {
			print("\(String(format: "%4d", h.m_nCount)) \(h.m_strPrimary)");
		}
	}
}

Test()
