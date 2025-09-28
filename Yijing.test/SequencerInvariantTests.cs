
using System;
using Xunit;

using ValueSequencer;

// ChatGPT's test sugestion ??????? 

namespace YijingMachine;

public interface IYijingMachine
{
	// Snapshot of full state
	int Hex { get; }            // 1..64 (King Wen index or your canonical index—just be consistent)
	bool[] Lines { get; }       // length 6, bottom..top, true=yang (or your convention, but consistent)
	bool[] Lower { get; }       // length 3, bottom..top
	bool[] Upper { get; }       // length 3, bottom..top

	// Control which “dial” Next() will move
	void ActivateHex();
	void ActivateLower();
	void ActivateUpper();
	void ActivateLine(int i);   // 0..5 (bottom..top)

	// Move the active dial forward one step in its own cycle
	void Next();

	// Your propagation passes
	void UpdateInner();         // recompute trigrams/lines from hex (outer → inner)
	void UpdateOuter();         // recompute hex (and peers) from the inner part you changed (inner → outer)

	// Convenience to put the machine in a known hexagram
	void ResetToHex(int hex);   // must leave machine coherent after a call to UpdateInner()
}

// ---- Adapter sketch (fill this with your real object) ----
public sealed class VSAdapter : IYijingMachine
{
	private readonly ValueSequencer.CValueSequencer _vs;
	public VSAdapter(ValueSequencer.CValueSequencer vs) { _vs = vs; }

	public int Hex => 0 /* map to 1..64 from your internal index */; // ??????????????????????????????????????????????????????????????????????????????????????????
	public bool[] Lines => new bool[] { true, true, true, true, true, true } /* return 6 booleans bottom..top */;
	public bool[] Lower => new bool[] { true, true, true } /* return 3 booleans bottom..top */;
	public bool[] Upper => new bool[] { true, true, true } /* return 3 booleans bottom..top */;

	public void ActivateHex() { /* set active to hexagram mode */ }
	public void ActivateLower() { /* set active to lower trigram mode */ }
	public void ActivateUpper() { /* set active to upper trigram mode */ }
	public void ActivateLine(int i) { /* set active to specific line i */ }

	public void Next() { /* call your next/advance */ }
	public void UpdateInner() { /* call your UpdateInner(); */ }
	public void UpdateOuter() { /* call your UpdateOuter(); */ }
	public void ResetToHex(int hex)
	{
		/* set hex to 'hex' (1..64), call UpdateInner() so trigrams/lines match */
	}
}

// ---- Invariant helpers ----
public static class Invariants
{
	public static void AssertDecompositionIsCoherent(IYijingMachine m)
	{
		var lines = m.Lines;
		Assert.Equal(6, lines.Length);
		var lower = m.Lower;
		var upper = m.Upper;
		Assert.Equal(3, lower.Length);
		Assert.Equal(3, upper.Length);

		// Lower/upper trigrams must match the hexagram line split
		Assert.Equal(lower[0], lines[0]);
		Assert.Equal(lower[1], lines[1]);
		Assert.Equal(lower[2], lines[2]);
		Assert.Equal(upper[0], lines[3]);
		Assert.Equal(upper[1], lines[4]);
		Assert.Equal(upper[2], lines[5]);
	}
}

// ---- Tests ----
public class SequencerInvariantTests
{
	private IYijingMachine NewMachine()
	{
		// Construct your real sequencer and wrap it here.
		var core = new ValueSequencer.CValueSequencer(2, 64, 63);
		var m = new VSAdapter(core);
		return m;
	}

	[Fact]
	public void Reset_Then_UpdateInner_Produces_Coherent_State_For_All_64()
	{
		var m = NewMachine();
		for (int h = 1; h <= 64; h++)
		{
			m.ResetToHex(h);
			m.UpdateInner();
			Invariants.AssertDecompositionIsCoherent(m);
		}
	}

	[Fact]
	public void Advance_Hex_Then_UpdateInner_Keeps_Coherence()
	{
		var m = NewMachine();
		m.ResetToHex(1);
		for (int k = 0; k < 64; k++)
		{
			m.ActivateHex();
			m.Next();
			m.UpdateInner();
			Invariants.AssertDecompositionIsCoherent(m);
		}
	}

	[Fact]
	public void Advance_Lower_Then_UpdateOuter_Pushes_Into_Hex_Coherently()
	{
		var m = NewMachine();
		for (int h = 1; h <= 64; h++)
		{
			m.ResetToHex(h);
			m.UpdateInner();

			// Snapshot the upper before we touch the lower
			var upperBefore = (bool[])m.Upper.Clone();

			m.ActivateLower();
			m.Next();            // mutate only the lower trigram in your domain-specific way
			m.UpdateOuter();     // push change “outward” into the hex

			Invariants.AssertDecompositionIsCoherent(m);

			// Upper trigram should remain what it was (only lower changed)
			Assert.Equal(upperBefore, m.Upper);
		}
	}

	[Fact]
	public void Advance_Upper_Then_UpdateOuter_Pushes_Into_Hex_Coherently()
	{
		var m = NewMachine();
		for (int h = 1; h <= 64; h++)
		{
			m.ResetToHex(h);
			m.UpdateInner();

			var lowerBefore = (bool[])m.Lower.Clone();

			m.ActivateUpper();
			m.Next();
			m.UpdateOuter();

			Invariants.AssertDecompositionIsCoherent(m);
			Assert.Equal(lowerBefore, m.Lower);
		}
	}

	[Fact]
	public void Advance_Each_Line_Individually_Preserves_Trigram_Splits()
	{
		var m = NewMachine();
		m.ResetToHex(1);
		m.UpdateInner();

		for (int step = 0; step < 64; step++)
		{
			for (int i = 0; i < 6; i++)
			{
				m.ActivateLine(i);
				m.Next();
				m.UpdateOuter();
				Invariants.AssertDecompositionIsCoherent(m);
			}
		}
	}
}
