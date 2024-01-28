using Godot;
using System;

public partial class HealthBar : Node2D
{
	[Export] public Node2D fill;
	[Export] public Label text;
	[Export] public int fullPosition, emptyPosition;

	public void SetHealth(int current, int max) {
		var percent = (float)current / max;
		var pos = Mathf.Lerp(emptyPosition, fullPosition, percent);
		fill.Position = new Vector2(pos, 0);
		text.Text = current + "/" + max;
		if( current < max / 2 )
		{
			fill.Modulate = new Color( 1.0f, 0, 0 );
		}
		else
		{
			fill.Modulate = new Color( 0, 1.0f, 0 );
		}
	}
}
