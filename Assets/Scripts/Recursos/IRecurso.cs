using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecurso {

	void PickedUp(string name, bool hasWeapon, bool hasTrap, bool hasPowerUp);
	void SetUses (int newUses);

}
