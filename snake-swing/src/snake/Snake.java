package snake;

import java.util.List;

public class Snake {

	public int stepH;
	public int stepV;
	public List<SimplePoint> trail;
	
	
	public void faceToDown()	{ this.stepV = 1;  this.stepH = 0; }
	public void faceToUp()   	{ this.stepV = -1; this.stepH = 0; }
	public void faceToLeft()	{ this.stepV = 0;  this.stepH = -1; }
	public void faceToRight()	{ this.stepV = 0;  this.stepH = 1; }
	
}