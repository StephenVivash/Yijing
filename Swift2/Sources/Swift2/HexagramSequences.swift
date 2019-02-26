
public class CHexagramSequences
{
}

public class CHexagram : IComparable
{
	public init(hvsPrimary: CHexagramValueSequencer) {
		m_hvsPrimary = CHexagramValueSequencer(hvs: hvsPrimary)
	}

	public var DescribeCast: String {
		get { return m_hvsPrimary.DescribeCast() }
	}

	public func CompareTo(obj: object ) -> Int {
		CHexagram h = obj as! CHexagram
		return m_hvsPrimary.HexagramId().CompareTo(h.m_hvsPrimary.HexagramId())
	}

	public func Add() => ++Count
	public var Count: Int = 0

	private var m_hvsPrimary: CHexagramValueSequencer
}

public class CHexagramArray //: IEnumerable
{
	public init() {
		var hvsPrimary: CHexagramValueSequencer = CHexagramValueSequencer(nValue: 0)
		hvsPrimary.First()
		for p in 0 ... 63 {
			for s in 0 ... 63 {
				var hvs: CHexagramValueSequencer = CHexagramValueSequencer(hvs: hvsPrimary)
				for l in 0 ... 5 {
					if (((s & (1 << l)) >> l) == 1) {
						hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(false)
					}
				}
				Add(hvsPrimary: hvs)
			}
			hvsPrimary.Next()
		}
		Array.Sort(m_arrHexagram)
	}

	public func Add(hvsPrimary: CHexagramValueSequencer) {
		m_arrHexagram.SetValue(CHexagram(hvs: hvsPrimary),++m_nCount)
	}

	public func MultiCast(nCount: Int) -> CHexagramArray {
		var hvs: CHexagramValueSequencer = CHexagramValueSequencer(nValue: 63) // more even ????????
		for i in 0 ... nCount - 1 {
			//CHexagramValueSequencer hvs = new CHexagramValueSequencer(63)
			AutoCast(hvsPrimary: hvs)
			var h: CHexagram = CHexagram(hvsPrimary: hvs)
			var nIndex: Int = Array.BinarySearch(m_arrHexagram, h)
			if nIndex >= 0 {
				((CHexagram) m_arrHexagram.GetValue(nIndex)).Add()
			}
		}
		return self
	}

	public func AutoCast(ref CHexagramValueSequencer hvs) -> CHexagramValueSequencer { //////// Random ?????????????????????????????
		var r: Random = true ? Sequences.m_ranSession : Random(DateTime.Now.Millisecond)
		for l in 0 ... 5 {
			var count: Int = (r.Next(5) + 1) * 100 + r.Next(100)
			for c in 0 count - 1 {
				hvs.Trigram(nIndex: l / 3).Line(nIndex: l % 3).Next(bRatio: true)
			}
		}
		return hvs
	}

	public func GetEnumerator() -> IEnumerator {
		throw new NotImplementedException()
	}

	public func HexagramArray() -> Array => m_arrHexagram
	public func self[int i] -> CHexagram => (CHexagram) m_arrHexagram.GetValue(i)

	private var m_arrHexagram: Array = Array.CreateInstance(typeof(CHexagram), 4096)
	private var m_nCount: Int = -1
}
