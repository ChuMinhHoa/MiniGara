using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : GenericSingleton<ProfileManager>
{
    public PlayerData playerData;
    public ProfileDataConfig dataConfig;
    public override void Awake()
    {
        base.Awake();
        playerData.LoadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
