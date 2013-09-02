using UnityEngine;
using System.Collections.Generic;

public enum Ability
{
	MOVE_LEFT,
	MOVE_RIGHT,
	MOVE_UP,
	MOVE_DOWN,
	JUMP,
	FAST_FALL
}

public class AbilityManager : MonoBehaviour {
	public List<Ability> abilities;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public bool hasAbility(Ability ability) {
		return abilities.Contains(ability);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
