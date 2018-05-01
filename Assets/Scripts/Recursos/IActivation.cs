using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivation{

	void Activate ();
	void Deactivate();
	int GetUses ();
	GameObject GetGameObject ();

}
