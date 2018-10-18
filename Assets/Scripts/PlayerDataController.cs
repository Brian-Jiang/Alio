using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class PlayerDataController : MonoBehaviour {

	public bool GameCenterState;
	public string userInfo;


	void Start () {  
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);
		IAchievement achievement = Social.CreateAchievement();
		achievement.id = "axy01";

		Social.localUser.Authenticate(HandleAuthenticated); 

		Debug.Log (Social.localUser);
	}

	private void HandleAuthenticated(bool success)  
	{  
		GameCenterState = success;  
		Debug.Log("*** HandleAuthenticated: success = " + success);  
		///初始化成功  
		if (success) {   
			userInfo = "Username: " + Social.localUser.userName +   
				"\nUser ID: " + Social.localUser.id +   
				"\nIsUnderage: " + Social.localUser.underage; 

			Social.LoadAchievements(HandleAchievementsLoaded);  
			Social.LoadAchievementDescriptions(HandleAchievementDescriptionsLoaded);  
			Debug.Log (userInfo);  
		} else {  
			///初始化失败  
			Debug.Log ("authenticate failed");
		}  
	} 

	private void HandleAchievementsLoaded(IAchievement[] achievements)  
	{  
		Debug.Log("* HandleAchievementsLoaded");  
		foreach(IAchievement achievement in achievements)  
		{  
			Debug.Log("* achievement = " + achievement.ToString());  
		}  
	}  

	private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)  
	{  
		Debug.Log("*** HandleAchievementDescriptionsLoaded");  
		foreach(IAchievementDescription achievementDescription in achievementDescriptions)  
		{  
			Debug.Log("* achievementDescription = " + achievementDescription.ToString());  
		}  
	}  

	private void HandleProgressReported(bool success)  
	{  
		Debug.Log("*** HandleProgressReported: success = " + success);  
	}  

	public void completeAchievement(string achievementID){
		Social.ReportProgress (achievementID, 100.0, HandleProgressReported);
	}

	public void unlockSong (int songNumber) {
		if (PlayerPrefs.GetInt ("songUnlock_" + songNumber) == 0) {
			PlayerPrefs.SetInt ("songUnlock_" + songNumber, 1);
			PlayerPrefs.SetInt ("experience_" + songNumber, 0);
		}
	}



	void OnEnable(){
		//0=false
		//1=true
		//PlayerPrefs.SetInt ("firstTimeStart", 0);
		PlayerPrefs.SetInt ("activeSongNumber",1);

		if (!PlayerPrefs.HasKey ("firstTimeStart") || PlayerPrefs.GetInt ("firstTimeStart") == 1) {
//			if (Social.localUser.authenticated) {  
//				Social.ReportProgress("70490135", 0.0, HandleProgressReported);   
//			}  
			PlayerPrefs.SetInt ("firstTimeStart", 1);

			PlayerPrefs.SetInt ("experience_1", 0);

			PlayerPrefs.SetInt ("experience_2", 0);

			PlayerPrefs.SetInt ("experience_3", 0);
			PlayerPrefs.SetInt ("experience_4", 0);

			PlayerPrefs.SetInt ("clueUnlock_1", 0);

			PlayerPrefs.SetInt ("clueUnlock_2", 0);		

			PlayerPrefs.SetInt ("clueUnlock_3", 0);		

			PlayerPrefs.SetInt ("songUnlock_1", 1);
		
			PlayerPrefs.SetInt ("songUnlock_2", 1);
		
			PlayerPrefs.SetInt ("songUnlock_3", 1);
			PlayerPrefs.SetInt ("songUnlock_4", 0);

			PlayerPrefs.SetInt ("highScore_1_special", 0);
			PlayerPrefs.SetInt ("highScore_1_easy", 0);
			PlayerPrefs.SetInt ("highScore_2_normal", 0);
			PlayerPrefs.SetInt ("highScore_2_veryEasy", 0);
			PlayerPrefs.SetInt ("highScore_3_veryEasy", 0);
			PlayerPrefs.SetInt ("highScore_3_hard", 0);
			PlayerPrefs.SetInt ("highScore_4_hard", 0);

			PlayerPrefs.SetInt ("totalExperience", 0);
			PlayerPrefs.SetInt ("perfectPerformances", 0);
			PlayerPrefs.SetInt ("amazingPerformances", 0);
			PlayerPrefs.SetInt ("noPerformances", 0);
			PlayerPrefs.SetFloat ("timeAdjust", 0f);
			PlayerPrefs.SetFloat ("volume", 0.7f);

			PlayerPrefs.SetInt ("plotProgress", 0);
			PlayerPrefs.SetInt ("plotChoice0_1", 0);
			
		}

//		PlayerPrefs.SetInt ("firstTimeStart", 1);  //only for test



	}
}
