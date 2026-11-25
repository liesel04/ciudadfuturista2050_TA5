using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovil : MonoBehaviour
{
    public float velocidad = 5f, sensibilidad = 0.15f; CharacterController cc; Transform cam; float pitch;
    int idMov = -1, idMir = -1; Vector2 mov;
    float S { get { return Screen.height * .22f; } }
    Rect Base { get { return new Rect(Screen.width * .08f, Screen.height * .70f, S, S); } }
    void Start() { cc = GetComponent<CharacterController>(); cam = GetComponentInChildren<Camera>().transform; }
    void Update()
    {
        Vector2 m = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetMouseButton(0) && Input.touchCount == 0) Mirar(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 8f);
        Vector2 centro = new Vector2(Base.center.x, Screen.height - Base.center.y);
        foreach (Touch t in Input.touches)
        {
            bool izq = t.position.x < Screen.width / 2f;
            if (t.phase == TouchPhase.Began) { if (izq && idMov < 0) idMov = t.fingerId; else if (!izq && idMir < 0) idMir = t.fingerId; }
            if (t.fingerId == idMov) { mov = Vector2.ClampMagnitude((t.position - centro) / (S / 2f), 1f); if (t.phase >= TouchPhase.Ended) { idMov = -1; mov = Vector2.zero; } }
            if (t.fingerId == idMir) { Mirar(t.deltaPosition); if (t.phase >= TouchPhase.Ended) idMir = -1; }
        }
        m += mov;
        cc.Move(((transform.right * m.x + transform.forward * m.y) * velocidad + Vector3.down * 9.8f) * Time.deltaTime);
    }
    void Mirar(Vector2 d) { transform.Rotate(0, d.x * sensibilidad, 0); pitch = Mathf.Clamp(pitch - d.y * sensibilidad, -80, 80); cam.localRotation = Quaternion.Euler(pitch, 0, 0); }
    void OnGUI()
    {
        GUI.color = new Color(0, 1, 1, .25f); GUI.DrawTexture(Base, Texture2D.whiteTexture);
        Vector2 k = new Vector2(Base.center.x + mov.x * S / 2, Base.center.y - mov.y * S / 2);
        GUI.color = new Color(0, 1, 1, .8f); GUI.DrawTexture(new Rect(k.x - S * .2f, k.y - S * .2f, S * .4f, S * .4f), Texture2D.whiteTexture);
        GUI.color = Color.white; GUI.Label(new Rect(Screen.width * .55f, Screen.height * .9f, Screen.width * .4f, 30), "Arrastra aquí para mirar");
    }
}