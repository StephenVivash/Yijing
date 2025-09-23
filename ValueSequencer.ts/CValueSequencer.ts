import { Sequences } from "./Sequences";

export abstract class CValueSequencer {
  protected m_nValue: number;
  protected m_nSequence: number;
  protected m_nRatio: number;

  protected m_nInnerSequencers: number;
  protected m_nValues: number;

  protected m_nSequences: number[][];
  protected m_nRatios: number[][];
  protected m_pvsParent: CValueSequencer | null;
  protected m_pvsInner: CValueSequencer[];

  constructor(nInnerSequencers: number, nValues: number, nValue: number) {
    this.m_nInnerSequencers = nInnerSequencers;
    this.m_nValues = nValues;
    this.m_nValue = nValue;
    this.m_nSequence = 0;
    this.m_nRatio = 0;
    this.m_nSequences = [];
    this.m_nRatios = [];
    this.m_pvsParent = null;
    this.m_pvsInner = [];
  }

  get value(): number {
    return this.m_nValue;
  }

  set value(value: number) {
    this.setValue(value);
  }

  get sequence(): number {
    return this.m_nSequence;
  }

  set sequence(sequence: number) {
    this.setValue(this.findValue(sequence));
  }

  get valueStr(): string {
    const sequences = Sequences.nHexagramSequences;
    if (sequences.length > 0 && sequences[0] && this.value < sequences[0].length) {
      return sequences[0][this.value].toString();
    }
    return this.value.toString();
  }

  get sequenceStr(): string {
    return (this.sequence + 1).toString();
  }

  get label(): string {
    return this.getLabel();
  }

  get isMoving(): boolean {
    return this.getMoving();
  }

  public first(): this {
    this.setValue(this.getFirstSequence());
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public previous(bRatio = false): this {
    if (!bRatio || --this.m_nRatio === 0) {
      this.setValue(this.getPreviousSequence(this.m_nSequence));
      this.updateInnerValues();
      this.updateOuterValues();
    }
    return this;
  }

  public next(bRatio = false): this {
    if (!bRatio || --this.m_nRatio === 0) {
      this.setValue(this.getNextSequence(this.m_nSequence));
      this.updateInnerValues();
      this.updateOuterValues();
    }
    return this;
  }

  public last(): this {
    this.setValue(this.getLastSequence());
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public inverse(): this {
    return this;
  }

  public opposite(): this {
    this.setValue(~this.m_nValue & (this.m_nValues - 1));
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public transverse(): this {
    return this;
  }

  public nuclear(): this {
    return this;
  }

  public move(): this {
    return this;
  }

  public young(): this {
    return this;
  }

  public old(): this {
    return this;
  }

  public update(): this {
    this.setValue(this.m_nValue);
    this.updateInnerValues();
    this.updateOuterValues();
    return this;
  }

  public updateInnerValues(): void {}
  public updateOuterValues(): void {}

  public setParent(pvsParent: CValueSequencer): void {
    this.m_pvsParent = pvsParent;
  }

  protected getParent(): CValueSequencer | null {
    return this.m_pvsParent;
  }

  protected getChild(nIndex: number): CValueSequencer | undefined {
    return this.m_pvsInner[nIndex];
  }

  protected setValue(nValue: number): this {
    if (nValue >= 0 && nValue <= this.m_nValues) {
      this.m_nValue = nValue;
      const sequenceRow = this.m_nSequences[this.getCurrentSequence()];
      if (sequenceRow && nValue < sequenceRow.length) {
        this.m_nSequence = sequenceRow[nValue];
      }
      const ratioRow = this.m_nRatios[this.getCurrentRatio()];
      if (ratioRow && nValue < ratioRow.length) {
        this.m_nRatio = ratioRow[nValue];
      }
    }
    return this;
  }

  protected findValue(nSequence: number): number {
    const sequenceRow = this.m_nSequences[this.getCurrentSequence()];
    if (!sequenceRow) {
      return -1;
    }
    for (let i = 0; i < this.m_nValues; ++i) {
      if (sequenceRow[i] === nSequence) {
        return i;
      }
    }
    return -1;
  }

  protected getFirstSequence(): number {
    return this.findValue(0);
  }

  protected getPreviousSequence(nSequence: number): number {
    if (nSequence > 0) {
      return this.findValue(nSequence - 1);
    }
    return this.getLastSequence();
  }

  protected getNextSequence(nSequence: number): number {
    if (nSequence < this.m_nValues - 1) {
      const value = this.findValue(nSequence + 1);
      if (value !== -1) {
        return value;
      }
    }
    return this.getFirstSequence();
  }

  protected getLastSequence(): number {
    let nSequence2 = this.m_nValues;
    while (nSequence2 > -1) {
      const nSequence1 = this.findValue(--nSequence2);
      if (nSequence1 !== -1) {
        return nSequence1;
      }
    }
    return -1;
  }

  protected getLabel(): string {
    return "";
  }

  protected getMoving(): boolean {
    return false;
  }

  protected getCurrentSequence(): number {
    return 0;
  }

  protected getCurrentRatio(): number {
    return 0;
  }

  protected getCurrentLabel(): number {
    return 0;
  }
}
