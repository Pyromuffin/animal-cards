using Godot;
using System;

public partial class HealthBar : Node2D
{
	[Export] public Node2D fill;
	[Export] public int fullPosition, emptyPosition;

	public void SetHealth(int current, int max) {
		var percent = (float)current / max;
		var pos = Mathf.Lerp(emptyPosition, fullPosition, percent);
		fill.Position = new Vector2(pos, 0);
	}
}
