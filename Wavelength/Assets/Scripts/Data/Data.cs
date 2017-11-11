using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
	List<string> unlockedLevels;
	List<float> bestTimes;
	string lastPlayed;

	//default data constructor
	public Data ()
	{
		unlockedLevels = new List<string> { };
		bestTimes = new List<float> { };
		lastPlayed = null;
	}
}