using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelController
{
    void StartLevel();
    void EndLevel();
    void ResetLevel();

    List<Transform> GetCheckpoints();
    Transform GetCurrentCP();

}
