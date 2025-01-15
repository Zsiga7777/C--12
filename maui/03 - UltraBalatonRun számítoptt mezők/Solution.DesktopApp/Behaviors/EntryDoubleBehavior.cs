using CommunityToolkit.Common;
using ErrorOr;
using System.Linq;

namespace Solution.DesktopApp.Behaviors;

public class EntryDoubleBehavior : Behavior<Entry>
{
	protected override void OnAttachedTo(Entry entry)
	{
		base.OnAttachedTo(entry);

		// Perform setup
		entry.TextChanged += OnTextChanged;
	}

	protected override void OnDetachingFrom(Entry entry)
	{
		base.OnDetachingFrom(entry);

		// Perform clean up
		entry.TextChanged -= OnTextChanged;
	}

	private void OnTextChanged(object? sender, TextChangedEventArgs e)
	{
		Entry entry = sender as Entry;

		if (string.IsNullOrEmpty(e.NewTextValue))
		{
			entry!.Text = null;
			return;
		}

		int temp2 = 0;
		for(int i = 0; i < e.NewTextValue.Length; i++)
		{
			if (e.NewTextValue[i] == ',' || e.NewTextValue[i] == '.')
			{
				temp2++;

			}
		}

		if (temp2 == 1 && (e.NewTextValue.Last() == ',' || e.NewTextValue.Last() == '.'))
		{
            return;
        }
		
		bool isValid = double.TryParse(e.NewTextValue, out double result);
			entry!.Text = isValid ? result.ToString() : e.OldTextValue;
		
	}
}
