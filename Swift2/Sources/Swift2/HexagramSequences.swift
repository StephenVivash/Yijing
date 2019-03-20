
//public class CHexagramSequences {
//}

public struct CHexagram : Comparable {

	public init() {
	}

	public init(description: String, count: Int = 0) {
		self.description = description
		self.count = count
	}

	public static func < (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.description < rhs.description
	}

	public static func == (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.description == rhs.description
	}

	public mutating func Add() {
		count += 1
	}

	public var count = 0
	public var description = ""
}

public class CHexagramArray {

	public init() {
		let hvs1 = CHexagramValueSequencer()
		hvs1.First()
		for _ in 0 ... 63 {
			for s in 0 ... 63 {
				let hvs2 = CHexagramValueSequencer(hvs: hvs1)
				for l in 0 ... 5 {
					if (((s & (1 << l)) >> l) == 1) {
						hvs2.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(bRatio: false)
					}
				}
				Add(description: hvs2.HexagramId(bValueId: true))
			}
			hvs1.Next()
		}
		hexagramArray.sort()
	}

	public func MultiCast(count: Int) {
		let hvs = CHexagramValueSequencer()
		for _ in 0 ... count - 1 {
			AutoCast(hvs: hvs)
			let h = CHexagram(description: hvs.HexagramId(bValueId: true))
			if let nIndex = binarySearch(inputArr: hexagramArray, searchItem: h) {
				hexagramArray[nIndex].Add()
			}
			hvs.Move()
		}
	}
	
	public func HexagramArray() -> [CHexagram] {
		var ha = Array(repeating: CHexagram(), count: 4096)
		var i = -1;
		for h in hexagramArray {
			i += 1
			let hvs = CHexagramValueSequencer(strValue: h.description)
			ha[i] = CHexagram(description: hvs.DescribeCast(), count: h.count)
		}
		ha.sort()
		return ha
	}

	private func AutoCast(hvs: CHexagramValueSequencer) {
		//var r: Random = true ? Sequences.m_ranSession : Random(DateTime.Now.Millisecond)
		for l in 0 ... 5 {
			let lvs = hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3)
			let count = Int.random(in: 101 ... 599)
			for _ in 0 ... count {
				lvs.Next(bRatio: true)
			}
		}
	}

	private func Add(description: String) {
		count += 1
		hexagramArray[count] = CHexagram(description: description)
	}

	private func binarySearch<T:Comparable>(inputArr: Array<T>, searchItem: T) -> Int? {
		var lowerIndex = 0
		var upperIndex = inputArr.count - 1
		while true {
			let currentIndex = (lowerIndex + upperIndex) / 2
			if inputArr[currentIndex] == searchItem {
				return currentIndex
			} else if lowerIndex > upperIndex {
				return nil
			} else {
				if inputArr[currentIndex] > searchItem {
					upperIndex = currentIndex - 1
				} else {
					lowerIndex = currentIndex + 1
				}
			}
		}
	}

	private var hexagramArray = Array(repeating: CHexagram(), count: 4096)
	private var count = -1
}
