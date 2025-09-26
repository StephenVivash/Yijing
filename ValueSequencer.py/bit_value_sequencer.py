from __future__ import annotations

from .value_sequencer import CValueSequencer
from .sequences import Sequences


class CBitValueSequencer(CValueSequencer):
    def __init__(self, value: int) -> None:
        super().__init__(0, 2, value)
        self._nSequences = Sequences.nBitSequences
        self._nRatios = Sequences.nBitRatios

    def update_outer_values(self) -> None:
        if self._pvsParent:
            self._pvsParent.update_outer_values()
