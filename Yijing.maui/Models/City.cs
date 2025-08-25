using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Yijing.Models;

public class City : INotifyPropertyChanged
{
	private double population;

	public double Population { get => population; set { population = value; OnPropertyChanged(); } }

	#nullable enable
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(propertyName, new PropertyChangedEventArgs(propertyName));
	}
}
