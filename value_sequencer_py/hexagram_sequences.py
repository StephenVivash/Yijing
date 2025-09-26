from __future__ import annotations

from typing import List

from hexagram_value_sequencer import CHexagramValueSequencer
from sequences import Sequences


class CHexagramSequences:
    def auto_cast(self, hvs: CHexagramValueSequencer) -> CHexagramValueSequencer:
        random = Sequences.randomSession
        for line_index in range(6):
            count = (random.next(5) + 1) * 100 + random.next(100)
            for _ in range(count):
                hvs.trigram(line_index // 3).line(line_index % 3).next(True)
        return hvs


class CHexagram:
    def __init__(self, primary: CHexagramValueSequencer) -> None:
        self._hvsPrimary = CHexagramValueSequencer(copy_from=primary)
        self._count = 0

    @property
    def describe_cast(self) -> str:
        return self._hvsPrimary.describe_cast()

    def compare_to(self, other: "CHexagram") -> int:
        return (self._hvsPrimary.hexagram_id()
                > other._hvsPrimary.hexagram_id()) - (
                    self._hvsPrimary.hexagram_id()
                    < other._hvsPrimary.hexagram_id())

    def add(self) -> None:
        self._count += 1

    @property
    def count(self) -> int:
        return self._count
