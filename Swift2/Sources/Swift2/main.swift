
import Foundation

func Test() {
	
	Sequences.Initialise()
	
	CLineValueSequencer.SetCurrentRatio(nRatio: 5); // 0 - 5 "Equal", "Coin", "Yarrow", "Marbles", "Yin", "Yang"
	CHexagramValueSequencer.SetCurrentSequence(nSequence: 2); // 0 - 3 "Numeric", "Fuxi", "Wen", "Mystery"

	var total = 0
	var coverage = 0
	let ha = CHexagramArray()
	let start = Date()
	ha.MultiCast(count: 30000);
	let time = abs(Int(start.timeIntervalSinceNow))
	for h in ha.HexagramArray() {
		if h.count > 0 {
			print("\(String(format: "%4d", h.count)) \(h.description)");
			total += h.count
			coverage += 1
		}
	}
	print("\nTotal: \(total) Coverage: \(String(format: "%0.2f",Double(coverage) / 40.96))% Time: \(time)s")
}

Test()
