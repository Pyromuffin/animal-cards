using Godot;
using System;
using System.Collections.Generic;

public partial class Pips : Node2D
{
	[Export] public float pipSpacing;
	[Export] public Texture2D filledTexture, emptyTexture;
	
	public List<Sprite2D> pips = new List<Sprite2D>();


	public void Make(int pipCount, int filled) {
		for(int i = 0; i <pipCount; i++){
			var pip = new Sprite2D();
			AddChild(pip);
			pips.Add(pip);
			pip.Position = new Vector2(pipSpacing * i, 0);
			pip.TextureFilter = TextureFilterEnum.NearestWithMipmaps;
		}

		FillPips(filled);
	}

	public void FillPips(int filled){
		for(int i = 0; i <pips.Count; i++){
			if(i < filled) {
				pips[i].Texture = filledTexture;
			} else {
				pips[i].Texture = emptyTexture;
			}
		}
	}

}
