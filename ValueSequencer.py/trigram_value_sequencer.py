from __future__ import annotations

from typing import List

from .line_value_sequencer import CLineValueSequencer
from .value_sequencer import CValueSequencer
from .sequences import Sequences


class CTrigramValueSequencer(CValueSequencer):
    _nCurrentSequence = 0
    _nCurrentRatio = 0
    _nCurrentLabel = 0

    def __init__(self, value: int) -> None:
        super().__init__(3, 8, value)
        self._pvsInner = [CLineValueSequencer(0) for _ in range(3)]
        for line in self._pvsInner:
            line.set_parent(self)
        self._nSequences = Sequences.nTrigramSequences
        self._nRatios = Sequences.nTrigramRatios
        self._nYinLine = 2
        self._nYangLine = 1

    def line(self, index: int) -> CLineValueSequencer:
        return self._pvsInner[index]  # type: ignore[return-value]

    def inverse(self) -> "CTrigramValueSequencer":
        moving = self._save_moving()
        self.value = (
            (self.line(2).value % 2) +
            (self.line(1).value % 2) * 2 +
            (self.line(0).value % 2) * 4
        )
        self.update_inner_values()
        self.update_outer_values()
        self._restore_moving(moving, True)
        return self

    def opposite(self) -> "CTrigramValueSequencer":
        moving = self._save_moving()
        super().opposite()
        self._restore_moving(moving, False)
        return self

    def move(self) -> "CTrigramValueSequencer":
        for idx in range(3):
            self.line(idx).move()
        return self

    def young(self) -> "CTrigramValueSequencer":
        for idx in range(3):
            self.line(idx).young()
        return self

    def update_inner_values(self) -> None:
        configs: List[List[int]] = [
            [self._nYinLine, self._nYinLine, self._nYinLine],
            [self._nYinLine, self._nYinLine, self._nYangLine],
            [self._nYinLine, self._nYangLine, self._nYinLine],
            [self._nYinLine, self._nYangLine, self._nYangLine],
            [self._nYangLine, self._nYinLine, self._nYinLine],
            [self._nYangLine, self._nYinLine, self._nYangLine],
            [self._nYangLine, self._nYangLine, self._nYinLine],
            [self._nYangLine, self._nYangLine, self._nYangLine],
        ]
        values = configs[self.value]
        for index, line in enumerate(reversed(self._pvsInner)):
            line.value = values[index]
            line.update_inner_values()

    def update_outer_values(self) -> None:
        self.value = (
            (self.line(0).value % 2) +
            (self.line(1).value % 2) * 2 +
            (self.line(2).value % 2) * 4
        )
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
        return Sequences.strTrigramLabels[self.get_current_label()][self.value]

    def get_moving(self) -> bool:
        return any(self.line(idx).is_moving for idx in range(3))

    def get_current_sequence(self) -> int:
        return CTrigramValueSequencer._nCurrentSequence

    def get_current_ratio(self) -> int:
        return CTrigramValueSequencer._nCurrentRatio

    def get_current_label(self) -> int:
        return CTrigramValueSequencer._nCurrentLabel

    def _save_moving(self) -> List[bool]:
        moving = [False, False, False]
        for idx in range(3):
            line = self.line(idx)
            if line.value in {0, 3}:
                moving[idx] = True
        return moving

    def _restore_moving(self, moving: List[bool], inverse_line: bool) -> None:
        for idx, is_moving in enumerate(moving):
            if not is_moving:
                continue
            target = idx
            if inverse_line:
                if target == 0:
                    target = 2
                elif target == 2:
                    target = 0
            self.line(target).old()
