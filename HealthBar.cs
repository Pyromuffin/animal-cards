using Godot;
using System;

public partial class HealthBar : Node2D
{
	[Export] public Node2D fill;
	[Export] public Label text;
	[Export] public int fullPosition, emptyPosition;

	public void SetHealth(int current, int max, bool changeColor = true) {
		var percent = (float)current / max;
		var pos = Mathf.Lerp(emptyPosition, fullPosition, percent);
		var tween = CreateTween();
		tween.TweenProperty(fill, "position", new Vector2(pos, 0), 0.2f);
		text.Text = current + "/" + max;

		if(changeColor){
			if( current < max / 2 )
			{
				fill.Modulate = new Color( 0.54f, 0.098f, 0.098f );
			}
			else
			{
				fill.Modulate = new Color( 0.627f, 0.651f, 0.227f );
			}
		}
		
	}
}
