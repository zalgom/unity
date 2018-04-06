using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour {

	// Device Time
	private float deviceTimer = 39600f;
	private int min;
	private int hour;
	public Text deviceTimer_text;

	// Calling
	public Image calling_bg;
    public Image calling_status_bg;
    public GameObject[] calling_number;
    public GameObject calling_end_text;
    public Image bt_calling;
    public Sprite[] bt_calling_sprite;
    public Text bt_calling_text;
    public int calling_state_num = 0;

	public bool vibrateStatus;
	public float vibrateTimer;

	public bool callStatus;
	public float callTimer;
	public Text callTimer_text;

	public Image status_bg;
	public Sprite status_bg_callend;

	public AudioSource audio;
	public AudioClip bell;
	public AudioClip voice;

	// Info
	public GameObject info_panel;
	public GameObject info_next_page;
	public GameObject info_msg_0;
	public GameObject info_msg_1;
    public GameObject info_msg_2;
    public GameObject info_bt_Start;
	public GameObject info_bt_Next;
	public InputField nameInputField;
	public string nameString;
	public GameObject info_alert_panel;

	// Name
	public Text[] nameText;

	// Main
	public GameObject main_panel;
	public Text mainTimer_text;
	public Text mainDate_text;
	public bool startStatus;
	public GameObject alert_panel;

	// Main Push
	[System.Serializable]
	public class MainPush {
		public string main_bt_name;
		public Image main_bt_image;
		public GameObject main_new_tag;
		public bool runStatus;
		public GameObject push_group;
		public GameObject[] push_msg;
		public AudioClip push_clip;
		public GameObject sns_panel;
	}
	public List<MainPush> mainPush = new List<MainPush>();

	public int runAmount;
	public int curActiveNumber;
	public bool mainPushStatus;

	private float pushTimer;
	public int pushCount;
	public int prevPushNum;

	public int curPanelNumber;

	// Kakao
	public GameObject kakao_group_a;
	public GameObject[] kakao_msg_a;
	public GameObject kakao_group_b;
	public GameObject[] kakao_msg_b;
	public GameObject kakao_group_c;
	public GameObject[] kakao_msg_c;
	public GameObject kakao_group_d;
	public GameObject[] kakao_msg_d;
	public GameObject kakao_bt_back;
	public GameObject kakao_bt_close;
	public int kakao_msg_count;
	public GameObject kakao_list_panel;
	public bool kakao_ani_status;
	public RectTransform[] kakao_content;
	public GameObject[] kakao_scroll_view;

	public enum KakaoType {
		Atype,
		Btype,
		Ctype,
		Dtype
	}
	public KakaoType kakaoType = KakaoType.Atype;

	public Vector3 kakao_positon;
	public float kakao_pos_y;
	public AudioClip moanClip;

	// Facebook
	public GameObject fb_feed;
	public GameObject fb_bt_back;
	public GameObject fb_movie;
	public MediaPlayerCtrl fb_movie_control;
	public Scrollbar fb_scroll;
	public bool fb_check_status;

	// Message
	public GameObject message_content;
	public GameObject[] message_msg;
	public GameObject message_sub_panel;
	public GameObject[] message_sub;
	public int message_content_height;
	public GameObject message_bt_close;
	public Text[] message_date_text;
    public bool message_ani_status;
    public int message_sub_number;

	// Common
	public AudioClip msg_clip;
	public GameObject popup_pause;
	public bool isHomeStatus;

	// Ending
	public bool endingStatus;
	[System.Serializable]
	public class EndingPush
	{
		public string endingPush_kind;
		public GameObject endingPush_group;
		//public GameObject[] endingPush_msg;
		public List<GameObject> endingPush_msg;
	}
	public List<EndingPush> endingPush = new List<EndingPush>();

	public GameObject endingPush_array;
	private float endingPush_timer;
	private float endingPush_spawnTime = 0.5f;
	public int endingPush_count;
	//public GameObject prevEndingPush;
	public GameObject ending_bg;
	public AudioClip ending_clip;
	private int ending_kind;

	public GameObject ending_msg_panel;
    public float play_timer;
    public Text ending_time_text;
	public GameObject ending_msg_0;
	public GameObject ending_msg_1;
	public GameObject ending_bt_Sign;
	public GameObject ending_bt_Link;
	public GameObject ending_bt_Restart;
	public MediaPlayerCtrl ending_movie_control;
	public GameObject ending_movie_panel;
	public GameObject ending_movie;

	// Movie
	public int movie_number;


	// Use this for initialization
	void Start () {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        fb_movie_control.OnEnd += OnEnd;
		fb_movie_control.OnReady += OnReady;

		ending_movie_control.OnEnd += EndingOnEnd;
		ending_movie_control.OnReady += EndingOnReady;

		KakaoSetting();
		MessageSetting();
		EndingSetting();

		vibrateStatus = true;
		Handheld.Vibrate();
	}

	void KakaoSetting()
	{
		kakao_msg_a = new GameObject[kakao_group_a.transform.childCount];
		for (int i = 0; i < kakao_msg_a.Length; i++)
			kakao_msg_a[i] = kakao_group_a.transform.GetChild(i).gameObject;

		kakao_msg_b = new GameObject[kakao_group_b.transform.childCount];
		for (int i = 0; i < kakao_msg_b.Length; i++)
			kakao_msg_b[i] = kakao_group_b.transform.GetChild(i).gameObject;

		kakao_msg_c = new GameObject[kakao_group_c.transform.childCount];
		for (int i = 0; i < kakao_msg_c.Length; i++)
			kakao_msg_c[i] = kakao_group_c.transform.GetChild(i).gameObject;

		kakao_msg_d = new GameObject[kakao_group_d.transform.childCount];
		for (int i = 0; i < kakao_msg_d.Length; i++)
			kakao_msg_d[i] = kakao_group_d.transform.GetChild(i).gameObject;
	}

	void MessageSetting()
	{
		message_msg = new GameObject[message_content.transform.childCount];
		for (int i = 0; i < message_msg.Length; i++)
		{
			message_msg[i] = message_content.transform.GetChild(i).gameObject;
		}
    }

	void EndingSetting()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < endingPush[i].endingPush_group.transform.childCount; j++)
			{
				endingPush[i].endingPush_msg.Add(endingPush[i].endingPush_group.transform.GetChild(j).gameObject);
			}
		}
	}

	// Update is called once per frame
	void Update () {

		// Device Time ==========================================================================================================
		deviceTimer += Time.deltaTime * 3f;
		int tmpMin = (int)deviceTimer / 60;
		min = tmpMin % 60;
		hour = tmpMin / 60;
		deviceTimer_text.text = System.String.Format("{0:#00}:{1:00}", hour, min);
		mainTimer_text.text = System.String.Format("{0:#00}:{1:00}", hour, min);
		mainDate_text.text = System.DateTime.Now.ToString("M월 d일"); // + "(" + DateTime.Today.DayOfWeek + ")";

        // Calling ==========================================================================================================
		if (vibrateStatus)
		{
			vibrateTimer += Time.deltaTime;
			if (vibrateTimer >= 1f)
			{
				Handheld.Vibrate();
				vibrateTimer = 0f;
			}
		}

		if (callStatus)
		{
			callTimer += Time.deltaTime;
			callTimer_text.text = System.String.Format("{0:#00}:{1:00}", Mathf.Floor(callTimer) / 60, Mathf.Floor(callTimer) % 60);
		}

        // Playe Time
        if(startStatus)
        {
            play_timer += Time.deltaTime;
        }

        // Main Push ==========================================================================================================
            if (mainPushStatus)
            {
                pushTimer += Time.deltaTime;
                if (pushTimer >= 2f)
                {
                    mainPush[curActiveNumber].push_msg[prevPushNum].transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(mainPush[curActiveNumber].push_msg[pushCount], iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.5f));
                    AudioSource.PlayClipAtPoint(mainPush[curActiveNumber].push_clip, Vector3.zero);
                    prevPushNum = pushCount;
                    pushCount++;
                    if (pushCount == mainPush[curActiveNumber].push_msg.Length)
                        pushCount = 0;
                    pushTimer = 0f;
                }
            }

		if (curPanelNumber == 1 && fb_check_status)
		{
			if (fb_scroll.value <= 0f)
			{
				fb_bt_back.SetActive(true);
			}
		}

		// Ending Push ==========================================================================================================
		if (endingStatus)
		{
			endingPush_timer += Time.deltaTime;
			if (endingPush_timer >= endingPush_spawnTime)
			{
				ending_kind = Random.Range(0, 3);
				while (endingPush[ending_kind].endingPush_msg.Count == 0)
					ending_kind = Random.Range(0, 3);
				int rndNumber = Random.Range(0, endingPush[ending_kind].endingPush_msg.Count);
				endingPush[ending_kind].endingPush_msg[rndNumber].transform.parent = endingPush_array.transform;
				if (endingPush_count >= 2)
				{
					endingPush[ending_kind].endingPush_msg[rndNumber].transform.localPosition = new Vector3(Random.Range(-100f, 100f), Random.Range(-740f, 785f), 0);
					endingPush[ending_kind].endingPush_msg[rndNumber].transform.localEulerAngles = new Vector3(0, 0, Random.Range(-10f, 10f));
				}
				iTween.ScaleTo(endingPush[ending_kind].endingPush_msg[rndNumber], iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.2f));
				endingPush[ending_kind].endingPush_msg.RemoveAt(rndNumber);
				AudioSource.PlayClipAtPoint(mainPush[ending_kind].push_clip, Vector3.zero);

				endingPush_spawnTime -= 0.025f;

				if (endingPush[0].endingPush_msg.Count == 0 && endingPush[1].endingPush_msg.Count == 0 && endingPush[2].endingPush_msg.Count == 0)
				{
					endingStatus = false;
					Invoke("EndingProgress", 0.5f);
				}
				endingPush_count++;
				endingPush_timer = 0f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Time.timeScale = 0f;
			popup_pause.SetActive(true);
		}
	}

    // Calling ==========================================================================================================
    public void CallingPressed()
    {
        switch (calling_state_num)
        {
            case 0: // 전화받기
                calling_state_num = 1;
                calling_number[0].SetActive(false);
                calling_number[1].SetActive(true);
                bt_calling.sprite = bt_calling_sprite[0];
                bt_calling_text.text = "종료";
                audio.clip = voice;
                audio.loop = false;
                audio.Play();
                vibrateStatus = false;
                callStatus = true;
                Invoke("CallEnd", 4f);
                break;
            case 1: // 전화끊기
                CancelInvoke("CallEnd");
                CallEnd();
                break;
        }
    }

    void CallEnd()
    {
        calling_state_num = 2;

        audio.Stop();
        calling_bg.color = new Color(0.85f, 0.43f, 0.05f, 1f);
        calling_status_bg.color = new Color(0.02f, 0.21f, 0.24f, 1f);
        calling_end_text.SetActive(true);
        bt_calling.sprite = bt_calling_sprite[1];
        bt_calling_text.text = "";
        callStatus = false;
        Invoke("CampaignMain", 1f);
    }

    // Info ==========================================================================================================
    void CampaignMain()
    {
        info_panel.SetActive(true);
        StartCoroutine(InfoPanelProcess());
    }

    IEnumerator InfoPanelProcess()
    {


        yield return new WaitForSeconds(1f);

        info_msg_0.SetActive(true);

        //yield return new WaitForSeconds(0.5f);

        //iTween.ScaleTo(info_msg_0, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.3f, "easeType", "easeInExpo"));

        yield return new WaitForSeconds(1.5f);

        info_msg_1.SetActive(true);
        info_msg_2.SetActive(true);
        nameInputField.gameObject.SetActive(true);
        info_bt_Start.SetActive(true);

        //yield return new WaitForSeconds(1f);

        //info_bt_Next.SetActive(true);
    }

    public void InfoPressed(int index)
    {
        switch (index)
        {
            case 0: // Next
                info_next_page.SetActive(true);
                break;
            case 1: // Start
                if (nameString.Length > 0)
                {
                    for (int i = 0; i < nameText.Length; i++)
                        nameText[i].text = nameString;
                    alert_panel.SetActive(true);
                    main_panel.SetActive(true);
                    iTween.ScaleTo(info_panel, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.5f));
                    //Invoke("MainSetting", 3f);
                }
                else
                {
                    info_alert_panel.SetActive(true);
                }
                break;
            case 2: // Sign
                Application.OpenURL("http://uri-i.or.kr/community-2/Signing");
                break;
            case 3: // Exit
                Application.Quit();
                break;
            case 4: // Info Close
                info_alert_panel.SetActive(false);
                break;
        }
    }

    public void NameChanged()
    {
        nameString = nameInputField.text;
    }

    // Main ==========================================================================================================
    public void StartPressed()
    {
        alert_panel.SetActive(false);
        MainSetting();
    }

    void MainSetting()
    {
        startStatus = true;
        curActiveNumber = 0;

        for (int i = 2; i > -1; i--)
        {
            if (mainPush[i].runStatus == true)
            {
                mainPush[i].main_bt_image.color = new Color(1, 1, 1, 0.5f);
                mainPush[i].main_bt_image.gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                curActiveNumber = i;
            }
        }

        if (runAmount == 3)
        {
            endingStatus = true;
        }
        else
        {
            pushCount = 0;
            prevPushNum = 0;
            pushTimer = 2f;

            mainPushStatus = true;
            mainPush[curActiveNumber].main_new_tag.SetActive(true);
        }
    }

    public void MainBTPressed(int index)
    {
        if (!startStatus)
            return;

        mainPush[index].runStatus = true;
        runAmount++;

        mainPushStatus = false;
        mainPush[curActiveNumber].main_new_tag.SetActive(false);

        curPanelNumber = index;
        iTween.ScaleTo(mainPush[curPanelNumber].sns_panel, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.5f));

        switch (index)
        {
            case 0:
                StartCoroutine("KakaoAtypeProcess");
                break;
            case 1:
                StartCoroutine("FacebookProcess");
                break;
            case 2:
                StartCoroutine("MessageProcess");
                break;
        }
    }

    public void MainPushReset()
    {
        for (int i = 0; i < mainPush[curActiveNumber].push_msg.Length; i++)
        {
            mainPush[curActiveNumber].push_msg[i].transform.localScale = new Vector3(0, 0, 0);
        }
    }

    //void OnApplicationFocus(bool isFocus)
    //{
    //  if (isFocus)
    //  {
    //      if (isHomeStatus)
    //      {
    //          Time.timeScale = 0f;
    //          popup_home.SetActive(true);
    //          isHomeStatus = false;
    //      }
    //  }
    //  else {
    //      isHomeStatus = true;
    //  }
    //}

    public void PauseBTPressed(int index)
    {
        switch (index)
        {
            case 0: // Continue
                Time.timeScale = 1f;
                popup_pause.SetActive(false);
                break;
            case 1: // Exit
                Application.Quit();
                break;
            case 2: // Sign
                Application.OpenURL("http://uri-i.or.kr/community-2/Signing");
                break;
        }
    }

	// Ending ==========================================================================================================
	void EndingProgress()
	{
		AudioSource.PlayClipAtPoint(ending_clip, Vector3.zero);
		ending_bg.SetActive(true);

		Invoke("EndingMovieReady", 1f);
	}

	public void EndingMovieReady()
	{
		ending_movie_panel.SetActive(true);

		Invoke("EndingMoviePlay", 1f);
	}

	public void EndingMoviePlay()
	{
		ending_movie.SetActive(true);
		ending_movie_control.Load("ending.mp4");
	}

	void EndingOnReady()
	{
		Debug.Log("Movie Ready!!");

		ending_movie_control.Play();
	}

	void EndingOnEnd()
	{
		Debug.Log("Movie End!!");

		StartCoroutine(EndingProcess());
	}

	IEnumerator EndingProcess()
	{
		yield return new WaitForSeconds(1f);

		ending_msg_panel.SetActive(true);

        yield return new WaitForSeconds(1f);

        ending_time_text.text = System.String.Format("{0:#00}분 {1:00}초", Mathf.Floor(play_timer) / 60, Mathf.Floor(play_timer) % 60);
        ending_time_text.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

		ending_msg_0.SetActive(true);

		yield return new WaitForSeconds(1f);

		ending_msg_1.SetActive(true);

		//yield return new WaitForSeconds(1f);

		//ending_msg_2.SetActive(true);

		yield return new WaitForSeconds(1f);

		ending_bt_Sign.SetActive(true);
		ending_bt_Link.SetActive(true);
		ending_bt_Restart.SetActive(true);

	}

	public void EndingBTPressed(int index)
	{
		switch (index)
		{
			case 0: // Sign
				Application.OpenURL("http://uri-i.or.kr/community-2/Signing");
				break;
			case 1: // Link
				Application.OpenURL("http://www.uri-i.or.kr/community-2/online-list");
				break;
			case 2: // Restart
				SceneManager.LoadScene("MainScene");
				break;
				
		}
	}

	// Kakao ==============================================================================================================================
	IEnumerator KakaoAtypeProcess()
	{
		kakao_msg_count = 1;

		yield return new WaitForSeconds(1f);

		//kakao_positon = kakao_group_b.transform.position;
		//kakao_pos_y = kakao_group_b.GetComponent<RectTransform>().position.y;
		MainPushReset();

		float rndTime;
		for (int i = 1; i < 6; i++)
		{
			kakao_msg_a[i].SetActive(true);
			kakao_msg_count++;
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(rndTime);
		}

		for (int i = 6; i < 12; i++)
		{
			kakao_msg_a[i].SetActive(true);
			kakao_msg_count++;
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		for (int i = 12; i < 26; i++)
		{
			kakao_msg_a[i].SetActive(true);
			kakao_msg_count++;
			kakao_content[0].sizeDelta = new Vector2(1080f, kakao_content[0].sizeDelta.y + 200f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_a[26].SetActive(true);
		kakao_msg_count++;
		kakao_content[0].sizeDelta = new Vector2(1080f, kakao_content[0].sizeDelta.y + 690f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		for (int i = 27; i < 30; i++)
		{
			kakao_msg_a[i].SetActive(true);
			kakao_msg_count++;
			kakao_content[0].sizeDelta = new Vector2(1080f, kakao_content[0].sizeDelta.y + 200f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_a[30].SetActive(true);
		kakao_msg_count++;
		kakao_content[0].sizeDelta = new Vector2(1080f, kakao_content[0].sizeDelta.y + 110f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		kakao_bt_back.SetActive(false);

		for (int i = 31; i < 35; i++)
		{
			kakao_msg_a[i].SetActive(true);
			kakao_msg_count++;
			kakao_content[0].sizeDelta = new Vector2(1080f, kakao_content[0].sizeDelta.y + 80f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_bt_close.SetActive(true);
	}

	public void MoanPressed()
	{
		audio.clip = moanClip;
		audio.Play();
	}

	public void BackBTPressed()
	{
		if (kakao_ani_status)
			return;

		kakao_ani_status = true;

		audio.Stop();

		StopCoroutine("KakaoAtypeProcess");
		StopCoroutine("KakaoBtypeProcess");
		StopCoroutine("KakaoCtypeProcess");

		//kakao_bt_back.SetActive(false);
		kakao_list_panel.SetActive(true);

		iTween.MoveAdd(mainPush[0].sns_panel, iTween.Hash("x", 1080f, "time", 0.5f));

		Invoke("KakaoTypeSetting", 1f);
	}

	public void KakaoClosePressed()
	{
		audio.Stop();

		iTween.ScaleTo(mainPush[curPanelNumber].sns_panel, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.5f));

		MainSetting();
	}

	void KakaoTypeSetting()
	{
		// Reset
		kakao_group_a.SetActive(false);
		kakao_group_b.SetActive(false);
		kakao_group_c.SetActive(false);
		kakao_group_d.SetActive(false);

		for (int i = 0; i < 4; i++)
		{
			kakao_scroll_view[i].SetActive(false);
		}

		if (kakaoType == KakaoType.Atype)
		{
			if (kakao_msg_count > 16) // C Type
				kakaoType = KakaoType.Ctype;
			else
				kakaoType = KakaoType.Btype;
		} else if(kakaoType == KakaoType.Btype || kakaoType == KakaoType.Ctype)
		{
			kakaoType = KakaoType.Dtype;
		}

		switch (kakaoType)
		{
			case KakaoType.Btype:
				kakao_scroll_view[1].SetActive(true);
				kakao_group_b.SetActive(true);
				StartCoroutine("KakaoBtypeProcess");
				break;
			case KakaoType.Ctype:
				kakao_scroll_view[2].SetActive(true);
				kakao_group_c.SetActive(true);
				StartCoroutine("KakaoCtypeProcess");
				break;
			case KakaoType.Dtype:
				kakao_scroll_view[3].SetActive(true);
				kakao_group_d.SetActive(true);
				kakao_bt_back.SetActive(false);
				StartCoroutine("KakaoDtypeProcess");
				break;
		}
		iTween.MoveAdd(mainPush[0].sns_panel, iTween.Hash("x", -1080f, "time", 0.5f));
	}

	IEnumerator KakaoBtypeProcess()
	{
		yield return new WaitForSeconds(1f);

		kakao_ani_status = false;
		kakao_list_panel.SetActive(false);

		float rndTime;
		for (int i = 1; i < 6; i++)
		{
			kakao_msg_b[i].SetActive(true);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_b[6].SetActive(true);
		kakao_content[1].sizeDelta = new Vector2(1080f, kakao_content[1].sizeDelta.y + 36f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		for (int i = 7; i < 22; i++)
		{
			kakao_msg_b[i].SetActive(true);
			kakao_content[1].sizeDelta = new Vector2(1080f, kakao_content[1].sizeDelta.y + 200f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_b[22].SetActive(true);
		kakao_content[1].sizeDelta = new Vector2(1080f, kakao_content[1].sizeDelta.y + 690f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		for (int i = 23; i < 26; i++)
		{
			kakao_msg_b[i].SetActive(true);
			kakao_content[1].sizeDelta = new Vector2(1080f, kakao_content[1].sizeDelta.y + 200f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_b[26].SetActive(true);
		kakao_content[1].sizeDelta = new Vector2(1080f, kakao_content[1].sizeDelta.y + 110f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		kakao_bt_back.SetActive(false);

		for (int i = 27; i < 31; i++)
		{
			kakao_msg_b[i].SetActive(true);
			kakao_content[1].sizeDelta = new Vector2(1080f, kakao_content[1].sizeDelta.y + 80f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_bt_close.SetActive(true);
	}

	IEnumerator KakaoCtypeProcess()
	{
		yield return new WaitForSeconds(1f);

		kakao_ani_status = false;
		kakao_list_panel.SetActive(false);

		float rndTime;
		for (int i = 1; i < 6; i++)
		{
			kakao_msg_c[i].SetActive(true);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_c[6].SetActive(true);
		kakao_content[2].sizeDelta = new Vector2(1080f, kakao_content[2].sizeDelta.y + 36f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		for (int i = 7; i < 14; i++)
		{
			kakao_msg_c[i].SetActive(true);
			kakao_content[2].sizeDelta = new Vector2(1080f, kakao_content[2].sizeDelta.y + 200f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_c[14].SetActive(true);
		kakao_content[2].sizeDelta = new Vector2(1080f, kakao_content[2].sizeDelta.y + 690f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		for (int i = 15; i < 18; i++)
		{
			kakao_msg_c[i].SetActive(true);
			kakao_content[2].sizeDelta = new Vector2(1080f, kakao_content[2].sizeDelta.y + 200f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_c[18].SetActive(true);
		kakao_content[2].sizeDelta = new Vector2(1080f, kakao_content[2].sizeDelta.y + 110f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(1f);

		kakao_bt_back.SetActive(false);

		for (int i = 19; i < 23; i++)
		{
			kakao_msg_c[i].SetActive(true);
			kakao_content[2].sizeDelta = new Vector2(1080f, kakao_content[2].sizeDelta.y + 80f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_bt_close.SetActive(true);	}

	IEnumerator KakaoDtypeProcess()
	{
		yield return new WaitForSeconds(1f);

		kakao_ani_status = false;
		kakao_list_panel.SetActive(false);

		float rndTime;
		for (int i = 1; i < 7; i++)
		{
			kakao_msg_d[i].SetActive(true);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.7f, 1.2f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_msg_d[7].SetActive(true);
		kakao_content[3].sizeDelta = new Vector2(1080f, kakao_content[3].sizeDelta.y + 66f);
		AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
		yield return new WaitForSeconds(0.5f);

		for (int i = 8; i < 11; i++)
		{
			kakao_msg_d[i].SetActive(true);
			kakao_content[3].sizeDelta = new Vector2(1080f, kakao_content[3].sizeDelta.y + 80f);
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			rndTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(rndTime);
		}

		kakao_bt_close.SetActive(true);
	}

	// Facebook ==============================================================================================================================
	IEnumerator FacebookProcess()
	{
		yield return new WaitForSeconds(0.5f);

		fb_feed.transform.localPosition = new Vector3(0, 0, 0);
		fb_feed.SetActive(true);

		MainPushReset();
	
		yield return new WaitForSeconds(1f);
		fb_check_status = true;
	}

	public void FacebookBackPressed()
	{
		fb_movie_control.Stop();
		iTween.ScaleTo(mainPush[1].sns_panel, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.5f));

		MainSetting();
	}

	public void FacebookMoviePlay()
	{
		fb_movie.SetActive(true);
		fb_movie_control.Load("movie_fb.mp4");
	}

	void OnReady()
	{
		Debug.Log("Movie Ready!!");

		fb_movie_control.Play();

	}

	void OnEnd()
	{
		Debug.Log("Movie End!!");

		//fb_bt_back.SetActive(true);
		fb_movie.SetActive(false);
	}

	// Message ==============================================================================================================================
	IEnumerator MessageProcess()
	{
		yield return new WaitForSeconds(0.6f);

		message_content.SetActive(true);
		MainPushReset();

        for (int i = 0; i < 8; i++)
        {
            message_date_text[i].text = System.DateTime.Now.ToString("yyyy/MM/dd"); // + "(" + DateTime.Today.DayOfWeek + ")";
        }

        yield return new WaitForSeconds(2f);

		for (int i = 8; i < 22; i++)
		{
			message_msg[i].SetActive(true);
			RectTransform rt = message_content.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(0, 1711 + ((i - 7) * 200));
			AudioSource.PlayClipAtPoint(msg_clip, Vector3.zero);
			float rndTime = Random.Range(1f, 2f);
			yield return new WaitForSeconds(rndTime);
		}

		message_bt_close.SetActive(true);
	}

	public void MessageClosePressed()
	{
        message_sub_panel.SetActive(false);

		iTween.ScaleTo(mainPush[curPanelNumber].sns_panel, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.5f));

		MainSetting();
	}

	public void MessageSelected(int index)
	{
        if (message_ani_status)
            return;

        message_ani_status = true;

		switch (index)
		{
			case 6: // 5115-5005
			case 15:
                message_sub_number = 6;
				break;
			case 0: // 동생 
                message_sub_number = 1;
                break;
			case 1: // 4731-1121
            case 8:
            case 11:
            case 12:
                message_sub_number = 2;
                break;
			case 2: // Mother
			case 17:
			case 19:
                message_sub_number = 3;
                break;
			case 3: // 1214-5840
            case 9:
			case 13:
			case 18:
			case 21:
                message_sub_number = 4;
                break;
			case 4: // 5633-9001
                message_sub_number = 5;
                break;
			case 7: // 7754-3214
			case 20:
                message_sub_number = 7;
                break;
			case 5: // 5455-1211
            case 10:
            case 14:
			case 16:
                message_sub_number = 0;
                break;
		}

		message_sub_panel.SetActive(true);
        message_sub[message_sub_number].SetActive(true);

		iTween.MoveAdd(mainPush[2].sns_panel, iTween.Hash("x", -1080f, "time", 0.4f));
        Invoke("MessageAniStatusReset", 0.4f);
	}

	public void MessageBackPressed()
	{
        message_ani_status = true;

		iTween.MoveAdd(mainPush[2].sns_panel, iTween.Hash("x", 1080f, "time", 0.4f));

		Invoke("MessageSubReset", 0.4f);
	}

	void MessageSubReset()
	{
        message_ani_status = false;

        message_sub[message_sub_number].SetActive(false);
	}

    void MessageAniStatusReset()
    {
        message_ani_status = false;
    }
}
