// npx ts-node Test.ts

import { Sequences } from "./Sequences.ts";
import { CHexagramArray } from "./CHexagramSequences.ts";
import { CHexagramValueSequencer } from "./CHexagramValueSequencer.ts";

function main(): void {
	Sequences.initialise();
	CHexagramValueSequencer.setCurrentSequence(2);

	const hvs = new CHexagramValueSequencer(0).last();
	for (let i = 0; i < 64; i++) {
		console.log(hvs.next().describeCast());
	}

	const ha = new CHexagramArray();
	ha.multiCast(10000);
	ha.hexagramArray().forEach(h => {
		if (h.count > 0) {
			console.log(h.count + " : " + h.describeCast );
		}
	});

	console.log(hvs.previous().previous().opposite().describeCast());
}

main();
