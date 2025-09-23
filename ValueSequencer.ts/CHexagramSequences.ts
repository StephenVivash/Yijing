import { CHexagramValueSequencer } from "./CHexagramValueSequencer";
import { Sequences } from "./Sequences";

export class CHexagramSequences {
  public autoCast(hvs: CHexagramValueSequencer): CHexagramValueSequencer {
    const random = Sequences.randomSession;
    for (let l = 0; l < 6; ++l) {
      const count = (random.next(5) + 1) * 100 + random.next(100);
      for (let c = 0; c < count; ++c) {
        hvs.trigram(Math.floor(l / 3)).line(l % 3).next(true);
      }
    }
    return hvs;
  }
}

export class CHexagram {
  private m_hvsPrimary: CHexagramValueSequencer;
  private _count = 0;

  constructor(hvsPrimary: CHexagramValueSequencer) {
    this.m_hvsPrimary = new CHexagramValueSequencer(hvsPrimary);
  }

  get describeCast(): string {
    return this.m_hvsPrimary.describeCast();
  }

  public compareTo(other: CHexagram): number {
    return this.m_hvsPrimary.hexagramId().localeCompare(other.m_hvsPrimary.hexagramId());
  }

  public add(): void {
    this._count += 1;
  }

  get count(): number {
    return this._count;
  }
}

export class CHexagramArray {
  private m_arrHexagram: CHexagram[] = [];

  constructor() {
    const hvsPrimary = new CHexagramValueSequencer(0);
    hvsPrimary.first();
    for (let p = 0; p < 64; ++p) {
      for (let s = 0; s < 64; ++s) {
        const hvs = new CHexagramValueSequencer(hvsPrimary);
        for (let l = 0; l < 6; ++l) {
          if (((s & (1 << l)) >> l) === 1) {
            hvs.trigram(Math.floor(l / 3)).line(l % 3).next(false);
          }
        }
        this.add(hvs);
      }
      hvsPrimary.next();
    }
    this.m_arrHexagram.sort((a, b) => a.compareTo(b));
  }

  public add(hvsPrimary: CHexagramValueSequencer): void {
    this.m_arrHexagram.push(new CHexagram(hvsPrimary));
  }

  public multiCast(nCount: number): this {
    const hvs = new CHexagramValueSequencer(63);
    for (let i = 0; i < nCount; ++i) {
      this.autoCast(hvs);
      const h = new CHexagram(hvs);
      const index = this.m_arrHexagram.findIndex((item) => item.compareTo(h) === 0);
      if (index >= 0) {
        this.m_arrHexagram[index].add();
      }
    }
    return this;
  }

  public autoCast(hvs: CHexagramValueSequencer): CHexagramValueSequencer {
    const random = Sequences.randomSession;
    for (let l = 0; l < 6; ++l) {
      const count = (random.next(5) + 1) * 100 + random.next(100);
      for (let c = 0; c < count; ++c) {
        hvs.trigram(Math.floor(l / 3)).line(l % 3).next(true);
      }
    }
    return hvs;
  }

  public hexagramArray(): CHexagram[] {
    return this.m_arrHexagram;
  }

  public at(index: number): CHexagram {
    return this.m_arrHexagram[index];
  }
}
