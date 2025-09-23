import { CValueSequencer } from "./CValueSequencer";
import { CLineValueSequencer } from "./CLineValueSequencer";
import { Sequences } from "./Sequences";

export class CTrigramValueSequencer extends CValueSequencer {
  private static m_nCurrentSequence = 0;
  private static m_nCurrentRatio = 0;
  private static m_nCurrentLabel = 0;

  protected m_nYinLine = 2;
  protected m_nYangLine = 1;

  constructor(nValue: number) {
    super(3, 8, nValue);
    this.m_pvsInner = [
      new CLineValueSequencer(0),
      new CLineValueSequencer(0),
      new CLineValueSequencer(0),
    ];
    this.line(2).setParent(this);
    this.line(1).setParent(this);
    this.line(0).setParent(this);
    this.m_nSequences = Sequences.nTrigramSequences;
    this.m_nRatios = Sequences.nTrigramRatios;
  }

  public line(nIndex: number): CLineValueSequencer {
    return this.m_pvsInner[nIndex] as CLineValueSequencer;
  }

  public override inverse(): this {
    const bMoving = [false, false, false];
    this.saveMoving(bMoving);
    this.value =
      (this.line(2).value % 2 === 0 ? 0 : 1) +
      (this.line(1).value % 2 === 0 ? 0 : 1) * 2 +
      (this.line(0).value % 2 === 0 ? 0 : 1) * 4;
    this.updateInnerValues();
    this.updateOuterValues();
    this.restoreMoving(bMoving, true);
    return this;
  }

  public override opposite(): this {
    const bMoving = [false, false, false];
    this.saveMoving(bMoving);
    super.opposite();
    this.restoreMoving(bMoving, false);
    return this;
  }

  public override move(): this {
    this.line(2).move();
    this.line(1).move();
    this.line(0).move();
    return this;
  }

  public override young(): this {
    this.line(2).young();
    this.line(1).young();
    this.line(0).young();
    return this;
  }

  public override updateInnerValues(): void {
    switch (this.m_nValue) {
      case 0:
        this.line(2).value = this.m_nYinLine;
        this.line(1).value = this.m_nYinLine;
        this.line(0).value = this.m_nYinLine;
        break;
      case 1:
        this.line(2).value = this.m_nYinLine;
        this.line(1).value = this.m_nYinLine;
        this.line(0).value = this.m_nYangLine;
        break;
      case 2:
        this.line(2).value = this.m_nYinLine;
        this.line(1).value = this.m_nYangLine;
        this.line(0).value = this.m_nYinLine;
        break;
      case 3:
        this.line(2).value = this.m_nYinLine;
        this.line(1).value = this.m_nYangLine;
        this.line(0).value = this.m_nYangLine;
        break;
      case 4:
        this.line(2).value = this.m_nYangLine;
        this.line(1).value = this.m_nYinLine;
        this.line(0).value = this.m_nYinLine;
        break;
      case 5:
        this.line(2).value = this.m_nYangLine;
        this.line(1).value = this.m_nYinLine;
        this.line(0).value = this.m_nYangLine;
        break;
      case 6:
        this.line(2).value = this.m_nYangLine;
        this.line(1).value = this.m_nYangLine;
        this.line(0).value = this.m_nYinLine;
        break;
      case 7:
        this.line(2).value = this.m_nYangLine;
        this.line(1).value = this.m_nYangLine;
        this.line(0).value = this.m_nYangLine;
        break;
    }
    this.line(2).updateInnerValues();
    this.line(1).updateInnerValues();
    this.line(0).updateInnerValues();
  }

  public override updateOuterValues(): void {
    this.value =
      (this.line(0).value % 2 === 0 ? 0 : 1) +
      (this.line(1).value % 2 === 0 ? 0 : 1) * 2 +
      (this.line(2).value % 2 === 0 ? 0 : 1) * 4;
    this.m_pvsParent?.updateOuterValues();
  }

  private saveMoving(bMoving: boolean[]): void {
    for (let l = 0; l < 3; ++l) {
      const line = this.line(l);
      if (line.value === 0 || line.value === 3) {
        bMoving[l] = true;
      }
    }
  }

  private restoreMoving(bMoving: boolean[], bInverseLine: boolean): void {
    for (let l = 0; l < 3; ++l) {
      if (bMoving[l]) {
        let l1 = l;
        if (bInverseLine) {
          if (l1 === 0) {
            l1 = 2;
          } else if (l1 === 2) {
            l1 = 0;
          }
        }
        this.line(l1).old();
      }
    }
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
    return Sequences.strTrigramLabels[this.getCurrentLabel()][this.value];
  }

  protected override getMoving(): boolean {
    return this.line(0).isMoving || this.line(1).isMoving || this.line(2).isMoving;
  }

  protected override getCurrentSequence(): number {
    return CTrigramValueSequencer.m_nCurrentSequence;
  }

  protected override getCurrentRatio(): number {
    return CTrigramValueSequencer.m_nCurrentRatio;
  }

  protected override getCurrentLabel(): number {
    return CTrigramValueSequencer.m_nCurrentLabel;
  }
}
