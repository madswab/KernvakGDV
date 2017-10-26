using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICubeWalker {
    GameObject gameObject { get; }
    Transform transform { get; }
    CubeSide CurrentWalkOnSide { get; set; }
}
