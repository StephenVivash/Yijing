import { CValueSequencer } from "./CValueSequencer.ts";
import { Sequences } from "./Sequences.ts";

export class CBitValueSequencer extends CValueSequencer {
  constructor(nValue: number) {
    super(0, 2, nValue);
    this.m_nSequences = Sequences.nBitSequences;
    this.m_nRatios = Sequences.nBitRatios;
  }

  public override updateOuterValues(): void {
    this.m_pvsParent?.updateOuterValues();
  }
}
