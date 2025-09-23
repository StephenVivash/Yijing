import { CValueSequencer } from "./CValueSequencer.ts";
import { CBitValueSequencer } from "./CBitValueSequencer.ts";
import { Sequences } from "./Sequences.ts";

export class CLineValueSequencer extends CValueSequencer {
  private static m_nCurrentSequence = 0;
  private static m_nCurrentRatio = 0;
  private static m_nCurrentLabel = 0;

  constructor(nValue: number) {
    super(3, 4, nValue);
    this.m_pvsInner = [
      new CBitValueSequencer(0),
      new CBitValueSequencer(0),
      new CBitValueSequencer(0),
    ];
    this.bit(2).setParent(this);
    this.bit(1).setParent(this);
    this.bit(0).setParent(this);
    this.m_nSequences = Sequences.nLineSequences;
    this.m_nRatios = Sequences.nLineRatios;
  }

  public bit(nIndex: number): CBitValueSequencer {
    return this.m_pvsInner[nIndex] as CBitValueSequencer;
  }

  public override inverse(): this {
    switch (this.m_nValue) {
      case 0:
        this.value = 1;
        break;
      case 1:
        this.value = 0;
        break;
      case 2:
        this.value = 3;
        break;
      case 3:
        this.value = 2;
        break;
    }
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public override move(): this {
    if (this.value === 0) {
      this.value = 1;
    } else if (this.value === 3) {
      this.value = 2;
    }
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public override young(): this {
    if (this.value === 0) {
      this.value = 2;
    } else if (this.value === 3) {
      this.value = 1;
    }
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public override old(): this {
    if (this.value === 2) {
      this.value = 0;
    } else if (this.value === 1) {
      this.value = 3;
    }
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public override updateInnerValues(): void {
    switch (this.m_nValue) {
      case 0:
        this.bit(2).value = 0;
        this.bit(1).value = 0;
        this.bit(0).value = 0;
        break;
      case 1:
        this.bit(2).value = 0;
        this.bit(1).value = 1;
        this.bit(0).value = 0;
        break;
      case 2:
        this.bit(2).value = 1;
        this.bit(1).value = 0;
        this.bit(0).value = 1;
        break;
      case 3:
        this.bit(2).value = 1;
        this.bit(1).value = 1;
        this.bit(0).value = 1;
        break;
    }
    this.bit(2).updateInnerValues();
    this.bit(1).updateInnerValues();
    this.bit(0).updateInnerValues();
  }

  public override updateOuterValues(): void {
    this.value = this.bit(0).value + this.bit(1).value + this.bit(2).value;
    this.m_pvsParent?.updateOuterValues();
  }

  public static setCurrentSequence(nSequence: number): void {
    this.m_nCurrentSequence = nSequence;
  }

  public static setCurrentRatio(nRatio: number): void {
    this.m_nCurrentRatio = nRatio;
  }

  public static setCurrentLabel(nLabel: number): void {
    this.m_nCurrentLabel = nLabel;
  }

  protected override getLabel(): string {
    return Sequences.strLineLabels[this.getCurrentLabel()][this.value];
  }

  protected override getMoving(): boolean {
    return this.value === 0 || this.value === 3;
  }

  protected override getCurrentSequence(): number {
    return CLineValueSequencer.m_nCurrentSequence;
  }

  protected override getCurrentRatio(): number {
    return CLineValueSequencer.m_nCurrentRatio;
  }

  protected override getCurrentLabel(): number {
    return CLineValueSequencer.m_nCurrentLabel;
  }
}
