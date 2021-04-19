using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICore
{
    void AssignAllJoysticksToSystemPlayer(bool removeFromOtherPlayers);
    void ChangeInputMaps(string name);
    void ChangeScene(string name);
    void MaskChangeScene(string name, bool withLoading);
}
