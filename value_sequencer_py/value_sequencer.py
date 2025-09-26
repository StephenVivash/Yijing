from __future__ import annotations

from typing import List, Optional

from sequences import Sequences


class CValueSequencer:
    """Base class for value sequencers."""

    def __init__(self, n_inner_sequencers: int, n_values: int, n_value: int) -> None:
        self._nInnerSequencers = n_inner_sequencers
        self._nValues = n_values
        self._nValue = n_value
        self._nSequence = 0
        self._nRatio = 0
        self._nSequences: List[List[int]] = []
        self._nRatios: List[List[int]] = []
        self._pvsParent: Optional[CValueSequencer] = None
        self._pvsInner: List[CValueSequencer] = []

    @property
    def value(self) -> int:
        return self._nValue

    @value.setter
    def value(self, value: int) -> None:
        self.set_value(value)

    @property
    def sequence(self) -> int:
        return self._nSequence

    @sequence.setter
    def sequence(self, sequence: int) -> None:
        self.set_value(self.find_value(sequence))

    @property
    def value_str(self) -> str:
        sequences = Sequences.nHexagramSequences
        if sequences and self.value < len(sequences[0]):
            return str(sequences[0][self.value])
        return str(self.value)

    @property
    def sequence_str(self) -> str:
        return str(self.sequence + 1)

    @property
    def label(self) -> str:
        return self.get_label()

    @property
    def is_moving(self) -> bool:
        return self.get_moving()

    def first(self) -> "CValueSequencer":
        self.set_value(self.get_first_sequence())
        self.update_inner_values()
        self.update_outer_values()
        return self

    def previous(self, ratio: bool = False) -> "CValueSequencer":
        if not ratio or self._decrement_ratio():
            self.set_value(self.get_previous_sequence(self._nSequence))
            self.update_inner_values()
            self.update_outer_values()
        return self

    def next(self, ratio: bool = False) -> "CValueSequencer":
        if not ratio or self._decrement_ratio():
            self.set_value(self.get_next_sequence(self._nSequence))
            self.update_inner_values()
            self.update_outer_values()
        return self

    def last(self) -> "CValueSequencer":
        self.set_value(self.get_last_sequence())
        self.update_inner_values()
        self.update_outer_values()
        return self

    def inverse(self) -> "CValueSequencer":
        return self

    def opposite(self) -> "CValueSequencer":
        self.set_value(~self._nValue & (self._nValues - 1))
        self.update_inner_values()
        self.update_outer_values()
        return self

    def transverse(self) -> "CValueSequencer":
        return self

    def nuclear(self) -> "CValueSequencer":
        return self

    def move(self) -> "CValueSequencer":
        return self

    def young(self) -> "CValueSequencer":
        return self

    def old(self) -> "CValueSequencer":
        return self

    def update(self) -> "CValueSequencer":
        self.set_value(self._nValue)
        self.update_inner_values()
        self.update_outer_values()
        return self

    def update_inner_values(self) -> None:
        pass

    def update_outer_values(self) -> None:
        pass

    def set_parent(self, parent: "CValueSequencer") -> None:
        self._pvsParent = parent

    def get_parent(self) -> Optional["CValueSequencer"]:
        return self._pvsParent

    def get_child(self, index: int) -> Optional["CValueSequencer"]:
        return self._pvsInner[index] if 0 <= index < len(self._pvsInner) else None

    def set_value(self, value: int) -> "CValueSequencer":
        if 0 <= value < self._nValues:
            self._nValue = value
            sequence_row = self._nSequences[self.get_current_sequence()]
            if value < len(sequence_row):
                self._nSequence = sequence_row[value]
            ratio_row = self._nRatios[self.get_current_ratio()]
            if value < len(ratio_row):
                self._nRatio = ratio_row[value]
        return self

    def find_value(self, sequence: int) -> int:
        sequence_row = self._nSequences[self.get_current_sequence()]
        for i in range(self._nValues):
            if sequence_row[i] == sequence:
                return i
        return -1

    def get_first_sequence(self) -> int:
        return self.find_value(0)

    def get_previous_sequence(self, sequence: int) -> int:
        if sequence > 0:
            return self.find_value(sequence - 1)
        return self.get_last_sequence()

    def get_next_sequence(self, sequence: int) -> int:
        if sequence < self._nValues - 1:
            value = self.find_value(sequence + 1)
            if value != -1:
                return value
        return self.get_first_sequence()

    def get_last_sequence(self) -> int:
        sequence = self._nValues
        while sequence > -1:
            found = self.find_value(sequence - 1)
            if found != -1:
                return found
            sequence -= 1
        return -1

    def get_label(self) -> str:
        return ""

    def get_moving(self) -> bool:
        return False

    def get_current_sequence(self) -> int:
        return 0

    def get_current_ratio(self) -> int:
        return 0

    def get_current_label(self) -> int:
        return 0

    def _decrement_ratio(self) -> bool:
        self._nRatio -= 1
        return self._nRatio == 0
