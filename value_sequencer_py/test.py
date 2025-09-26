
from __future__ import annotations

from typing import List

from hexagram_value_sequencer import CHexagramValueSequencer
from sequences import Sequences

def test_vs():

	#Sequences.HexagramLabel = 1
	#Sequences.HexagramSequence = 0

	CHexagramValueSequencer.set_current_label(9)
	CHexagramValueSequencer.set_current_sequence(2)
	CHexagramValueSequencer.set_current_ratio(0)

	hvs0 = CHexagramValueSequencer()

	hvs1 = hvs0 
	hvs1.next()

	lvs0 = hvs0.trigram(0).line(0)
	lvs0.last()
	for i in range(lvs0._nValues):
		print(lvs0.next().describe(True), end = " O: ")
		tvs1 = lvs0.get_parent()
		#hvs1 = tvs1.get_parent()
		#print(tvs1.describe(True), end = ", ")
		#print(hvs1.describe_cast(True))
	print()

	tvs0 = hvs0.trigram(0)
	tvs0.last()
	for i in range(tvs0._nValues):
		print(tvs0.next().describe(True), end = " I: ")
		for j in range(3):
			print(tvs0.line(j).describe(), end = ", ")
		hvs1 = tvs0.get_parent()
		#print("O:", hvs1.describe_cast(True))
	print()

	hvs0.last()
	for i in range(hvs0._nValues):
		print(hvs0.next().describe_cast(True), end = " I: ")
		for j in range(2):
			print(hvs0.trigram(j).describe(True), end = ", ")
		print()
	print()
	
	print(hvs0.transverse().previous().opposite().previous().inverse().describe_cast(True), "\n")

	hvs0.trigram(0).line(0).next()
	print(hvs0.describe_cast(True), "\n")

	print(tvs0.set_value(tvs0.find_value(2)).describe(True), "\n")

	hvs0.last()

test_vs()	