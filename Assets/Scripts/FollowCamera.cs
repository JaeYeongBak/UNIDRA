using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public float distance = 5.0f;
	public float horizontalAngle = 0.0f;
	public float rotAngle = 180.0f; // 화면 가로폭만큼 커서를 이동시켰을 때 몇 도 회전하는가.
	public float verticalAngle = 10.0f;
	public GameObject targetPlayer;
	public Vector3 offset = Vector3.zero;
    public Vector3 NPCTalkOffset = Vector3.zero;

    PlayerCtrl playerCtrl;

	InputManager inputManager;

	void CameraFollowPlayer()
    {
        if(playerCtrl.state == PlayerCtrl.State.Talking)
        {
            transform.position = playerCtrl.talkNPC.transform.position
                + (playerCtrl.talkNPC.transform.forward * 5.0f) + NPCTalkOffset;
            transform.rotation = playerCtrl.talkNPC.transform.rotation;
            transform.Rotate(0.0f, 180.0f, 0.0f);

            return;
        }

        // 드래그 입력으로 카메라 회전각을 갱신한다.
        if (inputManager.Moved())
        {
            float anglePerPixel = rotAngle / (float)Screen.width;
            Vector2 delta = inputManager.GetDeltaPosition();
            horizontalAngle += delta.x * anglePerPixel;
            horizontalAngle = Mathf.Repeat(horizontalAngle, 360.0f);
            verticalAngle -= delta.y * anglePerPixel;
            verticalAngle = Mathf.Clamp(verticalAngle, -60.0f, 60.0f);
        }

        // 카메라의 위치와 회전각을 갱신한다.
        if (targetPlayer != null)
        {
            Vector3 lookPosition = targetPlayer.transform.position + offset;
            // 주시 대상에서 상대 위치를 구한다.
            Vector3 relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0) * new Vector3(0, 0, -distance);

            // 주시 대상의 위치에 오프셋을 더한 위치로 이동시킨다.
            transform.position = lookPosition + relativePos;

            // 주시 대상을 주시하게 한다.
            transform.LookAt(lookPosition);

            // 장애물을 피한다.
            RaycastHit hitInfo;
            if (Physics.Linecast(lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Ground")))
                transform.position = hitInfo.point;
        }
    }

	void Start()
	{
		inputManager = FindObjectOfType<InputManager>();
        playerCtrl = targetPlayer.GetComponent<PlayerCtrl>();
	}

	// Update is called once per frame
	void LateUpdate () {

        switch(playerCtrl.story)
        {
            case PlayerCtrl.Story.Prologue:
                break;
            case PlayerCtrl.Story.Play:
                CameraFollowPlayer();
                break;
            case PlayerCtrl.Story.Ending:
                break;
        }
	}
}
