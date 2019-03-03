
public class CHexagramSequences {
}

public class CHexagram : Comparable {

	public init() {
	}

	public init(strPrimary: String, nCount: Int = 0) {
		m_strPrimary = strPrimary
		m_nCount = nCount
	}

	public static func < (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.m_strPrimary < rhs.m_strPrimary
	}

	public static func == (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.m_strPrimary == rhs.m_strPrimary
	}

	public func Add() {
		m_nCount += 1
	}

	public var m_nCount: Int = 0
	public var m_strPrimary = ""
}

public class CHexagramArray {

	public init() {
		let hvsPrimary: CHexagramValueSequencer = CHexagramValueSequencer(nValue: 63)
		hvsPrimary.First()
		for _ in 0 ... 63 {
			for s in 0 ... 63 {
				let hvs: CHexagramValueSequencer = CHexagramValueSequencer(hvs: hvsPrimary)
				for l in 0 ... 5 {
					if (((s & (1 << l)) >> l) == 1) {
						hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(bRatio: false)
					}
				}
				Add(strPrimary: hvs.HexagramId(bValueId: true))
			}
			hvsPrimary.Next()
		}
		m_arrHexagram.sort()
	}

	public func Add(strPrimary: String) {
		m_nCount += 1
		m_arrHexagram[m_nCount] = CHexagram(strPrimary: strPrimary)
	}

	public func MultiCast(nCount: Int) {
		let hvs: CHexagramValueSequencer = CHexagramValueSequencer(nValue: 63)
		for _ in 0 ... nCount - 1 {
			AutoCast(hvs: hvs)
			let h: CHexagram = CHexagram(strPrimary: hvs.HexagramId(bValueId: true))
			if let nIndex = binarySearch(inputArr: m_arrHexagram, searchItem: h) {
				m_arrHexagram[nIndex].Add()
			}
		}
	}
	
	public func AutoCast(hvs: CHexagramValueSequencer) {
		//var r: Random = true ? Sequences.m_ranSession : Random(DateTime.Now.Millisecond)
		for l in 0 ... 5 {
			let count: Int = (Int.random(in: 1 ... 5) * 100) + Int.random(in: 1 ... 99)
			for _ in 0 ... count - 1 {
				hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(bRatio: true)
			}
		}
	}

	public func HexagramArray() -> [CHexagram] {
		var arrH = Array(repeating: CHexagram(), count: 4096)
		var i = -1;
		for h in m_arrHexagram {
			i += 1
			let hvs = CHexagramValueSequencer(strValue: h.m_strPrimary)
			arrH[i] = CHexagram(strPrimary: hvs.DescribeCast(), nCount: h.m_nCount)
		}
		arrH.sort()
		return arrH
	}

	func binarySearch<T:Comparable>(inputArr: Array<T>, searchItem: T) -> Int? {
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

	private var m_arrHexagram: [CHexagram] = Array(repeating: CHexagram(), count: 4096)
	private var m_nCount: Int = -1
}
