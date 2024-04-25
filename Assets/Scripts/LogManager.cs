using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogManager : MonoBehaviour
{
    public TMP_Text Log;
#region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        Log.enabled = false;
    }

#endregion

#region Public Functions
    public void LogDisplay(string _text){
        StopAllCoroutines();
        Log.enabled = true;
        Log.text = _text;

        StartCoroutine(FadeOut());
    }
#endregion

#region Private Functions

    private IEnumerator FadeOut(){
        float _timer = 0f;
        float _duration = 1f;
        float _target = 0f;
        float _initial = 1f;

        Color transition = Log.color;
        transition.a = _initial;
        Log.color = transition;

        yield return new WaitForSeconds(3f);

        while (_timer <= _duration)
        {
            transition.a = Mathf.Lerp(_initial,_target,_timer/_duration);
            Log.color = transition;
            _timer += Time.deltaTime;
            yield return null;
        }

        Log.enabled = false;

    }
#endregion
}
