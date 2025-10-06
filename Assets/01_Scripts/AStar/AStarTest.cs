using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AStar))]
public class AStarTest : MonoBehaviour
{
    [SerializeField] private NPCPath npcPath = null;
    [SerializeField] private bool moveNPC = false;
    [SerializeField] private Vector2Int finishPosition;
    [SerializeField] private SceneName sceneName;
    [SerializeField] private AnimationClip idleDownAnimationClip = null;
    [SerializeField] private AnimationClip eventAnimationClip = null;
    private NPCMovement npcMovement;

    private void Start()
    {
        npcMovement = npcPath.GetComponent<NPCMovement>();
        npcMovement.npcFacingDirectionAtDestination = Direction.down;
        npcMovement.npcTargetAnimationClip = idleDownAnimationClip;
    }

    private void Update()
    {
        if (moveNPC)
        {
            moveNPC = false;

            NPCScheduleEvent npcScheduleEvent = new NPCScheduleEvent(0, 0, 0, 0, Weather.none, Season.None, sceneName, new GridCoordinate(finishPosition.x, finishPosition.y), eventAnimationClip);

            npcPath.BuildPath(npcScheduleEvent);
        }
    }
}
