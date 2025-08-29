from Sequences import Sequences

class ValueSequencer:

	_values: int = 0
	_inners: int = 0

	__value: int = 0
	__sequence: int = 0
	__ratio: int = 0

	_inner = None
	_parent = None
	
	_sequences = None
	_ratios = None
	_labels = None

	_currentSequence: int = 0
	_currentRatio: int = 0
	_currentLabel: int = 0

	def __init__(self, values: int, inners: int):
		self._values = values
		self._inners = inners

	def copy(self, vs):
		vs._currentSequence = self._currentSequence
		vs._currentRatio = self._currentRatio
		vs._currentLabel = self._currentLabel

	def Parent(self):
		return self._parent

	def Inner(self, idx: int): 
		if idx >= 0 and idx <= self._inners:
			return self._inner[idx]

	def Value(self) -> int:
		return self.__value

	def Sequence(self) -> int:
		return self.__sequence

	def Label(self) -> str: 
		return self._labels[self._currentLabel][self.__value]
	
	def Describe(self, value: bool = False) -> str:
		seq = self.__sequence + 1
		s = seq.__str__() + ". " + self.Label() 
		if value:
			s += " (" + self.Value().__str__() + ")"
		return s
	
	def First(self):
		self.SetValue(self.GetFirstSequence())
		return self

	def Previous(self, ratio: bool = False):
		self.__ratio -= 1
		if not ratio or self.__ratio == 0:
			self.SetValue(self.GetPreviousSequence(self.__sequence))
		return self

	def Next(self, ratio: bool = False):
		self.__ratio -= 1
		if not ratio or self.__ratio == 0:
			self.SetValue(self.GetNextSequence(self.__sequence))
		return self

	def Last(self):
		self.SetValue(self.GetLastSequence())
		return self

	def Inverse(self):
		return self
	
	def Opposite(self):
		self.SetValue(~self.__value & (self._values - 1))
		return self

	def Transverse(self):
		return self
	
	def Nuclear(self):
		return self

	def Move(self):
		return self
	
	def Young(self):
		return self
	
	def Old(self):
		return self

	def IsMoving(self) -> bool:
		return False
	
	def Update(self):
		self.SetValue(self.__value)
		return self

	def SetParent(self, parent):
		self._parent = parent

	def SetValue(self, value: int, update: bool = True): 
		if((value >= 0) and (value <= self._values)):
			self.__value = value
			self.__sequence = self._sequences[self._currentSequence][value]
			self.__ratio = self._ratios[self._currentRatio][value]
			if update:
				self.UpdateInnerValues()
				self.UpdateOuterValues()
		return self

	def FindValue(self, sequence: int) -> int:
		for i in range(self._values):
			if self._sequences[self._currentSequence][i] == sequence:
				return i
		return -1

	def GetFirstSequence(self)-> int:
		return self.FindValue(0)

	def GetPreviousSequence(self, sequence: int)-> int:
		if sequence > 0:
			sequence -= 1
			return self.FindValue(sequence)
		return self.GetLastSequence()

	def GetNextSequence(self, sequence: int)-> int:
		if sequence < self._values - 1:
			sequence += 1
			sequence = self.FindValue(sequence)
			if sequence  != -1:
				return sequence
		return self.GetFirstSequence()

	def GetLastSequence(self)-> int:
		sequence1 = 0
		sequence2 = self._values
		while sequence2 > -1:
			sequence2 -= 1
			sequence1 = self.FindValue(sequence2)
			if sequence1 != -1:
				return sequence1
		return -1

	def UpdateInnerValues(self):
		pass

	def UpdateOuterValues(self):
		pass

######################################################################################################################################
######################################################################################################################################
######################################################################################################################################

class LineValueSequencer(ValueSequencer):

	def __init__(self, value: int = 0):
		super().__init__(4, 0)
		self._sequences = Sequences.nLineSequences
		self._ratios = Sequences.nLineRatios
		self._labels = Sequences.strLineLabels
		self._currentSequence = Sequences.LineSequence
		self._currentRatio = Sequences.LineRatio
		self._currentLabel = Sequences.LineLabel
		self.SetValue(value, False)

	def Trigram(self) -> ValueSequencer:
		return super().Parent()

	def Inverse(self) -> ValueSequencer:
		return self
	
	def Opposite(self) -> ValueSequencer:
		return super().Opposite()

	def Transverse(self) -> ValueSequencer:
		return self
	
	def Nuclear(self) -> ValueSequencer:
		return self

	def Move(self) -> ValueSequencer:
		if self.Value() == 0:
			self.SetValue(1)
		else:
			if self.Value() == 3:
				self.SetValue(2)
		return self
	
	def Young(self) -> ValueSequencer:
		return self
	
	def Old(self) -> ValueSequencer:
		return self
	
	def IsMoving(self) -> bool:
		return (self.Value() == 0) or (self.Value() == 3)

	#def UpdateInnerValues(self):
	#	pass

	def UpdateOuterValues(self):
		#self._value = self.Bit(0)._value + self.Bit(1).Value + Bit(2).Value
		self._parent.UpdateOuterValues()

######################################################################################################################################
######################################################################################################################################
######################################################################################################################################

class TrigramValueSequencer(ValueSequencer):

	def __init__(self, value: int = 0):
		super().__init__(8, 3)
		self._inner = [LineValueSequencer(), LineValueSequencer(), LineValueSequencer()]
		self._inner[0].SetParent(self)
		self._inner[1].SetParent(self)
		self._inner[2].SetParent(self)
		self._sequences = Sequences.nTrigramSequences
		self._ratios = Sequences.nTrigramRatios
		self._labels = Sequences.strTrigramLabels
		self._currentSequence = Sequences.TrigramSequence
		self._currentRatio = Sequences.TrigramRatio
		self._currentLabel = Sequences.TrigramLabel
		self.SetValue(value, False)

	def Hexagram(self) -> ValueSequencer:
		return super().Parent()

	def Line(self, idx: int)-> LineValueSequencer: 
		return super().Inner(idx)

	def Inverse(self) -> ValueSequencer:
		return self
	
	def Opposite(self) -> ValueSequencer:
		return super().Opposite()

	def Transverse(self) -> ValueSequencer:
		return self
	
	def Nuclear(self) -> ValueSequencer:
		return self

	def Move(self) -> ValueSequencer:
		self.Line(2).Move()
		self.Line(1).Move()
		self.Line(0).Move()
		return self
	
	def Young(self) -> ValueSequencer:
		return self
	
	def Old(self) -> ValueSequencer:
		return self

	def IsMoving(self) -> bool:
		return self.Line(0).IsMoving() or self.Line(1).IsMoving() or self.Line(2).IsMoving()

	def UpdateInnerValues(self):
			
		YinLine: int = 2
		YangLine: int = 1

		match self.Value():
			case 0:
				self.Line(2).SetValue(YinLine, False)
				self.Line(1).SetValue(YinLine, False)
				self.Line(0).SetValue(YinLine, False)
			case 1:
				self.Line(2).SetValue(YinLine, False)
				self.Line(1).SetValue(YinLine, False)
				self.Line(0).SetValue(YangLine, False)
			case 2:
				self.Line(2).SetValue(YinLine, False)
				self.Line(1).SetValue(YangLine, False)
				self.Line(0).SetValue(YinLine, False)
			case 3:
				self.Line(2).SetValue(YinLine, False)
				self.Line(1).SetValue(YangLine, False)
				self.Line(0).SetValue(YangLine, False)
			case 4:
				self.Line(2).SetValue(YangLine, False)
				self.Line(1).SetValue(YinLine, False)
				self.Line(0).SetValue(YinLine, False)
			case 5:
				self.Line(2).SetValue(YangLine, False)
				self.Line(1).SetValue(YinLine, False)
				self.Line(0).SetValue(YangLine, False)
			case 6:
				self.Line(2).SetValue(YangLine, False)
				self.Line(1).SetValue(YangLine, False)
				self.Line(0).SetValue(YinLine, False)
			case 7:
				self.Line(2).SetValue(YangLine, False)
				self.Line(1).SetValue(YangLine, False)
				self.Line(0).SetValue(YangLine, False)

		#self.Line(2).UpdateInnerValues()
		#self.Line(1).UpdateInnerValues()
		#self.Line(0).UpdateInnerValues()

	def UpdateOuterValues(self):
		v: int = 0
		if (self.Line(0).Value() % 2) != 0:
			v = 1
		if (self.Line(1).Value() % 2) != 0:
			v += 2
		if (self.Line(2).Value() % 2) != 0:
			v += 4
		self.SetValue(v, False)
		self._parent.UpdateOuterValues()

######################################################################################################################################
######################################################################################################################################
######################################################################################################################################

class HexagramValueSequencer(ValueSequencer):

	def __init__(self, value: int = 0):
		super().__init__(64, 2)
		self._inner = [TrigramValueSequencer(), TrigramValueSequencer()]
		self._inner[0].SetParent(self)
		self._inner[1].SetParent(self)
		self._sequences = Sequences.nHexagramSequences
		self._ratios = Sequences.nHexagramRatios
		self._labels = Sequences.strHexagramLabels
		self._currentSequence = Sequences.HexagramSequence
		self._currentRatio = Sequences.HexagramRatio
		self._currentLabel = Sequences.HexagramLabel
		self.First()

	def copy(self):
		hvs = HexagramValueSequencer()
		super().copy(hvs)
		hvs.Trigram(1).Line(2).SetValue(self.Trigram(1).Line(2).Value())
		hvs.Trigram(1).Line(1).SetValue(self.Trigram(1).Line(1).Value())
		hvs.Trigram(1).Line(0).SetValue(self.Trigram(1).Line(0).Value())
		hvs.Trigram(0).Line(2).SetValue(self.Trigram(0).Line(2).Value())
		hvs.Trigram(0).Line(1).SetValue(self.Trigram(0).Line(1).Value())
		hvs.Trigram(0).Line(0).SetValue(self.Trigram(0).Line(0).Value())
		return hvs

	def Describe(self, value: bool = False) -> str:
		seq = self.Sequence() + 1
		s = seq.__str__() + "." 
		if self.IsMoving():
			for l  in range(6):
				if self.Trigram(l // 3).Line(l % 3).IsMoving():
					s += (l + 1).__str__()
		s += " " + self.Label() 
		if value:
			s += " (" + self.Value().__str__() + ")"
		return s

	def DescibeSecondary(self, value: bool = False) -> str:
		if self.IsMoving():
			hvs = HexagramValueSequencer()
			hvs = self.copy()
			hvs.Move()
			return " > " + hvs.Describe(value)
		return ""

	def DescribeCast(self, value: bool = False) -> str:
		return self.Describe(value) + self.DescibeSecondary(value)

	def Trigram(self, idx: int) -> TrigramValueSequencer: 
		return super().Inner(idx)

	def Inverse(self) -> ValueSequencer:
		#bool[,] bMoving = { { false, false, false }, { false, false, false } };
		#SaveMoving(ref bMoving);
		v: int = 0
		if self.Trigram(1).Line(2).Value() % 2 != 0:
			v += 1
		if self.Trigram(1).Line(1).Value() % 2 != 0:
			v += 2
		if self.Trigram(1).Line(0).Value() % 2 != 0:
			v += 4
		if self.Trigram(0).Line(2).Value() % 2 != 0:
			v += 8
		if self.Trigram(0).Line(1).Value() % 2 != 0:
			v += 16
		if self.Trigram(0).Line(0).Value() % 2 != 0:
			v += 32 
		self.SetValue(v)
		#RestoreMoving(bMoving,true,true);
		return self
	
	def Opposite(self) -> ValueSequencer:
		return super().Opposite()

	def Transverse(self) -> ValueSequencer:
		#bool[,] bMoving = { { false, false, false }, { false, false, false } };
		#SaveMoving(ref bMoving);
		self.SetValue(self.Trigram(1).Value() + self.Trigram(0).Value() * 8)
		#RestoreMoving(bMoving, true, false);
		return self
	
	def Nuclear(self) -> ValueSequencer:
		return self

	def Move(self) -> ValueSequencer:
		self.Trigram(1).Move()
		self.Trigram(0).Move()
		return self
	
	def Young(self) -> ValueSequencer:
		return self
	
	def Old(self) -> ValueSequencer:
		return self

	def IsMoving(self) -> bool:
		return self.Trigram(0).IsMoving() or self.Trigram(1).IsMoving()

	def UpdateInnerValues(self):
		self.Trigram(1).SetValue(self.Value() // 8, False)
		self.Trigram(0).SetValue(self.Value() % 8, False)
		self.Trigram(1).UpdateInnerValues()
		self.Trigram(0).UpdateInnerValues()

	def UpdateOuterValues(self):
		self.SetValue(self.Trigram(0).Value() + (self.Trigram(1).Value() * 8), False)

######################################################################################################################################
######################################################################################################################################
######################################################################################################################################

