using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class Giant : MonoBehaviour
{
    public float speed = 2;
    public float moveThreshold = 0.1f;
    public LineRenderer lr;
    public Tilemap tilemap;
    public Tile grass;
    public Tile stone;
    public Tile[] crackedStone;
    public Transform rightFoot;
    public Transform leftFoot;
    public CinemachineImpulseSource impulseSource;
    public AudioSource audioSource;
    public AudioClip[] footstepSFX;
    SpriteRenderer sr;
    Animator animator;
    public List<Vector2> waypoints;
    void Start()
    {
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
        waypoints = new List<Vector2>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePos = tilemap.WorldToCell(mousePos);
            if(tilemap.GetTile(tilePos) != grass)
            {
                waypoints.Add(mousePos);
                lr.positionCount++;
                lr.SetPosition(waypoints.Count, mousePos);
            } 
        }

        if (waypoints.Count == 0) return;

        if (Vector2.Distance(transform.position, waypoints[0]) < moveThreshold)
        {
            waypoints.RemoveAt(0);

            lr.positionCount--;
            for(int i = 0; i < waypoints.Count; i++)
            {
                lr.SetPosition(i+1, waypoints[i]);
            }
        }

        if (waypoints.Count == 0) 
        {
            animator.SetFloat("movement", 0);
        }
        else
        {
            Vector2 direction = waypoints[0] - (Vector2)transform.position;
            direction.Normalize();

            if (direction.x < 0)
            {
                sr.flipX = true;
            }
            if (direction.x > 0)
            {
                sr.flipX = false;
            }
            animator.SetFloat("movement", direction.magnitude);

            transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;
            lr.SetPosition(0, transform.position);
        }
    }

    public void FootStep()
    {
        audioSource.PlayOneShot(footstepSFX[Random.Range(0, footstepSFX.Length)]);
        impulseSource.GenerateImpulse();

        Vector3Int footPos;
        if (sr.flipX)
        {
            footPos = tilemap.WorldToCell(leftFoot.position);
        }
        else
        {
            footPos = tilemap.WorldToCell(rightFoot.position);
        }
        if (tilemap.GetTile(footPos) != grass)
        {
            tilemap.SetTile(footPos, crackedStone[Random.Range(0, crackedStone.Length)]);
        }
    }
}
