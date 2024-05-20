using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance { get; private set; }
    private Animator animator;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        animator = GetComponent<Animator>();
    }

    public void ReloadAnim() => animator.SetTrigger("Reload");

    public void FireAnim() => animator.SetTrigger("Fire");

    public bool CheckReloadAnimPlaying()
    {
        bool reloadAnim = animator.GetCurrentAnimatorStateInfo(1).IsName("Reloading");
        Debug.Log(reloadAnim);
        return reloadAnim;
    }
}
