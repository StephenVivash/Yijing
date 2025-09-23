import { CHexagramValueSequencer } from "./CHexagramValueSequencer";
import { Sequences } from "./Sequences";

function main(): void {
  CHexagramValueSequencer.setCurrentSequence(Sequences.HexagramSequence);
  CHexagramValueSequencer.setCurrentRatio(Sequences.HexagramRatio);
  CHexagramValueSequencer.setCurrentLabel(Sequences.HexagramLabel);

  const wenSequence = Sequences.nHexagramSequences[Sequences.HexagramSequence];
  const hexagram = new CHexagramValueSequencer(0).first();

  for (let i = 0; i < wenSequence.length; i++) {
    console.log(hexagram.describeCast());
    hexagram.next();
  }
}

main();
