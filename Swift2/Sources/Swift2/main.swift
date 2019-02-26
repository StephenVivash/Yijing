//
//  main.swift
//  Swift1
//
//  Created by Stephen Vivash on 21/2/19.
//  Copyright © 2019 Stephen Vivash. All rights reserved.
//

import Foundation

class Test1 {
	func Test() -> Int {
		return i
	}
	var i: Int = 1
}

class Test2: Test1 {
	override func Test() -> Int {
		return j
	}
	var j: Int = 2
}

func Test() {
	let t1a: Test1 = Test1()
	var t1b: Test1 = Test2()
	//let t2a: Test2 = Test2()
	
	print (t1b.Test())
	t1b = t1a
	print (t1b.Test())

	let vs = CValueSequencer(nInnerSequencers: 0, nValues: 4, nValue: 0)
	vs.Value = 0
    print (vs.First().Value, vs.Sequence)
    print (vs.Next().Value, vs.Sequence)
    print (vs.Next().Value, vs.Sequence)
    print (vs.Next().Value, vs.Sequence)
    print (vs.Next().Value, vs.Sequence)
	
	let lvs = CLineValueSequencer(nValue: 0)
	lvs.Value = 0
	print (lvs.First().Value, lvs.Sequence)
	print (lvs.Next().Value, lvs.Sequence)
	print (lvs.Next().Value, lvs.Sequence)
	print (lvs.Next().Value, lvs.Sequence)
	print (lvs.Next().Value, lvs.Sequence)


	let tvs = CTrigramValueSequencer(nValue: 0)
	tvs.Value = 0
	print (tvs.First().Value, tvs.Sequence)
	print (tvs.Next().Value, tvs.Sequence)
	print (tvs.Next().Value, tvs.Sequence)
	print (tvs.Next().Value, tvs.Sequence)
	print (tvs.Next().Value, tvs.Sequence)

	let hvs = CHexagramValueSequencer(nValue: 0)
	hvs.Value = 0
	print (hvs.First().Value, hvs.Sequence)
	print (hvs.Next().Value, hvs.Sequence)
	print (hvs.Next().Value, hvs.Sequence)
	print (hvs.Next().Value, hvs.Sequence)
	print (hvs.Next().Value, hvs.Sequence)


}

Test()
