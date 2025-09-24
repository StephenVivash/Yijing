import { CValueSequencer } from "./CValueSequencer.ts";
import { CTrigramValueSequencer } from "./CTrigramValueSequencer.ts";
import { Sequences } from "./Sequences.ts";

export class CHexagramValueSequencer extends CValueSequencer {
  private static m_nCurrentSequence = 0;
  private static m_nCurrentRatio = 0;
  private static m_nCurrentLabel = 0;

  constructor(nValue: number);
  constructor(copy: CHexagramValueSequencer);
  constructor(valueOrCopy: number | CHexagramValueSequencer) {
    super(2, 64, typeof valueOrCopy === "number" ? valueOrCopy : 0);
    this.m_pvsInner = [
      new CTrigramValueSequencer(-1),
      new CTrigramValueSequencer(-1),
    ];
    this.trigram(1).setParent(this);
    this.trigram(0).setParent(this);
    this.m_nSequences = Sequences.nHexagramSequences;
    this.m_nRatios = Sequences.nHexagramRatios;

    if (valueOrCopy instanceof CHexagramValueSequencer) {
      this.copyFrom(valueOrCopy);
    } else {
      this.value = valueOrCopy;
      this.updateInnerValues();
    }
  }

  private copyFrom(source: CHexagramValueSequencer): void {
    for (let t = 0; t < 2; ++t) {
      for (let l = 0; l < 3; ++l) {
        const line = this.trigram(t).line(l);
        const sourceLine = source.trigram(t).line(l);
        line.value = sourceLine.value;
        line.updateInnerValues();
        line.updateOuterValues();
      }
    }
  }

  public trigram(nIndex: number): CTrigramValueSequencer {
    return this.m_pvsInner[nIndex] as CTrigramValueSequencer;
  }

  public override inverse(): this {
    const bMoving = [
      [false, false, false],
      [false, false, false],
    ];
    this.saveMoving(bMoving);
    this.value =
      (this.trigram(1).line(2).value % 2 === 0 ? 0 : 1) +
      (this.trigram(1).line(1).value % 2 === 0 ? 0 : 1) * 2 +
      (this.trigram(1).line(0).value % 2 === 0 ? 0 : 1) * 4 +
      (((this.trigram(0).line(2).value % 2 === 0 ? 0 : 1) +
        (this.trigram(0).line(1).value % 2 === 0 ? 0 : 1) * 2 +
        (this.trigram(0).line(0).value % 2 === 0 ? 0 : 1) * 4) * 8);
    this.updateInnerValues();
    this.updateOuterValues();
    this.restoreMoving(bMoving, true, true);
    return this;
  }

  public override opposite(): this {
    const bMoving = [
      [false, false, false],
      [false, false, false],
    ];
    this.saveMoving(bMoving);
    super.opposite();
    this.restoreMoving(bMoving, false, false);
    return this;
  }

  public override transverse(): this {
    const bMoving = [
      [false, false, false],
      [false, false, false],
    ];
    this.saveMoving(bMoving);
    const temp = this.trigram(0).value;
    this.trigram(0).value = this.trigram(1).value;
    this.trigram(1).value = temp;
    this.value = this.trigram(0).value + this.trigram(1).value * 8;
    this.updateInnerValues();
    this.updateOuterValues();
    this.restoreMoving(bMoving, true, false);
    return this;
  }

  public override nuclear(): this {
    const bMoving = [
      [false, false, false],
      [false, false, false],
    ];
    this.saveMoving(bMoving);
    const temp = this.trigram(1).line(0).value;
    this.trigram(1).line(2).value = this.trigram(1).line(1).value;
    this.trigram(1).line(1).value = this.trigram(1).line(0).value;
    this.trigram(1).line(0).value = this.trigram(0).line(2).value;
    this.trigram(0).line(0).value = this.trigram(0).line(1).value;
    this.trigram(0).line(1).value = this.trigram(0).line(2).value;
    this.trigram(0).line(2).value = temp;
    this.value =
      (((this.trigram(1).line(2).value % 2 === 0 ? 0 : 1) * 4 +
        (this.trigram(1).line(1).value % 2 === 0 ? 0 : 1) * 2 +
        (this.trigram(1).line(0).value % 2 === 0 ? 0 : 1)) *
        8) +
      (this.trigram(0).line(2).value % 2 === 0 ? 0 : 1) * 4 +
      (this.trigram(0).line(1).value % 2 === 0 ? 0 : 1) * 2 +
      (this.trigram(0).line(0).value % 2 === 0 ? 0 : 1);
    this.updateInnerValues();
    this.updateOuterValues();
    if (bMoving[1][1]) {
      this.trigram(1).line(2).old();
    }
    if (bMoving[1][0]) {
      this.trigram(1).line(0).old();
      this.trigram(1).line(1).old();
    }
    if (bMoving[0][2]) {
      this.trigram(0).line(2).old();
      this.trigram(0).line(1).old();
    }
    if (bMoving[0][1]) {
      this.trigram(0).line(0).old();
    }
    return this;
  }

  public override move(): this {
    this.trigram(1).move();
    this.trigram(0).move();
    return this;
  }

  public override young(): this {
    this.trigram(1).young();
    this.trigram(0).young();
    return this;
  }

  public override updateInnerValues(): void {
    this.trigram(1).value = Math.floor(this.value / 8);
    this.trigram(0).value = this.value % 8;
    this.trigram(1).updateInnerValues();
    this.trigram(0).updateInnerValues();
  }

  public override updateOuterValues(): void {
    this.value = this.trigram(0).value + this.trigram(1).value * 8;
  }

  private saveMoving(bMoving: boolean[][]): void {
    for (let t = 0; t < 2; ++t) {
      for (let l = 0; l < 3; ++l) {
        const lineValue = this.trigram(t).line(l).value;
        if (lineValue === 0 || lineValue === 3) {
          bMoving[t][l] = true;
        }
      }
    }
  }

  private restoreMoving(bMoving: boolean[][], bInverseTrigram: boolean, bInverseLine: boolean): void {
    for (let t = 0; t < 2; ++t) {
      for (let l = 0; l < 3; ++l) {
        if (bMoving[t][l]) {
          let l1 = l;
          if (bInverseLine) {
            if (l1 === 0) {
              l1 = 2;
            } else if (l1 === 2) {
              l1 = 0;
            }
          }
          const trigramIndex = bInverseTrigram ? 1 - t : t;
          this.trigram(trigramIndex).line(l1).old();
        }
      }
    }
  }

  public static setCurrentSequence(nCurrentSequence: number): void {
    this.m_nCurrentSequence = nCurrentSequence;
  }

  public static setCurrentRatio(nRatio: number): void {
    this.m_nCurrentRatio = nRatio;
  }

  public static setCurrentLabel(nLabel: number): void {
    this.m_nCurrentLabel = nLabel;
  }

  protected override getLabel(): string {
    return Sequences.strHexagramLabels[this.getCurrentLabel()][this.value];
  }

  protected override getMoving(): boolean {
    return this.trigram(0).isMoving || this.trigram(1).isMoving;
  }

  protected override getCurrentSequence(): number {
    return CHexagramValueSequencer.m_nCurrentSequence;
  }

  protected override getCurrentRatio(): number {
    return CHexagramValueSequencer.m_nCurrentRatio;
  }

  protected override getCurrentLabel(): number {
    return CHexagramValueSequencer.m_nCurrentLabel;
  }

  public hexagramId(bValue = false): string {
    const baseId = (bValue ? this.value : this.sequence + 1).toString().padStart(2, " ");
    if (!this.isMoving) {
      return baseId;
    }
    let s = `${baseId}.`;
    for (let l = 0; l < 6; ++l) {
      if (this.trigram(Math.floor(l / 3)).line(l % 3).isMoving) {
        s += (l + 1).toString();
      }
    }
    return s;
  }

  public describePrimary(bValue = false): string {
    const label = this.label;
    const valueStr = bValue ? ` (${this.valueStr})` : "";
    return `${this.hexagramId()} ${label}${valueStr}`;
  }

  public describeSecondary(bValue = false): string {
    if (this.isMoving) {
      const primary = this;
      const secondary = new CHexagramValueSequencer(primary);
      secondary.move();
      const valueStr = bValue ? ` (${secondary.valueStr})` : "";
      return `${secondary.hexagramId()} ${secondary.label}${valueStr}`;
    }
    return "";
  }

  public describeCast(bValue = false): string {
    const primary = this.describePrimary(bValue);
    const secondary = this.describeSecondary(bValue);
    return secondary ? `${primary} > ${secondary}` : primary;
  }
}
