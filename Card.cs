using Godot;
using System;

public partial class Card : Sprite2D
{

	public bool hovered = false;
	public bool clonked = false;
	[Export] public float hoverScale = 1.5f;
	[Export] public float clonkScale = 2.0f;
	[Export] public float slideOut = 10f;
	float initialScale;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		initialScale = Scale.X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (clonked) {
			Scale = new Vector2(clonkScale * initialScale, clonkScale * initialScale);
		} else if(hovered){
			Scale = new Vector2(hoverScale * initialScale, hoverScale * initialScale);
		} else {
			Scale = new Vector2(initialScale, initialScale);
		}

	}


	void _on_area_2d_mouse_entered(){
		hovered = true;
	}

	void _on_area_2d_mouse_exited(){
		hovered = false;
		clonked = false;
	}

	void  _on_area_2d_input_event(Node viewPort, InputEvent e, int ShapeIdx){
		if(e is InputEventMouseButton){
			clonked = true;
		}
	}

}
