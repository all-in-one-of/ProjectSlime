using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem {
	private static bool init = false;
	private static int score;
	private static List<List<Record>> recordTables = new List<List<Record>>();
	private static List<Record> recordResult = new List<Record>();
	
	public ScoreSystem (){

		if (init) {
			return;
		}

		//Test
		RegistTable(0);
		RegistTable(1);
		RegistTable(2);
		RegistTable(3);

		//0 lostHit	
		//1 lava 3
		//2 jump 
		//3 dance
		//4 water 2
		//5 eatBullet 4
		//6 eatMan 2
		//7 reborn 2
		//8 reborned 4
		//9 camera 4

		init = true;
	}

	public static void RegistTable(int playerID) {
		recordTables.Add(new List<Record>());
	}
	
	public static int GetScore() {
		return score;
	}

	public static void ModifyScore(int modifyScore) {
		score += modifyScore;
		UIManager.direct.counter.text = score.ToString();
	}

	public static void AddRecord(int playerID, int actionID, int score) {
		foreach (var record in recordTables[playerID]) {
			if (record.actionID == actionID) {
				record.record += score;
				return;
			}
		}
		recordTables[playerID].Add(new Record(playerID , actionID, score));
	}

	public static void CaculateRecord() {
		List<Record> recordCaculate = new List<Record>();

		for (int i = 0; i < recordTables.Count; i++) {
			foreach (var record in recordTables[i]) {
				switch (record.actionID) {
					case 0:
						record.sum = record.record * 1;
						break;
					case 1:
						record.sum = record.record * 3;
						break;
					case 2:
						record.sum = record.record * 1;
						break;
					case 3:
						record.sum = record.record * 0.5f;
						break;
					case 4:
						record.sum = record.record * 2;
						break;
					case 5:
						record.sum = record.record * 4;
						break;
					case 6:
						record.sum = record.record * 2;
						break;
					case 7:
						record.sum = record.record * 2;
						break;
					case 8:
						record.sum = record.record * 4;
						break;
					case 9:
						record.sum = record.record * 4;
						break;
					default:
						break;
				}

				bool findFlag = false;

				for (int j = 0; j < recordCaculate.Count; j++) {
					if (recordCaculate[j].actionID == record.actionID) {
						if (recordCaculate[j].sum < record.sum) {
							recordCaculate[j] = record;
							findFlag = true;
							break;
						}
					}
				}

				if (findFlag == false) {
					recordCaculate.Add(record);
				}
			}
		}

		for (int i = 0; i < 4; i++) {
			if (recordCaculate.Count > 0) {
				int findID = 0;
				float findSum = 0;

				for (int j = 0; j < recordCaculate.Count; j++) {
					if (recordCaculate[j].sum > findSum) {
						if (recordCaculate[j].sum > findSum) {
							findID = j;
							findSum = recordCaculate[j].sum;
							recordResult.Add(recordCaculate[j]);
						}
					}
				}
				recordCaculate.RemoveAt(findID);
			}
		}

		UIManager.direct.OnScore();

		for (int i = 0; i < recordResult.Count; i++) {
			string show = "OAO??";
			string descript = "再加油點好ㄇ";

			//0 lostHit	
			//1 lava 3
			//2 jump 
			//3 dance
			//4 water 2
			//5 eatBullet 4
			//6 eatMan 2
			//7 reborn 2
			//8 reborned 4
			//9 camera 4

			if (recordResult[i] != null) {
				switch (recordResult[i].actionID) {
					case 0:
						show = "大舌頭";
						descript = "舔空氣" + recordResult[i].record + "次";
						break;
					case 1:
						show = "自虐傾向";
						descript = "撞牆" + recordResult[i].record + "次";
						break;
					case 2:
						show = "過動兒";
						descript = "跳躍" + recordResult[i].record + "次";
						break;
					case 3:
						show = "舞林高手";
						descript = "深呼吸" + recordResult[i].record + "次";
						break;
					case 4:
						show = "游泳健將";
						descript = "水中跳躍" + recordResult[i].record + "次";
						break;
					case 5:
						show = "吃壞肚子";
						descript = "吃雜物" + recordResult[i].record + "次";
						break;
					case 6:
						show = "食人族";//你餓了嗎
						descript = "吃人" + recordResult[i].record + "次";
						break;
					case 7:
						show = "不死鳥";//邪惡不死鳥
						descript = "復活" + recordResult[i].record + "次";
						break;
					case 8:
						show = "工具人";
						descript = "被復活" + recordResult[i].record + "次";
						break;
					case 9:
						show = "萬眾矚目";
						descript = "搶走鏡頭" + recordResult[i].record + "次";
						break;
					default:
						break;
				}
			} else {
				Debug.LogError("BufferErr");
			}	

			UIManager.direct.recordShower[i].text = show;
			UIManager.direct.recordShower2[i].text = descript;
			UIManager.direct.recordShower3[i].material = UIManager.direct.recordPlayer[recordResult[i].playerID];
		}
	}
}


public class Record {
	public int playerID = 0;
	public int actionID = 0;
	public int record = 0;
	public float sum = 0;

	public Record() {
	}

	public Record(int playerID , int actionID , int record) {
		this.playerID = playerID;
		this.actionID = actionID;
		this.record = record;
	}
}