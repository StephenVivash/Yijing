from __future__ import annotations

from typing import List

"""
from setuptools import setup, Extension

sfc_module = Extension('superfastcode', sources = ['module.cpp'])

setup(
    name='superfastcode',
    version='1.0',
    description='Python Package with superfastcode C++ extension',
    ext_modules=[sfc_module]
)

from setuptools import setup, Extension
import pybind11

cpp_args = ['-std=c++11', '-stdlib=libc++', '-mmacosx-version-min=10.7']

sfc_module = Extension(
    'superfastcode2',
    sources=['module.cpp'],
    include_dirs=[pybind11.get_include()],
    language='c++',
    extra_compile_args=cpp_args,
)

setup(
    name='superfastcode2',
    version='1.0',
    description='Python package with superfastcode2 C++ extension (PyBind11)',
    ext_modules=[sfc_module],
)
"""

def access_table(conn, table):
	cursor = conn.cursor()

	create_table_sql = """
		CREATE TABLE IF NOT EXISTS Person (
		Id INTEGER NOT NULL PRIMARY KEY,
		FirstName TEXT NOT NULL,
		LastName TEXT NOT NULL,
		Age INTEGER NOT NULL )
	"""
	#cursor.execute(create_table_sql)

	cursor.execute("SELECT * FROM " + table + " ORDER BY Id")
	results = cursor.fetchall()
	return results

"""
	db_path = 'C:\Src\Yijing\YijingDb\Yijing.db'
	conn = sqlite3.connect(db_path)
	sequences = access_table(conn,"Sequences")
	for sequence in sequences:
		print(sequence)
	labels = access_table(conn,"Labels")
	for label in labels:
		print(label)
	conn.close()
"""
from Sequences import Sequences
from ValueSequencer import HexagramValueSequencer

def test_vs():

	#Sequences.HexagramLabel = 1
	#Sequences.HexagramSequence = 0

	hvs0 = HexagramValueSequencer()
	hvs1 = hvs0.copy()
	hvs1.Next()

	lvs0 = hvs0.Trigram(0).Line(0)
	lvs0.Last()
	for i in range(lvs0._values):
		print(lvs0.Next().Describe(True), end = " O: ")
		tvs1 = lvs0.Trigram()
		hvs1 = tvs1.Hexagram()
		print(tvs1.Describe(), end = ", ")
		print(hvs1.DescribeCast(True))
	print()

	tvs0 = hvs0.Trigram(0)
	tvs0.Last()
	for i in range(tvs0._values):
		print(tvs0.Next().Describe(True), end = " I: ")
		for j in range(tvs0._inners):
			print(tvs0.Line(j).Describe(), end = ", ")
		hvs1 = tvs0.Hexagram()
		print("O:", hvs1.Describe(True))
	print()

	hvs0.Last()
	for i in range(hvs0._values):
		print(hvs0.Next().Describe(), end = " I: ")
		for j in range(hvs0._inners):
			print(hvs0.Trigram(j).Describe(True), end = ", ")
		print()
	print()
	
	print(hvs0.Transverse().Previous().Opposite().Previous().Inverse().Describe(True), "\n")

	hvs0.Trigram(0).Line(0).Next()
	print(hvs0.DescribeCast(True), "\n")

	print(tvs0.SetValue(tvs0.FindValue(2)).Describe(True), "\n")

	hvs0.Last()


"""
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

from value_sequencer_py/hexagram_value_sequencer import CHexagramValueSequencer
from sequences import Sequences

def test_hexagram_value_sequencer() -> None:
    hvs = CHexagramValueSequencer()
    hvs.Move()
"""

test_vs()