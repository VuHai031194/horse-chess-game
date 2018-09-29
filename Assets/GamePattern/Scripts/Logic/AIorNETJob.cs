using UnityEngine;
using System.Collections;

public class AIorNETJob : ThreadedJob {

    private int bestMove = 0;

	protected override void ThreadFunction() {
        //Debug.Log("Start Thread!");
		bestMove = AI.main.SearchPosition();

	}
	
	protected override void OnFinished ()
	{
        //Debug.Log("Finish");
		// Tinh toan nuoc di

        GUIPlay.main.ComMoveCall(bestMove);
	}

	// Move Chess
	
}
