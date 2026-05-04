using System;
using System.Collections.Generic;
using System.Linq;

namespace YijingGrayCode;

public static class GrayCode6
{
	private static readonly Random _random = new();
	private static readonly Lazy<int[]> _nonSimpleBaseCycle6 = new(() => BuildGrayCycleBySearch(bits: 6, seed: 42));

	// =========================================================
	// BASIC GENERATION
	// =========================================================

	public static int[] GenerateReflectedGrayCode(int bits = 6)
	{
		int count = 1 << bits;
		int[] result = new int[count];

		for (int n = 0; n < count; n++)
			result[n] = n ^ (n >> 1);

		return result;
	}

	public static int[] GenerateRandomGrayCycle(int bits = 6, int? seed = null)
	{
		Random rng = CreateCycleRandom(seed);
		int count = 1 << bits;
		int[] cycle = bits == 6
			? (int[])_nonSimpleBaseCycle6.Value.Clone()
			: GenerateReflectedGrayCode(bits);
		int[] bitPermutation = Enumerable.Range(0, bits).ToArray();

		Shuffle(bitPermutation, rng);

		cycle = TransformSequence(
			cycle,
			bitPermutation,
			xorMask: rng.Next(count),
			reverse: rng.Next(2) == 0,
			bits);

		cycle = RotateToStartAt(cycle, 0);

		int targetTwists = Math.Max(bits, count / 4);
		int attempts = 0;
		int applied = 0;
		int maxAttempts = targetTwists * 8;

		while (applied < targetTwists && attempts < maxAttempts)
		{
			if (TryApplyRandomCycleTwist(cycle, bits, rng))
				applied++;

			attempts++;
		}

		return cycle;
	}

	private static int[] BuildGrayCycleBySearch(int bits, int seed)
	{
		int count = 1 << bits;
		int[] path = new int[count];
		bool[] used = new bool[count];

		path[0] = 0;
		used[0] = true;

		if (!SearchGrayCycle(bits, path, used, 1, new Random(seed)))
			throw new InvalidOperationException("Failed to construct the base Gray cycle.");

		return path;
	}

	public static List<int[]> GenerateRandomGrayCycles(int howMany, int bits = 6, int startSeed = 1)
	{
		List<int[]> result = new();

		for (int i = 0; i < howMany; i++)
			result.Add(GenerateRandomGrayCycle(bits, startSeed + i));

		return result;
	}

	private static bool SearchGrayCycle(int bits, int[] path, bool[] used, int depth, Random rng)
	{
		int count = 1 << bits;
		int startDepth = depth;
		List<int>?[] candidatesByDepth = new List<int>[count];
		int[] nextCandidateIndex = new int[count];

		// Use an explicit stack so repeated searches do not rely on recursive call depth.
		while (true)
		{
			if (depth == count)
			{
				if (DiffersByOneBit(path[count - 1], path[0]))
					return true;

				if (depth == startDepth)
					return false;

				depth--;
				used[path[depth]] = false;
				continue;
			}

			if (candidatesByDepth[depth] == null)
			{
				candidatesByDepth[depth] = BuildSearchCandidates(path[depth - 1], used, bits, rng);
				nextCandidateIndex[depth] = 0;
			}

			List<int> candidates = candidatesByDepth[depth]!;
			bool advanced = false;

			while (nextCandidateIndex[depth] < candidates.Count)
			{
				int next = candidates[nextCandidateIndex[depth]++];

				if (depth == count - 1 && !DiffersByOneBit(next, path[0]))
					continue;

				used[next] = true;
				path[depth] = next;
				depth++;

				if (depth < count)
					candidatesByDepth[depth] = null;

				advanced = true;
				break;
			}

			if (advanced)
				continue;

			candidatesByDepth[depth] = null;

			if (depth == startDepth)
				return false;

			depth--;
			used[path[depth]] = false;
		}
	}

	private static List<int> BuildSearchCandidates(int current, bool[] used, int bits, Random rng)
	{
		List<int> candidates = new();

		for (int bit = 0; bit < bits; bit++)
		{
			int next = current ^ (1 << bit);
			if (!used[next])
				candidates.Add(next);
		}

		Shuffle(candidates, rng);

		// Heuristic: prefer tighter branches first
		candidates.Sort((a, b) =>
		{
			int da = CountUnusedNeighbors(a, used, bits);
			int db = CountUnusedNeighbors(b, used, bits);
			return da.CompareTo(db);
		});

		return candidates;
	}

	private static Random CreateCycleRandom(int? seed)
	{
		if (seed.HasValue)
			return new Random(seed.Value);

		lock (_random)
			return new Random(_random.Next());
	}

	private static bool TryApplyRandomCycleTwist(int[] cycle, int bits, Random rng)
	{
		for (int rotationAttempt = 0; rotationAttempt < Math.Min(8, cycle.Length); rotationAttempt++)
		{
			int[] working = RotateByOffset(cycle, rng.Next(cycle.Length));
			List<(int StartEdge, int EndEdge)> moves = new();
			int edgeCount = working.Length - 1;
			int[] flipBits = new int[edgeCount];

			for (int edge = 0; edge < edgeCount; edge++)
				flipBits[edge] = FlippedBitIndex(working[edge], working[edge + 1], bits);

			for (int startEdge = 0; startEdge < edgeCount - 2; startEdge++)
			{
				for (int endEdge = startEdge + 2; endEdge < edgeCount; endEdge++)
				{
					if (flipBits[startEdge] != flipBits[endEdge])
						continue;

					if (DiffersByOneBit(working[startEdge], working[endEdge]))
						moves.Add((startEdge, endEdge));
				}
			}

			if (moves.Count == 0)
				continue;

			(int moveStartEdge, int moveEndEdge) = moves[rng.Next(moves.Count)];
			ReverseRange(working, moveStartEdge + 1, moveEndEdge);
			working = RotateToStartAt(working, 0);
			Array.Copy(working, cycle, working.Length);
			return true;
		}

		return false;
	}

	private static void ReverseRange(int[] items, int start, int end)
	{
		while (start < end)
		{
			(items[start], items[end]) = (items[end], items[start]);
			start++;
			end--;
		}
	}

	private static int[] RotateByOffset(int[] sequence, int offset)
	{
		int[] result = new int[sequence.Length];

		for (int i = 0; i < sequence.Length; i++)
			result[i] = sequence[(offset + i) % sequence.Length];

		return result;
	}

	private static int CountUnusedNeighbors(int value, bool[] used, int bits)
	{
		int count = 0;

		for (int bit = 0; bit < bits; bit++)
		{
			int neighbor = value ^ (1 << bit);
			if (!used[neighbor])
				count++;
		}

		return count;
	}

	private static void Shuffle<T>(IList<T> list, Random rng)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			int j = rng.Next(i + 1);
			(list[i], list[j]) = (list[j], list[i]);
		}
	}

	// =========================================================
	// BASIC BIT / DISTANCE HELPERS
	// =========================================================

	public static string ToBinaryString(int value, int bits = 6)
	{
		return Convert.ToString(value, 2).PadLeft(bits, '0');
	}

	public static int HammingDistance(int a, int b)
	{
		int x = a ^ b;
		int count = 0;

		while (x != 0)
		{
			x &= (x - 1);
			count++;
		}

		return count;
	}

	public static bool DiffersByOneBit(int a, int b)
	{
		int x = a ^ b;
		return x != 0 && (x & (x - 1)) == 0;
	}

	public static int FlippedBitIndex(int a, int b, int bits = 6)
	{
		int x = a ^ b;

		if (x == 0 || (x & (x - 1)) != 0)
			return -1;

		for (int bit = 0; bit < bits; bit++)
		{
			if (x == (1 << bit))
				return bit;
		}

		return -1;
	}

	// =========================================================
	// VALIDATION
	// =========================================================

	public static bool IsValidGrayCode(int[] sequence, int bits = 6, bool cyclic = true)
	{
		if (sequence == null)
			return false;

		int count = 1 << bits;
		if (sequence.Length != count)
			return false;

		bool[] seen = new bool[count];

		for (int i = 0; i < sequence.Length; i++)
		{
			int value = sequence[i];
			if (value < 0 || value >= count || seen[value])
				return false;

			seen[value] = true;

			if (i > 0 && !DiffersByOneBit(sequence[i - 1], value))
				return false;
		}

		if (cyclic && !DiffersByOneBit(sequence[^1], sequence[0]))
			return false;

		return true;
	}

	// =========================================================
	// CUBE SYMMETRY TRANSFORMS
	// =========================================================

	public static int TransformState(int value, int[] bitPermutation, int xorMask, int bits = 6)
	{
		int transformed = 0;

		for (int oldBit = 0; oldBit < bits; oldBit++)
		{
			if ((value & (1 << oldBit)) != 0)
			{
				int newBit = bitPermutation[oldBit];
				transformed |= (1 << newBit);
			}
		}

		return transformed ^ xorMask;
	}

	public static int[] TransformSequence(int[] sequence, int[] bitPermutation, int xorMask, bool reverse = false, int bits = 6)
	{
		int[] result = new int[sequence.Length];

		if (!reverse)
		{
			for (int i = 0; i < sequence.Length; i++)
				result[i] = TransformState(sequence[i], bitPermutation, xorMask, bits);
		}
		else
		{
			for (int i = 0; i < sequence.Length; i++)
				result[i] = TransformState(sequence[sequence.Length - 1 - i], bitPermutation, xorMask, bits);
		}

		return result;
	}

	public static int[] RotateToStartAt(int[] sequence, int startValue)
	{
		int index = Array.IndexOf(sequence, startValue);
		if (index < 0)
			throw new ArgumentException("Start value not found in sequence.");

		int[] result = new int[sequence.Length];

		for (int i = 0; i < sequence.Length; i++)
			result[i] = sequence[(index + i) % sequence.Length];

		return result;
	}

	public static bool CyclicallyEqual(int[] a, int[] b)
	{
		if (a == null || b == null || a.Length != b.Length)
			return false;

		int n = a.Length;

		for (int offset = 0; offset < n; offset++)
		{
			bool match = true;

			for (int i = 0; i < n; i++)
			{
				if (a[i] != b[(i + offset) % n])
				{
					match = false;
					break;
				}
			}

			if (match)
				return true;
		}

		return false;
	}

	public static IEnumerable<int[]> GenerateBitPermutations(int bits)
	{
		int[] items = new int[bits];
		for (int i = 0; i < bits; i++)
			items[i] = i;

		foreach (int[] p in Permute(items, 0))
			yield return p;
	}

	private static IEnumerable<int[]> Permute(int[] items, int index)
	{
		if (index == items.Length - 1)
		{
			int[] copy = new int[items.Length];
			Array.Copy(items, copy, items.Length);
			yield return copy;
			yield break;
		}

		for (int i = index; i < items.Length; i++)
		{
			(items[index], items[i]) = (items[i], items[index]);

			foreach (int[] p in Permute(items, index + 1))
				yield return p;

			(items[index], items[i]) = (items[i], items[index]);
		}
	}

	public static bool IsSimpleVariantOfReflected(int[] candidate, int bits = 6)
	{
		int[] reflected = GenerateReflectedGrayCode(bits);

		foreach (int[] permutation in GenerateBitPermutations(bits))
		{
			int maxMask = 1 << bits;

			for (int mask = 0; mask < maxMask; mask++)
			{
				int[] transformed = TransformSequence(reflected, permutation, mask, false, bits);
				if (CyclicallyEqual(candidate, transformed))
					return true;

				int[] reversed = TransformSequence(reflected, permutation, mask, true, bits);
				if (CyclicallyEqual(candidate, reversed))
					return true;
			}
		}

		return false;
	}

	// =========================================================
	// KING WEN SCORING LAYER
	// =========================================================
	//
	// kingWenSequence:
	//   array of 64 unique ints 0..63 in King Wen order, using your chosen
	//   6-bit encoding of hexagrams.
	//
	// grayCycle:
	//   array of 64 unique ints 0..63 in a valid cyclic Gray traversal.
	//
	// These methods score how "close" a Gray cycle is to King Wen under
	// several notions of adjacency and ordering.
	// =========================================================

	public static KingWenGrayComparison CompareToKingWen(int[] grayCycle, int[] kingWenSequence, int bits = 6)
	{
		ValidatePermutation(grayCycle, bits, nameof(grayCycle));
		ValidatePermutation(kingWenSequence, bits, nameof(kingWenSequence));

		if (!IsValidGrayCode(grayCycle, bits, cyclic: true))
			throw new ArgumentException("grayCycle is not a valid cyclic Gray code.", nameof(grayCycle));

		KingWenGrayComparison result = new()
		{
			GrayCycle = (int[])grayCycle.Clone(),
			KingWenSequence = (int[])kingWenSequence.Clone(),
			GrayStepCount = grayCycle.Length,
			KingWenAdjacentHammingHistogram = BuildAdjacentHammingHistogram(kingWenSequence, cyclic: false, bits: bits),
			KingWenCyclicHammingHistogram = BuildAdjacentHammingHistogram(kingWenSequence, cyclic: true, bits: bits),
			GrayFlippedBitHistogram = BuildGrayFlipHistogram(grayCycle, bits),
			GrayFlipSequence = BuildGrayFlipSequence(grayCycle, bits),
			KingWenStepDistances = BuildAdjacentDistances(kingWenSequence, cyclic: false),
			KingWenCyclicStepDistances = BuildAdjacentDistances(kingWenSequence, cyclic: true)
		};

		Dictionary<int, int> grayIndex = BuildIndexMap(grayCycle);
		Dictionary<int, int> kwIndex = BuildIndexMap(kingWenSequence);

		// 1. How many adjacent King Wen pairs are also adjacent somewhere in the Gray cycle?
		int exactAdjacencyMatches = 0;
		int cyclicAdjacencyMatches = 0;

		for (int i = 0; i < kingWenSequence.Length - 1; i++)
		{
			int a = kingWenSequence[i];
			int b = kingWenSequence[i + 1];

			if (AreAdjacentInCycle(a, b, grayIndex, grayCycle.Length))
				exactAdjacencyMatches++;
		}

		for (int i = 0; i < kingWenSequence.Length; i++)
		{
			int a = kingWenSequence[i];
			int b = kingWenSequence[(i + 1) % kingWenSequence.Length];

			if (AreAdjacentInCycle(a, b, grayIndex, grayCycle.Length))
				cyclicAdjacencyMatches++;
		}

		result.KingWenAdjacentPairMatchesInGrayCycle = exactAdjacencyMatches;
		result.KingWenCyclicPairMatchesInGrayCycle = cyclicAdjacencyMatches;

		// 2. Average cyclic step distance between King Wen neighbors when laid onto Gray cycle
		result.AverageGrayCycleDistanceForKingWenAdjacentPairs =
			AverageCyclicPairDistance(kingWenSequence, grayIndex, grayCycle.Length, cyclic: false);

		result.AverageGrayCycleDistanceForKingWenCyclicPairs =
			AverageCyclicPairDistance(kingWenSequence, grayIndex, grayCycle.Length, cyclic: true);

		// 3. Spearman-like rough order correlation by position
		result.SumOfAbsolutePositionDisplacements =
			SumOfAbsolutePositionDisplacements(grayIndex, kwIndex);

		result.SumOfSquaredPositionDisplacements =
			SumOfSquaredPositionDisplacements(grayIndex, kwIndex);

		// 4. In King Wen order, what are the Hamming distances of adjacent pairs?
		result.KingWenAverageAdjacentHammingDistance =
			AverageAdjacentHammingDistance(kingWenSequence, cyclic: false);

		result.KingWenAverageCyclicHammingDistance =
			AverageAdjacentHammingDistance(kingWenSequence, cyclic: true);

		// 5. In King Wen order, how often do adjacent pairs already differ by 1 bit?
		result.KingWenOneBitAdjacentPairCount =
			CountAdjacentPairsAtHammingDistance(kingWenSequence, 1, cyclic: false);

		result.KingWenOneBitCyclicPairCount =
			CountAdjacentPairsAtHammingDistance(kingWenSequence, 1, cyclic: true);

		// 6. For each adjacent King Wen pair, where are those nodes on the Gray cycle?
		result.KingWenPairsProjectedOntoGrayCycle =
			BuildProjectedPairData(kingWenSequence, grayIndex, grayCycle.Length, cyclic: false);

		result.KingWenCyclicPairsProjectedOntoGrayCycle =
			BuildProjectedPairData(kingWenSequence, grayIndex, grayCycle.Length, cyclic: true);

		return result;
	}

	public static List<KingWenGrayComparison> CompareManyToKingWen(IEnumerable<int[]> grayCycles, int[] kingWenSequence, int bits = 6)
	{
		List<KingWenGrayComparison> result = new();

		foreach (int[] cycle in grayCycles)
			result.Add(CompareToKingWen(cycle, kingWenSequence, bits));

		return result;
	}

	public static KingWenGrayComparison FindBestGrayCycleAgainstKingWen(
		int[] kingWenSequence,
		int candidateCount = 100,
		int startSeed = 1,
		int bits = 6)
	{
		KingWenGrayComparison best = null;

		for (int i = 0; i < candidateCount; i++)
		{
			int[] candidate = GenerateRandomGrayCycle(bits, startSeed + i);
			KingWenGrayComparison comparison = CompareToKingWen(candidate, kingWenSequence, bits);

			if (best == null || comparison.Score > best.Score)
				best = comparison;
		}

		return best;
	}

	// Combined heuristic score. Adjust weights to taste.
	public static double ComputeKingWenAffinityScore(KingWenGrayComparison c)
	{
		if (c == null)
			return double.NegativeInfinity;

		double score = 0.0;

		score += c.KingWenAdjacentPairMatchesInGrayCycle * 10.0;
		score += c.KingWenCyclicPairMatchesInGrayCycle * 5.0;

		score -= c.AverageGrayCycleDistanceForKingWenAdjacentPairs * 2.0;
		score -= c.AverageGrayCycleDistanceForKingWenCyclicPairs * 1.0;

		score -= c.SumOfAbsolutePositionDisplacements / 64.0;
		score -= c.SumOfSquaredPositionDisplacements / 512.0;

		score += c.KingWenOneBitAdjacentPairCount * 2.0;
		score += c.KingWenOneBitCyclicPairCount * 1.0;

		return score;
	}

	// =========================================================
	// KING WEN / CYCLE ANALYSIS HELPERS
	// =========================================================

	private static void ValidatePermutation(int[] sequence, int bits, string paramName)
	{
		if (sequence == null)
			throw new ArgumentNullException(paramName);

		int count = 1 << bits;
		if (sequence.Length != count)
			throw new ArgumentException($"Sequence must have length {count}.", paramName);

		bool[] seen = new bool[count];

		foreach (int v in sequence)
		{
			if (v < 0 || v >= count)
				throw new ArgumentException($"Sequence contains out-of-range value {v}.", paramName);

			if (seen[v])
				throw new ArgumentException($"Sequence contains duplicate value {v}.", paramName);

			seen[v] = true;
		}
	}

	private static Dictionary<int, int> BuildIndexMap(int[] sequence)
	{
		Dictionary<int, int> map = new(sequence.Length);

		for (int i = 0; i < sequence.Length; i++)
			map[sequence[i]] = i;

		return map;
	}

	private static bool AreAdjacentInCycle(int a, int b, Dictionary<int, int> cycleIndex, int cycleLength)
	{
		int ia = cycleIndex[a];
		int ib = cycleIndex[b];
		int d = Math.Abs(ia - ib);

		return d == 1 || d == cycleLength - 1;
	}

	private static int CyclicDistance(int aIndex, int bIndex, int cycleLength)
	{
		int d = Math.Abs(aIndex - bIndex);
		return Math.Min(d, cycleLength - d);
	}

	private static double AverageCyclicPairDistance(int[] sequence, Dictionary<int, int> cycleIndex, int cycleLength, bool cyclic)
	{
		int pairCount = cyclic ? sequence.Length : sequence.Length - 1;
		int total = 0;

		for (int i = 0; i < pairCount; i++)
		{
			int a = sequence[i];
			int b = sequence[(i + 1) % sequence.Length];

			total += CyclicDistance(cycleIndex[a], cycleIndex[b], cycleLength);
		}

		return (double)total / pairCount;
	}

	private static long SumOfAbsolutePositionDisplacements(Dictionary<int, int> grayIndex, Dictionary<int, int> kwIndex)
	{
		long total = 0;

		foreach (int state in grayIndex.Keys)
			total += Math.Abs(grayIndex[state] - kwIndex[state]);

		return total;
	}

	private static long SumOfSquaredPositionDisplacements(Dictionary<int, int> grayIndex, Dictionary<int, int> kwIndex)
	{
		long total = 0;

		foreach (int state in grayIndex.Keys)
		{
			long d = grayIndex[state] - kwIndex[state];
			total += d * d;
		}

		return total;
	}

	public static int[] BuildAdjacentHammingDistances(int[] sequence, bool cyclic)
	{
		int pairCount = cyclic ? sequence.Length : sequence.Length - 1;
		int[] result = new int[pairCount];

		for (int i = 0; i < pairCount; i++)
		{
			int a = sequence[i];
			int b = sequence[(i + 1) % sequence.Length];
			result[i] = HammingDistance(a, b);
		}

		return result;
	}

	public static Dictionary<int, int> BuildAdjacentHammingHistogram(int[] sequence, bool cyclic, int bits = 6)
	{
		Dictionary<int, int> histogram = new();

		for (int d = 0; d <= bits; d++)
			histogram[d] = 0;

		foreach (int distance in BuildAdjacentHammingDistances(sequence, cyclic))
			histogram[distance]++;

		return histogram;
	}

	public static double AverageAdjacentHammingDistance(int[] sequence, bool cyclic)
	{
		int[] distances = BuildAdjacentHammingDistances(sequence, cyclic);
		return distances.Average();
	}

	public static int CountAdjacentPairsAtHammingDistance(int[] sequence, int distance, bool cyclic)
	{
		int count = 0;

		foreach (int d in BuildAdjacentHammingDistances(sequence, cyclic))
		{
			if (d == distance)
				count++;
		}

		return count;
	}

	public static int[] BuildGrayFlipSequence(int[] grayCycle, int bits = 6)
	{
		if (!IsValidGrayCode(grayCycle, bits, cyclic: true))
			throw new ArgumentException("Sequence must be a valid cyclic Gray code.", nameof(grayCycle));

		int[] flips = new int[grayCycle.Length];

		for (int i = 0; i < grayCycle.Length; i++)
		{
			int a = grayCycle[i];
			int b = grayCycle[(i + 1) % grayCycle.Length];
			flips[i] = FlippedBitIndex(a, b, bits);
		}

		return flips;
	}

	public static Dictionary<int, int> BuildGrayFlipHistogram(int[] grayCycle, int bits = 6)
	{
		int[] flips = BuildGrayFlipSequence(grayCycle, bits);
		Dictionary<int, int> histogram = new();

		for (int bit = 0; bit < bits; bit++)
			histogram[bit] = 0;

		foreach (int bit in flips)
			histogram[bit]++;

		return histogram;
	}

	public static int[] BuildAdjacentDistances(int[] sequence, bool cyclic)
	{
		int pairCount = cyclic ? sequence.Length : sequence.Length - 1;
		int[] result = new int[pairCount];

		Dictionary<int, int> index = BuildIndexMap(sequence);

		for (int i = 0; i < pairCount; i++)
		{
			int a = sequence[i];
			int b = sequence[(i + 1) % sequence.Length];
			result[i] = Math.Abs(index[a] - index[b]);
		}

		return result;
	}

	private static List<ProjectedPairData> BuildProjectedPairData(
		int[] kingWenSequence,
		Dictionary<int, int> grayIndex,
		int grayCycleLength,
		bool cyclic)
	{
		int pairCount = cyclic ? kingWenSequence.Length : kingWenSequence.Length - 1;
		List<ProjectedPairData> result = new(pairCount);

		for (int i = 0; i < pairCount; i++)
		{
			int a = kingWenSequence[i];
			int b = kingWenSequence[(i + 1) % kingWenSequence.Length];

			int indexA = grayIndex[a];
			int indexB = grayIndex[b];
			int distance = CyclicDistance(indexA, indexB, grayCycleLength);
			int hamming = HammingDistance(a, b);

			result.Add(new ProjectedPairData
			{
				PairIndex = i,
				A = a,
				B = b,
				ABinary = ToBinaryString(a),
				BBinary = ToBinaryString(b),
				GrayIndexA = indexA,
				GrayIndexB = indexB,
				CyclicDistanceOnGrayCycle = distance,
				HammingDistance = hamming,
				AreAdjacentOnGrayCycle = distance == 1
			});
		}

		return result;
	}

	// =========================================================
	// PRETTY PRINT HELPERS
	// =========================================================

	public static string FormatSequenceAsBinaryLines(int[] sequence, int bits = 6)
	{
		return string.Join(Environment.NewLine, sequence.Select(v => ToBinaryString(v, bits)));
	}

	public static string FormatSequenceAsIndexedBinaryLines(int[] sequence, int bits = 6)
	{
		List<string> lines = new(sequence.Length);

		for (int i = 0; i < sequence.Length; i++)
			lines.Add($"{i,2}: {sequence[i],2}  {ToBinaryString(sequence[i], bits)}");

		return string.Join(Environment.NewLine, lines);
	}

	public static string FormatHistogram<TKey>(IDictionary<TKey, int> histogram)
	{
		return string.Join(Environment.NewLine, histogram.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}: {kv.Value}"));
	}

	public static string DescribeComparison(KingWenGrayComparison c)
	{
		List<string> lines = new();

		lines.Add($"Score: {c.Score:F3}");
		lines.Add($"KingWenAdjacentPairMatchesInGrayCycle: {c.KingWenAdjacentPairMatchesInGrayCycle}");
		lines.Add($"KingWenCyclicPairMatchesInGrayCycle: {c.KingWenCyclicPairMatchesInGrayCycle}");
		lines.Add($"AverageGrayCycleDistanceForKingWenAdjacentPairs: {c.AverageGrayCycleDistanceForKingWenAdjacentPairs:F3}");
		lines.Add($"AverageGrayCycleDistanceForKingWenCyclicPairs: {c.AverageGrayCycleDistanceForKingWenCyclicPairs:F3}");
		lines.Add($"SumOfAbsolutePositionDisplacements: {c.SumOfAbsolutePositionDisplacements}");
		lines.Add($"SumOfSquaredPositionDisplacements: {c.SumOfSquaredPositionDisplacements}");
		lines.Add($"KingWenAverageAdjacentHammingDistance: {c.KingWenAverageAdjacentHammingDistance:F3}");
		lines.Add($"KingWenAverageCyclicHammingDistance: {c.KingWenAverageCyclicHammingDistance:F3}");
		lines.Add($"KingWenOneBitAdjacentPairCount: {c.KingWenOneBitAdjacentPairCount}");
		lines.Add($"KingWenOneBitCyclicPairCount: {c.KingWenOneBitCyclicPairCount}");
		lines.Add("");
		lines.Add("King Wen adjacent Hamming histogram:");
		lines.Add(FormatHistogram(c.KingWenAdjacentHammingHistogram));
		lines.Add("");
		lines.Add("King Wen cyclic Hamming histogram:");
		lines.Add(FormatHistogram(c.KingWenCyclicHammingHistogram));
		lines.Add("");
		lines.Add("Gray flip histogram:");
		lines.Add(FormatHistogram(c.GrayFlippedBitHistogram));

		return string.Join(Environment.NewLine, lines);
	}
}

// =============================================================
// RESULT CLASSES
// =============================================================

public sealed class KingWenGrayComparison
{
	public int[] GrayCycle { get; set; }
	public int[] KingWenSequence { get; set; }

	public int GrayStepCount { get; set; }

	public int KingWenAdjacentPairMatchesInGrayCycle { get; set; }
	public int KingWenCyclicPairMatchesInGrayCycle { get; set; }

	public double AverageGrayCycleDistanceForKingWenAdjacentPairs { get; set; }
	public double AverageGrayCycleDistanceForKingWenCyclicPairs { get; set; }

	public long SumOfAbsolutePositionDisplacements { get; set; }
	public long SumOfSquaredPositionDisplacements { get; set; }

	public double KingWenAverageAdjacentHammingDistance { get; set; }
	public double KingWenAverageCyclicHammingDistance { get; set; }

	public int KingWenOneBitAdjacentPairCount { get; set; }
	public int KingWenOneBitCyclicPairCount { get; set; }

	public Dictionary<int, int> KingWenAdjacentHammingHistogram { get; set; }
	public Dictionary<int, int> KingWenCyclicHammingHistogram { get; set; }

	public Dictionary<int, int> GrayFlippedBitHistogram { get; set; }
	public int[] GrayFlipSequence { get; set; }

	public int[] KingWenStepDistances { get; set; }
	public int[] KingWenCyclicStepDistances { get; set; }

	public List<ProjectedPairData> KingWenPairsProjectedOntoGrayCycle { get; set; }
	public List<ProjectedPairData> KingWenCyclicPairsProjectedOntoGrayCycle { get; set; }

	public double Score => GrayCode6.ComputeKingWenAffinityScore(this);
}

public sealed class ProjectedPairData
{
	public int PairIndex { get; set; }

	public int A { get; set; }
	public int B { get; set; }

	public string ABinary { get; set; }
	public string BBinary { get; set; }

	public int GrayIndexA { get; set; }
	public int GrayIndexB { get; set; }

	public int CyclicDistanceOnGrayCycle { get; set; }
	public int HammingDistance { get; set; }

	public bool AreAdjacentOnGrayCycle { get; set; }

	public override string ToString()
	{
		return $"{PairIndex,2}: {A,2} ({ABinary}) -> {B,2} ({BBinary}), GrayDist={CyclicDistanceOnGrayCycle}, Hamming={HammingDistance}, Adj={AreAdjacentOnGrayCycle}";
	}
}

// =============================================================
// EXAMPLE PROGRAM
// =============================================================
//
// Replace kingWenHexagrams with your own 64-element encoding.
// Each hexagram must be encoded as a unique int from 0 to 63.
//
// If your bit encoding is bottom-line = LSB, keep it consistent
// across both King Wen and Gray sequences.
// =============================================================

public static class Program
{
	public static void Main()
	{
		int[] reflected = GrayCode6.GenerateReflectedGrayCode();
		Console.WriteLine("Reflected Gray code valid: " + GrayCode6.IsValidGrayCode(reflected));
		Console.WriteLine();

		Console.WriteLine("First 16 reflected states:");
		for (int i = 0; i < 16; i++)
			Console.WriteLine($"{i,2}: {reflected[i],2}  {GrayCode6.ToBinaryString(reflected[i])}");

		Console.WriteLine();

		int[] randomCycle = GrayCode6.GenerateRandomGrayCycle(seed: 42);
		Console.WriteLine("Random Gray cycle valid: " + GrayCode6.IsValidGrayCode(randomCycle));
		Console.WriteLine("Random cycle is symmetry variant of reflected: " + GrayCode6.IsSimpleVariantOfReflected(randomCycle));
		Console.WriteLine();

		Console.WriteLine("First 16 random cycle states:");
		for (int i = 0; i < 16; i++)
			Console.WriteLine($"{i,2}: {randomCycle[i],2}  {GrayCode6.ToBinaryString(randomCycle[i])}");

		Console.WriteLine();
		Console.WriteLine("Random cycle flip histogram:");
		Console.WriteLine(GrayCode6.FormatHistogram(GrayCode6.BuildGrayFlipHistogram(randomCycle)));
		Console.WriteLine();

		// ---------------------------------------------------------
		// TODO:
		// Replace this placeholder with your actual King Wen mapping.
		// It MUST be a permutation of 0..63 in King Wen order.
		// ---------------------------------------------------------
		//int[] kingWenHexagrams = Enumerable.Range(0, 64).ToArray();
		//int[] kingWenHexagrams = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63};
		int[] kingWenHexagrams = { 1, 23, 6, 18, 14, 35, 45, 10, 15, 50, 39, 53, 61, 54, 31, 33, 7, 2, 28, 59, 38, 62, 47, 4, 44, 16, 46, 57, 30, 48, 27, 42, 22, 26, 3, 40, 51, 21, 17, 25, 34, 20, 63, 37, 55, 29, 49, 13, 19, 41, 58, 60, 52, 36, 56, 8, 11, 24, 5, 9, 32, 12, 43, 0 };

		KingWenGrayComparison reflectedComparison = GrayCode6.CompareToKingWen(reflected, kingWenHexagrams);
		KingWenGrayComparison randomComparison = GrayCode6.CompareToKingWen(randomCycle, kingWenHexagrams);

		Console.WriteLine("Reflected vs King Wen:");
		Console.WriteLine(GrayCode6.DescribeComparison(reflectedComparison));
		Console.WriteLine();

		Console.WriteLine("Random vs King Wen:");
		Console.WriteLine(GrayCode6.DescribeComparison(randomComparison));
		Console.WriteLine();

		KingWenGrayComparison best = GrayCode6.FindBestGrayCycleAgainstKingWen(
			kingWenHexagrams,
			candidateCount: 50,
			startSeed: 100);

		Console.WriteLine("Best random candidate vs King Wen:");
		Console.WriteLine(GrayCode6.DescribeComparison(best));
		Console.WriteLine();

		Console.WriteLine("First 20 projected King Wen adjacent pairs onto best Gray cycle:");
		foreach (ProjectedPairData pair in best.KingWenPairsProjectedOntoGrayCycle.Take(20))
			Console.WriteLine(pair);
	}
}
