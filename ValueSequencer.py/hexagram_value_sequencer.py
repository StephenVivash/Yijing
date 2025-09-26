from __future__ import annotations

from typing import List, Optional

from .trigram_value_sequencer import CTrigramValueSequencer
from .value_sequencer import CValueSequencer
from .sequences import Sequences


class CHexagramValueSequencer(CValueSequencer):
    _nCurrentSequence = 0
    _nCurrentRatio = 0
    _nCurrentLabel = 0

    def __init__(
        self,
        value: int = 0,
        copy_from: Optional["CHexagramValueSequencer"] = None,
    ) -> None:
        super().__init__(2, 64, value if copy_from is None else 0)
        self._pvsInner = [
            CTrigramValueSequencer(-1),
            CTrigramValueSequencer(-1),
        ]
        self.trigram(1).set_parent(self)
        self.trigram(0).set_parent(self)
        self._nSequences = Sequences.nHexagramSequences
        self._nRatios = Sequences.nHexagramRatios
        if copy_from is not None:
            self._copy_from(copy_from)
            self.update_outer_values()
        else:
            self.value = value
            self.update_inner_values()

    def _copy_from(self, source: "CHexagramValueSequencer") -> None:
        for t in range(2):
            for l in range(3):
                line = self.trigram(t).line(l)
                source_line = source.trigram(t).line(l)
                line.value = source_line.value
                line.update_inner_values()
                line.update_outer_values()

    def trigram(self, index: int) -> CTrigramValueSequencer:
        return self._pvsInner[index]  # type: ignore[return-value]

    def inverse(self) -> "CHexagramValueSequencer":
        moving = self._save_moving()
        self.value = (
            (self.trigram(1).line(2).value % 2) +
            (self.trigram(1).line(1).value % 2) * 2 +
            (self.trigram(1).line(0).value % 2) * 4 +
            (
                (
                    (self.trigram(0).line(2).value % 2) +
                    (self.trigram(0).line(1).value % 2) * 2 +
                    (self.trigram(0).line(0).value % 2) * 4
                )
                * 8
            )
        )
        self.update_inner_values()
        self.update_outer_values()
        self._restore_moving(moving, True, True)
        return self

    def opposite(self) -> "CHexagramValueSequencer":
        moving = self._save_moving()
        super().opposite()
        self._restore_moving(moving, False, False)
        return self

    def transverse(self) -> "CHexagramValueSequencer":
        moving = self._save_moving()
        temp = self.trigram(0).value
        self.trigram(0).value = self.trigram(1).value
        self.trigram(1).value = temp
        self.value = self.trigram(0).value + self.trigram(1).value * 8
        self.update_inner_values()
        self.update_outer_values()
        self._restore_moving(moving, True, False)
        return self

    def nuclear(self) -> "CHexagramValueSequencer":
        moving = self._save_moving()
        temp = self.trigram(1).line(0).value
        self.trigram(1).line(2).value = self.trigram(1).line(1).value
        self.trigram(1).line(1).value = self.trigram(1).line(0).value
        self.trigram(1).line(0).value = self.trigram(0).line(2).value
        self.trigram(0).line(0).value = self.trigram(0).line(1).value
        self.trigram(0).line(1).value = self.trigram(0).line(2).value
        self.trigram(0).line(2).value = temp
        self.value = (
            (
                (self.trigram(1).line(2).value % 2) * 4 +
                (self.trigram(1).line(1).value % 2) * 2 +
                (self.trigram(1).line(0).value % 2)
            )
            * 8
            + (self.trigram(0).line(2).value % 2) * 4
            + (self.trigram(0).line(1).value % 2) * 2
            + (self.trigram(0).line(0).value % 2)
        )
        self.update_inner_values()
        self.update_outer_values()
        if moving[1][1]:
            self.trigram(1).line(2).old()
        if moving[1][0]:
            self.trigram(1).line(0).old()
            self.trigram(1).line(1).old()
        if moving[0][2]:
            self.trigram(0).line(2).old()
            self.trigram(0).line(1).old()
        if moving[0][1]:
            self.trigram(0).line(0).old()
        return self

    def move(self) -> "CHexagramValueSequencer":
        self.trigram(1).move()
        self.trigram(0).move()
        return self

    def young(self) -> "CHexagramValueSequencer":
        self.trigram(1).young()
        self.trigram(0).young()
        return self

    def update_inner_values(self) -> None:
        self.trigram(1).value = self.value // 8
        self.trigram(0).value = self.value % 8
        self.trigram(1).update_inner_values()
        self.trigram(0).update_inner_values()

    def update_outer_values(self) -> None:
        self.value = self.trigram(0).value + self.trigram(1).value * 8

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
        return Sequences.strHexagramLabels[self.get_current_label()][self.value]

    def get_moving(self) -> bool:
        return self.trigram(0).is_moving or self.trigram(1).is_moving

    def get_current_sequence(self) -> int:
        return CHexagramValueSequencer._nCurrentSequence

    def get_current_ratio(self) -> int:
        return CHexagramValueSequencer._nCurrentRatio

    def get_current_label(self) -> int:
        return CHexagramValueSequencer._nCurrentLabel

    def hexagram_id(self, use_value: bool = False) -> str:
        base = f"{(self.value if use_value else self.sequence + 1):2d}"
        if not self.is_moving:
            return base
        suffix = ''.join(
            str(line_index + 1)
            for line_index in range(6)
            if self.trigram(line_index // 3).line(line_index % 3).is_moving
        )
        return f"{base}.{suffix}"

    def describe_primary(self, use_value: bool = False) -> str:
        value_str = f" ({self.value_str})" if use_value else ""
        return f"{self.hexagram_id()} {self.label}{value_str}"

    def describe_secondary(self, use_value: bool = False) -> str:
        if not self.is_moving:
            return ""
        secondary = CHexagramValueSequencer(copy_from=self)
        secondary.move()
        value_str = f" ({secondary.value_str})" if use_value else ""
        return f"{secondary.hexagram_id()} {secondary.label}{value_str}"

    def describe_cast(self, use_value: bool = False) -> str:
        primary = self.describe_primary(use_value)
        secondary = self.describe_secondary(use_value)
        return f"{primary} > {secondary}" if secondary else primary

    def _save_moving(self) -> List[List[bool]]:
        moving = [[False, False, False], [False, False, False]]
        for t in range(2):
            for l in range(3):
                line_value = self.trigram(t).line(l).value
                if line_value in {0, 3}:
                    moving[t][l] = True
        return moving

    def _restore_moving(
        self,
        moving: List[List[bool]],
        inverse_trigram: bool,
        inverse_line: bool,
    ) -> None:
        for t in range(2):
            for l in range(3):
                if not moving[t][l]:
                    continue
                line_index = l
                if inverse_line:
                    if line_index == 0:
                        line_index = 2
                    elif line_index == 2:
                        line_index = 0
                trigram_index = 1 - t if inverse_trigram else t
                self.trigram(trigram_index).line(line_index).old()
