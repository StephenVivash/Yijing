using Xunit;
using Yijing.Services;

namespace Yijing.Maui.Test;

public sealed class TriggerToneFeedbackTests
{
	private static readonly DateTime Start = new(2026, 10, 15, 12, 0, 0);

	[Fact]
	public void BandCrossingHighProducesSingleToneImmediatelyBetweenComparisons()
	{
		var feedback = new TriggerToneFeedback();
		Assert.Null(feedback.Update(Start, 21, 0.3f, 0.2f, 0.4f));
		Assert.Equal(0.4f, feedback.Update(Start.AddSeconds(10), 21, 0.35f, 0.2f, 0.4f));
		Assert.Equal(0.0f, feedback.Update(Start.AddSeconds(11), 21, 0.45f, 0.2f, 0.4f));
	}

	[Fact]
	public void AboveHighDoesNotRepeatTheCrossingCueAndNextPairUsesLow()
	{
		var feedback = new TriggerToneFeedback();
		Assert.Equal(0.0f, feedback.Update(Start, 21, 0.45f, 0.2f, 0.4f));
		Assert.Null(feedback.Update(Start.AddSeconds(1), 21, 0.5f, 0.2f, 0.4f));
		Assert.Equal(0.2f, feedback.Update(Start.AddSeconds(10), 21, 0.5f, 0.2f, 0.4f));
		Assert.Equal(0.0f, feedback.Update(Start.AddSeconds(11), 21, 0.15f, 0.2f, 0.4f));
		Assert.Null(feedback.Update(Start.AddSeconds(12), 21, 0.1f, 0.2f, 0.4f));
		Assert.Equal(0.4f, feedback.Update(Start.AddSeconds(21), 21, 0.3f, 0.2f, 0.4f));
	}

	[Fact]
	public void CrossingAtComparisonTimeProducesOnlyTheSingleCue()
	{
		var feedback = new TriggerToneFeedback();
		feedback.Update(Start, 21, 0.3f, 0.2f, 0.4f);
		Assert.Equal(0.0f, feedback.Update(Start.AddSeconds(10), 21, 0.4f, 0.2f, 0.4f));
		Assert.Null(feedback.Update(Start.AddSeconds(10.1), 21, 0.4f, 0.2f, 0.4f));
	}

	[Fact]
	public void ResetAfterMindCastOrNewSessionRearmsTheHighTrigger()
	{
		var feedback = new TriggerToneFeedback();
		feedback.Update(Start, 21, 0.5f, 0.2f, 0.4f);
		feedback.Reset(Start.AddSeconds(20));
		Assert.Null(feedback.Update(Start.AddSeconds(20), 21, 0.3f, 0.2f, 0.4f));
		Assert.Equal(0.4f, feedback.Update(Start.AddSeconds(30), 21, 0.3f, 0.2f, 0.4f));
	}

	[Fact]
	public void SelectingAnotherChannelDoesNotInheritTheOldLowTrigger()
	{
		var feedback = new TriggerToneFeedback();
		feedback.Update(Start, 21, 0.5f, 0.2f, 0.4f);
		Assert.Null(feedback.Update(Start.AddSeconds(1), 24, 0.3f, 0.2f, 0.4f));
		Assert.Equal(0.0f, feedback.Update(Start.AddSeconds(2), 24, 0.45f, 0.2f, 0.4f));
	}

	[Fact]
	public void MovingTriggerCanProduceACrossingWithoutBandMovement()
	{
		var feedback = new TriggerToneFeedback();
		feedback.Update(Start, 21, 0.3f, 0.2f, 0.4f);
		Assert.Equal(0.0f, feedback.Update(Start.AddSeconds(1), 21, 0.3f, 0.1f, 0.25f));
	}

	[Fact]
	public void UninitialisedTriggersAndNonfiniteSamplesDoNotProduceCues()
	{
		var feedback = new TriggerToneFeedback();
		Assert.Null(feedback.Update(Start, 21, 0.0f, 0.0f, 0.0f));
		Assert.Null(feedback.Update(Start, 21, float.NaN, 0.2f, 0.4f));
		Assert.Null(feedback.Update(Start, 21, 0.3f, 0.2f, 0.4f));
	}
}
