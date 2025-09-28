using ValueSequencer;
using Xunit;

namespace Yijing.test;

//[Collection("MySequentialCollection")]
public class HexagramTests
{
	[Fact]
	public void CopyConstructor()
	{
		Sequences.Initialise();
		CHexagramValueSequencer.SetCurrentSequence(0); // Numeric
		var hvs1 = new CHexagramValueSequencer(0);
		var hvs2 = new CHexagramValueSequencer(ref hvs1);
		Assert.Equal(hvs1.Value, hvs2.Value);
		hvs1.Next();
		Assert.NotEqual(hvs1.Value, hvs2.Value);
		hvs2.Next();
		Assert.Equal(hvs1.Value, hvs2.Value);
	}

	[Fact]
	public void NumericSequence()
	{
		Sequences.Initialise();
		CHexagramValueSequencer.SetCurrentSequence(0); // Numeric
		Sequences.SetLSB(false); // Bottom so Value = Sequence = i
		var hvs = new CHexagramValueSequencer(0);
		hvs.Last();

		for (int i = 0; i < 64; i++)
		{
			Assert.Equal(i, hvs.Next().Sequence);
			Assert.Equal(i, hvs.Value);
		}
	}

	[Fact]
	public void FuxiSequence()
	{
		// Value always calculated LSB = Bottom
		int[] expectedValues =
		{
			0,32,16,48,8,40,24,56,4,36,20,52,12,44,28,60,2,34,18,50,10,42,26,58,6,38,22,54,
			14,46,30,62,1,33,17,49,9,41,25,57,5,37,21,53,13,45,29,61,3,35,19,51,11,43,27,59,
			7,39,23,55,15,47,31,63
		};

		Sequences.Initialise();
		CHexagramValueSequencer.SetCurrentSequence(1); // Fuxi
		Sequences.SetLSB(true); // Top - irrelevant with expected Value
		var hvs = new CHexagramValueSequencer(0);
		hvs.Last();

		for (int i = 0; i < 64; i++)
		{
			Assert.Equal(i, hvs.Next().Sequence);
			Assert.Equal(expectedValues[i], hvs.Value);
		}
	}

	[Fact]
	public void WenSequence()
	{
		int[] expectedValues =
		{
			63,0,17,34,23,58,2,16,55,59,7,56,61,47,4,8,25,38,3,48,41,37,32,1,57,39,33,30,18,
			45,28,14,60,15,40,5,53,43,20,10,35,49,31,62,24,6,26,22,29,46,9,36,52,11,13,44,54,
			27,50,19,51,12,21,42,
		};

		Sequences.Initialise();
		CHexagramValueSequencer.SetCurrentSequence(2); // Wen
		var hvs = new CHexagramValueSequencer(0);
		hvs.Last();

		for (int i = 0; i < 32; i++)
		{
			Assert.Equal(i * 2, hvs.Next().Sequence);
			Assert.Equal(expectedValues[i * 2], hvs.Value);

			int v = hvs.Value;
			if (hvs.Inverse().Value == v)
				hvs.Opposite();

			Assert.Equal(i * 2 + 1, hvs.Sequence);
			Assert.Equal(expectedValues[i * 2 + 1], hvs.Value);
		}
	}

	[Fact]
	public void WenFirstHexagramMoveProducesSecond()
	{
		Sequences.Initialise();
		CHexagramValueSequencer.SetCurrentSequence(2); // Wen
		var hvs = new CHexagramValueSequencer(0);
		hvs.First();

		Assert.Equal(0, hvs.Sequence);
		Assert.Equal(63, hvs.Value);

		for (int trigram = 0; trigram < 2; trigram++)
			for (int line = 0; line < 3; line++)
				hvs.Trigram(trigram).Line(line).Old();

		Assert.True(hvs.IsMoving);
		Assert.Equal(0, hvs.Sequence);
		Assert.Equal(63, hvs.Value);

		hvs.Move();

		Assert.Equal(1, hvs.Sequence);
		Assert.Equal(0, hvs.Value);
		Assert.False(hvs.IsMoving);
	}

	[Fact]
	public void MysterySequence()
	{
		Sequences.Initialise();
		CHexagramValueSequencer.SetCurrentSequence(3); // Mystery
		var hvs = new CHexagramValueSequencer(0);
		hvs.Last();

		// Highlight the sub-sequence pattern
		int[] expectedValues = {
			63, 62, 61, 59, 55, 47, 31,
			60, 57, 51, 39, 15,
			58, 53, 43, 23,
			54, 45, 27,
			46, 29,
			30,
			56, 49, 35, 7,
			50, 37, 11,
			52, 41, 19,

			38, 13,
			44, 25,
			14, 28,
			26, 22,
			21, 42,
			48, 33, 3,

			34, 5, 36, 9, 40, 17, 6, 12, 24, 18, 20, 10, // ???
			
			1, 2, 4, 8, 16, 32, 0
		};

		for (int i = 0; i < 64; i++)
		{
			Assert.Equal(i, hvs.Next().Sequence);
			Assert.Equal(expectedValues[i], hvs.Value);
		}
	}
}
