from __future__ import annotations

from typing import List

from value_sequencer_py.bit_value_sequencer import CBitValueSequencer
from value_sequencer_py.value_sequencer import CValueSequencer
from value_sequencer_py.sequences import Sequences


class CLineValueSequencer(CValueSequencer):
    _nCurrentSequence = 0
    _nCurrentRatio = 0
    _nCurrentLabel = 0

    def __init__(self, value: int) -> None:
        super().__init__(3, 4, value)
        self._pvsInner = [CBitValueSequencer(0) for _ in range(3)]
        for bit in self._pvsInner:
            bit.set_parent(self)
        self._nSequences = Sequences.nLineSequences
        self._nRatios = Sequences.nLineRatios

    def bit(self, index: int) -> CBitValueSequencer:
        return self._pvsInner[index]  # type: ignore[return-value]

    def inverse(self) -> "CLineValueSequencer":
        mapping = {0: 1, 1: 0, 2: 3, 3: 2}
        self.value = mapping.get(self.value, self.value)
        self.update_inner_values()
        self.update_outer_values()
        return self

    def move(self) -> "CLineValueSequencer":
        if self.value == 0:
            self.value = 1
        elif self.value == 3:
            self.value = 2
        self.update_inner_values()
        self.update_outer_values()
        return self

    def young(self) -> "CLineValueSequencer":
        if self.value == 0:
            self.value = 2
        elif self.value == 3:
            self.value = 1
        self.update_inner_values()
        self.update_outer_values()
        return self

    def old(self) -> "CLineValueSequencer":
        if self.value == 2:
            self.value = 0
        elif self.value == 1:
            self.value = 3
        self.update_inner_values()
        self.update_outer_values()
        return self

    def update_inner_values(self) -> None:
        configurations: List[List[int]] = [
            [0, 0, 0],
            [0, 1, 0],
            [1, 0, 1],
            [1, 1, 1],
        ]
        bit_values = configurations[self.value]
        for index, bit in enumerate(reversed(self._pvsInner)):
            bit.value = bit_values[index]
            bit.update_inner_values()

    def update_outer_values(self) -> None:
        self.value = sum(bit.value for bit in self._pvsInner)
        if self._pvsParent:
            self._pvsParent.update_outer_values()

    @classmethod
    def set_current_sequence(cls, sequence: int) -> None:
        cls._nCurrentSequence = sequence

    @classmethod
    def set_current_ratio(cls, ratio: int) -> None:
        cls._nCurrentRatio = ratio

    @classmethod
    def set_current_label(cls, label: int) -> None:
        cls._nCurrentLabel = label

    def get_label(self) -> str:
        return Sequences.strLineLabels[self.get_current_label()][self.value]

    def get_moving(self) -> bool:
        return self.value in {0, 3}

    def get_current_sequence(self) -> int:
        return CLineValueSequencer._nCurrentSequence

    def get_current_ratio(self) -> int:
        return CLineValueSequencer._nCurrentRatio

    def get_current_label(self) -> int:
        return CLineValueSequencer._nCurrentLabel
