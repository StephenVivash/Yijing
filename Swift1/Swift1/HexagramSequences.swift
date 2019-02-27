
public class CHexagramSequences {
}

public class CHexagram : Comparable {
	public init() {
	}

	public init(hvsPrimary: CHexagramValueSequencer) {
		m_hvsPrimary = CHexagramValueSequencer(hvs: hvsPrimary)
	}

	public var DescribeCast: String {
		get { return m_hvsPrimary.DescribeCast() }
	}

	public static func < (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.DescribeCast < rhs.DescribeCast
	}
	
	public static func > (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.DescribeCast > rhs.DescribeCast
	}
	
	public static func == (lhs: CHexagram, rhs: CHexagram) -> Bool {
		return lhs.DescribeCast == rhs.DescribeCast
	}
	
	public func Add() {
		Count = Count + 1
	}
	
	public var Count: Int = 0

	private var m_hvsPrimary: CHexagramValueSequencer!
}

public class CHexagramArray { //: EnumeratedSequence<<#Base: Sequence#>> {
	public init() {
		let hvsPrimary: CHexagramValueSequencer = CHexagramValueSequencer(nValue: 0)
		hvsPrimary.First()
		for _ in 0 ... 63 {
			for s in 0 ... 63 {
				let hvs: CHexagramValueSequencer = CHexagramValueSequencer(hvs: hvsPrimary)
				for l in 0 ... 5 {
					if (((s & (1 << l)) >> l) == 1) {
						hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(bRatio: false)
					}
				}
				Add(hvsPrimary: hvs)
			}
			hvsPrimary.Next()
		}
		//m_arrHexagram.sort { $0.DescribeCast < $1.DescribeCast }
		m_arrHexagram.sort()
	}

	public func sorterForCHexagram(this:CHexagram, that:CHexagram) -> Bool {
		return this.DescribeCast < that.DescribeCast
	}
	
	public func Add(hvsPrimary: CHexagramValueSequencer) {
		m_nCount += 1
		m_arrHexagram[m_nCount] = CHexagram(hvsPrimary: hvsPrimary)
	}

	@discardableResult
	public func MultiCast(nCount: Int) -> CHexagramArray {
		let hvs: CHexagramValueSequencer = CHexagramValueSequencer(nValue: 63) // more even ????????
		for _ in 0 ... nCount - 1 {
			//CHexagramValueSequencer hvs = new CHexagramValueSequencer(63)
			AutoCast(hvs: hvs)
			//print("\(hvs.DescribeCast()) ++++")
			let h: CHexagram = CHexagram(hvsPrimary: hvs)
			if let nIndex = binarySearch(inputArr: m_arrHexagram, searchItem: h){
				m_arrHexagram[nIndex].Add()
			}
		}
		return self
	}
	
	func binarySearch<T:Comparable>(inputArr:Array<T>, searchItem: T) -> Int? {
		var lowerIndex = 0;
		var upperIndex = inputArr.count - 1
		while (true) {
			let currentIndex = (lowerIndex + upperIndex)/2
			if(inputArr[currentIndex] == searchItem) {
				return currentIndex
			} else if (lowerIndex > upperIndex) {
				return nil
			} else {
				if (inputArr[currentIndex] > searchItem) {
					upperIndex = currentIndex - 1
				} else {
					lowerIndex = currentIndex + 1
				}
			}
		}
	}

	public func AutoCast(hvs: CHexagramValueSequencer) { //////// Random ?????????????????????????????
		//var r: Random = true ? Sequences.m_ranSession : Random(DateTime.Now.Millisecond)
		for l in 0 ... 5 {
			let count: Int = (Int.random(in: 1 ... 5) * 100) + Int.random(in: 1 ... 99)
			for _ in 0 ... count - 1 {
				hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(bRatio: true)
			}
		}
	}

	public func HexagramArray() -> [CHexagram] {
		return m_arrHexagram
	}
	
	private var m_arrHexagram: [CHexagram] = Array(repeating: CHexagram(), count: 4096)
	private var m_nCount: Int = -1
}
